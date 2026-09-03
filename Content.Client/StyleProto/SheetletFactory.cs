using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;
using Robust.Shared.Reflection;
using Robust.Shared.Utility;

namespace Content.Client.StyleProto;

public sealed partial class SheetletFactory : ISheetletFactory
{
    [Dependency] private IReflectionManager _reflectionManager = default!;
    [Dependency] private IDynamicTypeFactory _typeFactory = default!;
    [Dependency] private IDependencyCollection _dependencyCollection = default!;

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

    public void Initialize()
    {
        RegisterSheetlet();
        RegisterConfigs();
    }

    public bool TryGetConfigName(Type type, [NotNullWhen(true)] out string? name)
    {
        return _configTypes.TryGetValue(type, out name);
    }

    public bool TryGetConfigType(string name, [NotNullWhen(true)] out Type? type)
    {
        return _configNames.TryGetValue(name, out type);
    }

    public bool TryGetSheetletName(Type type, [NotNullWhen(true)] out string? name)
    {
        return _sheetletTypes.TryGetValue(type, out name);
    }

    public bool TryGetSheetletType(string name, [NotNullWhen(true)] out Type? type)
    {
        return _sheetletNames.TryGetValue(name, out type);
    }

    public T GetSheetlet<T>() where T : ISheetlet
    {
        if (!_sheetletTypes.ContainsKey(typeof(T)))
            throw new ArgumentException($"Sheetlet type is not registered: {nameof(T)}");

        return (T)_sheetletInstances[typeof(T)];
    }

    public ISheetlet GetSheetlet(string name)
    {
        if (!TryGetSheetletType(name, out var type))
            throw new ArgumentException($"Sheetlet name is not registered: {name}");

        return _sheetletInstances[type];
    }

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

            // TODO: add more checking

            // Sheetlets are stateless, so we can share one instance across all users.
            var instance = _typeFactory.CreateInstance<ISheetlet>(sheetlet);
            _dependencyCollection.InjectDependencies(instance);

            var name = CalculateName(sheetlet, SheetletSuffix, attribute.Name);

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

            // TODO: add more checking
            var name = CalculateName(config, ConfigSuffix, attribute.Name);

            if (!types.TryAdd(config, name))
                throw new InvalidOperationException($"Config type is already registered: {config}");

            if (!names.TryAdd(name, config))
                throw new InvalidOperationException($"Config name is already registered: {name}");
        }

        _configNames = names.ToFrozenDictionary();
        _configTypes = types.ToFrozenDictionary();
    }

    private static string CalculateName(
        Type type,
        string suffix,
        string? nameOverride)
    {
        if (!type.Name.EndsWith(suffix))
            throw new InvalidComponentNameException($"{type} must end with the word {suffix}");

        var typeName = type.Name[..^suffix.Length];
        DebugTools.Assert(typeName != string.Empty, $"{type} has invalid name {type.Name}");
        var name = nameOverride ?? typeName;

        return name;
    }
}
