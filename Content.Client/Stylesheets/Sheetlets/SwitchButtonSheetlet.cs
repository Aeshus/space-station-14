using Content.Client.Resources;
using Content.Client.Stylesheets.SheetletConfigs;
using Content.Client.UserInterface.Controls;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using static Content.Client.Stylesheets.StylesheetHelpers;

namespace Content.Client.Stylesheets.Sheetlets;

[Sheetlet]
public sealed partial class SwitchButtonSheetlet : ISheetlet
{
    [Dependency] private IResourceCache _resCache = default!;

    public StyleRule[] Generate(SheetletConfigRegistry configs)
    {
        var switchButtons = configs.GetConfig<SwitchButtonConfig>();
        var palettes = configs.GetConfig<PaletteConfig>();

        var trackFillTex = _resCache.GetTexture(switchButtons.SwitchButtonTrackFillPath);
        var trackOutlineTex = _resCache.GetTexture(switchButtons.SwitchButtonTrackOutlinePath);
        var thumbFillTex = _resCache.GetTexture(switchButtons.SwitchButtonThumbFillPath);
        var thumbOutlineTex = _resCache.GetTexture(switchButtons.SwitchButtonThumbOutlinePath);
        var symbolOffTex = _resCache.GetTexture(switchButtons.SwitchButtonSymbolOffPath);
        var symbolOnTex = _resCache.GetTexture(switchButtons.SwitchButtonSymbolOnPath);

        return
        [
            // SwitchButton
            E<SwitchButton>().Prop(SwitchButton.StylePropertySeparation, 10),

            E<SwitchButton>()
                .ParentOf(E<TextureRect>().Class(SwitchButton.StyleClassTrackFill))
                .Prop(TextureRect.StylePropertyTexture, trackFillTex)
                .Modulate(palettes.SecondaryPalette.BackgroundDark),

            E<SwitchButton>()
                .ParentOf(E<TextureRect>().Class(SwitchButton.StyleClassTrackOutline))
                .Prop(TextureRect.StylePropertyTexture, trackOutlineTex)
                .Modulate(palettes.SecondaryPalette.Text),

            E<SwitchButton>()
                .ParentOf(E<TextureRect>().Class(SwitchButton.StyleClassThumbFill))
                .Prop(TextureRect.StylePropertyTexture, thumbFillTex)
                .Modulate(palettes.PrimaryPalette.Element)
                .HorizontalAlignment(Control.HAlignment.Left),

            E<SwitchButton>()
                .ParentOf(E<TextureRect>().Class(SwitchButton.StyleClassThumbOutline))
                .Prop(TextureRect.StylePropertyTexture, thumbOutlineTex)
                .Modulate(palettes.PrimaryPalette.Text)
                .HorizontalAlignment(Control.HAlignment.Left),

            E<SwitchButton>()
                .ParentOf(E<TextureRect>().Class(SwitchButton.StyleClassSymbol))
                .Prop(TextureRect.StylePropertyTexture, symbolOffTex)
                .Modulate(palettes.SecondaryPalette.Text),

            // Pressed styles
            E<SwitchButton>()
                .PseudoPressed()
                .ParentOf(E<TextureRect>().Class(SwitchButton.StyleClassTrackFill))
                .Modulate(palettes.PositivePalette.Text),

            E<SwitchButton>()
                .PseudoPressed()
                .ParentOf(E<TextureRect>().Class(SwitchButton.StyleClassSymbol))
                .Prop(TextureRect.StylePropertyTexture, symbolOnTex)
                .Modulate(Color.White), // Same color as text, not yet in any of the palettes

            E<SwitchButton>()
                .PseudoPressed()
                .ParentOf(E<TextureRect>().Class(SwitchButton.StyleClassThumbFill))
                .HorizontalAlignment(Control.HAlignment.Right),

            E<SwitchButton>()
                .PseudoPressed()
                .ParentOf(E<TextureRect>().Class(SwitchButton.StyleClassThumbOutline))
                .HorizontalAlignment(Control.HAlignment.Right),

            // Disabled styles
            E<SwitchButton>()
                .PseudoDisabled()
                .ParentOf(E<TextureRect>().Class(SwitchButton.StyleClassTrackFill))
                .Modulate(palettes.SecondaryPalette.DisabledElement),

            E<SwitchButton>()
                .PseudoDisabled()
                .ParentOf(E<TextureRect>().Class(SwitchButton.StyleClassTrackOutline))
                .Modulate(palettes.SecondaryPalette.DisabledElement),

            E<SwitchButton>()
                .PseudoDisabled()
                .ParentOf(E<TextureRect>().Class(SwitchButton.StyleClassThumbFill))
                .Modulate(palettes.PrimaryPalette.DisabledElement),

            E<SwitchButton>()
                .PseudoDisabled()
                .ParentOf(E<TextureRect>().Class(SwitchButton.StyleClassThumbOutline))
                .Modulate(palettes.PrimaryPalette.TextDark),

            E<SwitchButton>()
                .PseudoDisabled()
                .ParentOf(E<TextureRect>().Class(SwitchButton.StyleClassSymbol))
                .Modulate(palettes.SecondaryPalette.TextDark),

            E<SwitchButton>()
                .PseudoDisabled()
                .ParentOf(E<Label>())
                .Modulate(palettes.PrimaryPalette.TextDark),

            // Both pressed & disabled styles
            // Note that some of the pressed-only and disabled-only styles do not conflict
            // and will also be used
            E<SwitchButton>()
                .PseudoPressed()
                .PseudoDisabled()
                .ParentOf(E<TextureRect>().Class(SwitchButton.StyleClassTrackFill))
                .Modulate(palettes.PositivePalette.DisabledElement),

            E<SwitchButton>()
                .PseudoPressed()
                .PseudoDisabled()
                .ParentOf(E<TextureRect>().Class(SwitchButton.StyleClassSymbol))
                .Modulate(palettes.PositivePalette.Text),
        ];
    }
}
