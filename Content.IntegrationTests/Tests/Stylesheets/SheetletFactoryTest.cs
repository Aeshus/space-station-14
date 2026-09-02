using Content.Client.StyleProto;
using Content.IntegrationTests.Fixtures;
using Content.IntegrationTests.Fixtures.Attributes;
using Robust.Client.UserInterface;
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

    [SheetletConfig("Named")]
    public sealed partial class TestNamedConfig : SheetletConfig
    {
        [DataField]
        public double Test2 { get; set; }
    }

    public sealed partial class BadConfig : SheetletConfig
    {
        [DataField]
        public double Test2 { get; set; }
    }

    [Sheetlet]
    public sealed class TestSheetlet : ISheetlet
    {
        public StyleRule[] Generate(SheetletConfigRegistry configs)
        {
            throw new NotImplementedException();
        }
    }

    [Sheetlet("Named")]
    public sealed class TestNamedSheetlet : ISheetlet
    {
        public StyleRule[] Generate(SheetletConfigRegistry configs)
        {
            throw new NotImplementedException();
        }
    }

    public sealed class BadSheetlet : ISheetlet
    {
        public StyleRule[] Generate(SheetletConfigRegistry configs)
        {
            throw new NotImplementedException();
        }
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
    public void TestConfigNamed()
    {
        var config = _sheetletFactory.GetConfig<TestNamedConfig>();
        Assert.Throws<ArgumentException>(() => _sheetletFactory.GetConfig("TestNamed"));
        Assert.That(_sheetletFactory.GetConfig("Named"), Is.Not.SameAs(config));
        Assert.That(_sheetletFactory.GetConfig("Named"), Is.EqualTo(config));
    }

    [Test]
    [Description("Checks the unknown config SheetletFactory")]
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
    public void TestSheetletGet()
    {
        var sheetlet = _sheetletFactory.GetSheetlet<TestSheetlet>();
        Assert.That(_sheetletFactory.GetSheetlet<TestSheetlet>(), Is.SameAs(sheetlet));
        Assert.That(_sheetletFactory.GetSheetlet<TestSheetlet>(), Is.EqualTo(sheetlet));
        Assert.That(_sheetletFactory.GetSheetlet("Test"), Is.SameAs(sheetlet));
        Assert.That(_sheetletFactory.GetSheetlet("Test"), Is.EqualTo(sheetlet));
    }

    [Test]
    [Description("Checks the sheetlet name override functionality of SheetletFactory")]
    [RunOnSide(Side.Client)]
    public void TestSheetletNamed()
    {
        var sheetlet = _sheetletFactory.GetSheetlet<TestNamedSheetlet>();
        Assert.Throws<ArgumentException>(() => _sheetletFactory.GetSheetlet("TestNamed"));
        Assert.That(_sheetletFactory.GetSheetlet("Named"), Is.SameAs(sheetlet));
        Assert.That(_sheetletFactory.GetSheetlet("Named"), Is.EqualTo(sheetlet));
    }

    [Test]
    [Description("Checks the unknown sheetlets SheetletFactory")]
    [RunOnSide(Side.Client)]
    public void TestSheetletUnknown()
    {
        Assert.Throws<ArgumentException>(() => _sheetletFactory.GetSheetlet<BadSheetlet>());
        Assert.Throws<ArgumentException>(() => _sheetletFactory.GetSheetlet<ISheetlet>());
        Assert.Throws<ArgumentException>(() => _sheetletFactory.GetSheetlet("NotARealSheetlet"));
    }
}
