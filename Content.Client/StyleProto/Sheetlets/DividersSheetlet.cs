using System.Numerics;
using Content.Client.StyleProto.SheetletConfigs;
using JetBrains.Annotations;
using Robust.Client.Graphics;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using static Content.Client.Stylesheets.StylesheetHelpers;
using StyleClass = Content.Client.Stylesheets.StyleClass;

namespace Content.Client.StyleProto.Sheetlets;

[Sheetlet]
[UsedImplicitly]
public sealed partial class DividersSheetlet : ISheetlet
{
    public StyleRule[] Generate(SheetletConfigRegistry configs)
    {
        var palette = configs.GetConfig<PaletteConfig>();
        var boxHighDivider = new StyleBoxFlat
        {
            BackgroundColor = palette.HighlightPalette.Base,
            ContentMarginBottomOverride = 2,
            ContentMarginLeftOverride = 2,
        };

        var boxLowDivider = new StyleBoxFlat(palette.SecondaryPalette.TextDark);

        // High divider and low divider styles are very inconsistent, but changing them is outside this migration.
        return
        [
            E<PanelContainer>()
                .Class(StyleClass.LowDivider)
                .Panel(boxLowDivider)
                .MinSize(new Vector2(2, 2)),
            E<PanelContainer>().Class(StyleClass.HighDivider).Panel(boxHighDivider),
        ];
    }
}
