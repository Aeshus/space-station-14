using System.Collections.Frozen;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Robust.Client.UserInterface;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Client.StyleProto;

public sealed class StylesheetManager : IStylesheetManager, IPostInjectInit
{
    [Dependency] private IPrototypeManager _prototype = default!;
    [Dependency] private ILogManager _log = default!;

    private ISawmill _sawmill = default!;

    FrozenDictionary<ProtoId<StylesheetPrototype>, Stylesheet> _stylesheets = default!;
    FrozenDictionary<ProtoId<StylesheetPrototype>, SheetletConfigRegistry> _configs = default!;

    private readonly StylesheetAccessorImpl _accessor;
    private readonly List<Action<IStylesheetAccessor>> _subscriptions = [];

    public event Action<IStylesheetAccessor> StyleChanged
    {
        add
        {
            DebugTools.Assert(!_subscriptions.Contains(value), "Attempted to subscribe the same style action twice.");
            _subscriptions.Add(value);

            try
            {
                value(_accessor);
            }
            catch (Exception e)
            {
                _sawmill.Error($"Caught exception while updating styles on controls! {e}");
            }
        }
        remove
        {
            DebugTools.Assert(_subscriptions.Contains(value),
                "Attempted to unsubscribe from a style action that was not subscribed.");
            _subscriptions.Remove(value);
        }
    }

    private bool _initialized;

    public StylesheetManager()
    {
        _accessor = new StylesheetAccessorImpl(this);
    }

    public void Initialize()
    {
        _sawmill.Debug("Initializing Stylesheets...");
        var sw = Stopwatch.StartNew();

        LoadPrototypes();

        _initialized = true;
    }

    public void Dirty()
    {
        throw new NotImplementedException();
    }

    public void PostInject()
    {
        _sawmill = _log.GetSawmill("loc");
        _prototype.PrototypesReloaded += OnPrototypesReloaded;
    }

    private void OnPrototypesReloaded(PrototypesReloadedEventArgs _)
    {
        LoadPrototypes();
    }

    private void LoadPrototypes()
    {
        var stylesheets = new Dictionary<ProtoId<StylesheetPrototype>, Stylesheet>();
        var configs = new Dictionary<ProtoId<StylesheetPrototype>, SheetletConfigRegistry>();

        foreach (var proto in _prototype.EnumeratePrototypes<StylesheetPrototype>())
        {
            // Don't try to override/merge prototypes as that will override modifications made, which we don't want.

            // I'm not really sure how to handle runtime modifications tbh... I wonder if I should expose functionality
            // s.t. subscribers are notified on reloaded stylesheet prototypes so they can re-add modifications (mostly
            // for things like color blindness and fonts?)

            // This would be useful for adding new themes as well so that I can
            if (_stylesheets.TryGetValue(proto.ID, out var sheet) && _configs.TryGetValue(proto.ID, out var config))
            {
                stylesheets.Add(proto.ID, sheet);
                configs.Add(proto.ID, config);
                continue;
            }

            // TODO: implement building rules
            stylesheets.Add(proto.ID, new Stylesheet([]));
            configs.Add(proto.ID, proto.Configs);
        }

        _stylesheets = stylesheets.ToFrozenDictionary();
        _configs = configs.ToFrozenDictionary();

        _initialized = true;

        UpdateStyles();
    }

    /// <summary>
    /// Updates all controls that have subscribed to style changes.
    /// </summary>
    private void UpdateStyles()
    {
        foreach (var sub in _subscriptions)
        {
            try
            {
                sub.Invoke(_accessor);
            }
            catch (Exception e)
            {
                _sawmill.Error($"Caught exception while updating styles on controls! {e}");
            }
        }
    }


    private sealed class StylesheetAccessorImpl(StylesheetManager owner) : IStylesheetAccessor
    {
        public Stylesheet GetStylesheet(ProtoId<StylesheetPrototype> id)
        {
            return TryGetStylesheet(id, out var stylesheet)
                ? stylesheet
                : throw new KeyNotFoundException($"Stylesheet {id} was not found!");
        }

        public bool TryGetStylesheet(ProtoId<StylesheetPrototype> id, [NotNullWhen(true)] out Stylesheet? stylesheet)
        {
            return !owner._initialized
                ? throw new InvalidOperationException("Stylesheets not initialized yet!")
                : owner._stylesheets.TryGetValue(id, out stylesheet);
        }

        public SheetletConfigRegistry GetConfigs(ProtoId<StylesheetPrototype> id)
        {
            return TryGetConfigs(id, out var config)
                ? config
                : throw new KeyNotFoundException($"Config {id} was not found!");
        }

        public bool TryGetConfigs(ProtoId<StylesheetPrototype> id,
            [NotNullWhen(true)] out SheetletConfigRegistry? stylesheet)
        {
            return !owner._initialized
                ? throw new InvalidOperationException("Stylesheets not initialized yet!")
                : owner._configs.TryGetValue(id, out stylesheet);
        }
    }
}
