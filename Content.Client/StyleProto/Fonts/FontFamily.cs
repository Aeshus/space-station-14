using System.Collections.Frozen;
using Robust.Client.Graphics;
using Robust.Shared.Utility;

namespace Content.Client.StyleProto.Fonts;

public abstract class FontFamily
{
    public required FrozenDictionary<FontWidth, FrozenDictionary<FontSlant, FrozenDictionary<FontWeight, ResPath[]>>>
        Options
    {
        get;
        set;
    }

    private readonly Dictionary<FontOptions, FontOptions> _optionsCache = new();

    public abstract string Name { get; }

    private FontOptions ClosestOptions(FontOptions options)
    {
        if (_optionsCache.TryGetValue(options, out var cached))
            return cached;

        var width = options.Width;
        var slant = options.Slant;
        var weight = options.Weight;

        width = ClosestWidth(Options, width);
        slant = ClosestSlant(Options[width], slant);
        weight = ClosestWeight(Options[width][slant], weight);

        var closest = new FontOptions(weight, slant, width);
        _optionsCache.Add(options, closest);
        return closest;
    }

    public abstract Font GetFont(int size, FontOptions options);

    private FontWidth ClosestWidth(
        FrozenDictionary<FontWidth, FrozenDictionary<FontSlant, FrozenDictionary<FontWeight, ResPath[]>>> choices,
        FontWidth width)
    {
        // https://www.w3.org/TR/css-fonts-3/#font-style-matching
        if (choices.ContainsKey(width))
            return width;

        FontWidth? smaller = null;
        FontWidth? bigger = null;

        foreach (var choice in choices.Keys)
        {
            if (choice < width)
            {
                if (smaller is null || choice > smaller)
                    smaller = choice;
            }
            else if (bigger is null || choice < bigger)
            {
                bigger = choice;
            }
        }

        var closest = width <= FontWidth.Normal
            ? smaller ?? bigger
            : bigger ?? smaller;

        return closest ?? throw new InvalidOperationException($"No font width found for font {Name}");
    }

    private FontSlant ClosestSlant(FrozenDictionary<FontSlant, FrozenDictionary<FontWeight, ResPath[]>> choices,
        FontSlant slant)
    {
        // https://www.w3.org/TR/css-fonts-3/#font-style-matching
        if (choices.ContainsKey(slant))
            return slant;

        if (slant == FontSlant.Italic)
        {
            if (choices.ContainsKey(FontSlant.Oblique))
                return FontSlant.Oblique;

            if (choices.ContainsKey(FontSlant.Normal))
                return FontSlant.Normal;
        }

        if (slant == FontSlant.Normal)
        {
            if (choices.ContainsKey(FontSlant.Oblique))
                return FontSlant.Oblique;

            if (choices.ContainsKey(FontSlant.Italic))
                return FontSlant.Italic;
        }

        if (slant == FontSlant.Oblique)
        {
            if (choices.ContainsKey(FontSlant.Italic))
                return FontSlant.Italic;

            if (choices.ContainsKey(FontSlant.Normal))
                return FontSlant.Normal;
        }

        throw new InvalidOperationException($"Not font slant found for font {Name}");
    }

    private FontWeight ClosestWeight(
        FrozenDictionary<FontWeight, ResPath[]> choices,
        FontWeight weight)
    {
        // https://www.w3.org/TR/css-fonts-4/#font-style-matching
        if (choices.ContainsKey(weight))
            return weight;

        FontWeight? smaller = null;
        FontWeight? bigger = null;

        foreach (var choice in choices.Keys)
        {
            if (choice < weight)
            {
                if (smaller is null || choice > smaller)
                    smaller = choice;
            }
            else if (bigger is null || choice < bigger)
            {
                bigger = choice;
            }
        }

        // CSS4 wraps weights between 400 and 500 first to the largest <500 before going back to normal.
        if (weight is >= FontWeight.Regular and <= FontWeight.Medium &&
            bigger is <= FontWeight.Medium)
        {
            return bigger.Value;
        }

        var closest = weight <= FontWeight.Medium
            ? smaller ?? bigger
            : bigger ?? smaller;

        return closest ?? throw new InvalidOperationException($"No font weight found for font {Name}");
    }
}

public sealed class FontFamilyBundled : FontFamily
{
    private BundledFontFace[] _faces;
    Dictionary<FontOptions, Font> _instanceCache = new();
    private readonly FontFamilyPrototype _prototype;

    public FontFamilyBundled(FontFamilyPrototype prototype)
    {
        _prototype = prototype;
        _faces = [.. prototype.Variants];

        throw new NotImplementedException();
    }

    public override string Name => _prototype.Name;

    public override Font GetFont(int size, FontOptions options)
    {
        return new DummyFont();
    }
}

public sealed class FontFamilySystem : FontFamily
{
    // var faces = _systemFontManager.SystemFontFaces.GroupBy(s =>
    //             s.GetLocalizedFamilyName(CultureInfo.InvariantCulture));
    private ISystemFontFace[] _fonts;

    public override string Name
    {
        get => _fonts[0].FamilyName;
    }

    public override Font GetFont(int size, FontOptions options)
    {
        // For all closest fonts that make sense
    }
}
