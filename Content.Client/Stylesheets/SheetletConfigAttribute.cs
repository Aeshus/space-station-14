using JetBrains.Annotations;

namespace Content.Client.Stylesheets;

/// <summary>
/// Marks a sheetlet config for registration for prototype-based stylesheet generation.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
[MeansImplicitUse]
[BaseTypeRequired(typeof(SheetletConfig))]
public sealed class SheetletConfigAttribute : Attribute
{
    /// <summary>
    /// Name to use in (de)serialization rather than the calculated one.
    /// </summary>
    public string? Name;

    /// <summary>
    /// Creates a sheetlet config attribute with the default calculated name.
    /// </summary>
    public SheetletConfigAttribute()
    {
    }

    /// <summary>
    /// Creates a sheetlet config attribute with a custom name.
    /// </summary>
    /// <param name="name">Name to use instead of calculated</param>
    public SheetletConfigAttribute(string name)
    {
        Name = name;
    }
}
