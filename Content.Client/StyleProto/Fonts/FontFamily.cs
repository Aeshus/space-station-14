using Robust.Client.Graphics;

namespace Content.Client.StyleProto.Fonts;

public interface IFontFamily
{
    string Name { get; }
    FontOptions ClosestOptions(FontWeight weight, FontSlant slant, FontWidth width);
    Font GetFont(int size, FontOptions options);
}

public sealed class FontFamilyBundled(FontFamilyPrototype prototype) : IFontFamily
{
    public string Name => prototype.Name;

    public FontOptions ClosestOptions(FontWeight weight, FontSlant slant, FontWidth width)
    {
        // Following rules described here: https://www.w3.org/TR/css-fonts-3/#font-style-matching

        // Width is not implemented/supported (due to prototype shape)
        width = FontWidth.Normal;
        slant = ClosestSlant(slant);
        weight = ClosestWeight(slant, weight);

        return new FontOptions(weight, slant, width);
    }

    private FontWeight ClosestWeight(FontSlant slant, FontWeight weight)
    {
        if (prototype.Variants[slant].ContainsKey(weight))
            return weight;

        if (weight == FontWeight.Normal)
        {
            if (prototype.Variants[slant].ContainsKey(FontWeight.Medium))
                return FontWeight.Medium;
        }

        if (weight == FontWeight.Medium)
        {
            if (prototype.Variants[slant].ContainsKey(FontWeight.Normal))
                return FontWeight.Normal;
        }

        if (weight <= FontWeight.Normal)
        {
            for (var i = weight; i >= FontWeight.Thin; i--)
            {
                if (prototype.Variants[slant].ContainsKey(i))
                    return i;
            }

            for (var i = weight; i <= FontWeight.ExtraBlack; i++)
            {
                if (prototype.Variants[slant].ContainsKey(i))
                    return i;
            }
        }

        if (weight <= FontWeight.Medium)
        {
            for (var i = weight; i <= FontWeight.ExtraBlack; i++)
            {
                if (prototype.Variants[slant].ContainsKey(i))
                    return i;
            }

            for (var i = weight; i >= FontWeight.Thin; i--)
            {
                if (prototype.Variants[slant].ContainsKey(i))
                    return i;
            }
        }

        throw new InvalidOperationException($"No font weights exist for the slant {slant} on font {prototype.ID}");
    }

    private FontSlant ClosestSlant(FontSlant slant)
    {
        // Oblique -> Italic -> Normal
        // Normal -> Oblique -> Italic
        // Italic -> Oblique -> Normal

        if (slant == FontSlant.Oblique && !prototype.Variants.ContainsKey(FontSlant.Oblique))
        {
            if (prototype.Variants.ContainsKey(FontSlant.Italic))
            {
                slant = FontSlant.Italic;
            }
            else if (prototype.Variants.ContainsKey(FontSlant.Normal))
            {
                slant = FontSlant.Normal;
            }
            else
            {
                throw new InvalidOperationException($"No font slants are defined for font {prototype.ID}");
            }
        }
        else if (slant == FontSlant.Normal && !prototype.Variants.ContainsKey(FontSlant.Normal))
        {
            if (prototype.Variants.ContainsKey(FontSlant.Oblique))
            {
                slant = FontSlant.Oblique;
            }
            else if (prototype.Variants.ContainsKey(FontSlant.Italic))
            {
                slant = FontSlant.Italic;
            }
            else
            {
                throw new InvalidOperationException($"No font slants are defined for font {prototype.ID}");
            }
        }
        else if (slant == FontSlant.Italic && !prototype.Variants.ContainsKey(FontSlant.Italic))
        {
            if (prototype.Variants.ContainsKey(FontSlant.Oblique))
            {
                slant = FontSlant.Oblique;
            }
            else if (prototype.Variants.ContainsKey(FontSlant.Normal))
            {
                slant = FontSlant.Normal;
            }
            else
            {
                throw new InvalidOperationException($"No font slants are defined for font {prototype.ID}");
            }
        }

        return slant;
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
