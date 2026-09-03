using Content.Client.StyleProto;
using Robust.Shared.Serialization.Manager.Attributes;

namespace Content.IntegrationTests.Tests.Stylesheets;

[TestOf(typeof(SheetletConfigRegistrySerializer))]
public sealed partial class SheetletConfigRegistrySerializerTest
{
    [SheetletConfig]
    private sealed partial class TestConfig : SheetletConfig
    {
        [DataField]
        public int Test { get; set; }
    }

    [SheetletConfig("Named")]
    private sealed partial class TestNamedConfig : SheetletConfig
    {
        [DataField]
        public double Test2 { get; set; }
    }
}
