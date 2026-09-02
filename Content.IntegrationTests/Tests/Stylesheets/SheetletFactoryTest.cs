using Content.Client.StyleProto;
using Content.IntegrationTests.Fixtures;
using Content.IntegrationTests.Fixtures.Attributes;
using Robust.Shared.Analyzers;
using Robust.Shared.Serialization.Manager.Attributes;

namespace Content.IntegrationTests.Tests.Stylesheets;

[TestOf(typeof(SheetletFactory))]
public sealed class SheetletFactoryTest : GameTest
{
    [SidedDependency(Side.Client)] private readonly ISheetletFactory _sheetletFactory = default!;

    [SheetletConfig]
    [Virtual]
    public partial class TestConfig : SheetletConfig
    {
        [DataField]
        public int Test { get; set; }
    }

    [SheetletConfig]
    [Virtual]
    public partial class Test2Config : SheetletConfig
    {
        [DataField]
        public double Test2 { get; set; }
    }

    [Virtual]
    public partial class BadConfig : SheetletConfig
    {
        [DataField]
        public double Test2 { get; set; }
    }

    [Test]
    [Description("Checks the sheetlet config functionality of SheetletFactory")]
    [RunOnSide(Side.Client)]
    public void TestConfigsGet()
    {
        var test1 = _sheetletFactory.GetConfig<TestConfig>();
        Assert.That(_sheetletFactory.GetConfig<TestConfig>(), Is.Not.SameAs(test1));
        test1.Test = 10;

        var test2 = (Test2Config)_sheetletFactory.GetConfig("test2");
        test2.Test2 = 10.0;
    }

    [Test]
    [Description("Checks the unknown sheetlets/config SheetletFactory")]
    [RunOnSide(Side.Client)]
    public void TestUnknown()
    {
        Assert.Throws<ArgumentException>(() => _sheetletFactory.GetConfig<BadConfig>());
        Assert.Throws<ArgumentException>(() => _sheetletFactory.GetConfig<SheetletConfig>());
        Assert.Throws<ArgumentException>(() => _sheetletFactory.GetConfig("NotARealConfig"));
    }

    [Test]
    [Description("Checks the sheetlet functionality of SheetletFactory")]
    [RunOnSide(Side.Client)]
    public void TestSheetlet()
    {
    }
}
