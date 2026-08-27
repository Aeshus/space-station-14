using System.Linq;
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

    /// <summary>
    /// Config type name suffix.
    /// </summary>
    private const string ConfigSuffix = "Config";

    /// <inheritdoc/>
    public ValidationNode Validate(ISerializationManager serializationManager,
        SequenceDataNode node,
        IDependencyCollection dependencies,
        ISerializationContext? context = null)
    {
        var list = new List<ValidationNode>();
        var seen = new HashSet<Type>();
        var types = ConfigTypes();

        foreach (var sequenceEntry in node.Sequence)
        {
            try
            {
                var (type, mapping) = ParseSheetletConfig(types, sequenceEntry);

                if (!seen.Add(type))
                {
                    throw new ArgumentException($"Config {sequenceEntry} is already defined");
                }

                var copy = mapping.Copy();
                copy.Remove("type");

                list.Add(serializationManager.ValidateNode(type, copy, context));
            }
            catch (Exception e)
            {
                list.Add(new ErrorNode(sequenceEntry, e.Message));
            }
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
        var types = ConfigTypes();

        foreach (var sequenceEntry in node.Sequence)
        {
            try
            {
                var (type, mapping) = ParseSheetletConfig(types, sequenceEntry);

                var copy = mapping.Copy();
                copy.Remove("type");

                var config = (SheetletConfig)serializationManager.Read(
                    type,
                    copy,
                    hookCtx,
                    context,
                    notNullableOverride: true)!;

                configs[type] = config;
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }
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

            var name = type.Name;

            if (!name.EndsWith(ConfigSuffix))
                Log.Error($"Config {name} must end with {ConfigSuffix}");

            mapping.Add("type", new ValueDataNode(name[..^ConfigSuffix.Length]));
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

        var c = ToTypeIndexedDictionary(child);
        var p = ToTypeIndexedDictionary(parent);

        foreach (var (parentType, parentIndex) in p)
        {
            if (c.TryGetValue(parentType, out var childIndex))
            {
                result[childIndex] = serializationManager.PushCompositionWithGenericNode(parentType,
                    (MappingDataNode)child[childIndex],
                    (MappingDataNode)parent[parentIndex],
                    context);
                continue;
            }

            result.Add((MappingDataNode)parent[parentIndex]);
        }

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
        target.Configs.Clear();
        target.Configs.EnsureCapacity(source.Configs.Count);

        foreach (var (type, config) in source.Configs)
        {
            var copy = serializationManager.CreateCopy(config, context, notNullableOverride: true);
            target.Configs.Add(type, copy);
        }
    }

    /// <summary>
    /// Converts the sequence node into dictionary mapping types to their index in the sequence.
    /// </summary>
    /// <param name="node">Sequence node</param>
    /// <returns>Dictionary between types and their sequence index</returns>
    private Dictionary<Type, int> ToTypeIndexedDictionary(SequenceDataNode node)
    {
        var result = new Dictionary<Type, int>();
        var types = ConfigTypes();

        for (var i = 0; i < node.Count; i++)
        {
            var sequenceEntry = node[i];

            try
            {
                var (type, _) = ParseSheetletConfig(types, sequenceEntry);
                result.Add(type, i);
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }
        }

        return result;
    }

    /// <summary>
    /// Resolves all configuration types.
    /// </summary>
    /// <returns>A dictionary matching string name to type.</returns>
    private Dictionary<string, Type> ConfigTypes()
    {
        // TODO: sourcegen?
        return _reflectionManager.FindTypesWithAttribute<SheetletConfigAttribute>()
            .ToDictionary(ty => ty.Name);
    }

    /// <summary>
    /// Parses a sheetlet config from a list of types and a DataNode.
    /// </summary>
    /// <param name="types">All valid config types.</param>
    /// <param name="sequenceEntry">DataNode to parse</param>
    /// <returns>Type of the mapping, and the mapping node</returns>
    /// <exception cref="InvalidNodeTypeException">Invalid node type</exception>
    /// <exception cref="KeyNotFoundException">No 'type' found</exception>
    /// <exception cref="TypeAccessException">'type' value not found in dictionary</exception>
    private static (Type, MappingDataNode) ParseSheetletConfig(Dictionary<string, Type> types, DataNode sequenceEntry)
    {
        if (sequenceEntry is not MappingDataNode configMapping)
        {
            throw new InvalidNodeTypeException($"Expected {nameof(MappingDataNode)}");
        }

        if (!configMapping.TryGet("type", out ValueDataNode? typeNode))
        {
            throw new KeyNotFoundException("The given key 'type' was not present in the dictionary.");
        }

        var name = typeNode.Value + ConfigSuffix;

        if (!types.TryGetValue(name, out var type))
        {
            throw new TypeAccessException($"Type {type} was not found");
        }

        return (type, configMapping);
    }
}
