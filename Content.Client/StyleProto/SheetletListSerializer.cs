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

    /// <inheritdoc/>
    public ValidationNode Validate(ISerializationManager serializationManager,
        SequenceDataNode node,
        IDependencyCollection dependencies,
        ISerializationContext? context = null)
    {
        var list = new List<ValidationNode>();
        var seen = new HashSet<Type>();
        var types = SheetletTypes();

        foreach (var sequenceEntry in node.Sequence)
        {
            try
            {
                var (type, mapping) = ParseSheetlet(types, sequenceEntry);

                if (!seen.Add(type))
                {
                    throw new ArgumentException($"Sheetlet {type.Name} is already defined");
                }

                list.Add(new ValidatedValueNode(sequenceEntry));
            }
            catch (Exception e)
            {
                list.Add(new ErrorNode(sequenceEntry, e.Message));
            }
        }

        return new ValidatedSequenceNode(list);
    }

    /// <inheritdoc/>
    public List<ISheetlet> Read(ISerializationManager serializationManager,
        SequenceDataNode node,
        IDependencyCollection dependencies,
        SerializationHookContext hookCtx,
        ISerializationContext? context = null,
        ISerializationManager.InstantiationDelegate<List<ISheetlet>>? instanceProvider = null)
    {
        var configs = new List<ISheetlet>();
        var types = SheetletTypes();

        foreach (var sequenceEntry in node.Sequence)
        {
            try
            {
                var (type, _) = ParseSheetlet(types, sequenceEntry);

                var sheetlet = _dynamicTypeFactory.CreateInstance<ISheetlet>(type);

                configs.Add(sheetlet);
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }
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

        foreach (var sheetlet in value)
        {
            var name = sheetlet.GetType().Name;

            if (!name.EndsWith(SheetletSuffix))
            {
                Log.Error($"Sheetlet {name} must end with {SheetletSuffix}");
                continue;
            }

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


    /// <summary>
    /// Resolves all configuration types.
    /// </summary>
    /// <returns>A dictionary matching string name to type.</returns>
    private Dictionary<string, Type> SheetletTypes()
    {
        // TODO: sourcegen?
        return _reflectionManager.FindTypesWithAttribute<SheetletAttribute>()
            .ToDictionary(ty => ty.Name);
    }

    /// <summary>
    /// Parses a sheetlet config from a list of types and a DataNode.
    /// </summary>
    /// <param name="types">All valid config types.</param>
    /// <param name="sequenceEntry">DataNode to parse</param>
    /// <returns>Type of the mapping, and the mapping node</returns>
    /// <exception cref="InvalidNodeTypeException">Invalid node type</exception>
    /// <exception cref="KeyNotFoundException">No 'type' found</exception>
    /// <exception cref="TypeAccessException">'type' value not found in dictionary</exception>
    private static (Type, ValueDataNode) ParseSheetlet(Dictionary<string, Type> types, DataNode sequenceEntry)
    {
        if (sequenceEntry is not ValueDataNode valueNode)
        {
            throw new InvalidNodeTypeException($"Expected {nameof(ValueDataNode)}");
        }

        var name = valueNode.Value + SheetletSuffix;

        if (!types.TryGetValue(name, out var type))
        {
            throw new TypeAccessException($"Type {name} was not found");
        }

        return (type, valueNode);
    }
}
