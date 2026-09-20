using Content.Client.StyleProto.Fonts;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.Manager;
using Robust.Shared.Serialization.Markdown.Mapping;
using Robust.Shared.Serialization.Markdown.Validation;
using Robust.Shared.Serialization.TypeSerializers.Interfaces;

namespace Content.Client.StyleProto.Serializers;

/// <summary>
/// (de)Serializes IFontFamily.
/// </summary>
/// <remarks>
/// It acts like it's always reading a FontFamilyBundled, as it wouldn't make sense otherwise.
/// </remarks>
[TypeSerializer]
public sealed class IFontFamilySerializer : BaseTypeSerializer, ITypeReader<IFontFamily, MappingDataNode>,
    ITypeCopyCreator<IFontFamily>
{
    /// <inheritdoc/>
    public ValidationNode Validate(ISerializationManager serializationManager,
        MappingDataNode node,
        IDependencyCollection dependencies,
        ISerializationContext? context = null)
    {
        return serializationManager.ValidateNode<FontFamilyBundled>(node, context);
    }

    /// <inheritdoc/>
    public IFontFamily Read(ISerializationManager serializationManager,
        MappingDataNode node,
        IDependencyCollection dependencies,
        SerializationHookContext hookCtx,
        ISerializationContext? context = null,
        ISerializationManager.InstantiationDelegate<IFontFamily>? instanceProvider = null)
    {
        var data = serializationManager.Read<FontFamilyBundled>(node, context, notNullableOverride: true);

        // It has an internal ResCache that is uses, so we populate that here.
        dependencies.InjectDependencies(data);

        return data;
    }

    /// <inheritdoc/>
    public IFontFamily CreateCopy(ISerializationManager serializationManager,
        IFontFamily source,
        IDependencyCollection dependencies,
        SerializationHookContext hookCtx,
        ISerializationContext? context = null)
    {
        var copy = serializationManager.CreateCopy((FontFamilyBundled)source,
            hookCtx,
            context,
            notNullableOverride: true);
        dependencies.InjectDependencies(copy);
        return copy;
    }
}
