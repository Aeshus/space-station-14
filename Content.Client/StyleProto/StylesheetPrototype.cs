using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Array;

namespace Content.Client.StyleProto;

[Prototype]
public sealed partial class StylesheetPrototype : IPrototype, IInheritingPrototype
{
    [IdDataField]
    public string ID { get; private set; }

    [ViewVariables]
    [ParentDataField(typeof(AbstractPrototypeIdArraySerializer<StylesheetPrototype>))]
    public string[]? Parents { get; private set; }

    [ViewVariables]
    [NeverPushInheritance]
    [AbstractDataField]
    public bool Abstract { get; private set; }

    [AlwaysPushInheritance]
    [DataField(required: true)]
    public SheetletConfigRegistry Configs = new();

    //[AlwaysPushInheritance]
    //[DataField(required: true)]
    //public SheetletRegistry Sheetlets = new();
}
