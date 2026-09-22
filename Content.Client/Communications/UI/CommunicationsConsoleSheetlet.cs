using Content.Client.Resources;
using Content.Client.Stylesheets;
using Content.Client.Stylesheets.SheetletConfigs;
using Robust.Client.Graphics;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using static Content.Client.Stylesheets.StylesheetHelpers;

namespace Content.Client.Communications.UI;

/// <summary>
/// A sheetlet for the communications console, for the character limit labels.
/// </summary>
[Sheetlet]
public sealed class CommunicationsConsoleSheetlet : ISheetlet
{
    /// <summary>
    /// The name of a style class for char limit labels.
    /// </summary>
    public const string CharLimit = "CommsConsoleCharLimit";

    /// <summary>
    /// The name of a style class for char limit labels when the reference text has exceeded its limit.
    /// </summary>
    public const string CharLimitExceeded = "CommsConsoleCharLimitExceeded";

    public StyleRule[] Generate(SheetletConfigRegistry configs)
    {
        var fonts = configs.GetConfig<FontConfig>();

        return
        [
            E<Label>()
                .Class(CharLimit)
                .Font(fonts.Main.GetFont(8)),

            E<Label>()
                .Class(CharLimitExceeded)
                .Font(fonts.Main.GetFont(8))
                .FontColor(Color.Red)
        ];
    }
}
