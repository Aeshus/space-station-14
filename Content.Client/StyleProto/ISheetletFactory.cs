using System.Diagnostics.CodeAnalysis;

namespace Content.Client.StyleProto;

public interface ISheetletFactory
{
    void Initialize();

    T GetSheetlet<T>() where T : ISheetlet;
    bool TryGetConfigType(string name, [NotNullWhen(true)] out Type? type);
    bool TryGetSheetletType(string name, [NotNullWhen(true)] out Type? type);
    bool TryGetConfigName(Type type, [NotNullWhen(true)] out string? name);
    public bool TryGetSheetletName(Type type, [NotNullWhen(true)] out string? name);

    ISheetlet GetSheetlet(string name);
}
