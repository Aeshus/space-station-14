using JetBrains.Annotations;
using Robust.Client.Graphics;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using static Content.Client.Stylesheets.StylesheetHelpers;

namespace Content.Client.StyleProto.Sheetlets;

[Sheetlet]
[UsedImplicitly]
public sealed partial class ProgressBarSheetlet : ISheetlet
{
    public StyleRule[] Generate(SheetletConfigRegistry configs)
    {
        var progressBarBackground = new StyleBoxFlat
        {
            BackgroundColor = new Color(0.25f, 0.25f, 0.25f),
        };
        progressBarBackground.SetContentMarginOverride(StyleBox.Margin.Vertical, 14.5f);
        var progressBarForeground = new StyleBoxFlat
        {
            BackgroundColor = new Color(0.25f, 0.50f, 0.25f),
        };
        progressBarForeground.SetContentMarginOverride(StyleBox.Margin.Vertical, 14.5f);

        return
        [
            E<ProgressBar>()
                .Prop(ProgressBar.StylePropertyBackground, progressBarBackground)
                .Prop(ProgressBar.StylePropertyForeground, progressBarForeground),
        ];
    }
}
