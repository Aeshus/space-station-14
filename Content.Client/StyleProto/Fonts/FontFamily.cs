using Robust.Client.Graphics;

namespace Content.Client.StyleProto.Fonts;

public interface IFontFamily
{
    string Name { get; }
    Font GetFont(int size, FontOptions options);
}

public sealed class FontFamilyBundled(FontFamilyPrototype prototype) : IFontFamily
{
    Dictionary<FontOptions, FontOptions> _optionsCache = new();
    Dictionary<FontOptions, Font> _instanceCache = new();

    public string Name => prototype.Name;

    private FontOptions ClosestOptions(FontWeight weight, FontSlant slant, FontWidth width)
    {
        var options = new FontOptions(weight, slant, width);

        if (_optionsCache.TryGetValue(options, out var cached))
            return cached;

        // Width is not implemented/supported (due to prototype shape)
        width = FontWidth.Normal;
        slant = ClosestSlant(slant);
        weight = ClosestWeight(slant, weight);

        var closest = new FontOptions(weight, slant, width);
        _optionsCache.Add(options, closest);
        return closest;
    }

    private FontWeight ClosestWeight(FontSlant slant, FontWeight weight)
    {
        // Following rules described here: https://www.w3.org/TR/css-fonts-3/#font-style-matching
        if (prototype.Variants[slant].ContainsKey(weight))
            return weight;

        if (weight == FontWeight.Regular)
        {
            if (prototype.Variants[slant].ContainsKey(FontWeight.Medium))
                return FontWeight.Medium;
        }

        if (weight == FontWeight.Medium)
        {
            if (prototype.Variants[slant].ContainsKey(FontWeight.Regular))
                return FontWeight.Regular;
        }

        if (weight <= FontWeight.Regular)
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
        // Following rules described here: https://www.w3.org/TR/css-fonts-3/#font-style-matching

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
    // var faces = _systemFontManager.SystemFontFaces.GroupBy(s =>
    //             s.GetLocalizedFamilyName(CultureInfo.InvariantCulture));
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
        // For all closest fonts that make sense
    }
}
