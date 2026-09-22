using Content.Client.Stylesheets.Fonts;

namespace Content.Client.Stylesheets.SheetletConfigs;

[SheetletConfig]
public sealed partial class FontConfig : SheetletConfig
{
    [DataField]
    public IFontFamily Main { get; set; }

    [DataField]
    public IFontFamily Monospace { get; set; }

    [DataField]
    public IFontFamily Decorative { get; set; }
}
