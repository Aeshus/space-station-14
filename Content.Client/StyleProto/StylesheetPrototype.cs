using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Array;

namespace Content.Client.StyleProto;

/// <summary>
/// Prototype representing a sheetlet-powered stylesheet.
/// </summary>
[Prototype]
public sealed partial class StylesheetPrototype : IPrototype, IInheritingPrototype
{
    /// <inheritdoc/>
    [IdDataField]
    public string ID { get; private set; }

    /// <inheritdoc/>
    [ParentDataField(typeof(AbstractPrototypeIdArraySerializer<StylesheetPrototype>))]
    public string[]? Parents { get; private set; }

    /// <inheritdoc/>
    [NeverPushInheritance]
    [AbstractDataField]
    public bool Abstract { get; private set; }

    /// <summary>
    /// The sheetlet configs.
    /// </summary>
    [AlwaysPushInheritance]
    [DataField(required: true)]
    public SheetletConfigRegistry Configs;

    /// <summary>
    /// The sheetlet names that will be used to generate a stylesheet using the sheetlet config registry.
    /// </summary>
    [AlwaysPushInheritance]
    [DataField(required: true, customTypeSerializer: typeof(SheetletListSerializer))]
    public List<ISheetlet> Sheetlets;
}
