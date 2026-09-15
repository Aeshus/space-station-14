using Content.Client.StyleProto.Fonts;

namespace Content.Client.StyleProto.SheetletConfigs;

[SheetletConfig]
public sealed partial class FontConfig : SheetletConfig
{
    [DataField(required: true)]
    public IFontFamily Base { get; set; } = default!;

    [DataField(required: true)]
    public IFontFamily Monospace { get; set; } = default!;

    [DataField(required: true)]
    public IFontFamily Display { get; set; } = default!;

    [DataField(required: true)]
    public IFontFamily Decorative { get; set; } = default!;
}
