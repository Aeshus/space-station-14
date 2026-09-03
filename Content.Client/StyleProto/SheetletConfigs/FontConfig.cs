using Content.Client.Stylesheets.Fonts;

namespace Content.Client.StyleProto.SheetletConfigs;

[SheetletConfig]
public sealed partial class FontConfig : SheetletConfig
{
    [DataField]
    public List<(string?, int)> CommonFontSizes { get; set; }

    [DataField]
    public FontFamily BaseFont { get; set; }

    [DataField]
    public FontFamily MonoFont { get; set; }

    [DataField]
    public FontFamily DisplayFont { get; set; }

    [DataField]
    public FontFamily DecorativeFont { get; set; }
}
