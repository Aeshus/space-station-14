using Content.Client.StyleProto.Fonts;

namespace Content.Client.StyleProto.SheetletConfigs;

[SheetletConfig]
public sealed partial class FontConfig : SheetletConfig
{
    [DataField(required: true)]
    public IFontFamily Base { get; private set; }

    [DataField(required: true)]
    public IFontFamily Monospace { get; private set; }

    [DataField(required: true)]
    public IFontFamily Display { get; private set; }

    [DataField(required: true)]
    public IFontFamily Decorative { get; private set; }
}
