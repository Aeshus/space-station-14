using System.Linq;
using Content.Shared.Chemistry.Reagent;
using Robust.Shared.Reflection;
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
/// (De)serializes YAML to/from Sheetlet lists
/// </summary>
public sealed partial class SheetletListSerializer : BaseTypeSerializer,
    ITypeSerializer<List<ISheetlet>, SequenceDataNode>,
    ITypeInheritanceHandler<List<ISheetlet>, SequenceDataNode>,
    ITypeCopier<List<ISheetlet>>
{
    [Dependency] private IReflectionManager _reflectionManager = default!;
    [Dependency] private IDynamicTypeFactory _dynamicTypeFactory = default!;

    private const string SheetletSuffix = "Sheetlet";

    public ValidationNode Validate(ISerializationManager serializationManager,
        SequenceDataNode node,
        IDependencyCollection dependencies,
        ISerializationContext? context = null)
    {
        var list = new List<ValidationNode>();
        var seen = new HashSet<string>();
        var validTypes = _reflectionManager.FindTypesWithAttribute<SheetletAttribute>()
            .ToDictionary(ty => ty.Name);

        foreach (var sequenceEntry in node.Sequence)
        {
            if (sequenceEntry is not ValueDataNode sheetletNode)
            {
                list.Add(new ErrorNode(sequenceEntry, $"Expected {nameof(ValueDataNode)}"));
                continue;
            }

            var name = sheetletNode.Value + SheetletSuffix;

            if (!validTypes.TryGetValue(name, out var type))
            {
                list.Add(new ErrorNode(sheetletNode,
                    $"Unknown type {name} (may not have proper attribute)"));
                continue;
            }

            if (!seen.Add(name))
            {
                list.Add(new ErrorNode(sheetletNode, "Duplicate sheetlet."));
                continue;
            }

            var copy = sheetletNode.Copy();
            list.Add(serializationManager.ValidateNode(type, copy, context));
        }

        return new ValidatedSequenceNode(list);
    }

    public List<ISheetlet> Read(ISerializationManager serializationManager,
        SequenceDataNode node,
        IDependencyCollection dependencies,
        SerializationHookContext hookCtx,
        ISerializationContext? context = null,
        ISerializationManager.InstantiationDelegate<List<ISheetlet>>? instanceProvider = null)
    {
        var configs = new List<ISheetlet>();
        var validTypes = _reflectionManager.FindTypesWithAttribute<SheetletAttribute>()
            .ToDictionary(ty => ty.Name);

        foreach (var sequenceEntry in node.Sequence)
        {
            if (sequenceEntry is not ValueDataNode sheetletNode)
            {
                throw new InvalidNodeTypeException($"Expected {nameof(MappingDataNode)}");
            }

            var name = sheetletNode.Value + SheetletSuffix;

            if (!validTypes.TryGetValue(name, out var type))
            {
                Log.Error($"Unknown config {name} (may not have proper attribute)");
                continue;
            }

            var sheetlet = _dynamicTypeFactory.CreateInstance<ISheetlet>(type);

            configs.Add(sheetlet);
        }

        return configs;
    }

    public DataNode Write(ISerializationManager serializationManager,
        List<ISheetlet> value,
        IDependencyCollection dependencies,
        bool alwaysWrite = false,
        ISerializationContext? context = null)
    {
        var configSequence = new SequenceDataNode();

        foreach (var type in value)
        {
            var name = type.GetType().Name;

            if (!name.EndsWith(SheetletSuffix))
                Log.Error($"Sheetlet {name} must end with {SheetletSuffix}");

            var node = serializationManager.WriteValue(
                name[..^SheetletSuffix.Length],
                alwaysWrite,
                context,
                notNullableOverride: true);

            if (node is not ValueDataNode mapping)
                throw new InvalidNodeTypeException();

            configSequence.Add(mapping);
        }

        return configSequence;
    }

    public SequenceDataNode PushInheritance(ISerializationManager serializationManager,
        SequenceDataNode child,
        SequenceDataNode parent,
        IDependencyCollection dependencies,
        ISerializationContext? context)
    {
        var result = child.Copy();

        foreach (var parentNode in parent.Reverse())
        {
            if (!result.Contains(parentNode))
                result.Insert(0, parentNode.Copy());
        }

        return result;
    }

    public void CopyTo(ISerializationManager serializationManager,
        List<ISheetlet> source,
        ref List<ISheetlet> target,
        IDependencyCollection dependencies,
        SerializationHookContext hookCtx,
        ISerializationContext? context = null)
    {
        target.Clear();
        target.EnsureCapacity(source.Count);

        foreach (var sheetlet in source)
        {
            target.Add(_dynamicTypeFactory.CreateInstance<ISheetlet>(sheetlet.GetType()));
        }
    }


}
