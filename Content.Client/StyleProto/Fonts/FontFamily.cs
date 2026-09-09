using System.Collections.Frozen;
using Robust.Client.Graphics;
using Robust.Shared.Utility;

namespace Content.Client.StyleProto.Fonts;

public abstract class IFontFamily
{
    private FrozenDictionary<FontWidth, FrozenDictionary<FontSlant, FontWeight>> choices;
    private Dictionary<FontOptions, ResPath[]> _fontsPaths;
    private Dictionary<FontOptions, Font> _fontsCache = new();
    private Dictionary<FontOptions, FontOptions> _optionsCache = new();

    public abstract string Name { get; }
    public abstract Font GetFont(int size, FontOptions options);

    private FontOptions ClosestOptions(FontWeight weight, FontSlant slant, FontWidth width)
    {
        var options = new FontOptions(weight, slant, width);

        if (_optionsCache.TryGetValue(options, out var cached))
            return cached;

        width = ClosestWidth(choices, width);
        slant = ClosestSlant(choices[width], slant);
        weight = ClosestWeight(choices[width][slant], weight);

        var closest = new FontOptions(weight, slant, width);
        _optionsCache.Add(options, closest);
        return closest;
    }

    private FontWidth ClosestWidth(FrozenDictionary<FontWidth, FrozenDictionary<FontSlant, FontWeight>> choices,
        FontWidth width)
    {
        // https://www.w3.org/TR/css-fonts-3/#font-style-matching

        if (width is < FontWidth.UltraCondensed or > FontWidth.UltraExpanded)
            throw new ArgumentOutOfRangeException(nameof(width));

        if (choices.ContainsKey(width))
            return width;

        // If the value of ‘font-stretch’ is ‘normal’ or one of the condensed values, narrower width values are
        // checked first, then wider values.
        if (width <= FontWidth.Normal)
        {
            for (var i = width - 1; i >= FontWidth.UltraCondensed; i--)
            {
                if (choices.ContainsKey(i))
                    return i;
            }

            for (var i = width + 1; i <= FontWidth.UltraExpanded; i++)
            {
                if (choices.ContainsKey(i))
                    return i;
            }
        }
        else
        {
            // If the value of ‘font-stretch’ is one of the expanded values, wider values are checked first, followed
            // by narrower values.
            for (var i = width + 1; i <= FontWidth.UltraExpanded; i++)
            {
                if (choices.ContainsKey(i))
                    return i;
            }

            for (var i = width - 1; i >= FontWidth.UltraCondensed; i--)
            {
                if (choices.ContainsKey(i))
                    return i;
            }
        }

        throw new InvalidOperationException($"No font widths are defined for font {Name}");
    }
}

public sealed class FontFamilyBundled(FontFamilyPrototype prototype) : IFontFamily
{
    private BundledFontFace[] _faces = [.. prototype.Variants];

    Dictionary<FontOptions, FontOptions> _optionsCache = new();
    Dictionary<FontOptions, Font> _instanceCache = new();

    public override string Name => prototype.Name;

    public override Font GetFont(int size, FontOptions options)
    {
        throw new NotImplementedException();
    }
}

public sealed class FontFamilySystem : IFontFamily
{
    // var faces = _systemFontManager.SystemFontFaces.GroupBy(s =>
    //             s.GetLocalizedFamilyName(CultureInfo.InvariantCulture));
    private ISystemFontFace[] _fonts;

    public override string Name
    {
        get => throw new NotImplementedException();
    }

    public FontOptions ClosestOptions(FontWeight weight, FontSlant slant, FontWidth width)
    {
        throw new NotImplementedException();
    }

    public override Font GetFont(int size, FontOptions options)
    {
        // For all closest fonts that make sense
    }
}
