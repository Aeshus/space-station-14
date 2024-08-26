using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Content.Client.Popups;
using Content.Shared.Construction;
using Content.Shared.Construction.Prototypes;
using Content.Shared.Examine;
using Content.Shared.Input;
using Content.Shared.Popups;
using Content.Shared.Wall;
using JetBrains.Annotations;
using Robust.Client.GameObjects;
using Robust.Client.Placement;
using Robust.Client.Player;
using Robust.Shared.Enums;
using Robust.Shared.Input;
using Robust.Shared.Input.Binding;
using Robust.Shared.Map;
using Robust.Shared.Prototypes;
using YamlDotNet.Serialization.NodeTypeResolvers;

namespace Content.Client.Construction;

/// <summary>
/// The client-side implementation of the construction system, which is used for constructing entities in game.
/// </summary>
[UsedImplicitly]
public sealed class ConstructionSystem : SharedConstructionSystem
{
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly IPlacementManager _placementManager = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;
    [Dependency] private readonly ExamineSystemShared _examineSystem = default!;
    [Dependency] private readonly SharedTransformSystem _transformSystem = default!;
    [Dependency] private readonly PopupSystem _popupSystem = default!;

    public List<ConstructionPrototype> Favorites { get; set; } = [];

    public event EventHandler? PlacementChanged;
    public event EventHandler<string>? ConstructionGuideReceived;

    private readonly Dictionary<int, EntityUid> _ghosts = new();
    private readonly Dictionary<string, ConstructionGuide> _guideCache = new();

    /// <inheritdoc />
    public override void Initialize()
    {
        base.Initialize();

        UpdatesOutsidePrediction = true;

        CommandBinds.Builder
            .Bind(EngineKeyFunctions.Use,
                new PointerInputCmdHandler(HandleUse, outsidePrediction: true))
            //.Bind(ContentKeyFunctions.EditorFlipObject,
            //new PointerInputCmdHandler(HandleFlip, outsidePrediction: true))
            .Register<ConstructionSystem>();

        SubscribeLocalEvent<ConstructionGhostComponent, ExaminedEvent>(HandleConstructionGhostExamined);
        SubscribeNetworkEvent<AckStructureConstructionMessage>(HandleAckStructure);
        SubscribeNetworkEvent<ResponseConstructionGuide>(HandleConstructionGuideReceived);

        _placementManager.PlacementChanged += HandlePlacementChanged;
    }

    private void HandleConstructionGuideReceived(ResponseConstructionGuide ev)
    {
        _guideCache[ev.ConstructionId] = ev.Guide;
        ConstructionGuideReceived?.Invoke(this, ev.ConstructionId);
    }

    /// <summary>
    /// Forwards the `PlacementChanged` event from <see cref="PlacementManager" />.
    /// </summary>
    private void HandlePlacementChanged(object? sender, EventArgs e)
    {
        PlacementChanged?.Invoke(sender, e);
    }

    /// <inheritdoc />
    public override void Shutdown()
    {
        base.Shutdown();

        CommandBinds.Unregister<ConstructionSystem>();
    }

    /// <summary>
    /// Get the guide from the cache, or request it from the server if not found.
    /// </summary>
    public ConstructionGuide? GetGuide(ConstructionPrototype prototype)
    {
        if (_guideCache.TryGetValue(prototype.ID, out var guide))
            return guide;

        RaiseNetworkEvent(new RequestConstructionGuide(prototype.ID));
        return null;
    }

    /// <summary>
    /// Handle flipping a prototype.
    /// </summary>
    /*
    private bool HandleFlip(in PointerInputCmdHandler.PointerInputCmdArgs args)
    {
        if (!_placementManager.IsActive || _placementManager.Eraser || ?.Mirror == null)
        return false;

        _selection = _prototypeManager.Index<ConstructionPrototype>(_selection.Mirror);
        UpdateGhostPlacement();

        return true;
    }
    */

