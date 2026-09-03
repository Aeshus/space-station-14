using Content.Client.StyleProto.SheetletConfigs;
using Content.Client.Stylesheets.Fonts;
using JetBrains.Annotations;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using static Content.Client.Stylesheets.StylesheetHelpers;
using StyleClass = Content.Client.Stylesheets.StyleClass;

namespace Content.Client.StyleProto.Sheetlets.Hud;

[Sheetlet]
[UsedImplicitly]
public sealed partial class ItemStatusSheetlet : ISheetlet
{
    public StyleRule[] Generate(SheetletConfigRegistry configs)
    {
        var font = configs.GetConfig<FontConfig>();

        return
        [
            E()
                .Class(StyleClass.ItemStatus)
                .Prop("font", font.BaseFont.GetFont(10)),

            E()
                .Class(StyleClass.ItemStatusNotHeld)
                .Prop("font", font.BaseFont.GetFont(10, FontKind.Italic))
                .Prop("font-color", Color.Gray),

            E<RichTextLabel>()
                .Class(StyleClass.ItemStatus)
                .Prop(nameof(RichTextLabel.LineHeightScale), 0.7f)
                .Prop(nameof(Control.Margin), new Thickness(0, 0, 0, -6)),
        ];
    }
}
