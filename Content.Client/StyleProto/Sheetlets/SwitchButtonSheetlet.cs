using Content.Client.Resources;
using Content.Client.StyleProto.SheetletConfigs;
using Content.Client.UserInterface.Controls;
using JetBrains.Annotations;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using static Content.Client.Stylesheets.StylesheetHelpers;

namespace Content.Client.StyleProto.Sheetlets;

[Sheetlet]
[UsedImplicitly]
public sealed partial class SwitchButtonSheetlet : ISheetlet
{
    [Dependency] private IResourceCache _resourceCache = default!;

    public StyleRule[] Generate(SheetletConfigRegistry configs)
    {
        var button = configs.GetConfig<SwitchButtonConfig>();
        var palette = configs.GetConfig<PaletteConfig>();
        var trackFillTex = _resourceCache.GetTexture(button.SwitchButtonTrackFillPath);
        var trackOutlineTex = _resourceCache.GetTexture(button.SwitchButtonTrackOutlinePath);
        var thumbFillTex = _resourceCache.GetTexture(button.SwitchButtonThumbFillPath);
        var thumbOutlineTex = _resourceCache.GetTexture(button.SwitchButtonThumbOutlinePath);
        var symbolOffTex = _resourceCache.GetTexture(button.SwitchButtonSymbolOffPath);
        var symbolOnTex = _resourceCache.GetTexture(button.SwitchButtonSymbolOnPath);

        return
        [
            E<SwitchButton>().Prop(SwitchButton.StylePropertySeparation, 10),

            E<SwitchButton>()
                .ParentOf(E<TextureRect>().Class(SwitchButton.StyleClassTrackFill))
                .Prop(TextureRect.StylePropertyTexture, trackFillTex)
                .Modulate(palette.SecondaryPalette.BackgroundDark),

            E<SwitchButton>()
                .ParentOf(E<TextureRect>().Class(SwitchButton.StyleClassTrackOutline))
                .Prop(TextureRect.StylePropertyTexture, trackOutlineTex)
                .Modulate(palette.SecondaryPalette.Text),

            E<SwitchButton>()
                .ParentOf(E<TextureRect>().Class(SwitchButton.StyleClassThumbFill))
                .Prop(TextureRect.StylePropertyTexture, thumbFillTex)
                .Modulate(palette.PrimaryPalette.Element)
                .HorizontalAlignment(Control.HAlignment.Left),

            E<SwitchButton>()
                .ParentOf(E<TextureRect>().Class(SwitchButton.StyleClassThumbOutline))
                .Prop(TextureRect.StylePropertyTexture, thumbOutlineTex)
                .Modulate(palette.PrimaryPalette.Text)
                .HorizontalAlignment(Control.HAlignment.Left),

            E<SwitchButton>()
                .ParentOf(E<TextureRect>().Class(SwitchButton.StyleClassSymbol))
                .Prop(TextureRect.StylePropertyTexture, symbolOffTex)
                .Modulate(palette.SecondaryPalette.Text),

            E<SwitchButton>()
                .PseudoPressed()
                .ParentOf(E<TextureRect>().Class(SwitchButton.StyleClassTrackFill))
                .Modulate(palette.PositivePalette.Text),

            E<SwitchButton>()
                .PseudoPressed()
                .ParentOf(E<TextureRect>().Class(SwitchButton.StyleClassSymbol))
                .Prop(TextureRect.StylePropertyTexture, symbolOnTex)
                .Modulate(Color.White),

            E<SwitchButton>()
                .PseudoPressed()
                .ParentOf(E<TextureRect>().Class(SwitchButton.StyleClassThumbFill))
                .HorizontalAlignment(Control.HAlignment.Right),

            E<SwitchButton>()
                .PseudoPressed()
                .ParentOf(E<TextureRect>().Class(SwitchButton.StyleClassThumbOutline))
                .HorizontalAlignment(Control.HAlignment.Right),

            E<SwitchButton>()
                .PseudoDisabled()
                .ParentOf(E<TextureRect>().Class(SwitchButton.StyleClassTrackFill))
                .Modulate(palette.SecondaryPalette.DisabledElement),

            E<SwitchButton>()
                .PseudoDisabled()
                .ParentOf(E<TextureRect>().Class(SwitchButton.StyleClassTrackOutline))
                .Modulate(palette.SecondaryPalette.DisabledElement),

            E<SwitchButton>()
                .PseudoDisabled()
                .ParentOf(E<TextureRect>().Class(SwitchButton.StyleClassThumbFill))
                .Modulate(palette.PrimaryPalette.DisabledElement),

            E<SwitchButton>()
                .PseudoDisabled()
                .ParentOf(E<TextureRect>().Class(SwitchButton.StyleClassThumbOutline))
                .Modulate(palette.PrimaryPalette.TextDark),

            E<SwitchButton>()
                .PseudoDisabled()
                .ParentOf(E<TextureRect>().Class(SwitchButton.StyleClassSymbol))
                .Modulate(palette.SecondaryPalette.TextDark),

            E<SwitchButton>()
                .PseudoDisabled()
                .ParentOf(E<Label>())
                .Modulate(palette.PrimaryPalette.TextDark),

            E<SwitchButton>()
                .PseudoPressed()
                .PseudoDisabled()
                .ParentOf(E<TextureRect>().Class(SwitchButton.StyleClassTrackFill))
                .Modulate(palette.PositivePalette.DisabledElement),

            E<SwitchButton>()
                .PseudoPressed()
                .PseudoDisabled()
                .ParentOf(E<TextureRect>().Class(SwitchButton.StyleClassSymbol))
                .Modulate(palette.PositivePalette.Text),
        ];
    }
}
