using Robust.Client.Graphics;

namespace Content.Client.StyleProto.Fonts;

/// <summary>
/// A font family, where multiple faces with different variations (e.g. weights/slants/etc) can be passed and queried
/// from one place.
/// </summary>
public interface IFontFamily
{
    // The ordering of the parameters is to mimic the resolution order used in CSS' font matching algorithm:
    // https://www.w3.org/TR/css-fonts-3/#font-matching-algorithm

    /// <summary>
    /// Gets the font that is the closest to the specified font face variation requested at a given size.
    /// </summary>
    /// <param name="size">The size of the font in points (and scaled by UIScale)</param>
    /// <param name="width">The width of the font (css 'font-stretch')</param>
    /// <param name="slant">The slant of the font (css 'font-style')</param>
    /// <param name="weight">The weight of the font (css 'font-weight')</param>
    /// <returns></returns>
    Font GetFont(int size,
        FontWidth width = FontWidth.Normal,
        FontSlant slant = FontSlant.Normal,
        FontWeight weight = FontWeight.Regular);
}

/// <summary>
/// A font family that is based on bundled font files distributed in Resources.
/// </summary>
public sealed partial class FontFamilyBundled : IFontFamily
{
    /// <inheritdoc/>
    public Font GetFont(int size,
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
public sealed partial class FontFamilySystem : IFontFamily
{
    /// <inheritdoc/>
    public Font GetFont(int size,
        FontWidth width = FontWidth.Normal,
        FontSlant slant = FontSlant.Normal,
        FontWeight weight = FontWeight.Normal)
    {
        throw new NotImplementedException();
    }
}
