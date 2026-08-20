using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Client.StyleProto;

[Prototype]
public sealed partial class FontFamilyPrototype : IPrototype
{
    [IdDataField]
    public string ID { get; private set; }

    [DataField(required: true)]
    public ResPath[] Regular;

    [DataField(required: true)]
    public ResPath[] Italic;

    [DataField(required: true)]
    public ResPath[] Bold;

    [DataField(required: true)]
    public ResPath[] BoldItalic;

    [DataField]
    public ResPath[] Extra;
}
