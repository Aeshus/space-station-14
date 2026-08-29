using System.Diagnostics.CodeAnalysis;
using Robust.Shared.Prototypes;

namespace Content.Client.StyleProto;

public interface IStylesheetManager
{
    event Action<SheetletConfigRegistry>? OnStyleReload;
    void Initialize();
    void ReloadStylesheets();

    bool TryStyleSubscription(ProtoId<StylesheetPrototype> proto,
        [NotNullWhen(true)] out StylesheetManager.StylesheetAccessor? accessor);
}
