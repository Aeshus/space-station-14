using Content.Client.StyleProto.SheetletConfigs;
using JetBrains.Annotations;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using static Robust.Client.UserInterface.StylesheetHelpers;

namespace Content.Client.StyleProto.Sheetlets;

[Sheetlet]
[UsedImplicitly]
public sealed class TestSheetlet : ISheetlet
{
    public StyleRule[]? Generate(SheetletConfigRegistry configs)
    {
        if (!configs.TryGetConfig<PaletteConfig>(out var palette))
            return null;

        return
        [
            Element<Label>().Prop(Label.StylePropertyFontColor, palette.Primary),
        ];
    }
}
