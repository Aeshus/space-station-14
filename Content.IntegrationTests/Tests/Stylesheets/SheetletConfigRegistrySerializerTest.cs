using System.IO;
using Content.Client.StyleProto;
using Content.IntegrationTests.Fixtures;
using Content.IntegrationTests.Fixtures.Attributes;
using JetBrains.Annotations;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.Manager;
using Robust.Shared.Serialization.Manager.Attributes;
using Robust.Shared.Serialization.Markdown;
using Robust.Shared.Serialization.Markdown.Mapping;
using Robust.Shared.Serialization.Markdown.Sequence;
using YamlDotNet.RepresentationModel;

namespace Content.IntegrationTests.Tests.Stylesheets;

[TestOf(typeof(SheetletConfigRegistrySerializer))]
public sealed partial class SheetletConfigRegistrySerializerTest : GameTest
{
    [SidedDependency(Side.Client)] private readonly ISerializationManager _serializationManager = default!;

    private readonly SheetletConfigRegistrySerializer _serializer = new();

    [UsedImplicitly]
    [SheetletConfig]
    private sealed partial class SerializerTestConfig : SheetletConfig
    {
        [DataField]
        public int Test { get; set; }

        [DataField]
        public int Inherited { get; set; }
    }

    [UsedImplicitly]
    [SheetletConfig("SerializerNamed")]
    private sealed partial class SerializerTestNamedConfig : SheetletConfig
    {
        [DataField]
        public double Test2 { get; set; }
    }

    [Test]
    [Description("Checks that the sheetlet config serializer (de)serializes")]
    [RunOnSide(Side.Client)]
    public void TestSerializer()
    {
        var str = """
            - type: SerializerTest
              test: 42
            - type: SerializerNamed
              test2: 2.5
            """;
        var yamlStream = new YamlStream();
        yamlStream.Load(new StringReader(str));
        var node = yamlStream.Documents[0].RootNode.ToDataNodeCast<SequenceDataNode>();

        var original = node.Copy();
        var validation = _serializer.Validate(_serializationManager, node, Client.InstanceDependencyCollection);
        var registry = _serializer.Read(_serializationManager,
            node,
            Client.InstanceDependencyCollection,
            SerializationHookContext.DoSkipHooks);
        var written = (SequenceDataNode)_serializer.Write(_serializationManager,
            registry,
            Client.InstanceDependencyCollection);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(validation.Valid, Is.True);
            Assert.That(registry, Has.Count.EqualTo(2));
            Assert.That(registry.GetConfig<SerializerTestConfig>().Test, Is.EqualTo(42));
            Assert.That(registry.GetConfig<SerializerTestNamedConfig>().Test2, Is.EqualTo(2.5));
            Assert.That(written, Has.Count.EqualTo(2));
            Assert.That(written.Cast<MappingDataNode>(0), Is.EquivalentTo(original.Cast<MappingDataNode>(0)));
            Assert.That(written.Cast<MappingDataNode>(1), Is.EquivalentTo(original.Cast<MappingDataNode>(1)));
            Assert.That(node, Is.EqualTo(original));
        }
    }

    [Test]
    [Description("Checks that the SheetletConfigRegistrySerializer does inheritance")]
    [RunOnSide(Side.Client)]
    public void TestInheritance()
    {
        var parentStr = """
            - type: SerializerTest
              test: 1
              inherited: 7
            - type: SerializerNamed
              test2: 2.5
            """;
        var childStr = """
            - type: SerializerTest
              test: 42
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
        var registry = _serializer.Read(_serializationManager,
            result,
            Client.InstanceDependencyCollection,
            SerializationHookContext.DoSkipHooks);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(registry, Has.Count.EqualTo(2));
            Assert.That(registry.GetConfig<SerializerTestConfig>().Test, Is.EqualTo(42));
            Assert.That(registry.GetConfig<SerializerTestConfig>().Inherited, Is.EqualTo(7));
            Assert.That(registry.GetConfig<SerializerTestNamedConfig>().Test2, Is.EqualTo(2.5));
            Assert.That(parent, Is.EqualTo(parentCopy));
            Assert.That(child, Is.EqualTo(childCopy));
            Assert.That(result[1], Is.Not.SameAs(parent[1]));
        }
    }
}
