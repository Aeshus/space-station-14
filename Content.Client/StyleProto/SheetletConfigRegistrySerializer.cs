using Robust.Shared.Serialization;
using Robust.Shared.Serialization.Manager;
using Robust.Shared.Serialization.Markdown;
using Robust.Shared.Serialization.Markdown.Mapping;
using Robust.Shared.Serialization.Markdown.Sequence;
using Robust.Shared.Serialization.Markdown.Validation;
using Robust.Shared.Serialization.Markdown.Value;
using Robust.Shared.Serialization.TypeSerializers.Interfaces;

namespace Content.Client.StyleProto;

[TypeSerializer]
public sealed partial class SheetletConfigRegistrySerializer : BaseTypeSerializer,
    ITypeSerializer<SheetletConfigRegistry, SequenceDataNode>,
    ITypeInheritanceHandler<SheetletConfigRegistry, SequenceDataNode>, ITypeCopier<SheetletConfigRegistry>
{
    [Dependency] private ISheetletFactory _factory = default!;

    public ValidationNode Validate(ISerializationManager serializationManager,
        SequenceDataNode node,
        IDependencyCollection dependencies,
        ISerializationContext? context = null)
    {
        var list = new List<ValidationNode>();
        var seen = new HashSet<Type>();

        foreach (var entry in node.Sequence)
        {
            if (entry is not MappingDataNode mapping)
            {
                list.Add(new ErrorNode(entry, $"{entry} is not a mapping data node"));
                continue;
            }

            if (!mapping.TryGet<ValueDataNode>("type", out var typeNode))
            {
                list.Add(new ErrorNode(mapping, "Missing sheetlet config type."));
                continue;
            }

            if (!_factory.TryGetConfigType(typeNode.Value, out var type))
            {
                list.Add(new ErrorNode(
                    typeNode,
                    $"Unknown sheetlet config '{typeNode.Value}'."));
                continue;
            }

            if (!seen.Add(type))
            {
                list.Add(new ErrorNode(mapping, "Duplicate Component."));
                continue;
            }

            var copy = mapping.Copy();
            copy.Remove("type");
            list.Add(serializationManager.ValidateNode(type, copy, context));
        }

        return new ValidatedSequenceNode(list);
    }

    public SheetletConfigRegistry Read(ISerializationManager serializationManager,
        SequenceDataNode node,
        IDependencyCollection dependencies,
        SerializationHookContext hookCtx,
        ISerializationContext? context = null,
        ISerializationManager.InstantiationDelegate<SheetletConfigRegistry>? instanceProvider = null)
    {
        var configs = instanceProvider != null ? instanceProvider() : new SheetletConfigRegistry();

        foreach (var entry in node.Sequence)
        {
            if (entry is not MappingDataNode mapping)
            {
                Log.Error($"{entry} is not a mapping data node");
                continue;
            }

            if (!mapping.TryGet<ValueDataNode>("type", out var typeNode))
            {
                Log.Error("Missing sheetlet config type.");
                continue;
            }

            if (!_factory.TryGetConfigType(typeNode.Value, out var type))
            {
                Log.Error($"Unknown sheetlet config '{typeNode.Value}'.");
                continue;
            }

            var copy = mapping.Copy();
            copy.Remove("type");
            var config = serializationManager.Read<SheetletConfig>(
                copy,
                hookCtx,
                context,
                notNullableOverride: true);

            configs[type] = config;
        }

        return configs;
    }

    public DataNode Write(ISerializationManager serializationManager,
        SheetletConfigRegistry value,
        IDependencyCollection dependencies,
        bool alwaysWrite = false,
        ISerializationContext? context = null)
    {
        var sequence = new SequenceDataNode();
        foreach (var (type, config) in value)
        {
            if (!_factory.TryGetConfigName(type, out var name))
            {
                Log.Error($"{type} is not a registered sheetlet config");
                continue;
            }

            var node = serializationManager.WriteValue(
                config,
                alwaysWrite,
                context,
                true);

            if (node is not MappingDataNode mapping)
            {
                Log.Error($"{node} is not a mapping data node");
                continue;
            }

            mapping.Add("type", new ValueDataNode(name));
            sequence.Add(mapping);
        }

        return sequence;
    }

    public SequenceDataNode PushInheritance(ISerializationManager serializationManager,
        SequenceDataNode child,
        SequenceDataNode parent,
        IDependencyCollection dependencies,
        ISerializationContext? context)
    {
        throw new NotImplementedException();
    }

    public void CopyTo(ISerializationManager serializationManager,
        SheetletConfigRegistry source,
        ref SheetletConfigRegistry target,
        IDependencyCollection dependencies,
        SerializationHookContext hookCtx,
        ISerializationContext? context = null)
    {
        throw new NotImplementedException();
    }
}
