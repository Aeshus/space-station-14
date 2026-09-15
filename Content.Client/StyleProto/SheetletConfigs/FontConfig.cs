using Content.Client.StyleProto.Fonts;

namespace Content.Client.StyleProto.SheetletConfigs;

// Why?
//
// Essentially, there's no way to serialize interfaces to my understanding, due to its ambitious nature.
//
// The problem is that here, we only actually want to (de)serialize between a singular subtype of the interface,
// FontFamilyBundled, and we don't want to (de)serialize from a FontFamilySystem as that's brittle.
// Thus, we can do this hack (which is "kinda" seen in ConstructionGraphNode) so that we can write it in YAML.

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
