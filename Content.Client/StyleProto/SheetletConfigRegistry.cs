using System.Diagnostics.CodeAnalysis;
using Robust.Shared.Utility;

namespace Content.Client.StyleProto;

/// <summary>
/// A sheetlet config registry, which provides sheetlets access to concrete instances of configs they request.
/// </summary>
public sealed class SheetletConfigRegistry : Dictionary<string, SheetletConfig>
{
    /// <summary>
    /// Registers the config with the sheetlet config registry.
    /// </summary>
    /// <param name="config">Concrete config object to save</param>
    /// <typeparam name="T">Type of the config</typeparam>
    public void RegisterConfig<T>(T config)
        where T : SheetletConfig
    {
        Add(CalculateConfigName(config.GetType()), config);
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

        var attempt = TryGetValue(CalculateConfigName(typeof(T)), out var c);

        if (!attempt)
            return false;

        config = (T)c!;
        return true;
    }


    /// <summary>
    /// Calculates the name for the sheetlet config.
    /// </summary>
    /// <param name="type">Type of the sheetlet config</param>
    /// <returns></returns>
    private static string CalculateConfigName(Type type)
    {
        DebugTools.Assert(Attribute.GetCustomAttribute(type, typeof(SheetletConfigAttribute)) != null);

        if (Attribute.GetCustomAttribute(type, typeof(SheetletConfigAttribute)) is SheetletConfigAttribute
            {
                Name: not null,
            } attribute)
            return attribute.Name;

        const string config = "Config";
        var typeName = type.Name;
        if (!typeName.EndsWith(config))
        {
            throw new ArgumentException($"Config {type} must end with the word Config");
        }

        var name = typeName[..^config.Length];
        DebugTools.Assert(name != string.Empty, $"Config {type} has invalid name {type.Name}");

        return name;
    }
}
