using Content.Client.PDA;
using Content.Client.Resources;
using Content.Client.StyleProto.SheetletConfigs;
using JetBrains.Annotations;
using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using static Content.Client.Stylesheets.StylesheetHelpers;

namespace Content.Client.StyleProto.Sheetlets;

[Sheetlet]
[UsedImplicitly]
public sealed partial class PdaSheetlet : ISheetlet
{
    [Dependency] private IResourceCache _resourceCache = default!;

    public StyleRule[] Generate(SheetletConfigRegistry configs)
    {
        var panel = configs.GetConfig<PanelConfig>();
        var button = configs.GetConfig<ButtonConfig>();
        var font = configs.GetConfig<FontConfig>();
        var angleBorderRect = _resourceCache.GetTexture(panel.GeometricPanelBorderPath)
            .IntoPatch(StyleBox.Margin.All, 10);

        return
        [
            E<PanelContainer>()
                .Class("PdaContentBackground")
                .Prop(PanelContainer.StylePropertyPanel, StyleBoxHelpers.SquareStyleBox(_resourceCache, button))
                .Prop(Control.StylePropertyModulateSelf, Color.FromHex("#25252a")),

            E<PanelContainer>()
                .Class("PdaBackground")
                .Prop(PanelContainer.StylePropertyPanel, StyleBoxHelpers.SquareStyleBox(_resourceCache, button))
                .Prop(Control.StylePropertyModulateSelf, Color.FromHex("#000000")),

            E<PanelContainer>()
                .Class("PdaBackgroundRect")
                .Prop(PanelContainer.StylePropertyPanel, StyleBoxHelpers.BaseStyleBox(_resourceCache, button))
                .Prop(Control.StylePropertyModulateSelf, Color.FromHex("#717059")),

            E<PanelContainer>()
                .Class("PdaBorderRect")
                .Prop(PanelContainer.StylePropertyPanel, angleBorderRect),

            E<PdaSettingsButton>()
                .Pseudo(ContainerButton.StylePseudoClassNormal)
                .Prop(PdaSettingsButton.StylePropertyBgColor, Color.FromHex(PdaSettingsButton.NormalBgColor))
                .Prop(PdaSettingsButton.StylePropertyFgColor, Color.FromHex(PdaSettingsButton.EnabledFgColor)),

            E<PdaSettingsButton>()
                .Pseudo(ContainerButton.StylePseudoClassHover)
                .Prop(PdaSettingsButton.StylePropertyBgColor, Color.FromHex(PdaSettingsButton.HoverColor))
                .Prop(PdaSettingsButton.StylePropertyFgColor, Color.FromHex(PdaSettingsButton.EnabledFgColor)),

            E<PdaSettingsButton>()
                .Pseudo(ContainerButton.StylePseudoClassPressed)
                .Prop(PdaSettingsButton.StylePropertyBgColor, Color.FromHex(PdaSettingsButton.PressedColor))
                .Prop(PdaSettingsButton.StylePropertyFgColor, Color.FromHex(PdaSettingsButton.EnabledFgColor)),

            E<PdaSettingsButton>()
                .Pseudo(ContainerButton.StylePseudoClassDisabled)
                .Prop(PdaSettingsButton.StylePropertyBgColor, Color.FromHex(PdaSettingsButton.NormalBgColor))
                .Prop(PdaSettingsButton.StylePropertyFgColor, Color.FromHex(PdaSettingsButton.DisabledFgColor)),

            E<PdaProgramItem>()
                .Pseudo(ContainerButton.StylePseudoClassNormal)
                .Prop(PdaProgramItem.StylePropertyBgColor, Color.FromHex(PdaProgramItem.NormalBgColor)),

            E<PdaProgramItem>()
                .Pseudo(ContainerButton.StylePseudoClassHover)
                .Prop(PdaProgramItem.StylePropertyBgColor, Color.FromHex(PdaProgramItem.HoverColor)),

            E<PdaProgramItem>()
                .Pseudo(ContainerButton.StylePseudoClassPressed)
                .Prop(PdaProgramItem.StylePropertyBgColor, Color.FromHex(PdaProgramItem.HoverColor)),

            E<Label>()
                .Class("PdaContentFooterText")
                .Prop(Label.StylePropertyFont, font.BaseFont.GetFont(10))
                .Prop(Label.StylePropertyFontColor, Color.FromHex("#757575")),

            E<Label>()
                .Class("PdaWindowFooterText")
                .Prop(Label.StylePropertyFont, font.BaseFont.GetFont(10))
                .Prop(Label.StylePropertyFontColor, Color.FromHex("#333d3b")),
        ];
    }
}
