using System.Collections.Frozen;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Robust.Client.UserInterface;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Client.StyleProto;

public sealed partial class StylesheetManager : IPostInjectInit, IStylesheetManager
{
    [Dependency] private IPrototypeManager _prototypeManager = default!;
    [Dependency] private ILogManager _logManager = default!;

    private Dictionary<ProtoId<StylesheetPrototype>, StyleAccessor> _styleAccessors = [];
    private ISawmill _sawmill = default!;

    public event Action<SheetletConfigRegistry>? OnStyleReload;

    public void Initialize()
    {
        ReloadStylesheets();

        _prototypeManager.PrototypesReloaded += OnPrototypesReloaded;
    }

    private void OnPrototypesReloaded(PrototypesReloadedEventArgs eventArgs)
    {
        if (!eventArgs.WasModified<StylesheetPrototype>())
            return;

        ReloadStylesheets();
    }

    public void ReloadStylesheets()
    {
        foreach (var proto in _prototypeManager.EnumeratePrototypes<StylesheetPrototype>())
        {
            // Let subscribers mutate configs before loading
            OnStyleReload?.Invoke(proto.Configs);

            var rules = new List<StyleRule>();
            foreach (var sheetlet in proto.Sheetlets)
            {
                rules.AddRange(sheetlet.Generate(proto.Configs));
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
    }

    public bool TryGetStyleSubscription(ProtoId<StylesheetPrototype> proto,
        [NotNullWhen(true)] out StyleAccessor? accessor)
    {
        return _styleAccessors.TryGetValue(proto, out accessor);
    }

    public StyleAccessor GetStyleSubscription(ProtoId<StylesheetPrototype> proto)
    {
        return _styleAccessors[proto];
    }


    /// <inheritdoc/>
    public sealed class StyleAccessor(Stylesheet stylesheet, SheetletConfigRegistry configs) : IStyleAccessor
    {
        /// <inheritdoc/>
        public Stylesheet Stylesheet { get; private set; } = stylesheet;

        /// <inheritdoc/>
        public SheetletConfigRegistry Configs { get; private set; } = configs;

        /// <inheritdoc/>
        public event Action? StyleChanged;

        /// <inheritdoc/>
        public void Update(Stylesheet stylesheet, SheetletConfigRegistry configs)
        {
            Stylesheet = stylesheet;
            Configs = configs;

            StyleChanged?.Invoke();
        }
    }

    /// <inheritdoc/>
    public void PostInject()
    {
        _sawmill = _logManager.GetSawmill("stylesheet");
    }
}
