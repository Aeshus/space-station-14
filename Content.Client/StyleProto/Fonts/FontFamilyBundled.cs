using System.Linq;
using Content.Client.Resources;
using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Shared.Utility;
using static Content.Client.StyleProto.Fonts.FontMatchingHelpers;

namespace Content.Client.StyleProto.Fonts;

/// <summary>
/// A font family that is based on bundled font files distributed in Resources.
/// </summary>
[DataDefinition]
public sealed partial class FontFamilyBundled : IFontFamily
{
    [Dependency] private IResourceCache _resCache = default!;

    private Dictionary<(FontWidth, FontSlant, FontWeight), ResPath> _faceCache = new();
    private Dictionary<(ResPath, int), Font> _fontCache = new();

    /// <summary>
    /// Creates a FontFamilyBundled and injects its dependencies.
    /// </summary>
    /// <param name="dependencies">Dependency collection</param>
    /// <param name="name">Name of the font family</param>
    /// <param name="faces">The faces to use</param>
    public FontFamilyBundled(IDependencyCollection dependencies, string name, FontFace[] faces)
    {
        dependencies.InjectDependencies(this);
        Name = name;
        Faces = faces;
    }

    /// <remarks>
    /// Hidden so that the resource cache is always initialized properly.
    /// </remarks>
    private FontFamilyBundled() { }

    /// <summary>
    /// The font faces.
    /// </summary>
    [DataField(required: true)]
    private FontFace[] Faces { get; set; } = [];

    /// <inheritdoc/>
    [DataField(required: true)]
    public string Name { get; private set; } = string.Empty;

    /// <inheritdoc/>
    public float Scale { get; set; } = 1;

    /// <inheritdoc/>
    public Font GetFont(int size,
        FontWidth width = FontWidth.Normal,
        FontSlant slant = FontSlant.Normal,
        FontWeight weight = FontWeight.Normal)
    {
        size = (int)(Scale * size);

        if (!_faceCache.TryGetValue((width, slant, weight), out var path))
        {
            path = GetClosest(width, slant, weight).Path;
            _faceCache.Add((width, slant, weight), path);
        }

        if (!_fontCache.TryGetValue((path, size), out var font))
        {
            font = _resCache.GetFont(path, size);
            _fontCache.Add((path, size), font);
        }

        return font;
    }

    /// <summary>
    /// Gets the closest matching FontFace to the provided parameters.
    /// </summary>
    /// <param name="width">Desired width</param>
    /// <param name="slant">Desired slant</param>
    /// <param name="weight">Desired weight</param>
    /// <returns>Closest FontFace</returns>
    private FontFace GetClosest(FontWidth width, FontSlant slant, FontWeight weight)
    {
        return Faces.OrderBy(f => ClosestWidth(f.Width, width))
            .ThenBy(f => ClosestSlant(f.Slant, slant))
            .ThenBy(f => ClosestWeight(f.Weight, weight))
            .First();
    }
}

/// <summary>
/// A font face with its associated metadata.
/// </summary>
/// <seealso cref="ISystemFontFace"/>
[DataDefinition]
public sealed partial class FontFace
{
    /// <summary>
    /// The font face's width.
    /// </summary>
    [DataField]
    public FontWidth Width { get; set; } = FontWidth.Normal;

    /// <summary>
    /// The font face's slant.
    /// </summary>
    [DataField(required: true)]
    public FontSlant Slant { get; set; } = FontSlant.Normal;

    /// <summary>
    /// The font face's weight.
    /// </summary>
    [DataField(required: true)]
    public FontWeight Weight { get; set; } = FontWeight.Regular;

    /// <summary>
    /// The font face's path.
    /// </summary>
    [DataField(required: true)]
    public ResPath Path { get; set; }
}
