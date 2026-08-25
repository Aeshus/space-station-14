using System.Diagnostics.CodeAnalysis;

namespace Content.Client.StyleProto;

/// <summary>
/// A sheetlet config registry, which provides sheetlets access to concrete instances of configs they request.
/// </summary>
/// <param name="configs">Configs</param>
public sealed class SheetletConfigRegistry(Dictionary<Type, SheetletConfig> configs)
{
    /// <summary>
    /// Concrete configs referenceable by their type.
    /// </summary>
    public readonly Dictionary<Type, SheetletConfig> Configs = configs;

    /// <summary>
    /// Checks if the specified config exists on this registry.
    /// </summary>
    /// <typeparam name="T">Type of the specific config</typeparam>
    /// <returns>True if present, false is not</returns>
    public bool HasConfig<T>()
        where T : SheetletConfig
    {
        return Configs.TryGetValue(typeof(T), out var _config);
    }

    /// <summary>
    /// Gets the specified config from the registry, or throws.
    /// </summary>
    /// <typeparam name="T">Type of the specific config</typeparam>
    /// <returns>Config instance from registry</returns>
    /// <exception cref="KeyNotFoundException">If the config was not found</exception>
    public T GetConfig<T>()
        where T : SheetletConfig
    {
        if (Configs.TryGetValue(typeof(T), out var config))
            return (T)config;

        throw new KeyNotFoundException($"Config {nameof(T)} was not registered.");
    }

    /// <summary>
    /// Gets the specified config from the registry, or returns false.
    /// </summary>
    /// <typeparam name="T">Type of the specific config</typeparam>
    /// <param name="config">Config instance from registry</param>
    /// <returns>True if found, false is not</returns>
    public bool TryGetConfig<T>([NotNullWhen(true)] out T? config)
        where T : SheetletConfig
    {
        config = null;

        if (!Configs.TryGetValue(typeof(T), out var c))
            return false;

        config = (T)c;
        return true;
    }
}
