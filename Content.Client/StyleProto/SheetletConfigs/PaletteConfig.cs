using Content.Client.Stylesheets.Palette;

namespace Content.Client.StyleProto.SheetletConfigs;

[SheetletConfig]
public sealed partial class PaletteConfig : SheetletConfig
{
    [DataField]
    public ColorPalette PrimaryPalette { get; set; }

    [DataField]
    public ColorPalette SecondaryPalette { get; set; }

    [DataField]
    public ColorPalette PositivePalette { get; set; }

    [DataField]
    public ColorPalette NegativePalette { get; set; }

    [DataField]
    public ColorPalette HighlightPalette { get; set; }
}
