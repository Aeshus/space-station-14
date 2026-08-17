using System.Diagnostics.CodeAnalysis;

namespace Content.Client.StyleProto;

/// <summary>
/// A sheetlet config registry, which provides sheetlets access to concrete instances of configs they request.
/// </summary>
public sealed class SheetletConfigRegistry : Dictionary<string, ISheetletConfig>
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="name"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public bool TryGetConfig(string name, [NotNullWhen(true)] ISheetletConfig? config)
    {
        return TryGetValue(name, out config);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="config"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public bool TryGetConfig<T>([NotNullWhen(true)] ISheetletConfig? config)
    {
        return TryGetConfig(typeof(T), config);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="type"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public bool TryGetConfig(Type type, [NotNullWhen(true)] ISheetletConfig? config)
    {
        return TryGetConfig(CalculateConfigName(type), config);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private static string CalculateConfigName(Type type)
    {
        return string.Empty;
    }
}
