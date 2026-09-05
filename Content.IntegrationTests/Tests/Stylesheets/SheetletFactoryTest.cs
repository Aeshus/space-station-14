using Content.Client.StyleProto;
using Content.IntegrationTests.Fixtures;
using Content.IntegrationTests.Fixtures.Attributes;
using JetBrains.Annotations;
using Robust.Client.UserInterface;
using Robust.Shared.Serialization.Manager.Attributes;

namespace Content.IntegrationTests.Tests.Stylesheets;

[TestOf(typeof(SheetletFactory))]
public sealed partial class SheetletFactoryTest : GameTest
{
    [SidedDependency(Side.Client)] private readonly ISheetletFactory _sheetletFactory = default!;

    [UsedImplicitly]
    [SheetletConfig]
    private sealed partial class FactoryTestConfig : SheetletConfig
    {
        [DataField]
        public int Test { get; set; }
    }

    [UsedImplicitly]
    [SheetletConfig("FactoryNamed")]
    private sealed partial class FactoryTestNamedConfig : SheetletConfig
    {
        [DataField]
        public double Test2 { get; set; }
    }

    [UsedImplicitly]
    private sealed partial class FactoryBadConfig : SheetletConfig
    {
        [DataField]
        public double Test2 { get; set; }
    }

    [UsedImplicitly]
    [Sheetlet]
    private sealed class FactoryTestSheetlet : ISheetlet
    {
        public StyleRule[] Generate(SheetletConfigRegistry configs)
        {
            throw new NotImplementedException();
        }
    }

    [UsedImplicitly]
    [Sheetlet("FactoryNamed")]
    private sealed class FactoryTestNamedSheetlet : ISheetlet
    {
        public StyleRule[] Generate(SheetletConfigRegistry configs)
        {
            throw new NotImplementedException();
        }
    }

    [UsedImplicitly]
    private sealed class FactoryBadSheetlet : ISheetlet
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
            Assert.That(_sheetletFactory.TryGetConfigName(typeof(FactoryTestConfig), out var name), Is.True);
            Assert.That(name, Is.EqualTo("FactoryTest"));
            Assert.That(_sheetletFactory.TryGetConfigType("FactoryTest", out var type), Is.True);
            Assert.That(type, Is.EqualTo(typeof(FactoryTestConfig)));
        }
    }

    [Test]
    [Description("Checks the sheetlet config name override functionality of SheetletFactory")]
    [RunOnSide(Side.Client)]
    public void TestConfigNamed()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(_sheetletFactory.TryGetConfigName(typeof(FactoryTestNamedConfig), out var name), Is.True);
            Assert.That(name, Is.EqualTo("FactoryNamed"));
            Assert.That(_sheetletFactory.TryGetConfigType("FactoryTestNamed", out var defaultNameType), Is.False);
            Assert.That(defaultNameType, Is.Null);
            Assert.That(_sheetletFactory.TryGetConfigType("FactoryNamed", out var type), Is.True);
            Assert.That(type, Is.EqualTo(typeof(FactoryTestNamedConfig)));
        }
    }

    [Test]
    [Description("Checks the unknown config SheetletFactory")]
    [RunOnSide(Side.Client)]
    public void TestConfigUnknown()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(_sheetletFactory.TryGetConfigName(typeof(FactoryBadConfig), out var badConfigName), Is.False);
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
        var sheetlet = _sheetletFactory.GetSheetlet<FactoryTestSheetlet>();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(_sheetletFactory.GetSheetlet<FactoryTestSheetlet>(), Is.SameAs(sheetlet));
            Assert.That(_sheetletFactory.GetSheetlet<FactoryTestSheetlet>(), Is.EqualTo(sheetlet));
            Assert.That(_sheetletFactory.GetSheetlet("FactoryTest"), Is.SameAs(sheetlet));
        }
    }

    [Test]
    [Description("Checks the sheetlet name override functionality of SheetletFactory")]
    [RunOnSide(Side.Client)]
    public void TestSheetletNamed()
    {
        var sheetlet = _sheetletFactory.GetSheetlet<FactoryTestNamedSheetlet>();
        using (Assert.EnterMultipleScope())
        {
            Assert.Throws<ArgumentException>(() => _sheetletFactory.GetSheetlet("FactoryTestNamed"));
            Assert.That(_sheetletFactory.GetSheetlet("FactoryNamed"), Is.SameAs(sheetlet));
        }
    }

    [Test]
    [Description("Checks the unknown sheetlets SheetletFactory")]
    [RunOnSide(Side.Client)]
    public void TestSheetletUnknown()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.Throws<ArgumentException>(() => _sheetletFactory.GetSheetlet<FactoryBadSheetlet>());
            Assert.Throws<ArgumentException>(() => _sheetletFactory.GetSheetlet<ISheetlet>());
            Assert.Throws<ArgumentException>(() => _sheetletFactory.GetSheetlet("NotARealSheetlet"));
        }
    }
}
