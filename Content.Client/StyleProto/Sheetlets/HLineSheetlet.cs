using Content.Client.StyleProto.SheetletConfigs;
using Content.Client.UserInterface.Controls;
using JetBrains.Annotations;
using Robust.Client.Graphics;
using Robust.Client.UserInterface;
using static Content.Client.Stylesheets.StylesheetHelpers;
using StyleClass = Content.Client.Stylesheets.StyleClass;

namespace Content.Client.StyleProto.Sheetlets;

[Sheetlet]
[UsedImplicitly]
public sealed partial class HLineSheetlet : ISheetlet
{
    public StyleRule[] Generate(SheetletConfigRegistry configs)
    {
        var palette = configs.GetConfig<PaletteConfig>();

        return
        [
            E<HLine>()
                .Class(StyleClass.Positive)
                .Panel(new StyleBoxFlat(palette.PositivePalette.Text)),
            E<HLine>()
                .Class(StyleClass.Highlight)
                .Panel(new StyleBoxFlat(palette.HighlightPalette.Text)),
            E<HLine>()
                .Class(StyleClass.Negative)
                .Panel(new StyleBoxFlat(palette.NegativePalette.Text)),
        ];
    }
}
