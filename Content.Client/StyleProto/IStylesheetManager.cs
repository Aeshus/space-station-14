using System.Diagnostics.CodeAnalysis;
using Robust.Client.UserInterface;
using Robust.Shared.Prototypes;

namespace Content.Client.StyleProto;

public interface IStylesheetManager
{
    event Action<IStylesheetAccessor> StyleChanged;
    void Initialize();
}

public interface IStylesheetAccessor
{
    Stylesheet GetStylesheet(ProtoId<StylesheetPrototype> id);
    bool TryGetStylesheet(ProtoId<StylesheetPrototype> id, [NotNullWhen(true)] out Stylesheet? stylesheet);
    Stylesheet GetStylesheetOrDefault(ProtoId<StylesheetPrototype> id, Stylesheet defaultStylesheet);

    SheetletConfigRegistry GetConfigs(ProtoId<StylesheetPrototype> id);
    bool TryGetConfigs(ProtoId<StylesheetPrototype> id, [NotNullWhen(true)] out Stylesheet? stylesheet);
    SheetletConfigRegistry GetConfigsOrDefault(ProtoId<StylesheetPrototype> id, Stylesheet defaultStylesheet);
}
