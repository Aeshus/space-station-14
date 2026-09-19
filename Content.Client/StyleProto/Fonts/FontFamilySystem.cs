using System.Linq;
using Robust.Client.Graphics;
using static Content.Client.StyleProto.Fonts.FontMatchingHelpers;

namespace Content.Client.StyleProto.Fonts;

/// <summary>
/// A font family that is built on font faces provided by the operating system.
/// </summary>
public sealed class FontFamilySystem : IFontFamily
{
    private readonly IFontFamily _fallback;
    private readonly ISystemFontFace[] _faces;

    private Dictionary<(FontWidth, FontSlant, FontWeight), ISystemFontFace> _faceCache = new();
    private Dictionary<(ISystemFontFace, int), Font> _fontCache = new();

    /// <inheritdoc/>
    public string Name { get; }

    /// <inheritdoc/>
    public float Scale { get; set; } = 1;

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

        Name = _faces[0].FamilyName;

        // This makes it safe for an ordering s.t. the font scale is mutated _before_ the font is overridden with
        // its system variant.
        Scale = _fallback.Scale;

        // We rely on the scale being default for the fallback so the stacked font looks correct.
        _fallback.Scale = 1;
    }

    /// <inheritdoc/>
    public Font GetFont(int size,
        FontWidth width = FontWidth.Normal,
        FontSlant slant = FontSlant.Normal,
        FontWeight weight = FontWeight.Normal)
    {
        size = (int)(Scale * size);

        if (!_faceCache.TryGetValue((width, slant, weight), out var face))
        {
            face = GetClosest(width, slant, weight);
            _faceCache.Add((width, slant, weight), face);
        }

        if (!_fontCache.TryGetValue((face, size), out var font))
        {
            font = new StackedFont(face.Load(size), _fallback.GetFont(size));
            _fontCache.Add((face, size), font);
        }

        return font;
    }

    /// <summary>
    /// Gets the closest matching FontFace to the provided parameters.
    /// </summary>
    /// <param name="width">Desired width</param>
    /// <param name="slant">Desired slant</param>
    /// <param name="weight">Desired weight</param>
    /// <returns>Closest ISystemFontFace</returns>
    private ISystemFontFace GetClosest(FontWidth width, FontSlant slant, FontWeight weight)
    {
        return _faces.OrderBy(f => ClosestWidth(f.Width, width))
            .ThenBy(f => ClosestSlant(f.Slant, slant))
            .ThenBy(f => ClosestWeight(f.Weight, weight))
            .First();
    }
}
