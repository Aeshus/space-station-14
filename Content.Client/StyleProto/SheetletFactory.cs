using System.Collections.Frozen;
using Robust.Shared.Reflection;
using Robust.Shared.Utility;

namespace Content.Client.StyleProto;

public sealed partial class SheetletFactory : ISheetletFactory
{
    [Dependency] private IReflectionManager _reflectionManager = default!;
    [Dependency] private IDynamicTypeFactory _typeFactory = default!;

    private FrozenDictionary<string, Type> _configNames
        = FrozenDictionary<string, Type>.Empty;

    private FrozenDictionary<string, ISheetlet> _sheetletNames
        = FrozenDictionary<string, ISheetlet>.Empty;

    private FrozenSet<Type> _configTypes = FrozenSet<Type>.Empty;

    private FrozenDictionary<Type, ISheetlet> _sheetletTypes
        = FrozenDictionary<Type, ISheetlet>.Empty;

    private const string SheetletSuffix = "Sheetlet";
    private const string ConfigSuffix = "Config";

    public void Initialize()
    {
        RegisterSheetlet();
        RegisterConfigs();
    }

    public T GetConfig<T>()
        where T : SheetletConfig
    {
        if (!_configTypes.Contains(typeof(T)))
            throw new ArgumentException($"Type {typeof(T).Name} is not registered.");

        return _typeFactory.CreateInstance<T>(typeof(T));
    }

    public SheetletConfig GetConfig(string name)
    {
        if (!_configNames.TryGetValue(name, out var type))
            throw new ArgumentException($"Config name {name} is not registered.", nameof(name));

        return _typeFactory.CreateInstance<SheetletConfig>(type);
    }

    public T GetSheetlet<T>() where T : ISheetlet
    {
        return (T)_sheetletTypes[typeof(T)];
    }

    public ISheetlet GetSheetlet(string name)
    {
        return _sheetletNames[name];
    }

    private void RegisterSheetlet()
    {
        var sheetlets = _reflectionManager.FindTypesWithAttribute<SheetletAttribute>();

        var names = new Dictionary<string, ISheetlet>();
        var types = new Dictionary<Type, ISheetlet>();

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

            if (!types.TryAdd(sheetlet, instance))
                throw new InvalidOperationException($"Sheetlet type is already registered: {sheetlet}");

            var name = CalculateName(sheetlet, SheetletSuffix, attribute.Name);

            if (!names.TryAdd(name, instance))
                throw new InvalidOperationException($"Sheetlet name is already registered: {name}");
        }

        _sheetletNames = names.ToFrozenDictionary();
        _sheetletTypes = types.ToFrozenDictionary();
    }

    private void RegisterConfigs()
    {
        var configs = _reflectionManager.FindTypesWithAttribute<SheetletConfigAttribute>();

        var names = new Dictionary<string, Type>();
        var types = new HashSet<Type>();

        foreach (var config in configs)
        {
            if (!types.Add(config))
                throw new InvalidOperationException($"Config type is already registered: {config}");

            var attribute =
                (SheetletConfigAttribute)Attribute.GetCustomAttribute(config, typeof(SheetletConfigAttribute))!;

            if (!typeof(SheetletConfig).IsAssignableFrom(config))
            {
                throw new InvalidOperationException(
                    $"Type {config} has {nameof(SheetletConfig)}'s Attribute but does not extend {nameof(SheetletConfig)}.");
            }

            // TODO: add more checking
            var name = CalculateName(config, ConfigSuffix, attribute.Name);

            if (!names.TryAdd(name, config))
                throw new InvalidOperationException($"Config name is already registered: {name}");
        }

        _configNames = names.ToFrozenDictionary();
        _configTypes = types.ToFrozenSet();
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
