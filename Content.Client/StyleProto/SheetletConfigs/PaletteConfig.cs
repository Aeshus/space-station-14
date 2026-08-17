namespace Content.Client.StyleProto.SheetletConfigs;

[DataDefinition]
[SheetletConfig]
public sealed partial class PaletteConfig : SheetletConfig
{
    [DataField]
    public Color Primary { get; set; }

    [DataField]
    public Color Secondary { get; set; }

    [DataField]
    public Color Tertiary { get; set; }
}
