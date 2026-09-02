using System.Diagnostics.CodeAnalysis;

namespace Content.Client.StyleProto;

public interface ISheetletFactory
{
    void Initialize();

    T GetConfig<T>()
        where T : SheetletConfig;

    SheetletConfig GetConfig(string name);
    T GetSheetlet<T>() where T : ISheetlet;
    bool TryGetConfigType(string name, [NotNullWhen(true)] out Type? type);
    ISheetlet GetSheetlet(string name);
}
