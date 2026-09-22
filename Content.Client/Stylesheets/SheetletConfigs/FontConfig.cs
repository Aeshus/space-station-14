using Content.Client.Stylesheets.Fonts;

namespace Content.Client.Stylesheets.SheetletConfigs;

[SheetletConfig]
public sealed partial class FontConfig : SheetletConfig
{
    [DataField]
    public IFontFamily BaseFont { get; set; }
}
