using Robust.Client.UserInterface;

namespace Content.Client.StyleProto;

/// <summary>
/// Provides access to style properties of a specific style theme, including both its stylesheet and specific configs.
/// </summary>
public interface IStyleAccessor
{
    /// <summary>
    /// The style's stylesheets.
    /// </summary>
    Stylesheet Stylesheet { get; }

    /// <summary>
    /// The style's configs.
    /// </summary>
    SheetletConfigRegistry Configs { get; }

    /// <summary>
    /// A subscription for when the style accessor is modified.
    /// </summary>
    event Action? StyleChanged;

    /// <summary>
    /// Updates the internal stylesheet and configs.
    /// </summary>
    /// <param name="stylesheet"></param>
    /// <param name="configs"></param>
    void Update(Stylesheet stylesheet, SheetletConfigRegistry configs);
}
