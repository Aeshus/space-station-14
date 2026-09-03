using Content.Client.StyleProto.SheetletConfigs;
using JetBrains.Annotations;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using static Robust.Client.UserInterface.StylesheetHelpers;

namespace Content.Client.StyleProto.Sheetlets;

[Sheetlet]
[UsedImplicitly]
public sealed partial class PaletteSheetlet : ISheetlet
{
    public StyleRule[] Generate(SheetletConfigRegistry configs)
    {
        var palette = configs.GetConfig<PaletteConfig>();

        return
        [
            Element<Label>().Prop(Label.StylePropertyFontColor, palette.PrimaryPalette.Text),
        ];
    }
}
