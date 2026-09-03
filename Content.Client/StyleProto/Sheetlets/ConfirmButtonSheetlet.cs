using Content.Client.StyleProto.SheetletConfigs;
using Content.Client.UserInterface.Controls;
using JetBrains.Annotations;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using static Content.Client.Stylesheets.StylesheetHelpers;

namespace Content.Client.StyleProto.Sheetlets;

[Sheetlet]
[UsedImplicitly]
public sealed partial class ConfirmButtonSheetlet : ISheetlet
{
    public StyleRule[] Generate(SheetletConfigRegistry configs)
    {
        var palette = configs.GetConfig<PaletteConfig>();

        return
        [
            E<ConfirmButton>()
                .Pseudo(ConfirmButton.ConfirmPrefix + ContainerButton.StylePseudoClassNormal)
                .Prop(Control.StylePropertyModulateSelf, palette.NegativePalette.Element),

            E<ConfirmButton>()
                .Pseudo(ConfirmButton.ConfirmPrefix + ContainerButton.StylePseudoClassHover)
                .Prop(Control.StylePropertyModulateSelf, palette.NegativePalette.HoveredElement),

            E<ConfirmButton>()
                .Pseudo(ConfirmButton.ConfirmPrefix + ContainerButton.StylePseudoClassPressed)
                .Prop(Control.StylePropertyModulateSelf, palette.NegativePalette.PressedElement),

            E<ConfirmButton>()
                .Pseudo(ConfirmButton.ConfirmPrefix + ContainerButton.StylePseudoClassDisabled)
                .Prop(Control.StylePropertyModulateSelf, palette.NegativePalette.DisabledElement),
        ];
    }
}
