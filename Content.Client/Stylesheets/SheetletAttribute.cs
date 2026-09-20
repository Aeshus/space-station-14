using JetBrains.Annotations;

namespace Content.Client.Stylesheets;

/// <summary>
/// Marks a sheetlet for registration for prototype-based stylesheet generation.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
[BaseTypeRequired(typeof(ISheetlet))]
public sealed class SheetletAttribute : Attribute
{
    /// <summary>
    /// Name to use in (de)serialization rather than the calculated one.
    /// </summary>
    public string? Name;

    /// <summary>
    /// Creates a sheetlet attribute with the default calculated name.
    /// </summary>
    public SheetletAttribute()
    {
    }

    /// <summary>
    /// Creates a sheetlet attribute with a custom name.
    /// </summary>
    /// <param name="name">Name to use instead of calculated</param>
    public SheetletAttribute(string name)
    {
        Name = name;
    }
}
