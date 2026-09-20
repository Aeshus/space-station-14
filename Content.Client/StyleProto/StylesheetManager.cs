using System.Diagnostics.CodeAnalysis;
using Robust.Client.UserInterface;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.Manager;

namespace Content.Client.StyleProto;

public sealed partial class StylesheetManager : IPostInjectInit
{
    [Dependency] private IPrototypeManager _prototypeManager = default!;
    [Dependency] private ISerializationManager _serializationManager = default!;
    [Dependency] private ILogManager _logManager = default!;

    private Dictionary<ProtoId<StylesheetPrototype>, StyleAccessor> _styleAccessors = [];
    private ISawmill _sawmill = default!;

    /// <summary>
    /// An event that gets invoked whenever the Stylesheets are reloaded.
    /// </summary>
    /// <remarks>
    /// This is used for mutating Sheetlet Configs. Note, it is in subscription order.
    /// </remarks>
    public event Action<SheetletConfigRegistry>? OnStyleReload;

    /// <summary>
    /// Initialize the StylesheetManager.
    /// </summary>
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

    /// <summary>
    /// Dirties all the Stylesheets so that they are reloaded/rebuilt.
    /// </summary>
    public void DirtyAll()
    {
        foreach (var proto in _prototypeManager.EnumeratePrototypes<StylesheetPrototype>())
        {
            UpdateStylesheet(proto);
        }
    }

    /// <summary>
    /// Dirty a specific Stylesheet so it is reloaded/rebuilt.
    /// </summary>
    /// <param name="proto">The stylesheet prototype</param>
    public void Dirty(ProtoId<StylesheetPrototype> proto)
    {
        UpdateStylesheet(_prototypeManager.Index(proto));
    }

    /// <summary>
    /// Updates a stylesheet by rebuilding it
    /// </summary>
    /// <param name="proto"></param>
    private void UpdateStylesheet(StylesheetPrototype proto)
    {
        if (proto.Abstract)
            return;

        // Deep copy the configs (as to not mutate the Prototype's version) and then unordered notify subscribers
        // to mutate it. (TODO: move to event bus subscriptions for ordering?)
        var configs = _serializationManager.CreateCopy(
            proto.Configs,
            notNullableOverride: true);
        OnStyleReload?.Invoke(configs);

        var rules = new List<StyleRule>();
        foreach (var sheetlet in proto.Sheetlets)
        {
            rules.AddRange(sheetlet.Generate(configs));
        }

        if (!_styleAccessors.ContainsKey(proto))
        {
            _styleAccessors.Add(proto, new StyleAccessor(new Stylesheet(rules), configs));
        }
        else
        {
            // Implicitly calls StyleChanged for subscribers
            _styleAccessors[proto].Update(new Stylesheet(rules), configs);
        }
    }

    /// <summary>
    /// Tries and get a stylesheet subscription from a prototype.
    /// </summary>
    /// <param name="proto">Stylesheet prototype</param>
    /// <param name="accessor">An acessor which contains an event to subscribe to</param>
    /// <returns>True if the accessor is found, False if null</returns>
    public bool TryGetStyleSubscription(ProtoId<StylesheetPrototype> proto,
        [NotNullWhen(true)] out IStyleAccessor? accessor)
    {
        accessor = null;

        if (!_styleAccessors.TryGetValue(proto, out var acc))
            return false;

        accessor = acc;
        return true;
    }

    /// <summary>
    /// Gets the style subscription with the prototype.
    /// </summary>
    /// <param name="proto">Stylesheet prototype</param>
    /// <returns>The accessor</returns>
    public IStyleAccessor GetStyleSubscription(ProtoId<StylesheetPrototype> proto)
    {
        return _styleAccessors[proto];
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
    public sealed class StyleAccessor(Stylesheet stylesheet, SheetletConfigRegistry configs) : IStyleAccessor
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
        /// The actual internal event that users subscribe to.
        /// </summary>
        private event Action<Stylesheet, SheetletConfigRegistry>? StyleChangedInternal;

        /// <inheritdoc/>
        public event Action<Stylesheet, SheetletConfigRegistry> StyleChanged
        {
            add
            {
                try
                {
                    value(Stylesheet, Configs);
                }
                catch (Exception)
                {
                    // ignored
                }

                StyleChangedInternal += value;
            }
            remove => StyleChangedInternal -= value;
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

            StyleChangedInternal?.Invoke(stylesheet, configs);
        }
    }
}
