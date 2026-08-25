using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Robust.Shared.Prototypes;
using Robust.Shared.Reflection;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.Manager;
using Robust.Shared.Serialization.Markdown;
using Robust.Shared.Serialization.Markdown.Mapping;
using Robust.Shared.Serialization.Markdown.Sequence;
using Robust.Shared.Serialization.Markdown.Validation;
using Robust.Shared.Serialization.Markdown.Value;
using Robust.Shared.Serialization.TypeSerializers.Interfaces;

namespace Content.Client.StyleProto;

/// <summary>
/// (De)serializes YAML to/from SheetletConfigRegistries
/// </summary>
[TypeSerializer]
public sealed partial class SheetletConfigRegistrySerializer : BaseTypeSerializer,
    ITypeSerializer<SheetletConfigRegistry, SequenceDataNode>,
    ITypeInheritanceHandler<SheetletConfigRegistry, SequenceDataNode>,
    ITypeCopier<SheetletConfigRegistry>
{
    [Dependency] private IReflectionManager _reflectionManager = default!;
    [Dependency] private IDynamicTypeFactory _dynamicTypeFactory = default!;

    private const string ConfigSuffix = "Config";

    /// <inheritdoc/>
    public ValidationNode Validate(ISerializationManager serializationManager,
        SequenceDataNode node,
        IDependencyCollection dependencies,
        ISerializationContext? context = null)
    {
        var list = new List<ValidationNode>();
        var seen = new HashSet<string>();
        var validTypes = _reflectionManager.FindTypesWithAttribute<SheetletConfigAttribute>()
            .ToDictionary(ty => ty.Name);

        foreach (var sequenceEntry in node.Sequence)
        {
            if (sequenceEntry is not MappingDataNode configMapping)
            {
                list.Add(new ErrorNode(sequenceEntry, $"Expected {nameof(MappingDataNode)}"));
                continue;
            }

            var name = ((ValueDataNode)configMapping.Get("type")).Value + ConfigSuffix;

            if (!validTypes.TryGetValue(name, out var type))
            {
                list.Add(new ErrorNode(configMapping,
                    $"Unknown type {name} (may not have proper attribute)"));
                continue;
            }

            if (!seen.Add(name))
            {
                list.Add(new ErrorNode(configMapping, "Duplicate sheetlet config."));
                continue;
            }

            var copy = configMapping.Copy();
            copy.Remove("type");
            list.Add(serializationManager.ValidateNode(type, copy, context));
        }

        return new ValidatedSequenceNode(list);
    }

    /// <inheritdoc/>
    public SheetletConfigRegistry Read(ISerializationManager serializationManager,
        SequenceDataNode node,
        IDependencyCollection dependencies,
        SerializationHookContext hookCtx,
        ISerializationContext? context = null,
        ISerializationManager.InstantiationDelegate<SheetletConfigRegistry>? instanceProvider = null)
    {
        var configs = new Dictionary<Type, SheetletConfig>();
        var validTypes = _reflectionManager.FindTypesWithAttribute<SheetletConfigAttribute>()
            .ToDictionary(ty => ty.Name);

        foreach (var sequenceEntry in node.Sequence)
        {
            if (sequenceEntry is not MappingDataNode configMapping)
            {
                throw new InvalidCastException($"Expected {nameof(MappingDataNode)}");
            }

            var name = ((ValueDataNode)configMapping.Get("type")).Value + ConfigSuffix;

            if (!validTypes.TryGetValue(name, out var type))
            {
                Log.Error($"Unknown config {name} (may not have proper attribute)");
                continue;
            }

            var copy = configMapping.Copy();
            copy.Remove("type");

            var config = (SheetletConfig)serializationManager.Read(
                type,
                copy,
                hookCtx,
                context,
                notNullableOverride: true)!;

            configs[type] = config;
        }

        return new SheetletConfigRegistry(configs);
    }

    /// <inheritdoc/>
    public DataNode Write(ISerializationManager serializationManager,
        SheetletConfigRegistry value,
        IDependencyCollection dependencies,
        bool alwaysWrite = false,
        ISerializationContext? context = null)
    {
        var configSequence = new SequenceDataNode();
        foreach (var (type, config) in value.Configs)
        {
            var node = serializationManager.WriteValue(
                type,
                config,
                alwaysWrite,
                context);

            if (node is not MappingDataNode mapping)
                throw new InvalidNodeTypeException();

            mapping.Add("type", new ValueDataNode(type.Name));
            configSequence.Add(mapping);
        }

        return configSequence;
    }

    /// <inheritdoc/>
    public SequenceDataNode PushInheritance(ISerializationManager serializationManager,
        SequenceDataNode child,
        SequenceDataNode parent,
        IDependencyCollection dependencies,
        ISerializationContext? context)
    {
        var result = child.Copy();

        return result;
    }

    /// <inheritdoc/>
    public void CopyTo(ISerializationManager serializationManager,
        SheetletConfigRegistry source,
        ref SheetletConfigRegistry target,
        IDependencyCollection dependencies,
        SerializationHookContext hookCtx,
        ISerializationContext? context = null)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Tries to get the config type based on the string name.
    /// </summary>
    /// <param name="name">Name (excluding "Config")</param>
    /// <param name="type">Type</param>
    /// <returns></returns>
    private bool TryGetConfigType(string name, [NotNullWhen(true)] Type? type)
    {
        return
    }
}
