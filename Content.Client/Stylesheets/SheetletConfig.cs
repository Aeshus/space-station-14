namespace Content.Client.Stylesheets;

/// <summary>
/// A sheetlet config, which holds <see cref="DataFieldAttribute"/> that can be populated via YAML and then requested
/// and interpreted by sheetlets/c#.
/// </summary>
/// <seealso cref="SheetletConfigAttribute"/>
/// <seealso cref="ISheetlet"/>
[ImplicitDataDefinitionForInheritors]
public abstract partial class SheetletConfig;
