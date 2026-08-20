using JetBrains.Annotations;

namespace Content.Client.StyleProto;

/// <summary>
/// A sheetlet config, which holds datafields that can be populated via YAML and then requested and interpreted by sheetlets.
/// </summary>
/// <seealso cref="SheetletConfigAttribute"/>
/// <seealso cref="ISheetlet"/>
[ImplicitDataDefinitionForInheritors]
[UsedImplicitly(ImplicitUseTargetFlags.WithInheritors)]
public abstract partial class SheetletConfig;
