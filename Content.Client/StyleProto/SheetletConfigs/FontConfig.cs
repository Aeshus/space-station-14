using System.Diagnostics.CodeAnalysis;
using Content.Client.StyleProto.Fonts;

namespace Content.Client.StyleProto.SheetletConfigs;

[SheetletConfig]
public sealed partial class FontConfig : SheetletConfig
{
    [DataField("base", required: true)]
    private FontFamilyBundled _base = default!;

    public IFontFamily Base
    {
        get => field ?? _base;
        set;
    }

    [DataField("monospace", required: true)]
    private FontFamilyBundled _monospace = default!;

    public IFontFamily Monospace
    {
        get => field ?? _monospace;
        set;
    }

    [DataField("display", required: true)]
    private FontFamilyBundled _display = default!;

    public IFontFamily Display
    {
        get => field ?? _display;
        set;
    }

    [DataField("decorative", required: true)]
    private FontFamilyBundled _decorative = default!;

    public IFontFamily Decorative
    {
        get => field ?? _decorative;
        set;
    }
}
