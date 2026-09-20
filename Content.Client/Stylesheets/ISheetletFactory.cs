using System.Diagnostics.CodeAnalysis;

namespace Content.Client.Stylesheets;

/// <summary>
/// Factory that manages the resolution and creation of sheetlets and sheetlet configs.
/// </summary>
/// <seealso cref="SheetletFactory"/>
/// <seealso cref="ISheetlet"/>
/// <seealso cref="SheetletConfig"/>
public interface ISheetletFactory
{
    /// <summary>
    /// Initializes the ISheetletFactory.
    /// </summary>
    void Initialize();

    /// <summary>
    /// Gets the sheetlet matching the type.
    /// </summary>
    /// <typeparam name="T">Sheetlet to get</typeparam>
    /// <exception cref="ArgumentException">If the sheetlet is not found</exception>
    /// <returns>The sheetlet</returns>
    T GetSheetlet<T>() where T : ISheetlet;

    /// <summary>
    /// Gets the sheetlet matching the type.
    /// </summary>
    /// <param name="type">Sheetlet type to get</param>
    /// <exception cref="ArgumentException">If the sheetlet is not found</exception>
    /// <returns>The sheetlet</returns>
    ISheetlet GetSheetlet(Type type);

    /// <summary>
    /// Tries to get the config type matching the name.
    /// </summary>
    /// <param name="name">The name of the config</param>
    /// <param name="type">The type of the config</param>
    /// <returns>True if the type was found, false is not</returns>
    bool TryGetConfigType(string name, [NotNullWhen(true)] out Type? type);

    /// <summary>
    /// Tries to get the sheetlet type matching the name.
    /// </summary>
    /// <param name="name">The name of the sheetlet</param>
    /// <param name="type">The type of the sheetlet</param>
    /// <returns>True if the type was found, false is not</returns>
    bool TryGetSheetletType(string name, [NotNullWhen(true)] out Type? type);

    /// <summary>
    /// Tries to get the config name matching the type.
    /// </summary>
    /// <param name="type">The type of the sheetlet</param>
    /// <param name="name">The name of the sheetlet</param>
    /// <returns>True if the name was found, false is not</returns>
    bool TryGetConfigName(Type type, [NotNullWhen(true)] out string? name);

    /// <summary>
    /// Tries to get the sheetlet name matching the type.
    /// </summary>
    /// <param name="type">The type of the sheetlet</param>
    /// <param name="name">The name of the sheetlet</param>
    /// <returns>True if the name was found, false is not</returns>
    bool TryGetSheetletName(Type type, [NotNullWhen(true)] out string? name);
}
