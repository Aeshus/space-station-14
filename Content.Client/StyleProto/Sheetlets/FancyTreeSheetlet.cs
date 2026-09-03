using Content.Client.StyleProto.SheetletConfigs;
using Content.Client.UserInterface.Controls.FancyTree;
using JetBrains.Annotations;
using Robust.Client.Graphics;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using static Content.Client.Stylesheets.StylesheetHelpers;

namespace Content.Client.StyleProto.Sheetlets;

[Sheetlet]
[UsedImplicitly]
public sealed partial class FancyTreeSheetlet : ISheetlet
{
    public StyleRule[] Generate(SheetletConfigRegistry configs)
    {
        var palette = configs.GetConfig<PaletteConfig>();

        return
        [
            E<ContainerButton>()
                .Identifier(TreeItem.StyleIdentifierTreeButton)
                .Class(TreeItem.StyleClassEvenRow)
                .Prop(ContainerButton.StylePropertyStyleBox,
                    new StyleBoxFlat(palette.SecondaryPalette.BackgroundLight)),
            E<ContainerButton>()
                .Identifier(TreeItem.StyleIdentifierTreeButton)
                .Class(TreeItem.StyleClassOddRow)
                .Prop(ContainerButton.StylePropertyStyleBox, new StyleBoxFlat(palette.SecondaryPalette.Background)),

            E<ContainerButton>()
                .Identifier(TreeItem.StyleIdentifierTreeButton)
                .Class(TreeItem.StyleClassSelected)
                .Prop(ContainerButton.StylePropertyStyleBox, new StyleBoxFlat(palette.PrimaryPalette.Element)),

            E<ContainerButton>()
                .Identifier(TreeItem.StyleIdentifierTreeButton)
                .Pseudo(ContainerButton.StylePseudoClassHover)
                .Prop(ContainerButton.StylePropertyStyleBox, new StyleBoxFlat(palette.PrimaryPalette.HoveredElement)),
        ];
    }
}
