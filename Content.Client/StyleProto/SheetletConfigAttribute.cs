using JetBrains.Annotations;

namespace Content.Client.StyleProto;

/// <summary>
/// Marks a sheetlet config for registration for prototype-based stylesheet generation.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
[BaseTypeRequired(typeof(ISheetletConfig))]
public sealed class SheetletConfigAttribute : Attribute
{
    /// <summary>
    /// Overriding config name.
    /// </summary>
    public string? Name;

    /// <summary>
    /// Marks a sheetlet config for registration with an overriding name.
    /// </summary>
    /// <param name="name">Overriding config name</param>
    public SheetletConfigAttribute(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Marks a sheetlet config for registration.
    /// </summary>
    public SheetletConfigAttribute() { }
}
