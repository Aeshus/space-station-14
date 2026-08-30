using JetBrains.Annotations;

namespace Content.Client.StyleProto;

/// <summary>
/// Marks a sheetlet for registration for prototype-based stylesheet generation.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
[BaseTypeRequired(typeof(ISheetlet))]
public sealed class SheetletAttribute : Attribute
{
    public string? Name;

    public SheetletAttribute()
    {
    }

    public SheetletAttribute(string name)
    {
        Name = name;
    }
}
