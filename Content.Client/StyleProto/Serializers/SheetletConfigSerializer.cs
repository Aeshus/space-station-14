using Content.Client.Stylesheets;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.Manager;
using Robust.Shared.Serialization.Markdown;
using Robust.Shared.Serialization.Markdown.Mapping;
using Robust.Shared.Serialization.Markdown.Validation;
using Robust.Shared.Serialization.Markdown.Value;
using Robust.Shared.Serialization.TypeSerializers.Interfaces;

namespace Content.Client.StyleProto.Serializers;

/// <summary>
/// (De)Serializes SheetletConfigs.
/// </summary>
[TypeSerializer]
public sealed class SheetletConfigSerializer : BaseTypeSerializer, ITypeSerializer<ISheetletConfig, MappingDataNode>
{
    /// <inheritdoc/>
    public ValidationNode Validate(ISerializationManager serializationManager,
        MappingDataNode node,
        IDependencyCollection dependencies,
        ISerializationContext? context = null)
    {
        var factory = dependencies.Resolve<ISheetletFactory>();

        if (!node.TryGet<ValueDataNode>("type", out var typeNode))
            return new ErrorNode(node, "The given key 'type' is not present in the sheetlet config mapping.");

        if (!factory.TryGetConfigType(typeNode.Value, out var type))
            return new ErrorNode(typeNode, $"Unknown sheetlet config '{typeNode.Value}'");

        // We could also make a private field on the prototype that resolves to type, but that'd waste memory.
        var copy = node.Copy();
        copy.Remove("type");

        return serializationManager.ValidateNode(type, copy, context);
    }

    /// <inheritdoc/>
    public ISheetletConfig Read(ISerializationManager serializationManager,
        MappingDataNode node,
        IDependencyCollection dependencies,
        SerializationHookContext hookCtx,
        ISerializationContext? context = null,
        ISerializationManager.InstantiationDelegate<ISheetletConfig>? instanceProvider = null)
    {
        var factory = dependencies.Resolve<ISheetletFactory>();

        if (!node.TryGet<ValueDataNode>("type", out var typeNode))
            throw new KeyNotFoundException("The given key 'type' is not present in the sheetlet config mapping.");

        if (!factory.TryGetConfigType(typeNode.Value, out var type))
            throw new InvalidOperationException($"Unknown sheetlet config '{typeNode.Value}'");

        var copy = node.Copy();
        copy.Remove("type");

        return (ISheetletConfig)serializationManager.Read(type, copy, context)!;
    }

    /// <inheritdoc/>
    public DataNode Write(ISerializationManager serializationManager,
        ISheetletConfig value,
        IDependencyCollection dependencies,
        bool alwaysWrite = false,
        ISerializationContext? context = null)
    {
        var factory = dependencies.Resolve<ISheetletFactory>();

        var type = value.GetType();

        if (!factory.TryGetConfigName(type, out var name))
            throw new InvalidOperationException($"{type} is not a registered sheetlet config");

        var node = serializationManager.WriteValue(
            type,
            value,
            alwaysWrite,
            context,
            true);

        if (node is not MappingDataNode mapping)
            throw new InvalidNodeTypeException($"{node} is not a mapping data node");

        mapping.Add("type", new ValueDataNode(name));

        return mapping;
    }
}
