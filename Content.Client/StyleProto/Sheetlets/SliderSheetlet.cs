using Content.Client.Resources;
using Content.Client.StyleProto.SheetletConfigs;
using JetBrains.Annotations;
using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using static Content.Client.Stylesheets.StylesheetHelpers;

namespace Content.Client.StyleProto.Sheetlets;

[Sheetlet]
[UsedImplicitly]
public sealed partial class SliderSheetlet : ISheetlet
{
    [Dependency] private IResourceCache _resourceCache = default!;

    public StyleRule[] Generate(SheetletConfigRegistry configs)
    {
        var slider = configs.GetConfig<SliderConfig>();
        var palette = configs.GetConfig<PaletteConfig>();
        var sliderFillTex = _resourceCache.GetTexture(slider.SliderFillPath);

        var sliderFillBox = new StyleBoxTexture
        {
            Texture = sliderFillTex,
            Modulate = palette.PositivePalette.TextDark,
        };

        var sliderBackBox = new StyleBoxTexture
        {
            Texture = sliderFillTex,
            Modulate = palette.SecondaryPalette.BackgroundDark,
        };

        var sliderForeBox = new StyleBoxTexture
        {
            Texture = _resourceCache.GetTexture(slider.SliderOutlinePath),
            Modulate = Color.FromHex("#494949"),
        };

        var sliderGrabBox = new StyleBoxTexture
        {
            Texture = _resourceCache.GetTexture(slider.SliderGrabber),
        };

        sliderFillBox.SetPatchMargin(StyleBox.Margin.All, 12);
        sliderBackBox.SetPatchMargin(StyleBox.Margin.All, 12);
        sliderForeBox.SetPatchMargin(StyleBox.Margin.All, 12);
        sliderGrabBox.SetPatchMargin(StyleBox.Margin.All, 12);

        return
        [
            E<Slider>()
                .Prop(Slider.StylePropertyBackground, sliderBackBox)
                .Prop(Slider.StylePropertyForeground, sliderForeBox)
                .Prop(Slider.StylePropertyGrabber, sliderGrabBox)
                .Prop(Slider.StylePropertyFill, sliderFillBox),
        ];
    }
}
