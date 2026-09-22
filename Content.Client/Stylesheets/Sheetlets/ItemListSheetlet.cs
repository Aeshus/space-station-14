using Content.Client.Stylesheets.SheetletConfigs;
using Robust.Client.Graphics;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using static Content.Client.Stylesheets.StylesheetHelpers;

namespace Content.Client.Stylesheets.Sheetlets;

[Sheetlet]
public sealed class ItemListSheetlet : ISheetlet
{
    public StyleRule[] Generate(SheetletConfigRegistry configs)
    {
        var palette = configs.GetConfig<PaletteConfig>();

        var boxBackground = new StyleBoxFlat { BackgroundColor = palette.PrimaryPalette.Background };
        var boxItemBackground = Box(palette.PrimaryPalette.Background);
        var boxSelected = Box(palette.PrimaryPalette.Element);
        var boxDisabled = Box(palette.PrimaryPalette.BackgroundDark);

        return
        [
            E<ItemList>()
                .Prop(ItemList.StylePropertyBackground, boxBackground)
                .Prop(ItemList.StylePropertyItemBackground, boxItemBackground)
                .Prop(ItemList.StylePropertyDisabledItemBackground, boxDisabled)
                .Prop(ItemList.StylePropertySelectedItemBackground, boxSelected),
        ];
    }

    private static StyleBoxFlat Box(Color c)
    {
        return new StyleBoxFlat(c)
            // TODO: dont hardcode these maybe
            {
                ContentMarginLeftOverride = 4,
                ContentMarginTopOverride = 2,
                ContentMarginRightOverride = 4,
                ContentMarginBottomOverride = 2,
            };
    }
}
