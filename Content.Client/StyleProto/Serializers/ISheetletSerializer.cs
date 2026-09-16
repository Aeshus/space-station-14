using Robust.Shared.Serialization;
using Robust.Shared.Serialization.Manager;
using Robust.Shared.Serialization.Markdown;
using Robust.Shared.Serialization.Markdown.Validation;
using Robust.Shared.Serialization.Markdown.Value;
using Robust.Shared.Serialization.TypeSerializers.Interfaces;

namespace Content.Client.StyleProto.Serializers;

/// <summary>
/// (De)Serializes ISheetlets interfaces into instances of their actual types.
/// </summary>
[TypeSerializer]
public sealed class ISheetletSerializer : BaseTypeSerializer, ITypeSerializer<ISheetlet, ValueDataNode>
{
    /// <inheritdoc/>
    public ValidationNode Validate(ISerializationManager serializationManager,
        ValueDataNode node,
        IDependencyCollection dependencies,
        ISerializationContext? context = null)
    {
        var factory = dependencies.Resolve<ISheetletFactory>();

        return !factory.TryGetSheetletType(node.Value, out _)
            ? throw new InvalidOperationException($"{node.Value} is not a registered sheetlet")
            : new ValidatedValueNode(node);
    }

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public DataNode Write(ISerializationManager serializationManager,
        ISheetlet value,
        IDependencyCollection dependencies,
        bool alwaysWrite = false,
        ISerializationContext? context = null)
    {
        var factory = dependencies.Resolve<ISheetletFactory>();

        return !factory.TryGetSheetletName(value.GetType(), out var name)
            ? throw new InvalidOperationException($"{value} is not a registered sheetlet")
            : new ValueDataNode(name);
    }
}
