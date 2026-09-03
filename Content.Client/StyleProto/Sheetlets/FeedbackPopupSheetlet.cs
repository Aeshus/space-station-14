using Content.Client.StyleProto.SheetletConfigs;
using JetBrains.Annotations;
using Robust.Client.Graphics;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using static Content.Client.Stylesheets.StylesheetHelpers;

namespace Content.Client.StyleProto.Sheetlets;

[Sheetlet]
[UsedImplicitly]
public sealed partial class FeedbackPopupSheetlet : ISheetlet
{
    public StyleRule[] Generate(SheetletConfigRegistry configs)
    {
        var palette = configs.GetConfig<PaletteConfig>();
        var borderTop = new StyleBoxFlat
        {
            BorderColor = palette.SecondaryPalette.Base,
            BorderThickness = new Thickness(0, 1, 0, 0),
        };

        var borderBottom = new StyleBoxFlat
        {
            BorderColor = palette.SecondaryPalette.Base,
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
