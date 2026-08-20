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
    public ResPath[] Regular { get; private set; } = [];

    /// <summary>
    /// Fonts used for <see cref="FontKind.Italic"/>
    /// </summary>
    [DataField]
    public ResPath[] Italic { get; private set; } = [];

    /// <summary>
    /// Fonts used for <see cref="FontKind.Bold"/>
    /// </summary>
    [DataField]
    public ResPath[] Bold { get; private set; } = [];

    /// <summary>
    /// Fonts used for <see cref="FontKind.BoldItalic"/>
    /// </summary>
    [DataField]
    public ResPath[] BoldItalic { get; private set; } = [];

    /// <summary>
    /// Fonts used in for every font kind.
    /// </summary>
    [DataField]
    public ResPath[] Extra { get; private set; } = [];

    /// <summary>
    /// Builds the FontFamilyStack that this prototype represents.
    /// </summary>
    /// <returns>Generated font family stack</returns>
    public FontFamilyStack Build()
    {
        var builder = FontFamilyStack.New();

        builder.AddKind(FontKind.Regular, Regular);

        if (Italic.Length != 0)
            builder.AddKind(FontKind.Italic, Italic);

        if (Bold.Length != 0)
            builder.AddKind(FontKind.Bold, Bold);

        if (BoldItalic.Length != 0)
            builder.AddKind(FontKind.BoldItalic, BoldItalic);

        if (Extra.Length != 0)
            builder.AddExtra(Extra);

        return builder.Build();
    }
}
