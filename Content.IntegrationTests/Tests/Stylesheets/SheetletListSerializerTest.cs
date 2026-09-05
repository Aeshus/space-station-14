using System.IO;
using Content.Client.StyleProto;
using Content.IntegrationTests.Fixtures;
using Content.IntegrationTests.Fixtures.Attributes;
using JetBrains.Annotations;
using Robust.Client.UserInterface;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.Manager;
using Robust.Shared.Serialization.Markdown;
using Robust.Shared.Serialization.Markdown.Sequence;
using YamlDotNet.RepresentationModel;

namespace Content.IntegrationTests.Tests.Stylesheets;

[TestOf(typeof(SheetletListSerializer))]
public sealed class SheetletListSerializerTest : GameTest
{
    [SidedDependency(Side.Client)] private readonly ISerializationManager _serializationManager = default!;
    [SidedDependency(Side.Client)] private readonly ISheetletFactory _sheetletFactory = default!;

    private readonly SheetletListSerializer _serializer = new();

    [UsedImplicitly]
    [Sheetlet]
    private sealed class ListSerializerTestSheetlet : ISheetlet
    {
        public StyleRule[] Generate(SheetletConfigRegistry configs)
        {
            throw new NotImplementedException();
        }
    }

    [UsedImplicitly]
    [Sheetlet("ListSerializerNamed")]
    private sealed class ListSerializerTestNamedSheetlet : ISheetlet
    {
        public StyleRule[] Generate(SheetletConfigRegistry configs)
        {
            throw new NotImplementedException();
        }
    }

    [Test]
    [Description("Checks that the sheetlet list serializer (de)serializes")]
    [RunOnSide(Side.Client)]
    public void TestSerializer()
    {
        var str = """
            - ListSerializerNamed
            - ListSerializerTest
            """;
        var yamlStream = new YamlStream();
        yamlStream.Load(new StringReader(str));
        var node = yamlStream.Documents[0].RootNode.ToDataNodeCast<SequenceDataNode>();

        var original = node.Copy();
        var validation = _serializer.Validate(_serializationManager, node, Client.InstanceDependencyCollection);
        var sheetlets = _serializer.Read(_serializationManager,
            node,
            Client.InstanceDependencyCollection,
            SerializationHookContext.DoSkipHooks);
        var written = (SequenceDataNode)_serializer.Write(_serializationManager,
            sheetlets,
            Client.InstanceDependencyCollection);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(validation.Valid, Is.True);
            Assert.That(sheetlets, Has.Count.EqualTo(2));
            Assert.That(sheetlets[0], Is.SameAs(_sheetletFactory.GetSheetlet<ListSerializerTestNamedSheetlet>()));
            Assert.That(sheetlets[1], Is.SameAs(_sheetletFactory.GetSheetlet<ListSerializerTestSheetlet>()));
            Assert.That(written, Has.Count.EqualTo(2));
            Assert.That(written, Is.EqualTo(original));
            Assert.That(node, Is.EqualTo(original));
        }
    }

    [Test]
    [Description("Checks that the SheetletListSerializer does inheritance")]
    [RunOnSide(Side.Client)]
    public void TestInheritance()
    {
        var parentStr = """
            - First
            - Shared
            - Second
            """;
        var childStr = """
            - Child
            - Shared
            """;
        var yamlStream = new YamlStream();
        yamlStream.Load(new StringReader(parentStr));
        var parent = yamlStream.Documents[0].RootNode.ToDataNodeCast<SequenceDataNode>();
        yamlStream.Load(new StringReader(childStr));
        var child = yamlStream.Documents[0].RootNode.ToDataNodeCast<SequenceDataNode>();

        var parentCopy = parent.Copy();
        var childCopy = child.Copy();

        var result = _serializer.PushInheritance(_serializationManager,
            child,
            parent,
            Client.InstanceDependencyCollection,
            null);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result, Is.EqualTo(new SequenceDataNode("First", "Second", "Child", "Shared")));
            Assert.That(parent, Is.EqualTo(parentCopy));
            Assert.That(child, Is.EqualTo(childCopy));
            Assert.That(result[0], Is.Not.SameAs(parent[0]));
            Assert.That(result[2], Is.Not.SameAs(child[0]));
        }
    }
}
