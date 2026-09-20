using System.Diagnostics.CodeAnalysis;
using Robust.Shared.Prototypes;

namespace Content.Client.Stylesheets;

public interface IStylesheetManager
{
    /// <summary>
    /// An event that gets invoked whenever the Stylesheets are reloaded.
    /// </summary>
    /// <remarks>
    /// This is used for mutating Sheetlet Configs. Note, it is in subscription order.
    /// </remarks>
    event Action<SheetletConfigRegistry>? OnStyleReload;

    /// <summary>
    /// Initialize the StylesheetManager.
    /// </summary>
    void Initialize();

    /// <summary>
    /// Dirties all the Stylesheets so that they are reloaded/rebuilt.
    /// </summary>
    /// <remarks>
    /// Deleted prototypes and failed rebuilds retain their last successfully built stylesheet and subscriptions.
    /// </remarks>
    void DirtyAll();

    /// <summary>
    /// Dirty a specific Stylesheet so it is reloaded/rebuilt.
    /// </summary>
    /// <remarks>
    /// If the prototype no longer exists, its cached stylesheet is left unchanged.
    /// </remarks>
    /// <param name="proto">The stylesheet prototype</param>
    void Dirty(ProtoId<StylesheetPrototype> proto);

    /// <summary>
    /// Tries to get a stylesheet subscription from a prototype.
    /// </summary>
    /// <param name="proto">Stylesheet prototype</param>
    /// <param name="accessor">An accessor which contains an event to subscribe to</param>
    /// <returns>True if the accessor is found, False if null</returns>
    bool TryGetStyleSubscription(ProtoId<StylesheetPrototype> proto,
        [NotNullWhen(true)] out StylesheetManager.IStyleAccessor? accessor);

    /// <summary>
    /// Gets the style subscription with the prototype.
    /// </summary>
    /// <param name="proto">Stylesheet prototype</param>
    /// <returns>The accessor</returns>
    StylesheetManager.IStyleAccessor GetStyleSubscription(ProtoId<StylesheetPrototype> proto);
}
