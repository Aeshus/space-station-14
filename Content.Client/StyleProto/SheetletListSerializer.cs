using System.Linq;
using Robust.Shared.Physics;
using Robust.Shared.Serialization.Manager;
using Robust.Shared.Serialization.Markdown.Sequence;
using Robust.Shared.Serialization.Markdown.Validation;
using Robust.Shared.Serialization.Markdown.Value;
using Robust.Shared.Serialization.TypeSerializers.Interfaces;

namespace Content.Client.StyleProto;

[TypeSerializer]
public sealed class SheetletListSerializer : BaseTypeSerializer, ITypeValidator<List<ISheetlet>, SequenceDataNode>,
    ITypeInheritanceHandler<List<ISheetlet>, SequenceDataNode>
{
    public ValidationNode Validate(ISerializationManager serializationManager,
        SequenceDataNode node,
        IDependencyCollection dependencies,
        ISerializationContext? context = null)
    {
        var list = new List<ValidationNode>();
        var seen = new HashSet<string>();

        foreach (var entry in node)
        {
            if (entry is not ValueDataNode value)
            {
                list.Add(new ErrorNode(entry, $"{entry} is not a mapping data node"));
                continue;
            }

            if (!seen.Add(value.Value))
            {
                list.Add(new ErrorNode(entry, $"Duplicate value {value.Value}"));
                continue;
            }

            list.Add(serializationManager.ValidateNode<ISheetlet>(value, context));
        }

        return new ValidatedSequenceNode(list);
    }

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
}
