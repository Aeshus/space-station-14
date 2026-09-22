using Content.Client.Stylesheets.Fonts;
using Content.Client.Stylesheets.SheetletConfigs;
using Robust.Client.Graphics;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using static Content.Client.Stylesheets.StylesheetHelpers;

namespace Content.Client.Stylesheets.Sheetlets.Hud;

[Sheetlet]
public sealed class ItemStatusSheetlet : ISheetlet
{
    public StyleRule[] Generate(SheetletConfigRegistry configs)
    {
        var fonts = configs.GetConfig<FontConfig>();

        return
        [
            E()
                .Class(StyleClass.ItemStatus)
                .Prop("font", fonts.Main.GetFont(10)),

            E()
                .Class(StyleClass.ItemStatusNotHeld)
                .Prop("font", fonts.Main.GetFont(10, slant: FontSlant.Italic))
                .Prop("font-color", Color.Gray),

            E<RichTextLabel>()
                .Class(StyleClass.ItemStatus)
                .Prop(nameof(RichTextLabel.LineHeightScale), 0.7f)
                .Prop(nameof(Control.Margin), new Thickness(0, 0, 0, -6)),
        ];
    }
}
