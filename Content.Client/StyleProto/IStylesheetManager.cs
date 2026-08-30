using System.Diagnostics.CodeAnalysis;
using Content.Client.Stylesheets;
using Robust.Shared.Prototypes;

namespace Content.Client.StyleProto;

/// <summary>
/// A stylesheet manager.
/// </summary>
public interface IStylesheetManager
{
    /// <summary>
    /// Called on all styles being reloaded for mutations to occur.
    /// </summary>
    event Action<SheetletConfigRegistry>? OnStyleReload;

    /// <summary>
    /// Initializes the stylesheet manager.
    /// </summary>
    void Initialize();

    /// <summary>
    /// Reloads all current prototypes,
    /// </summary>
    void ReloadStylesheets();

    /// <summary>
    ///
    /// </summary>
    /// <param name="proto"></param>
    /// <param name="accessor"></param>
    /// <returns></returns>
    bool TryGetStyleSubscription(ProtoId<StylesheetPrototype> proto,
        [NotNullWhen(true)] out StylesheetManager.StyleAccessor? accessor);

    /// <summary>
    ///
    /// </summary>
    /// <param name="proto"></param>
    /// <returns></returns>
    StylesheetManager.StyleAccessor GetStyleSubscription(ProtoId<StylesheetPrototype> proto)
}
