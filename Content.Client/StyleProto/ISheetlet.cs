using Robust.Client.UserInterface;

namespace Content.Client.StyleProto;

/// <summary>
/// A sheetlet that generates specific style rules after being provided the appropriate sheetlet configs.
/// </summary>
/// <seealso cref="SheetletConfig"/>
public interface ISheetlet
{
    /// <summary>
    /// Generates the style rules for this sheetlet.
    /// </summary>
    /// <param name="configs">Configuration registry</param>
    /// <returns>Generates rules, or an empty array if config requirements not met.</returns>
    StyleRule[]? Generate(SheetletConfigRegistry configs);
}
