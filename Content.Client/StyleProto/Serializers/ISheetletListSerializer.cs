using System.Linq;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.Manager;
using Robust.Shared.Serialization.Markdown;
using Robust.Shared.Serialization.Markdown.Sequence;
using Robust.Shared.Serialization.Markdown.Validation;
using Robust.Shared.Serialization.Markdown.Value;
using Robust.Shared.Serialization.TypeSerializers.Interfaces;

namespace Content.Client.StyleProto.Serializers;

/// <summary>
/// (De)serializes sheetlet lists.
/// </summary>
/// <remarks>
/// It mostly just 1) ensures there's no duplicates, and 2) handle inheritance.
/// </remarks>
[TypeSerializer]
public sealed class ISheetletListSerializer : BaseTypeSerializer, ITypeSerializer<List<ISheetlet>, SequenceDataNode>,
    ITypeInheritanceHandler<List<ISheetlet>, SequenceDataNode>
{
    /// <inheritdoc/>
    public SequenceDataNode PushInheritance(ISerializationManager serializationManager,
        SequenceDataNode child,
        SequenceDataNode parent,
        IDependencyCollection dependencies,
        ISerializationContext? context)
    {
        var result = child.Copy();

        foreach (var entry in parent.Reverse())
        {
            if (!result.Contains(entry))
                result.Insert(0, entry.Copy());
        }

        return result;
    }

    /// <inheritdoc/>
    public ValidationNode Validate(ISerializationManager serializationManager,
        SequenceDataNode node,
        IDependencyCollection dependencies,
        ISerializationContext? context = null)
    {
        var lst = new List<ValidationNode>();
        var seen = new HashSet<string>();

        foreach (var entry in node)
        {
            if (entry is not ValueDataNode value)
            {
                lst.Add(new ErrorNode(entry, $"{entry} is not a ValueDataNode"));
                continue;
            }

            if (seen.Contains(value.Value))
            {
                lst.Add(new ErrorNode(entry, $"{entry} is a duplicate value"));
                continue;
            }

            lst.Add(serializationManager.ValidateNode<ISheetlet>(value, context));
            seen.Add(value.Value);
        }

        return new ValidatedSequenceNode(lst);
    }

    /// <inheritdoc/>
    public List<ISheetlet> Read(ISerializationManager serializationManager,
        SequenceDataNode node,
        IDependencyCollection dependencies,
        SerializationHookContext hookCtx,
        ISerializationContext? context = null,
        ISerializationManager.InstantiationDelegate<List<ISheetlet>>? instanceProvider = null)
    {
        var lst = new List<ISheetlet>();

        foreach (var entry in node)
        {
            if (entry is not ValueDataNode)
                throw new InvalidOperationException($"{entry} is not a ValueDataNode");

            lst.Add(serializationManager.Read<ISheetlet>(entry, context, notNullableOverride: true));
        }

        return lst;
    }

    /// <inheritdoc/>
    public DataNode Write(ISerializationManager serializationManager,
        List<ISheetlet> value,
        IDependencyCollection dependencies,
        bool alwaysWrite = false,
        ISerializationContext? context = null)
    {
        var lst = new List<DataNode>();

        foreach (var sheetlet in value)
        {
            serializationManager.WriteValue(sheetlet, alwaysWrite, context, true);
        }

        return new SequenceDataNode(lst);
    }
}
