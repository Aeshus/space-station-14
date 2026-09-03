using Content.Client.StyleProto.SheetletConfigs;
using Content.Client.Stylesheets.Fonts;
using Content.Client.Stylesheets.Palette;
using JetBrains.Annotations;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using static Content.Client.Stylesheets.StylesheetHelpers;
using StyleClass = Content.Client.Stylesheets.StyleClass;

namespace Content.Client.StyleProto.Sheetlets;

[Sheetlet]
[UsedImplicitly]
public sealed partial class LabelSheetlet : ISheetlet
{
    public StyleRule[] Generate(SheetletConfigRegistry configs)
    {
        var font = configs.GetConfig<FontConfig>();
        var palette = configs.GetConfig<PaletteConfig>();
        var robotoMonoBold11 = font.MonoFont.GetFont(11, FontKind.Bold);
        var robotoMonoBold12 = font.MonoFont.GetFont(12, FontKind.Bold);
        var robotoMonoBold14 = font.MonoFont.GetFont(14, FontKind.Bold);

        return
        [
            E<Label>()
                .Class(StyleClass.LabelHeading)
                .Font(font.BaseFont.GetFont(16, FontKind.Bold))
                .FontColor(palette.HighlightPalette.Text),
            E<Label>()
                .Class(StyleClass.LabelHeadingBigger)
                .Font(font.BaseFont.GetFont(20, FontKind.Bold))
                .FontColor(palette.HighlightPalette.Text),
            E<Label>()
                .Class(StyleClass.LabelSubHeading)
                .Font(font.BaseFont.GetFont(14, FontKind.Italic))
                .FontColor(palette.HighlightPalette.TextDark),
            E<Label>()
                .Class(StyleClass.LabelSubText)
                .Font(font.BaseFont.GetFont(10))
                .FontColor(Color.DarkGray),
            E<Label>()
                .Class(StyleClass.LabelKeyText)
                .Font(font.BaseFont.GetFont(12, FontKind.Bold))
                .FontColor(palette.HighlightPalette.Text),
            E<Label>()
                .Class(StyleClass.LabelWeak)
                .FontColor(Color.DarkGray),

            E<Label>()
                .Class(StyleClass.Positive)
                .FontColor(palette.PositivePalette.Text),
            E<Label>()
                .Class(StyleClass.Negative)
                .FontColor(palette.NegativePalette.Text),
            E<Label>()
                .Class(StyleClass.Highlight)
                .FontColor(palette.HighlightPalette.Text),

            E<Label>()
                .Class(StyleClass.StatusGood)
                .FontColor(Palettes.Status.Good),
            E<Label>()
                .Class(StyleClass.StatusOkay)
                .FontColor(Palettes.Status.Okay),
            E<Label>()
                .Class(StyleClass.StatusWarning)
                .FontColor(Palettes.Status.Warning),
            E<Label>()
                .Class(StyleClass.StatusBad)
                .FontColor(Palettes.Status.Bad),
            E<Label>()
                .Class(StyleClass.StatusCritical)
                .FontColor(Palettes.Status.Critical),

            // Console text
            E<Label>()
                .Class(StyleClass.LabelMonospaceText)
                .Prop(Label.StylePropertyFont, robotoMonoBold11),
            E<Label>()
                .Class(StyleClass.LabelMonospaceSubHeading)
                .Prop(Label.StylePropertyFont, robotoMonoBold12),
            E<Label>()
                .Class(StyleClass.LabelMonospaceHeading)
                .Prop(Label.StylePropertyFont, robotoMonoBold14),
        ];
    }
}
