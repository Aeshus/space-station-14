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
    public void TestConfigsTryGet()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(_sheetletFactory.TryGetConfigName(typeof(TestConfig), out var name), Is.True);
            Assert.That(name, Is.EqualTo("Test"));
            Assert.That(_sheetletFactory.TryGetConfigType("Test", out var type), Is.True);
            Assert.That(type, Is.EqualTo(typeof(TestConfig)));
        }
    }

    [Test]
    [Description("Checks the sheetlet config name override functionality of SheetletFactory")]
    [RunOnSide(Side.Client)]
    public void TestConfigNamed()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(_sheetletFactory.TryGetConfigName(typeof(TestNamedConfig), out var name), Is.True);
            Assert.That(name, Is.EqualTo("Named"));
            Assert.That(_sheetletFactory.TryGetConfigType("TestNamed", out var defaultNameType), Is.False);
            Assert.That(defaultNameType, Is.Null);
            Assert.That(_sheetletFactory.TryGetConfigType("Named", out var type), Is.True);
            Assert.That(type, Is.EqualTo(typeof(TestNamedConfig)));
        }
    }

    [Test]
    [Description("Checks the unknown config SheetletFactory")]
    [RunOnSide(Side.Client)]
    public void TestConfigUnknown()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(_sheetletFactory.TryGetConfigName(typeof(BadConfig), out var badConfigName), Is.False);
            Assert.That(badConfigName, Is.Null);
            Assert.That(_sheetletFactory.TryGetConfigName(typeof(SheetletConfig), out var baseConfigName), Is.False);
            Assert.That(baseConfigName, Is.Null);
            Assert.That(_sheetletFactory.TryGetConfigType("NotARealConfig", out var type), Is.False);
            Assert.That(type, Is.Null);
        }
    }

    [Test]
    [Description("Checks the sheetlet functionality of SheetletFactory")]
    [RunOnSide(Side.Client)]
    public void TestSheetletGet()
    {
        var sheetlet = _sheetletFactory.GetSheetlet<TestSheetlet>();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(_sheetletFactory.GetSheetlet<TestSheetlet>(), Is.SameAs(sheetlet));
            Assert.That(_sheetletFactory.GetSheetlet<TestSheetlet>(), Is.EqualTo(sheetlet));
            Assert.That(_sheetletFactory.GetSheetlet("Test"), Is.SameAs(sheetlet));
        }
    }

    [Test]
    [Description("Checks the sheetlet name override functionality of SheetletFactory")]
    [RunOnSide(Side.Client)]
    public void TestSheetletNamed()
    {
        var sheetlet = _sheetletFactory.GetSheetlet<TestNamedSheetlet>();
        using (Assert.EnterMultipleScope())
        {
            Assert.Throws<ArgumentException>(() => _sheetletFactory.GetSheetlet("TestNamed"));
            Assert.That(_sheetletFactory.GetSheetlet("Named"), Is.SameAs(sheetlet));
        }
    }

    [Test]
    [Description("Checks the unknown sheetlets SheetletFactory")]
    [RunOnSide(Side.Client)]
    public void TestSheetletUnknown()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.Throws<ArgumentException>(() => _sheetletFactory.GetSheetlet<BadSheetlet>());
            Assert.Throws<ArgumentException>(() => _sheetletFactory.GetSheetlet<ISheetlet>());
            Assert.Throws<ArgumentException>(() => _sheetletFactory.GetSheetlet("NotARealSheetlet"));
        }
    }
}
