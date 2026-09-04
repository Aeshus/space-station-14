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
/// Serializes and deserializes Sheetlet Config Registries.
/// </summary>
[TypeSerializer]
public sealed class SheetletConfigRegistrySerializer : BaseTypeSerializer,
    ITypeSerializer<SheetletConfigRegistry, SequenceDataNode>,
    ITypeInheritanceHandler<SheetletConfigRegistry, SequenceDataNode>, ITypeCopier<SheetletConfigRegistry>
{
    /// <inheritdoc/>
    public ValidationNode Validate(ISerializationManager serializationManager,
        SequenceDataNode node,
        IDependencyCollection dependencies,
        ISerializationContext? context = null)
    {
        var list = new List<ValidationNode>();

        try
        {
            var factory = dependencies.Resolve<ISheetletFactory>();
            var dict = TypeToIndexDict(node, factory);
            foreach (var (type, index) in dict)
            {
                var copy = (MappingDataNode)node[index].Copy();
                copy.Remove("type");
                list.Add(serializationManager.ValidateNode(type, copy, context));
            }
        }
        catch (Exception e)
        {
            list.Add(new ErrorNode(node, e.Message));
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
        var configs = instanceProvider != null ? instanceProvider() : new SheetletConfigRegistry();

        var factory = dependencies.Resolve<ISheetletFactory>();
        var dict = TypeToIndexDict(node, factory);

        foreach (var (type, index) in dict)
        {
            var copy = (MappingDataNode)node[index].Copy();

            copy.Remove("type");
            var conf = serializationManager.Read(
                type,
                copy,
                hookCtx,
                context,
                notNullableOverride: true);

            configs[type] = (SheetletConfig)conf!;
        }

        return configs;
    }

    /// <inheritdoc/>
    public DataNode Write(ISerializationManager serializationManager,
        SheetletConfigRegistry value,
        IDependencyCollection dependencies,
        bool alwaysWrite = false,
        ISerializationContext? context = null)
    {
        var sequence = new SequenceDataNode();
        var factory = dependencies.Resolve<ISheetletFactory>();

        foreach (var (type, config) in value)
        {
            if (!factory.TryGetConfigName(type, out var name))
                throw new InvalidOperationException($"{type} is not a registered sheetlet config");

            var node = serializationManager.WriteValue(
                config,
                alwaysWrite,
                context,
                true);

            if (node is not MappingDataNode mapping)
                throw new InvalidNodeTypeException($"{node} is not a mapping data node");

            mapping.Add("type", new ValueDataNode(name));
            sequence.Add(mapping);
        }

        return sequence;
    }

    /// <inheritdoc/>
    public SequenceDataNode PushInheritance(ISerializationManager serializationManager,
        SequenceDataNode child,
        SequenceDataNode parent,
        IDependencyCollection dependencies,
        ISerializationContext? context)
    {
        var sequence = child.Copy();
        var factory = dependencies.Resolve<ISheetletFactory>();

        var childDict = TypeToIndexDict(child, factory);
        var parentDict = TypeToIndexDict(parent, factory);

        foreach (var (type, parentIndex) in parentDict)
        {
            if (childDict.TryGetValue(type, out var childIndex))
            {
                sequence[childIndex] = serializationManager.PushCompositionWithGenericNode(
                    type,
                    parent[parentIndex],
                    child[childIndex],
                    context);
                continue;
            }

            sequence.Add(parent[parentIndex].Copy());
        }

        return sequence;
    }

    /// <inheritdoc/>
    public void CopyTo(ISerializationManager serializationManager,
        SheetletConfigRegistry source,
        ref SheetletConfigRegistry target,
        IDependencyCollection dependencies,
        SerializationHookContext hookCtx,
        ISerializationContext? context = null)
    {
        target.Clear();
        target.EnsureCapacity(source.Count);

        foreach (var (type, config) in source)
        {
            var copy = serializationManager.CreateCopy(
                config,
                hookCtx,
                context,
                notNullableOverride: true);

            target.Add(type, copy);
        }
    }

    /// <summary>
    /// Turns a SequenceNode into a mapping from type to a mapping node.
    /// </summary>
    /// <param name="node">The sequence node</param>
    /// <param name="factory">Factory used to resolve config names</param>
    /// <returns>Mapping from type to its index in the sequence node</returns>
    private static Dictionary<Type, int> TypeToIndexDict(
        SequenceDataNode node,
        ISheetletFactory factory)
    {
        var dict = new Dictionary<Type, int>();
        for (var i = 0; i < node.Count; i++)
        {
            var entry = node[i];

            if (entry is not MappingDataNode mapping)
                throw new InvalidNodeTypeException($"{entry} is not a mapping data node");

            if (!mapping.TryGet<ValueDataNode>("type", out var typeNode))
                throw new KeyNotFoundException("The given key 'type' was not present in the dictionary.");

            if (!factory.TryGetConfigType(typeNode.Value, out var type))
                throw new InvalidOperationException($"Unknown sheetlet config '{typeNode.Value}' in prototype!");

            dict.Add(type, i);
        }

        return dict;
    }
}
