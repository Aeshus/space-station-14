using System.Diagnostics.CodeAnalysis;

namespace Content.Client.StyleProto;

/// <summary>
/// Factory that manages the resolution and creation of sheetlets and sheetlet configs.
/// </summary>
/// <seealso cref="ISheetlet"/>
/// <seealso cref="SheetletConfig"/>
public interface ISheetletFactory
{
    /// <summary>
    /// Initializes the ISheetletFactory.
    /// </summary>
    void Initialize();

    /// <summary>
    /// Gets the sheetlet matching the
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    T GetSheetlet<T>() where T : ISheetlet;
    bool TryGetConfigType(string name, [NotNullWhen(true)] out Type? type);
    bool TryGetSheetletType(string name, [NotNullWhen(true)] out Type? type);
    bool TryGetConfigName(Type type, [NotNullWhen(true)] out string? name);
    public bool TryGetSheetletName(Type type, [NotNullWhen(true)] out string? name);

    ISheetlet GetSheetlet(string name);
}
