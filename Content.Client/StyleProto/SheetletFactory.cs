using System.Collections.Frozen;
using Robust.Shared.Reflection;
using Robust.Shared.Utility;

namespace Content.Client.StyleProto;

public sealed partial class SheetletFactory
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
            throw new InvalidOperationException($"Type {typeof(T).Name} is not registered.");

        return _typeFactory.CreateInstance<T>(typeof(T));
    }

    public SheetletConfig GetConfig(string name)
    {
        return _typeFactory.CreateInstance<SheetletConfig>(_configNames[name]);
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

            if (types.ContainsKey(sheetlet))
                throw new InvalidOperationException($"Type is already registered: {sheetlet}");

            var name = CalculateName<ISheetlet>(sheetlet, SheetletSuffix, attribute.Name);

            // Sheetlets are stateless, so we can share one instance across all users.
            var instance = _typeFactory.CreateInstance<ISheetlet>(sheetlet);

            types.Add(sheetlet, instance);
            names.Add(name, instance);
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
            var attribute =
                (SheetletConfigAttribute)Attribute.GetCustomAttribute(config, typeof(SheetletConfigAttribute))!;

            if (types.Contains(config))
                throw new InvalidOperationException($"Type is already registered: {config}");

            var name = CalculateName<SheetletConfig>(config, ConfigSuffix, attribute.Name);

            types.Add(config);
            names.Add(name, config);
        }

        _configNames = names.ToFrozenDictionary();
        _configTypes = types.ToFrozenSet();
    }

    private static string CalculateName<T>(
        Type type,
        string suffix,
        string? nameOverride)
    {
        if (!typeof(T).IsAssignableFrom(type))
        {
            throw new InvalidOperationException(
                $"Type {type} has {typeof(T).Name}'s Attribute but does not implement {typeof(T).Name}.");
        }

        if (!type.Name.EndsWith(suffix))
            throw new InvalidComponentNameException($"{type} must end with the word {suffix}");

        var typeName = type.Name[..^suffix.Length];
        DebugTools.Assert(typeName != string.Empty, $"{type} has invalid name {type.Name}");
        var name = nameOverride ?? typeName;

        return name;
    }
}
