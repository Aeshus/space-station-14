using Robust.Client.Graphics;
using Robust.Shared.Prototypes;

namespace Content.Client.StyleProto.Fonts;

public sealed partial class FontFamilyResolver : IPostInjectInit
{
    [Dependency] private ILogManager _log = default!;
    [Dependency] private IPrototypeManager _prototypeManager = default!;
    [Dependency] private StylesheetManager _stylesheetManager = default!;

    private Dictionary<FontFamily, Dictionary<FontOptions, Font>> _families = new();
    private Dictionary<FontType, FontFamily> _fontOverrides = new();
    private Dictionary<FontType, float> _fontScales = new();

    private ISawmill _sawmill = default!;

    public void Initialize()
    {
    }

    Font Resolve(
        ProtoId<FontFamilyPrototype> id,
        int size,
        FontWeight weight = FontWeight.Regular,
        FontSlant slant = FontSlant.Normal,
        FontWidth width = FontWidth.Normal)
    {
        var proto = _prototypeManager.Index(id);

        // The settings thing where it'd have a string override name for the override font family and then a
        // bool toggle to use it or the bundled/default instead of mixing the two in one field.

        FontFamily family;
        if (_fontOverrides.TryGetValue(proto.FontType, out var @override))
        {
            family = @override;
        }
        else
        {
            family = new FontFamilyBundled(proto);
        }

        var font = family.GetFont(ScaleFontSize(size, _fontScales[proto.FontType]), options);

        _families[family].Add(options, font);
        return font;
    }

    private int ScaleFontSize(int size, float scale)
    {
        return (int)Math.Round(size * scale);
    }

    public void PostInject()
    {
        _sawmill = _log.GetSawmill("style.font");
        _prototypeManager.PrototypesReloaded += PrototypesReloaded;
    }

    private void PrototypesReloaded(PrototypesReloadedEventArgs obj)
    {
        if (!obj.WasModified<FontFamilyPrototype>())
            return;

        // Invalidate cache
        _families.Clear();

        _stylesheetManager.DirtyAll();
    }
}

// Expand this to handle overrides, so should be one for each dinstinct font usage
public enum FontType
{
    Display,
    Decorative,
    Main,
    Monospace
}

/// <summary>
///
/// </summary>
/// <param name="Weight"></param>
/// <param name="Slant"></param>
/// <param name="Width"></param>
public sealed record FontOptions(
    FontWeight Weight,
    FontSlant Slant,
    FontWidth Width
);
