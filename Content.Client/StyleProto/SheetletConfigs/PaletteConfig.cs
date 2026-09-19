namespace Content.Client.StyleProto.SheetletConfigs;

[SheetletConfig]
public sealed partial class PaletteConfig : SheetletConfig
{
    [DataField(required: true)]
    public Color Primary { get; set; }

    [DataField(required: true)]
    public Color Secondary { get; set; }

    [DataField(required: true)]
    public Color Tertiary { get; set; }
}
