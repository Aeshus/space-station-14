using Robust.Shared.Serialization;
using Robust.Shared.Serialization.Manager;
using Robust.Shared.Serialization.Markdown;
using Robust.Shared.Serialization.Markdown.Mapping;
using Robust.Shared.Serialization.Markdown.Sequence;
using Robust.Shared.Serialization.Markdown.Validation;
using Robust.Shared.Serialization.Markdown.Value;
using Robust.Shared.Serialization.TypeSerializers.Interfaces;

namespace Content.Client.Stylesheets.Serializers;

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
        list.EnsureCapacity(node.Count);

        foreach (var entry in node)
        {
            list.Add(serializationManager.ValidateNode<SheetletConfig>(entry, context));
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
        configs.EnsureCapacity(node.Count);

        foreach (var entry in node)
        {
            var data = serializationManager.Read<SheetletConfig>(entry, context, notNullableOverride: true);
            configs.Add(data.GetType(), data);
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

        foreach (var config in value.Values)
        {
            sequence.Add(serializationManager.WriteValue(config, notNullableOverride: true));
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
