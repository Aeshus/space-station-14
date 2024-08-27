using System.Linq;
using Content.Shared.Construction.Prototypes;
using Content.Shared.Input;
using Robust.Client.Placement;
using Robust.Client.Utility;
using Robust.Shared.Input.Binding;
using Robust.Shared.Map;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;

namespace Content.Client.Construction
{
    public sealed class ConstructionPlacementHijack : PlacementHijack
    {
        [Dependency] private readonly IPrototypeManager _prototypeManager = default!;

        private readonly ConstructionSystem _constructionSystem;
        private readonly ConstructionPrototype? _prototype;

        public override bool CanRotate { get; }

        public ConstructionPlacementHijack(ConstructionSystem constructionSystem, ConstructionPrototype? prototype)
        {
            _constructionSystem = constructionSystem;
            _prototype = prototype;
            CanRotate = prototype?.CanRotate ?? true;
        }

        /// <inheritdoc />
        public override bool HijackPlacementRequest(EntityCoordinates coordinates)
        {
            if (_prototype == null)
                return true;

            var dir = Manager.Direction;
            _constructionSystem.SpawnGhost(_prototype, coordinates, dir);

            return true;
        }

        /// <inheritdoc />
        public override bool HijackDeletion(EntityUid entity)
        {
            if (IoCManager.Resolve<IEntityManager>().HasComponent<ConstructionGhostComponent>(entity))
            {
                _constructionSystem.ClearGhost(entity.GetHashCode());
            }

            return true;
        }

        /// <inheritdoc />
        public override void StartHijack(PlacementManager manager)
        {
            base.StartHijack(manager);
            manager.CurrentTextures = _prototype?.Layers.Select(sprite => sprite.DirFrame0()).ToList();

            CommandBinds.Builder
                .Bind(ContentKeyFunctions.EditorFlipObject,
                    new PointerInputCmdHandler(HandleFlip, outsidePrediction: true))
                .Register<PlacementManager>();
        }

        private bool HandleFlip(ICommonSession? session, EntityCoordinates coords, EntityUid uid)
        {
            if (_prototype?.Mirror == null)
                return false;

            _constructionSystem.Select(_prototypeManager.Index<ConstructionPrototype>(_prototype.Mirror));

            return true;
        }
    }
}
