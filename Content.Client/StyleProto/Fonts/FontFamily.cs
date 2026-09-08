using Robust.Client.Graphics;

namespace Content.Client.StyleProto.Fonts;

public interface IFontFamily
{
    public string Name { get; }
    public FontOptions ClosestOptions(FontWeight weight, FontSlant slant, FontWidth width);
    public Font GetFont(int size, FontOptions options);
}

public sealed class FontFamilyBundled(FontFamilyPrototype prototype) : IFontFamily
{
    public string Name => prototype.ID;

    public FontOptions ClosestOptions(FontWeight weight, FontSlant slant, FontWidth width)
    {
        // Following rules described here: https://www.w3.org/TR/css-fonts-3/#font-style-matching

        // First, try to match it to the closest FontStyle/FontSlant
        if (slant == FontSlant.Oblique)
            slant = FontSlant.Italic;

        if (slant == FontSlant.Italic && prototype.Italic.Length == 0 && prototype.BoldItalic.Length == 0)
            slant = FontSlant.Normal;

        // Then, try to match to the weight

        // Finally, ignore width as engine currently doesn't support that...

        return new FontOptions(weight, slant, width);
    }

    public Font GetFont(int size, FontOptions options)
    {
        throw new NotImplementedException();
    }
}

public sealed class FontFamilySystem : IFontFamily
{
    private ISystemFontFace[] _fonts;

    public string Name
    {
        get => throw new NotImplementedException();
    }

    public FontOptions ClosestOptions(FontWeight weight, FontSlant slant, FontWidth width)
    {
        throw new NotImplementedException();
    }

    public Font GetFont(int size, FontOptions options)
    {
        throw new NotImplementedException();
    }
}
