using Content.Client.Stylesheets.SheetletConfigs;
using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using static Content.Client.Stylesheets.StylesheetHelpers;

namespace Content.Client.Stylesheets.Sheetlets;

[Sheetlet]
public sealed partial class PanelSheetlet : ISheetlet
{
    [Dependency] private IResourceCache _resCache = default!;

    public StyleRule[] Generate(SheetletConfigRegistry configs)
    {
        var palettes = configs.GetConfig<PaletteConfig>();
        var panels = configs.GetConfig<PanelConfig>();
        var buttons = configs.GetConfig<ButtonConfig>();

        var boxLight = new StyleBoxFlat
        {
            BackgroundColor = palettes.SecondaryPalette.BackgroundLight
        };
        var boxDark = new StyleBoxFlat
        {
            BackgroundColor = palettes.SecondaryPalette.BackgroundDark
        };
        var boxInsetDark = new StyleBoxFlat
        {
            BackgroundColor = palettes.SecondaryPalette.BackgroundDark,
            BorderColor = palettes.PrimaryPalette.Background,
            BorderThickness = new Thickness(2f)
        };
        var boxDeep = new StyleBoxFlat
        {
            BackgroundColor = panels.DeepPanelBackgroundColor
        };
        var boxInsetDeep = new StyleBoxFlat
        {
            BackgroundColor = panels.DeepPanelBackgroundColor,
            BorderColor = panels.DeepPanelBorderColor,
            BorderThickness = new Thickness(2f)
        };

        var boxPositive = new StyleBoxFlat { BackgroundColor = palettes.PositivePalette.Background };
        var boxNegative = new StyleBoxFlat { BackgroundColor = palettes.NegativePalette.Background };
        var boxHighlight = new StyleBoxFlat { BackgroundColor = palettes.HighlightPalette.Background };
        var boxDropTarget = new StyleBoxFlat
        {
            BackgroundColor = buttons.ButtonPalette.BackgroundDark.WithAlpha(0.5f),
            BorderColor = buttons.ButtonPalette.Base,
            BorderThickness = new(2)
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

            // TODO: this should probably be cleaned up but too many UIs rely on this hardcoded color so I'm scared to touch it
            E<PanelContainer>()
                .Class("BackgroundDark")
                .Prop(PanelContainer.StylePropertyPanel, new StyleBoxFlat(Color.FromHex("#25252A"))),

            // panels that have the same corner bezels as buttons
            E()
                .Class(StyleClass.BackgroundPanel)
                .Prop(PanelContainer.StylePropertyPanel, StyleBoxHelpers.BaseStyleBox(_resCache, buttons))
                .Modulate(palettes.SecondaryPalette.Background),
            E()
                .Class(StyleClass.BackgroundPanelDark)
                .Prop(PanelContainer.StylePropertyPanel, StyleBoxHelpers.BaseStyleBox(_resCache, buttons))
                .Modulate(palettes.SecondaryPalette.BackgroundDark),
            E()
                .Class(StyleClass.BackgroundPanelOpenLeft)
                .Prop(PanelContainer.StylePropertyPanel, StyleBoxHelpers.OpenLeftStyleBox(_resCache, buttons))
                .Modulate(palettes.SecondaryPalette.Background),
            E()
                .Class(StyleClass.BackgroundPanelOpenRight)
                .Prop(PanelContainer.StylePropertyPanel, StyleBoxHelpers.OpenRightStyleBox(_resCache, buttons))
                .Modulate(palettes.SecondaryPalette.Background)
        ];
    }
}
