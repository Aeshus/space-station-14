using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;
using Robust.Shared.Reflection;
using Robust.Shared.Utility;

namespace Content.Client.Stylesheets;

/// <summary>
/// An implementation of the sheetlet factory, which handles registration and creation of <see cref="ISheetlet"/> and
/// <see cref="SheetletConfig"/>.
/// </summary>
/// <seealso cref="ISheetletFactory"/>
public sealed partial class SheetletFactory : ISheetletFactory
{
    [Dependency] private IReflectionManager _reflectionManager = default!;
    [Dependency] private IDynamicTypeFactory _typeFactory = default!;

    private FrozenDictionary<string, Type> _configNames
        = FrozenDictionary<string, Type>.Empty;

    private FrozenDictionary<string, Type> _sheetletNames
        = FrozenDictionary<string, Type>.Empty;

    private FrozenDictionary<Type, string> _configTypes
        = FrozenDictionary<Type, string>.Empty;

    private FrozenDictionary<Type, string> _sheetletTypes
        = FrozenDictionary<Type, string>.Empty;

    private FrozenDictionary<Type, ISheetlet> _sheetletInstances
        = FrozenDictionary<Type, ISheetlet>.Empty;

    private const string SheetletSuffix = "Sheetlet";
    private const string ConfigSuffix = "Config";

    /// <inheritdoc/>
    public void Initialize()
    {
        RegisterSheetlet();
        RegisterConfigs();
    }

    /// <inheritdoc/>
    public bool TryGetConfigName(Type type, [NotNullWhen(true)] out string? name)
    {
        return _configTypes.TryGetValue(type, out name);
    }

    /// <inheritdoc/>
    public ISheetlet GetSheetlet(Type type)
    {
        if (!_sheetletTypes.ContainsKey(type))
            throw new ArgumentException($"Sheetlet type is not registered: {type}");

        return _sheetletInstances[type];
    }

    /// <inheritdoc/>
    public bool TryGetConfigType(string name, [NotNullWhen(true)] out Type? type)
    {
        return _configNames.TryGetValue(name, out type);
    }

    /// <inheritdoc/>
    public bool TryGetSheetletName(Type type, [NotNullWhen(true)] out string? name)
    {
        return _sheetletTypes.TryGetValue(type, out name);
    }

    /// <inheritdoc/>
    public bool TryGetSheetletType(string name, [NotNullWhen(true)] out Type? type)
    {
        return _sheetletNames.TryGetValue(name, out type);
    }

    /// <inheritdoc/>
    public T GetSheetlet<T>() where T : ISheetlet
    {
        if (!_sheetletTypes.ContainsKey(typeof(T)))
            throw new ArgumentException($"Sheetlet type is not registered: {nameof(T)}");

        return (T)_sheetletInstances[typeof(T)];
    }

    /// <summary>
    /// Registers all the sheetlet types via reflection.
    /// </summary>
    /// <exception cref="InvalidOperationException">If a sheetlet can't be registered</exception>
    private void RegisterSheetlet()
    {
        var sheetlets = _reflectionManager.FindTypesWithAttribute<SheetletAttribute>();

        var names = new Dictionary<string, Type>();
        var types = new Dictionary<Type, string>();
        var instances = new Dictionary<Type, ISheetlet>();

        foreach (var sheetlet in sheetlets)
        {
            var attribute = (SheetletAttribute)Attribute.GetCustomAttribute(sheetlet, typeof(SheetletAttribute))!;

            if (!typeof(ISheetlet).IsAssignableFrom(sheetlet))
            {
                throw new InvalidOperationException(
                    $"Type {sheetlet} has {nameof(ISheetlet)}'s Attribute but does not implement {nameof(ISheetlet)}.");
            }

            var name = CalculateName(sheetlet, SheetletSuffix, attribute.Name);

            // Sheetlets are stateless, so we can share one instance across all users.
            var instance = _typeFactory.CreateInstance<ISheetlet>(sheetlet);

            if (!types.TryAdd(sheetlet, name))
                throw new InvalidOperationException($"Sheetlet type is already registered: {sheetlet}");

            if (!names.TryAdd(name, sheetlet))
                throw new InvalidOperationException($"Sheetlet name is already registered: {name}");

            if (!instances.TryAdd(sheetlet, instance))
                throw new InvalidOperationException($"Sheetlet instance is already registered: {name}");
        }

        _sheetletNames = names.ToFrozenDictionary();
        _sheetletTypes = types.ToFrozenDictionary();
        _sheetletInstances = instances.ToFrozenDictionary();
    }

    /// <summary>
    /// Registers all the sheetlet config types via reflection.
    /// </summary>
    /// <exception cref="InvalidOperationException">If a sheetlet config can't be registered</exception>
    private void RegisterConfigs()
    {
        var configs = _reflectionManager.FindTypesWithAttribute<SheetletConfigAttribute>();

        var names = new Dictionary<string, Type>();
        var types = new Dictionary<Type, string>();

        foreach (var config in configs)
        {
            var attribute =
                (SheetletConfigAttribute)Attribute.GetCustomAttribute(config, typeof(SheetletConfigAttribute))!;

            if (!typeof(SheetletConfig).IsAssignableFrom(config))
            {
                throw new InvalidOperationException(
                    $"Type {config} has {nameof(SheetletConfig)}'s Attribute but does not extend {nameof(SheetletConfig)}.");
            }

            var name = CalculateName(config, ConfigSuffix, attribute.Name);

            if (!types.TryAdd(config, name))
                throw new InvalidOperationException($"Config type is already registered: {config}");

            if (!names.TryAdd(name, config))
                throw new InvalidOperationException($"Config name is already registered: {name}");
        }

        _configNames = names.ToFrozenDictionary();
        _configTypes = types.ToFrozenDictionary();
    }

    /// <summary>
    /// Calculates the name of the type (used in serialization) from the type name.
    /// </summary>
    /// <param name="type">The type</param>
    /// <param name="suffix">The suffix for the type (e.g. "Config")</param>
    /// <param name="nameOverride">An overriding name</param>
    /// <returns>The name for this type</returns>
    /// <exception cref="InvalidOperationException">If the type doesn't end with the suffix</exception>
    private static string CalculateName(
        Type type,
        string suffix,
        string? nameOverride)
    {
        if (nameOverride != null)
            return nameOverride;

        if (!type.Name.EndsWith(suffix))
            throw new InvalidOperationException($"{type} must end with the word {suffix}");

        var typeName = type.Name[..^suffix.Length];
        DebugTools.Assert(typeName != string.Empty, $"{type} has invalid name {type.Name}");

        return typeName;
    }
}