    /// <summary>
    /// Start building the ghost structure.
    /// </summary>
    private bool HandleUse(in PointerInputCmdHandler.PointerInputCmdArgs args)
    {
        if (!args.EntityUid.IsValid() || !IsClientSide(args.EntityUid) ||
            !HasComp<ConstructionGhostComponent>(args.EntityUid))
            return false;

        TryStartConstruction(args.EntityUid);
        return true;
    }

    private void TryStartConstruction(EntityUid ghostId)
    {
        ConstructionGhostComponent? ghostComp = null;

        if (!Resolve(ghostId, ref ghostComp))
            return;

        if (ghostComp.Prototype == null)
        {
            throw new ArgumentException($"Can't start construction for a ghost with no prototype. Ghost id: {ghostId}");
        }

        var transform = EntityManager.GetComponent<TransformComponent>(ghostId);
        var msg = new TryStartStructureConstructionMessage(GetNetCoordinates(transform.Coordinates),
            ghostComp.Prototype.ID,
            transform.LocalRotation,
            ghostId.GetHashCode());
        RaiseNetworkEvent(msg);
    }


    /// <summary>
    /// Displays construction information when ghost is examined.
    /// </summary>
    private void HandleConstructionGhostExamined(EntityUid uid,
        ConstructionGhostComponent component,
        ExaminedEvent args)
    {
        if (component.Prototype == null)
            return;

        using (args.PushGroup(nameof(ConstructionGhostComponent)))
        {
            args.PushMarkup(Loc.GetString(
                "construction-ghost-examine-message",
                ("name", component.Prototype.Name)));

            if (!_prototypeManager.TryIndex(component.Prototype.Graph, out ConstructionGraphPrototype? graph))
                return;

            var startNode = graph.Nodes[component.Prototype.StartNode];

            if (!graph.TryPath(component.Prototype.StartNode, component.Prototype.TargetNode, out var path) ||
                !startNode.TryGetEdge(path[0].Name, out var edge))
            {
                return;
            }

            foreach (var step in edge.Steps)
            {
                step.DoExamine(args);
            }
        }
    }

    /// <summary>
    /// Deletes the ghost when it starts being constructed.
    /// </summary>
    private void HandleAckStructure(AckStructureConstructionMessage msg)
    {
        ClearGhost(msg.GhostId);
    }

    /// <summary>
    /// Removes all construction entities.
    /// </summary>
    public void ClearAllGhosts()
    {
        foreach (var ghost in _ghosts.Values)
        {
            EntityManager.QueueDeleteEntity(ghost);
        }

        _ghosts.Clear();
    }

    public void SpawnGhost(ConstructionPrototype prototype, EntityCoordinates loc, Direction dir)
    {
        TrySpawnGhost(prototype, loc, dir, out _);
    }

    /// <summary>
    /// Creates a construction ghost at the given location.
    /// </summary>
    public bool TrySpawnGhost(ConstructionPrototype prototype,
        EntityCoordinates loc,
        Direction dir,
        [NotNullWhen(true)] out EntityUid? ghost)
    {
        ghost = null;

        if (_playerManager.LocalEntity is not { } user || !user.IsValid())
            return false;

        // Don't build two ghosts in the same place
        if (IsGhostPresent(loc))
            return false;

        // Check if the ghost is allowed to go where they are putting it.
        var predicate = GetPredicate(prototype.CanBuildInImpassable, _transformSystem.ToMapCoordinates(loc));
        if (!_examineSystem.InRangeUnOccluded(user, loc, 20f, predicate))
            return false;

        if (!CheckConstructionConditions(prototype, loc, dir, user))
            return false;

        return CreateGhostEntity(prototype, loc, dir, ref ghost);
    }

