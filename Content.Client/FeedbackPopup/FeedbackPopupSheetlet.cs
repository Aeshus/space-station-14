using Content.Client.Stylesheets;
using Content.Client.Stylesheets.SheetletConfigs;
using Robust.Client.Graphics;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using static Content.Client.Stylesheets.StylesheetHelpers;

namespace Content.Client.FeedbackPopup;

[Sheetlet]
public sealed class FeedbackPopupSheetlet : ISheetlet
{
    public StyleRule[] Generate(SheetletConfigRegistry configs)
    {
        var palettes = configs.GetConfig<PaletteConfig>();

        var borderTop = new StyleBoxFlat()
        {
            BorderColor = palettes.SecondaryPalette.Base,
            BorderThickness = new Thickness(0, 1, 0, 0),
        };

        var borderBottom = new StyleBoxFlat()
        {
            BorderColor = palettes.SecondaryPalette.Base,
            BorderThickness = new Thickness(0, 0, 0, 1),
        };

        return
        [
            E<PanelContainer>()
                .Identifier("FeedbackBorderThinTop")
                .Prop(PanelContainer.StylePropertyPanel, borderTop),
            E<PanelContainer>()
                .Identifier("FeedbackBorderThinBottom")
                .Prop(PanelContainer.StylePropertyPanel, borderBottom),
        ];
    }
}
