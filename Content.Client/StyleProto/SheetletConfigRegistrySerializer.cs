using Robust.Shared.Serialization;
using Robust.Shared.Serialization.Manager;
using Robust.Shared.Serialization.Markdown;
using Robust.Shared.Serialization.Markdown.Sequence;
using Robust.Shared.Serialization.Markdown.Validation;
using Robust.Shared.Serialization.TypeSerializers.Interfaces;

namespace Content.Client.StyleProto;

[TypeSerializer]
public sealed class SheetletConfigRegistrySerializer : BaseTypeSerializer,
    ITypeSerializer<SheetletConfigRegistry, SequenceDataNode>,
    ITypeInheritanceHandler<SheetletConfigRegistry, SequenceDataNode>, ITypeCopier<SheetletConfigRegistry>
{
    public ValidationNode Validate(ISerializationManager serializationManager,
        SequenceDataNode node,
        IDependencyCollection dependencies,
        ISerializationContext? context = null)
    {
        throw new NotImplementedException();
    }

    public SheetletConfigRegistry Read(ISerializationManager serializationManager,
        SequenceDataNode node,
        IDependencyCollection dependencies,
        SerializationHookContext hookCtx,
        ISerializationContext? context = null,
        ISerializationManager.InstantiationDelegate<SheetletConfigRegistry>? instanceProvider = null)
    {
        throw new NotImplementedException();
    }

    public DataNode Write(ISerializationManager serializationManager,
        SheetletConfigRegistry value,
        IDependencyCollection dependencies,
        bool alwaysWrite = false,
        ISerializationContext? context = null)
    {
        throw new NotImplementedException();
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
