using Content.Client.Communications.UI;
using Content.Client.StyleProto.SheetletConfigs;
using JetBrains.Annotations;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using static Content.Client.Stylesheets.StylesheetHelpers;

namespace Content.Client.StyleProto.Sheetlets;

/// <summary>
/// A sheetlet for the communications console character-limit labels.
/// </summary>
[Sheetlet]
[UsedImplicitly]
public sealed partial class CommunicationsConsoleSheetlet : ISheetlet
{
    public StyleRule[] Generate(SheetletConfigRegistry configs)
    {
        var font = configs.GetConfig<FontConfig>();

        return
        [
            E<Label>()
                .Class(ICommunicationsConsoleConfig.CharLimit)
                .Font(font.BaseFont.GetFont(8)),

            E<Label>()
                .Class(ICommunicationsConsoleConfig.CharLimitExceeded)
                .Font(font.BaseFont.GetFont(8))
                .FontColor(Color.Red),
        ];
    }
}
