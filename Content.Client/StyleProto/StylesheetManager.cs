using System.Diagnostics.CodeAnalysis;
using Robust.Client.UserInterface;
using Robust.Shared.Prototypes;

namespace Content.Client.StyleProto;

public sealed partial class StylesheetManager : IPostInjectInit
{
    [Dependency] private IPrototypeManager _prototypeManager = default!;
    [Dependency] private ILogManager _logManager = default!;

    private Dictionary<ProtoId<StylesheetPrototype>, StyleAccessor> _styleAccessors = [];
    private ISawmill _sawmill = default!;

    /// <inheritdoc/>
    public event Action<SheetletConfigRegistry>? OnStyleReload;

    /// <inheritdoc/>
    public void Initialize()
    {
        DirtyAll();

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

    public void Dirty(ProtoId<StylesheetPrototype> proto)
    {
        UpdateStylesheet(_prototypeManager.Index(proto));
    }

    private void UpdateStylesheet(StylesheetPrototype proto)
    {
        var rules = new List<StyleRule>();
        foreach (var sheetlet in proto.Sheetlets)
        {
            //rules.AddRange(sheetlet.Generate(proto.Configs));
        }

        if (!_styleAccessors.ContainsKey(proto))
        {
            _styleAccessors.Add(proto, new StyleAccessor(new Stylesheet(rules), proto.Configs));
        }
        else
        {
            // Implicitly calls StyleChanged for subscribers
            _styleAccessors[proto].Update(new Stylesheet(rules), proto.Configs);
        }
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

    /// <inheritdoc/>
    public void PostInject()
    {
        _sawmill = _logManager.GetSawmill("stylesheet");
    }
}
