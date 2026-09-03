using Content.Client.MainMenu.UI;
using Content.Client.StyleProto.SheetletConfigs;
using Content.Client.Stylesheets.Fonts;
using JetBrains.Annotations;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using static Content.Client.Stylesheets.StylesheetHelpers;

namespace Content.Client.StyleProto.Sheetlets;

[Sheetlet]
[UsedImplicitly]
public sealed partial class MainMenuSheetlet : ISheetlet
{
    public StyleRule[] Generate(SheetletConfigRegistry configs)
    {
        var font = configs.GetConfig<FontConfig>();

        return
        [
            E<Button>()
                .Identifier(MainMenuControl.StyleIdentifierMainMenu)
                .ParentOf(E<Label>())
                .Font(font.BaseFont.GetFont(16, FontKind.Bold)),
            E<BoxContainer>()
                .Identifier(MainMenuControl.StyleIdentifierMainMenuVBox)
                .Prop(BoxContainer.StylePropertySeparation, 2),
        ];
    }
}
