using Content.Client.StyleProto.SheetletConfigs;
using JetBrains.Annotations;
using Robust.Client.Graphics;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using static Content.Client.Stylesheets.StylesheetHelpers;

namespace Content.Client.StyleProto.Sheetlets;

[Sheetlet]
[UsedImplicitly]
public sealed partial class ItemListSheetlet : ISheetlet
{
    private static StyleBoxFlat Box(Color color)
    {
        return new StyleBoxFlat(color)
        {
            ContentMarginLeftOverride = 4,
            ContentMarginTopOverride = 2,
            ContentMarginRightOverride = 4,
            ContentMarginBottomOverride = 2,
        };
    }

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
}
