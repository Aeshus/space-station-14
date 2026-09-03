using Content.Client.StyleProto.SheetletConfigs;
using Content.Client.UserInterface.Controls;
using JetBrains.Annotations;
using Robust.Client.Graphics;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using static Content.Client.Stylesheets.StylesheetHelpers;

namespace Content.Client.StyleProto.Sheetlets;

[Sheetlet]
[UsedImplicitly]
public sealed partial class ListContainerSheetlet : ISheetlet
{
    public StyleRule[] Generate(SheetletConfigRegistry configs)
    {
        var button = configs.GetConfig<ButtonConfig>();
        var box = new StyleBoxFlat { BackgroundColor = Color.White };

        var rules = new List<StyleRule>(
        [
            E<ContainerButton>()
                .Class(ListContainer.StyleClassListContainerButton)
                .Box(box),
        ]);
        ButtonSheetlet.MakeButtonRules<ContainerButton>(rules,
            button.ButtonPalette,
            ListContainer.StyleClassListContainerButton);

        return rules.ToArray();
    }
}
