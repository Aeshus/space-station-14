using System.Diagnostics.CodeAnalysis;
using Robust.Client.UserInterface;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.Manager;
using Robust.Shared.Utility;

namespace Content.Client.StyleProto;

/// <summary>
/// Manages stylesheets, creating them from prototypes and allowing code to subscribe to updates.
/// </summary>
public sealed partial class StylesheetManager : IPostInjectInit, IStylesheetManager
{
    [Dependency] private IPrototypeManager _prototypeManager = default!;
    [Dependency] private ISerializationManager _serializationManager = default!;
    [Dependency] private ILogManager _logManager = default!;

    private readonly Dictionary<ProtoId<StylesheetPrototype>, StyleAccessor> _styleAccessors = [];
    private ISawmill _sawmill = default!;

    /// <inheritdoc/>
    public event Action<SheetletConfigRegistry>? OnStyleReload;

    /// <inheritdoc/>
    public void Initialize()
    {
        DirtyAll();
    }

    /// <inheritdoc/>
    public void PostInject()
    {
        _sawmill = _logManager.GetSawmill("stylesheet");
        _prototypeManager.PrototypesReloaded += OnPrototypesReloaded;
    }

    /// <summary>
    /// Reloads the stylesheets when stylesheet prototypes are modified.
    /// </summary>
    /// <param name="eventArgs">Event's arguments</param>
    private void OnPrototypesReloaded(PrototypesReloadedEventArgs eventArgs)
    {
        if (!eventArgs.WasModified<StylesheetPrototype>())
            return;

        DirtyAll();
    }

    /// <inheritdoc/>
    public void DirtyAll()
    {
        foreach (var proto in _prototypeManager.EnumeratePrototypes<StylesheetPrototype>())
        {
            UpdateStylesheet(proto);
        }
    }

    /// <inheritdoc/>
    public void Dirty(ProtoId<StylesheetPrototype> proto)
    {
        if (_prototypeManager.TryIndex(proto, out var prototype))
            UpdateStylesheet(prototype);
    }

    /// <inheritdoc/>
    public bool TryGetStyleSubscription(ProtoId<StylesheetPrototype> proto,
        [NotNullWhen(true)] out IStyleAccessor? accessor)
    {
        accessor = null;

        if (!_styleAccessors.TryGetValue(proto, out var acc))
            return false;

        accessor = acc;
        return true;
    }

    /// <inheritdoc/>
    public IStyleAccessor GetStyleSubscription(ProtoId<StylesheetPrototype> proto)
    {
        return _styleAccessors[proto];
    }

    /// <summary>
    /// Updates a stylesheet by rebuilding it
    /// </summary>
    /// <param name="proto"></param>
    private void UpdateStylesheet(StylesheetPrototype proto)
    {
        if (proto.Abstract)
            return;

        SheetletConfigRegistry configs;
        Stylesheet stylesheet;
        try
        {
            // Copy before subscribers mutate the configs, then notify them in subscription order.
            configs = _serializationManager.CreateCopy(
                proto.Configs,
                notNullableOverride: true);
            OnStyleReload?.Invoke(configs);

            var rules = new List<StyleRule>();
            foreach (var sheetlet in proto.Sheetlets)
            {
                rules.AddRange(sheetlet.Generate(configs));
            }

            stylesheet = new Stylesheet(rules);
        }
        catch (Exception e)
        {
            _sawmill.Error($"Failed to rebuild stylesheet '{proto.ID}': {e}");
            return;
        }

        if (!_styleAccessors.TryGetValue(proto, out var accessor))
        {
            _styleAccessors.Add(proto, new StyleAccessor(_sawmill, stylesheet, configs));
        }
        else
        {
            // Implicitly calls StyleChanged for subscribers
            accessor.Update(stylesheet, configs);
        }
    }

    /// <summary>
    /// Allows for accessing/subscribing to the current stylesheet and registry for a protoid.
    /// </summary>
    public interface IStyleAccessor
    {
        /// <summary>
        /// Event called when styles change.
        /// </summary>
        /// <remarks>
        /// This will also immediately call the specified delegate.
        /// </remarks>
        event Action<Stylesheet, SheetletConfigRegistry> StyleChanged;
    }

    /// <inheritdoc/>
    public sealed class StyleAccessor(ISawmill sawmill, Stylesheet stylesheet, SheetletConfigRegistry configs)
        : IStyleAccessor
    {
        /// <summary>
        /// The current stylesheet.
        /// </summary>
        private Stylesheet Stylesheet { get; set; } = stylesheet;

        /// <summary>
        /// The current sheetlet configs.
        /// </summary>
        /// <remarks>
        /// We assume these will be immutable after they are placed in here.
        /// </remarks>
        private SheetletConfigRegistry Configs { get; set; } = configs;

        /// <summary>
        /// The stylesheet subscriptions for updates.
        /// </summary>
        private readonly List<Action<Stylesheet, SheetletConfigRegistry>> _subscriptions = [];

        /// <inheritdoc/>
        public event Action<Stylesheet, SheetletConfigRegistry> StyleChanged
        {
            add
            {
                DebugTools.Assert(!_subscriptions.Contains(value),
                    "Attempted to subscribe the same stylesheet twice.");
                _subscriptions.Add(value);

                try
                {
                    value(Stylesheet, Configs);
                }
                catch (Exception e)
                {
                    sawmill.Error($"Exception caught during style update: {e}");
                }
            }
            remove
            {
                DebugTools.Assert(_subscriptions.Contains(value),
                    "Attempted to unsubscribe a stylesheet that was not subscribed to.");
                _subscriptions.Remove(value);
            }
        }

        /// <summary>
        /// Updates the internal stylesheet and configs.
        /// </summary>
        /// <param name="stylesheet">The stylesheet</param>
        /// <param name="configs">The sheetlet configs</param>
        public void Update(Stylesheet stylesheet, SheetletConfigRegistry configs)
        {
            Stylesheet = stylesheet;
            Configs = configs;

            foreach (var subscriber in _subscriptions.ToArray())
            {
                try
                {
                    subscriber(stylesheet, configs);
                }
                catch (Exception e)
                {
                    sawmill.Error($"Exception caught during style update: {e}");
                }
            }
        }
    }
}
