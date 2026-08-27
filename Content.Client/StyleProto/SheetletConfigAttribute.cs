using JetBrains.Annotations;

namespace Content.Client.StyleProto;

/// <summary>
/// Marks a sheetlet config for registration for prototype-based stylesheet generation.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
[BaseTypeRequired(typeof(SheetletConfig))]
public sealed class SheetletConfigAttribute : Attribute;
