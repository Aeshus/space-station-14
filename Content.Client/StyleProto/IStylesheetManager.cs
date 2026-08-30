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
    void DirtyAll();

    /// <summary>
    /// Tries to get the style subscription for the provided prototype.
    /// </summary>
    /// <param name="proto">Stylesheet ProtoId</param>
    /// <param name="accessor">Accessor, not null if true, null if false</param>
    /// <returns>True means accessor is not null, false means is null</returns>
    bool TryGetStyleSubscription(ProtoId<StylesheetPrototype> proto,
        [NotNullWhen(true)] out StylesheetManager.IStyleAccessor? accessor);

    /// <summary>
    /// Gets the style subscription for the provided prototype.
    /// </summary>
    /// <param name="proto">Stylesheet ProtoID</param>
    /// <returns>Accessor</returns>
    StylesheetManager.IStyleAccessor GetStyleSubscription(ProtoId<StylesheetPrototype> proto);
}
