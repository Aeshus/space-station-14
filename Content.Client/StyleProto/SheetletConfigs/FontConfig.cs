using Content.Client.Stylesheets.Fonts;

namespace Content.Client.StyleProto.SheetletConfigs;

[SheetletConfig]
public sealed partial class FontConfig : SheetletConfig
{
    [DataField]
    public List<(string?, int)> CommonFontSizes { get; set; }

    [DataField]
    public FontFamilyStack BaseFont { get; set; }

    [DataField]
    public FontFamilyStack MonoFont { get; set; }

    [DataField]
    public FontFamilyStack DisplayFont { get; set; }

    [DataField]
    public FontFamilyStack DecorativeFont { get; set; }
}
