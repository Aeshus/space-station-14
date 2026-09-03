using Content.Client.StyleProto;
using Robust.Shared.Serialization.Manager.Attributes;

namespace Content.IntegrationTests.Tests.Stylesheets;

[TestOf(typeof(SheetletConfigRegistrySerializer))]
public sealed partial class SheetletConfigRegistrySerializerTest
{
    [SheetletConfig]
    private sealed partial class SerializerTestConfig : SheetletConfig
    {
        [DataField]
        public int Test { get; set; }
    }

    [SheetletConfig("SerializerNamed")]
    private sealed partial class SerializerTestNamedConfig : SheetletConfig
    {
        [DataField]
        public double Test2 { get; set; }
    }
}
