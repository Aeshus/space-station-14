namespace Content.Client.StyleProto.SheetletConfigs;

[SheetletConfig]
public sealed partial class NewPaletteConfig : PaletteConfig
{
    [DataField]
    public Color Quantary { get; set; }
}
