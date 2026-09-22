using Content.Client.Stylesheets.SheetletConfigs;
using Content.Client.UserInterface.Controls;
using Robust.Client.UserInterface;
using static Content.Client.Stylesheets.StylesheetHelpers;

namespace Content.Client.Stylesheets.Sheetlets;

[Sheetlet]
public sealed class SeparatorSheetlet : ISheetlet
{
    public StyleRule[] Generate(SheetletConfigRegistry configs)
    {
        var palettes = configs.GetConfig<PaletteConfig>();

        return
        [
            E<Separator>()
                .Class(StyleClass.LowDivider)
                .Prop(Separator.StylePropertyColor, palettes.SecondaryPalette.TextDark),
            E<Separator>()
                .Class(StyleClass.HighDivider)
                .Prop(Separator.StylePropertyColor, palettes.HighlightPalette.Base),
            E<Separator>()
                .Class(StyleClass.Positive)
                .Prop(Separator.StylePropertyColor, palettes.PositivePalette.Text),
            E<Separator>()
                .Class(StyleClass.Highlight)
                .Prop(Separator.StylePropertyColor, palettes.HighlightPalette.Text),
            E<Separator>()
                .Class(StyleClass.Negative)
                .Prop(Separator.StylePropertyColor, palettes.NegativePalette.Text),
        ];
    }
}
