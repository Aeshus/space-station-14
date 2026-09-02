using Content.Client.StyleProto;
using Content.IntegrationTests.Fixtures;
using Content.IntegrationTests.Fixtures.Attributes;
using Robust.Shared.Analyzers;
using Robust.Shared.Serialization.Manager.Attributes;

namespace Content.IntegrationTests.Tests.Stylesheets;

[TestOf(typeof(SheetletFactory))]
public sealed partial class SheetletFactoryTest : GameTest
{
    [SidedDependency(Side.Client)] private readonly ISheetletFactory _sheetletFactory = default!;

    [SheetletConfig]
    public sealed partial class TestConfig : SheetletConfig
    {
        [DataField]
        public int Test { get; set; }
    }

    [SheetletConfig("Override")]
    public sealed partial class TestOverrideConfig : SheetletConfig
    {
        [DataField]
        public double Test2 { get; set; }
    }

    public sealed partial class BadConfig : SheetletConfig
    {
        [DataField]
        public double Test2 { get; set; }
    }

    [Test]
    [Description("Checks the sheetlet config functionality of SheetletFactory")]
    [RunOnSide(Side.Client)]
    public void TestConfigsGet()
    {
        var config = _sheetletFactory.GetConfig<TestConfig>();
        Assert.That(_sheetletFactory.GetConfig<TestConfig>(), Is.Not.SameAs(config));
        Assert.That(_sheetletFactory.GetConfig<TestConfig>(), Is.EqualTo(config));
        Assert.That(_sheetletFactory.GetConfig("Test"), Is.Not.SameAs(config));
        Assert.That(_sheetletFactory.GetConfig("Test"), Is.EqualTo(config));
        config.Test = 10;
        Assert.That(_sheetletFactory.GetConfig<TestConfig>().Test, Is.Not.EqualTo(config.Test));
    }

    [Test]
    [Description("Checks the sheetlet config name override functionality of SheetletFactory")]
    [RunOnSide(Side.Client)]
    public void TestConfigNameOverride()
    {
        var config = _sheetletFactory.GetConfig<TestOverrideConfig>();
        Assert.Throws<ArgumentException>(() => _sheetletFactory.GetConfig("TestOverride"));
        Assert.That(_sheetletFactory.GetConfig("Override"), Is.Not.SameAs(config));
        Assert.That(_sheetletFactory.GetConfig("Override"), Is.EqualTo(config));
    }

    [Test]
    [Description("Checks the unknown sheetlets/config SheetletFactory")]
    [RunOnSide(Side.Client)]
    public void TestConfigUnknown()
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
