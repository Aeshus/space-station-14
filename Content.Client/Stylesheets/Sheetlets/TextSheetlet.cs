using Content.Client.Resources;
using Content.Client.Stylesheets.Fonts;
using Content.Client.Stylesheets.SheetletConfigs;
using Robust.Client.Graphics;
using Robust.Client.UserInterface;
using static Content.Client.Stylesheets.StylesheetHelpers;

namespace Content.Client.Stylesheets.Sheetlets;

/// These are not in `LabelSheetlet` because a label is not the only thing you might want to be monospaced.
[Sheetlet]
public sealed class TextSheetlet : ISheetlet
{
    public StyleRule[] Generate(SheetletConfigRegistry configs)
    {
        var fonts = configs.GetConfig<FontConfig>();

        return
        [
            E().Class(StyleClass.Monospace).Font(fonts.Monospace.GetFont(12)),
            E().Class(StyleClass.Italic).Font(fonts.Main.GetFont(12, slant: FontSlant.Italic)),
            E().Class(StyleClass.FontLarge).Font(fonts.Main.GetFont(14)),
            E().Class(StyleClass.FontSmall).Font(fonts.Main.GetFont(10)),
        ];
    }
}