    /// <summary>
    /// Builds the actual ghost entity
    /// </summary>
    public bool CreateGhostEntity(ConstructionPrototype prototype,
        EntityCoordinates loc,
        Direction dir,
        [NotNullWhen(true)] ref EntityUid? ghost)
    {
        ghost = EntityManager.SpawnEntity("constructionghost", loc);

        var ghostComp = EntityManager.GetComponent<ConstructionGhostComponent>(ghost.Value);
        ghostComp.Prototype = prototype;

        var transformComp = EntityManager.GetComponent<TransformComponent>(ghost.Value);
        transformComp.LocalRotation = dir.ToAngle();

        var sprite = EntityManager.GetComponent<SpriteComponent>(ghost.Value);
        sprite.Color = new Color(48, 255, 48, 128);

        for (var i = 0; i < prototype.Layers.Count; i++)
        {
            // There is no way to actually check if this already exists, so we blindly insert a new one
            sprite.AddBlankLayer(i);
            sprite.LayerSetSprite(i, prototype.Layers[i]);
            sprite.LayerSetShader(i, "unshaded");
            sprite.LayerSetVisible(i, true);
        }

        if (prototype.CanBuildInImpassable)
            EnsureComp<WallMountComponent>(ghost.Value).Arc = new Angle(Math.Tau);

        _ghosts.Add(ghost.GetHashCode(), ghost.Value);

        return true;
    }

    private bool CheckConstructionConditions(ConstructionPrototype prototype,
        EntityCoordinates loc,
        Direction dir,
        EntityUid user)
    {
        foreach (var condition in prototype.Conditions)
        {
            if (condition.Condition(user, loc, dir))
                continue;

            var message = condition.GenerateGuideEntry()?.Localization;
            if (message is not null)
            {
                // was: PopupCoordinates(Loc.GetString(message), loc);
                _popupSystem.PopupCursor(Loc.GetString(message));
            }

            return false;
        }

        return true;
    }

    /// <summary>
    /// Removes a construction ghost entity with the given ID.
    /// </summary>
    public void ClearGhost(int ghostId)
    {
        if (!_ghosts.TryGetValue(ghostId, out var ghost))
            return;

        EntityManager.QueueDeleteEntity(ghost);
        _ghosts.Remove(ghostId);
    }

    /// <summary>
    /// Checks if any construction ghosts are present at the given location.
    /// </summary>
    private bool IsGhostPresent(EntityCoordinates loc)
    {
        return _ghosts.Any(ghost =>
            EntityManager.GetComponent<TransformComponent>(ghost.Value).Coordinates.Equals(loc));
    }


    public void ToggleFavorite(ConstructionPrototype? selection)
    {
        if (selection is null)
            return;

        if (!Favorites.Remove(selection))
            Favorites.Add(selection);
    }

    public void Select(ConstructionPrototype prototype)
    {
        UpdateGhostPlacement(prototype);
    }

    private void UpdateGhostPlacement(ConstructionPrototype? selection)
    {
        if (selection is null)
            return;

        if (selection.Type != ConstructionType.Structure)
        {
            return;
        }

        var info = new PlacementInformation
        {
            IsTile = false,
            PlacementOption = selection.PlacementMode,
        };

        var hijack = new ConstructionPlacementHijack(this, selection);

        _placementManager.BeginPlacing(info, hijack);
    }

    /// <summary>
    /// Toggle the eraser on and off.
    /// </summary>
    public void ToggleErase()
    {
        if (_placementManager.Eraser)
            _placementManager.Clear();

        // Hijack to clear the ghost placement thingy
        var hijack = new ConstructionPlacementHijack(this, null);

        _placementManager.ToggleEraserHijacked(hijack);
    }

    /// <summary>
    /// Start crafting an item.
    /// </summary>
    public void TryStartItemConstruction(ConstructionPrototype? selection)
    {
        if (selection is null)
            return;

        RaiseNetworkEvent(new TryStartItemConstructionMessage(selection.ID));
    }

    public void TryStartStructureConstruction(ConstructionPrototype? selection)
    {
        UpdateGhostPlacement(selection);
    }

    public void ClearPlacement()
    {
        _placementManager.Clear();
    }
}
