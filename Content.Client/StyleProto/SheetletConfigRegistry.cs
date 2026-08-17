namespace Content.Client.StyleProto;

/// <summary>
/// A sheetlet config registry, which provides sheetlets access to concrete instances of configs they request.
/// </summary>
public sealed class SheetletConfigRegistry : Dictionary<string, ISheetletConfig>;
