using Robust.Client.Graphics;
using Robust.Shared.Utility;

namespace Content.Client.StyleProto.Fonts;

/// <summary>
/// A font family, where multiple faces with different variations (e.g. weights/slants/etc) can be passed and queried
/// from one place.
/// </summary>
public interface IFontFamily
{
    /// <summary>
    /// The font scale that is multiplied against all font sizes used.
    /// </summary>
    float Scale { get; set; }

    // The ordering of the parameters is to mimic the resolution order used in CSS' font matching algorithm:
    // https://www.w3.org/TR/css-fonts-3/#font-matching-algorithm

    /// <summary>
    /// Gets the font that is the closest to the specified font face variation requested at a given size.
    /// </summary>
    /// <param name="size">The size of the font in points (and scaled by <see cref="Scale"/> + UIScale)</param>
    /// <param name="width">The width of the font (css 'font-stretch')</param>
    /// <param name="slant">The slant of the font (css 'font-style')</param>
    /// <param name="weight">The weight of the font (css 'font-weight')</param>
    /// <returns>The font (face) associated with this family</returns>
    Font GetFont(int size,
        FontWidth width = FontWidth.Normal,
        FontSlant slant = FontSlant.Normal,
        FontWeight weight = FontWeight.Regular);
}

public abstract class FontFamily : IFontFamily
{
    public abstract float Scale { get; set; }

    public abstract Font GetFont(int size,
        FontWidth width = FontWidth.Normal,
        FontSlant slant = FontSlant.Normal,
        FontWeight weight = FontWeight.Normal);

    protected record struct FontFace
    {
        public FontWidth Width;
        public FontSlant Slant;
        public FontWeight Weight;
    }
}

/// <summary>
/// A font family that is based on bundled font files distributed in Resources.
/// </summary>
public sealed partial class FontFamilyBundled : FontFamily
{
    /// <inheritdoc/>
    public override float Scale { get; set; } = 1;

    /// <inheritdoc/>
    public override Font GetFont(int size,
        FontWidth width = FontWidth.Normal,
        FontSlant slant = FontSlant.Normal,
        FontWeight weight = FontWeight.Normal)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// A font family that is built on font faces provided by the operating system.
/// </summary>
public sealed partial class FontFamilySystem : FontFamily
{
    private readonly IFontFamily _fallback;
    private readonly ISystemFontFace[] _faces;

    /// <inheritdoc/>
    public override float Scale { get; set; } = 1;

    /// <summary>
    /// Creates a FontFamilySystem font with a fallback font.
    /// </summary>
    /// <remarks>
    /// The fallback doesn't extend the supported font face variants, it only adds more characters for missing ones.
    /// </remarks>
    /// <param name="fallback">Default fallback</param>
    /// <param name="faces">System font faces to use</param>
    public FontFamilySystem(IFontFamily fallback, ISystemFontFace[] faces)
    {
        _fallback = fallback;
        _faces = faces;

        // This makes it safe for an ordering s.t. the font scale is mutated _before_ the font is overridden with
        // its system variant.
        Scale = _fallback.Scale;

        // We rely on the scale being default for the fallback so the stacked font looks correct.
        _fallback.Scale = 1;
    }

    /// <inheritdoc/>
    public override Font GetFont(int size,
        FontWidth width = FontWidth.Normal,
        FontSlant slant = FontSlant.Normal,
        FontWeight weight = FontWeight.Normal)
    {
        var scale = (int)(Scale * size);

        return new StackedFont(_faces[0].Load(0), _fallback.GetFont(scale));
    }
}
