using Content.Client.StyleProto.SheetletConfigs;
using JetBrains.Annotations;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using static Content.Client.Stylesheets.StylesheetHelpers;

namespace Content.Client.StyleProto.Sheetlets;

[Sheetlet]
[UsedImplicitly]
public sealed partial class ConstructionMenuSheetlet : ISheetlet
{
    public StyleRule[] Generate(SheetletConfigRegistry configs)
    {
        var font = configs.GetConfig<FontConfig>();

        return
        [
            E<Label>()
                .Identifier("RecipeHistoryNavButtonLabel")
                .Font(font.BaseFont.GetFont(8))
                .FontColor(Color.White),

            E<Label>()
                .Identifier("RecipeHistoryNavButtonLabel")
                .PseudoDisabled()
                .Font(font.BaseFont.GetFont(8))
                .FontColor(Color.Gray),
        ];
    }
}
