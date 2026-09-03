using Content.Client.StyleProto.SheetletConfigs;
using Content.Client.UserInterface.Screens;
using JetBrains.Annotations;
using Robust.Client.Graphics;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using static Content.Client.Stylesheets.StylesheetHelpers;

namespace Content.Client.StyleProto.Sheetlets.Hud;

[Sheetlet]
[UsedImplicitly]
public sealed partial class ChatGameScreenSheetlet : ISheetlet
{
    public StyleRule[] Generate(SheetletConfigRegistry configs)
    {
        var palette = configs.GetConfig<PaletteConfig>();

        return
        [
            E()
                .Class(SeparatedChatGameScreen.StyleClassChatContainer)
                .Panel(new StyleBoxFlat(palette.SecondaryPalette.Background)),
            E<OutputPanel>()
                .Class(SeparatedChatGameScreen.StyleClassChatOutput)
                .Panel(new StyleBoxFlat(palette.SecondaryPalette.BackgroundDark)),
        ];
    }
}
