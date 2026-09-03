using Robust.Shared.Serialization;
using Robust.Shared.Serialization.Manager;
using Robust.Shared.Serialization.Markdown;
using Robust.Shared.Serialization.Markdown.Validation;
using Robust.Shared.Serialization.Markdown.Value;
using Robust.Shared.Serialization.TypeSerializers.Interfaces;

namespace Content.Client.StyleProto;

[TypeSerializer]
public sealed class SheetletSerializer : BaseTypeSerializer, ITypeSerializer<ISheetlet, ValueDataNode>
{
    public ValidationNode Validate(ISerializationManager serializationManager,
        ValueDataNode node,
        IDependencyCollection dependencies,
        ISerializationContext? context = null)
    {
        var factory = dependencies.Resolve<ISheetletFactory>();

        if (!factory.TryGetSheetletType(node.Value, out var type))
        {
            return new ErrorNode(node, $"Unknown sheetlet type '{node.Value}' in prototype!");
        }

        return new ValidatedValueNode(node);
    }

    public ISheetlet Read(ISerializationManager serializationManager,
        ValueDataNode node,
        IDependencyCollection dependencies,
        SerializationHookContext hookCtx,
        ISerializationContext? context = null,
        ISerializationManager.InstantiationDelegate<ISheetlet>? instanceProvider = null)
    {
        var factory = dependencies.Resolve<ISheetletFactory>();
        return factory.GetSheetlet(node.Value);
    }

    public DataNode Write(ISerializationManager serializationManager,
        ISheetlet value,
        IDependencyCollection dependencies,
        bool alwaysWrite = false,
        ISerializationContext? context = null)
    {
        var factory = dependencies.Resolve<ISheetletFactory>();
        if (!factory.TryGetSheetletName(value.GetType(), out var name))
        {
            throw new InvalidOperationException($"{value.GetType()} is not a registered sheetlet");
        }

        return new ValueDataNode(name);
    }
}
