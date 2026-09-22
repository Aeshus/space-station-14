using Content.Client.Stylesheets;
using Content.Client.Stylesheets.SheetletConfigs;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using static Content.Client.Stylesheets.StylesheetHelpers;

namespace Content.Client.Construction.UI;

[Sheetlet]
public sealed class ConstructionMenuSheetlet : ISheetlet
{
    public StyleRule[] Generate(SheetletConfigRegistry configs)
    {
        var fonts = configs.GetConfig<FontConfig>();

        return
        [
            E<Label>()
                .Identifier("RecipeHistoryNavButtonLabel")
                .Font(fonts.Main.GetFont(8))
                .FontColor(Color.White),

            E<Label>()
                .Identifier("RecipeHistoryNavButtonLabel")
                .PseudoDisabled()
                .Font(fonts.Main.GetFont(8))
                .FontColor(Color.Gray),
        ];
    }
}
