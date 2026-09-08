using Content.Client.Stylesheets.Fonts;
using Robust.Client.Graphics;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Client.StyleProto.Fonts;

[Prototype]
public sealed partial class FontFamilyPrototype : IPrototype
{
    /// <inheritdoc/>
    [IdDataField]
    public string ID { get; private set; }

    /// <summary>
    /// The type of font this is, for accessability overrides.
    /// </summary>
    [DataField(required: true)]
    public FontType FontType { get; private set; }

    [DataField(required: true)]
    public Dictionary<FontSlant, Dictionary<FontWeight, ResPath[]>> Fonts = new();
}
