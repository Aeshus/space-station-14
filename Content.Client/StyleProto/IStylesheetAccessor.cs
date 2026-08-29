using Robust.Client.UserInterface;

namespace Content.Client.StyleProto;

public interface IStylesheetAccessor
{
    Stylesheet Stylesheet { get; }
    SheetletConfigRegistry Configs { get; }
    event Action? StyleChanged;
    void Update(Stylesheet stylesheet, SheetletConfigRegistry configs);
}
