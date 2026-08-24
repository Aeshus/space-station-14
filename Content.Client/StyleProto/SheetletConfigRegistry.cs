using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;
using Robust.Shared.Utility;

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
    private Dictionary<Type, SheetletConfig> _configs = configs;

    /// <summary>
    /// Removes the config from the registry.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public bool RemoveConfig<T>()
        where T : SheetletConfig
    {
        return _configs.Remove(typeof(T));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="config"></param>
    /// <typeparam name="T"></typeparam>
    public void AddConfig<T>(T config)
        where T : SheetletConfig
    {
        _configs.Add(typeof(T), config);
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public bool HasConfig<T>()
        where T : SheetletConfig
    {
        return _configs.TryGetValue(typeof(T), out var config);
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
        if (_configs.TryGetValue(typeof(T), out var config))
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

        if (!_configs.TryGetValue(typeof(T), out var c))
            return false;

        config = (T)c;
        return true;
    }
}
