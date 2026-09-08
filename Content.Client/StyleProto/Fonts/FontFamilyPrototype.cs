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
    [DataField(required: true)]
    public string Name { get; private set; }

    /// <summary>
    /// The type of font this is, for accessibility overrides.
    /// </summary>
    [DataField(required: true)]
    public FontType FontType { get; private set; }

    [DataField(required: true)]
    public List<BundledFontFace> Variants = [];
}

[DataDefinition]
public sealed partial class BundledFontFace
{
    /// <summary>
    /// The weight of the font face.
    /// </summary>
    [DataField]
    public FontWeight Weight { get; private set; } = FontWeight.Regular;

    /// <summary>
    /// The slant of the font face.
    /// </summary>
    [DataField]
    public FontSlant Slant { get; private set; } = FontSlant.Normal;

    /// <summary>
    /// The width of the font face.
    /// </summary>
    [DataField]
    public FontWidth Width { get; private set; } = FontWidth.Normal;

    /// <summary>
    /// The paths for the stacked font.
    /// </summary>
    [DataField(required: true)]
    public List<ResPath> Paths;
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
