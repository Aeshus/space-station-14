using Content.Client.StyleProto.SheetletConfigs;
using Content.Client.UserInterface.Controls;
using JetBrains.Annotations;
using Robust.Client.UserInterface;
using static Content.Client.Stylesheets.StylesheetHelpers;

namespace Content.Client.StyleProto.Sheetlets;

[Sheetlet]
[UsedImplicitly]
public sealed partial class SegmentedBarChartSheetlet : ISheetlet
{
    public StyleRule[] Generate(SheetletConfigRegistry configs)
    {
        var palette = configs.GetConfig<PaletteConfig>();

        return
        [
            E<SegmentedBarChart>()
                .Prop(SegmentedBarChart.StylePropertyNotchColor, Color.White.WithAlpha(0.25f))
                .Prop(SegmentedBarChart.StylePropertyBackgroundColor, palette.SecondaryPalette.BackgroundDark)
                .Prop(SegmentedBarChart.StylePropertyGap, 0f)
                .Prop(SegmentedBarChart.StylePropertyMediumNotchInterval, 5)
                .Prop(SegmentedBarChart.StylePropertyBigNotchInterval, 10)
                .Prop(SegmentedBarChart.StylePropertyMinEntryWidth, 0f)
                .Prop(SegmentedBarChart.StylePropertyMinSmallNotchScreenDistance, 2)
                .Prop(SegmentedBarChart.StylePropertySmallNotchHeight, 0.1f)
                .Prop(SegmentedBarChart.StylePropertyMediumNotchHeight, 0.25f)
                .Prop(SegmentedBarChart.StylePropertyBigNotchHeight, 1f)
                .Prop(SegmentedBarChart.StylePropertyAnimated, true)
                .Prop(SegmentedBarChart.StylePropertyShowBackground, true)
                .Prop(SegmentedBarChart.StylePropertyShowRuler, true),
            E<SegmentedBarChart>()
                .Class(SegmentedBarChart.StyleClassClassicSplitBar)
                .Prop(SegmentedBarChart.StylePropertyGap, 5f)
                .Prop(SegmentedBarChart.StylePropertyMinEntryWidth, 12f)
                .Prop(SegmentedBarChart.StylePropertyShowBackground, false)
                .Prop(SegmentedBarChart.StylePropertyShowRuler, false),
        ];
    }
}
