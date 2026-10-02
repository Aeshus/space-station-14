using Content.Client.Stylesheets.SheetletConfigs;
using Content.Client.UserInterface.Controls;
using Robust.Client.Graphics;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using static Content.Client.Stylesheets.StylesheetHelpers;

namespace Content.Client.Stylesheets.Sheetlets;

[Sheetlet]
public sealed class TextLinkLabelSheetlet : ISheetlet
{
    public StyleRule[] Generate(SheetletConfigRegistry configs)
    {
        var fonts = configs.GetConfig<FontConfig>();

        return
        [
            E<TextLinkLabel>().Prop(Label.StylePropertyFont, fonts.Main.GetFont(12, FontWeight.Bold)),
        ];
    }
}
