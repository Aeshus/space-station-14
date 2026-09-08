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
    /// The name of the font family.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// The type of font this is, for accessibility overrides.
    /// </summary>
    [DataField(required: true)]
    public FontType FontType { get; private set; }

    [DataField(required: true)]
    public Dictionary<FontSlant, Dictionary<FontWeight, ResPath[]>> Variants = new();
}

/*

- type: FontFamily
  id: NotoSans
  name: Noto Sans
  variants:
  - width: Normal
    weight: Regular
    slant: Italic
    paths:
    - "/Resources/Interface/Font/..."
    - "/Resources/Interface/Font/..."
  - width: Normal
    weight: Bold
    slant: Normal
    paths:
    - "/Resources/Interface/Font/..."
*/
