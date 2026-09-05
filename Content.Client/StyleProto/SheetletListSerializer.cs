using System.Linq;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.Manager;
using Robust.Shared.Serialization.Markdown;
using Robust.Shared.Serialization.Markdown.Sequence;
using Robust.Shared.Serialization.Markdown.Validation;
using Robust.Shared.Serialization.Markdown.Value;
using Robust.Shared.Serialization.TypeSerializers.Interfaces;

namespace Content.Client.StyleProto;

[TypeSerializer]
public sealed class SheetletListSerializer : BaseTypeSerializer,
    ITypeInheritanceHandler<List<ISheetlet>, SequenceDataNode>, ITypeSerializer<List<ISheetlet>, SequenceDataNode>
{
    public DataNode Write(ISerializationManager serializationManager,
        List<ISheetlet> value,
        IDependencyCollection dependencies,
        bool alwaysWrite = false,
        ISerializationContext? context = null)
    {
        var factory = dependencies.Resolve<ISheetletFactory>();
        var sequence = new SequenceDataNode(value.Count);

        foreach (var sheetlet in value)
        {
            if (!factory.TryGetSheetletName(sheetlet.GetType(), out var name))
            {
                throw new InvalidOperationException($"{value.GetType()} is not a registered sheetlet");
            }

            sequence.Add(new ValueDataNode(name));
        }

        return sequence;
    }

    public ValidationNode Validate(ISerializationManager serializationManager,
        SequenceDataNode node,
        IDependencyCollection dependencies,
        ISerializationContext? context = null)
    {
        var list = new List<ValidationNode>();
        var seen = new HashSet<string>();
        var factory = dependencies.Resolve<ISheetletFactory>();

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

            if (!factory.TryGetSheetletType(value.Value, out _))
            {
                return new ErrorNode(node, $"Unknown sheetlet type '{value.Value}' in prototype!");
            }

            list.Add(new ValidatedValueNode(node));
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

    public List<ISheetlet> Read(ISerializationManager serializationManager,
        SequenceDataNode node,
        IDependencyCollection dependencies,
        SerializationHookContext hookCtx,
        ISerializationContext? context = null,
        ISerializationManager.InstantiationDelegate<List<ISheetlet>>? instanceProvider = null)
    {
        var lst = new List<ISheetlet>(node.Count);

        var factory = dependencies.Resolve<ISheetletFactory>();
        foreach (var n in node)
        {
            if (n is not ValueDataNode entry)
            {
                throw new InvalidOperationException($"{n} is not a value data node");
            }

            lst.Add(factory.GetSheetlet(entry.Value));
        }

        return lst;
    }
}
