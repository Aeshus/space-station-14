using Content.Client.StyleProto.SheetletConfigs;
using JetBrains.Annotations;
using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using static Content.Client.Stylesheets.StylesheetHelpers;
using StyleClass = Content.Client.Stylesheets.StyleClass;

namespace Content.Client.StyleProto.Sheetlets;

[Sheetlet]
[UsedImplicitly]
public sealed partial class PanelSheetlet : ISheetlet
{
    [Dependency] private IResourceCache _resourceCache = default!;

    public StyleRule[] Generate(SheetletConfigRegistry configs)
    {
        var button = configs.GetConfig<ButtonConfig>();
        var palette = configs.GetConfig<PaletteConfig>();
        var panel = configs.GetConfig<PanelConfig>();

        var boxLight = new StyleBoxFlat
        {
            BackgroundColor = palette.SecondaryPalette.BackgroundLight,
        };
        var boxDark = new StyleBoxFlat
        {
            BackgroundColor = palette.SecondaryPalette.BackgroundDark,
        };
        var boxInsetDark = new StyleBoxFlat
        {
            BackgroundColor = palette.SecondaryPalette.BackgroundDark,
            BorderColor = palette.PrimaryPalette.Background,
            BorderThickness = new Thickness(2f),
        };
        var boxDeep = new StyleBoxFlat
        {
            BackgroundColor = panel.DeepPanelBackgroundColor,
        };
        var boxInsetDeep = new StyleBoxFlat
        {
            BackgroundColor = panel.DeepPanelBackgroundColor,
            BorderColor = panel.DeepPanelBorderColor,
            BorderThickness = new Thickness(2f),
        };

        var boxPositive = new StyleBoxFlat { BackgroundColor = palette.PositivePalette.Background };
        var boxNegative = new StyleBoxFlat { BackgroundColor = palette.NegativePalette.Background };
        var boxHighlight = new StyleBoxFlat { BackgroundColor = palette.HighlightPalette.Background };
        var boxDropTarget = new StyleBoxFlat
        {
            BackgroundColor = button.ButtonPalette.BackgroundDark.WithAlpha(0.5f),
            BorderColor = button.ButtonPalette.Base,
            BorderThickness = new(2),
        };

        return
        [
            E<PanelContainer>().Class(StyleClass.PanelLight).Panel(boxLight),
            E<PanelContainer>().Class(StyleClass.PanelDark).Panel(boxDark),
            E<PanelContainer>().Class(StyleClass.PanelDeep).Panel(boxDeep),
            E<PanelContainer>().Class(StyleClass.PanelDropTarget).Panel(boxDropTarget),
            E<PanelContainer>().Class(StyleClass.PanelInsetDark).Panel(boxInsetDark),
            E<PanelContainer>().Class(StyleClass.PanelInsetDeep).Panel(boxInsetDeep),

            E<PanelContainer>().Class(StyleClass.Positive).Panel(boxPositive),
            E<PanelContainer>().Class(StyleClass.Negative).Panel(boxNegative),
            E<PanelContainer>().Class(StyleClass.Highlight).Panel(boxHighlight),

            E<PanelContainer>()
                .Class("BackgroundDark")
                .Prop(PanelContainer.StylePropertyPanel, new StyleBoxFlat(Color.FromHex("#25252A"))),

            E()
                .Class(StyleClass.BackgroundPanel)
                .Prop(PanelContainer.StylePropertyPanel, StyleBoxHelpers.BaseStyleBox(_resourceCache, button))
                .Modulate(palette.SecondaryPalette.Background),
            E()
                .Class(StyleClass.BackgroundPanelDark)
                .Prop(PanelContainer.StylePropertyPanel, StyleBoxHelpers.BaseStyleBox(_resourceCache, button))
                .Modulate(palette.SecondaryPalette.BackgroundDark),
            E()
                .Class(StyleClass.BackgroundPanelOpenLeft)
                .Prop(PanelContainer.StylePropertyPanel, StyleBoxHelpers.OpenLeftStyleBox(_resourceCache, button))
                .Modulate(palette.SecondaryPalette.Background),
            E()
                .Class(StyleClass.BackgroundPanelOpenRight)
                .Prop(PanelContainer.StylePropertyPanel, StyleBoxHelpers.OpenRightStyleBox(_resourceCache, button))
                .Modulate(palette.SecondaryPalette.Background),
        ];
    }
}
