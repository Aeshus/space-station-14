using Robust.Client.Graphics;

namespace Content.Client.Stylesheets.Fonts;

/// <summary>
/// A font family, where multiple faces with different variations (e.g. weights/slants/etc) can be passed and queried
/// from one place.
/// </summary>
public interface IFontFamily
{
    /// <summary>
    /// The font name
    /// </summary>
    string Name { get; }

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
    /// <param name="weight">The weight of the font (css 'font-weight')</param>
    /// <param name="slant">The slant of the font (css 'font-style')</param>
    /// <param name="width">The width of the font (css 'font-stretch')</param>
    /// <returns>The font (face) associated with this family</returns>
    Font GetFont(int size,
        FontWeight weight = FontWeight.Regular,
        FontSlant slant = FontSlant.Normal,
        FontWidth width = FontWidth.Normal);
}
