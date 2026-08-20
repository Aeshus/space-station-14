using Robust.Shared.Prototypes;
using Robust.Shared.Utility;
using Content.Client.Stylesheets.Fonts;

namespace Content.Client.StyleProto;

/// <summary>
/// Prototype representing a font family stack, which combines multiple font files into a single FontFamilyStack interface.
/// </summary>
[Prototype]
public sealed partial class FontFamilyPrototype : IPrototype
{
    /// <inheritdoc/>
    [IdDataField]
    public string ID { get; private set; }

    /// <summary>
    /// Fonts used for <see cref="FontKind.Regular"/>
    /// </summary>
    [DataField(required: true)]
    public ResPath[] Regular;

    /// <summary>
    /// Fonts used for <see cref="FontKind.Italic"/>
    /// </summary>
    [DataField(required: true)]
    public ResPath[] Italic;

    /// <summary>
    /// Fonts used for <see cref="FontKind.Bold"/>
    /// </summary>
    [DataField(required: true)]
    public ResPath[] Bold;

    /// <summary>
    /// Fonts used for <see cref="FontKind.BoldItalic"/>
    /// </summary>
    [DataField(required: true)]
    public ResPath[] BoldItalic;

    /// <summary>
    /// Fonts used in for every font kind.
    /// </summary>
    [DataField]
    public ResPath[] Extra;
}
