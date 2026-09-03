using System.Numerics;
using Content.Client.Resources;
using Content.Client.StyleProto.SheetletConfigs;
using Content.Client.Stylesheets.Palette;
using JetBrains.Annotations;
using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using static Content.Client.Stylesheets.StylesheetHelpers;
using StyleClass = Content.Client.Stylesheets.StyleClass;

namespace Content.Client.StyleProto.Sheetlets;

[Sheetlet]
[UsedImplicitly]
public sealed partial class ButtonSheetlet : ISheetlet
{
    [Dependency] private IResourceCache _resourceCache = default!;

    public StyleRule[] Generate(SheetletConfigRegistry configs)
    {
        var button = configs.GetConfig<ButtonConfig>();
        var icon = configs.GetConfig<IconConfig>();
        var palette = configs.GetConfig<PaletteConfig>();
        var font = configs.GetConfig<FontConfig>();

        var crossTex = _resourceCache.GetTexture(icon.CrossIconPath);
        var refreshTex = _resourceCache.GetTexture(icon.RefreshIconPath);
        var helpTex = _resourceCache.GetTexture(icon.HelpIconPath);

        var rules = new List<StyleRule>
        {
            // Set textures for the kinds of buttons
            CButton()
                .Box(StyleBoxHelpers.BaseStyleBox(_resourceCache, button)),
            CButton()
                .Class(StyleClass.ButtonOpenLeft)
                .Box(StyleBoxHelpers.OpenLeftStyleBox(_resourceCache, button)),
            CButton()
                .Class(StyleClass.ButtonOpenRight)
                .Box(StyleBoxHelpers.OpenRightStyleBox(_resourceCache, button)),
            CButton()
                .Class(StyleClass.ButtonOpenBoth)
                .Box(StyleBoxHelpers.SquareStyleBox(_resourceCache, button)),
            CButton()
                .Class(StyleClass.ButtonSquare)
                .Box(StyleBoxHelpers.SquareStyleBox(_resourceCache, button)),
            CButton()
                .Class(StyleClass.ButtonSmall)
                .Box(StyleBoxHelpers.SmallStyleBox(_resourceCache, button)),
            CButton()
                .Class(StyleClass.ButtonSmall)
                .ParentOf(E<Label>())
                .Font(font.BaseFont.GetFont(8)),
            CButton().Class(StyleClass.ButtonBig).ParentOf(E<Label>()).Font(font.BaseFont.GetFont(16)),

            // Cross Button (Red)
            E<TextureButton>()
                .Class(StyleClass.CrossButtonRed)
                .Prop(TextureButton.StylePropertyTexture, crossTex),

            // Refresh Button
            E<TextureButton>()
                .Class(StyleClass.RefreshButton)
                .Prop(TextureButton.StylePropertyTexture, refreshTex),

            // Help button
            E<TextureButton>()
                .Class(StyleClass.HelpButton)
                .Prop(TextureButton.StylePropertyTexture, helpTex),

            // Ensure labels in buttons are aligned.
            E<Label>()
                // ReSharper disable once AccessToStaticMemberViaDerivedType
                .Class(Button.StyleClassButton)
                .AlignMode(Label.AlignMode.Center),

            // Have disabled button's text be faded
            CButton().PseudoDisabled().ParentOf(E<Label>()).FontColor(Color.FromHex("#E5E5E581")),
            CButton().PseudoDisabled().ParentOf(E()).ParentOf(E<Label>()).FontColor(Color.FromHex("#E5E5E581")),
        };
        // Texture button modulation
        MakeButtonRules<TextureButton>(rules, Palettes.AlphaModulate, null);
        MakeButtonRules<TextureButton>(rules, palette.NegativePalette, StyleClass.CrossButtonRed);

        MakeButtonRules(rules, button.ButtonPalette, null);
        MakeButtonRules(rules, button.PositiveButtonPalette, StyleClass.Positive);
        MakeButtonRules(rules, button.NegativeButtonPalette, StyleClass.Negative);

        return rules.ToArray();
    }

    public static void MakeButtonRules<TControl>(
        List<StyleRule> rules,
        ColorPalette palette,
        string? styleClass)
        where TControl : Control
    {
        rules.AddRange([
            E<TControl>().MaybeClass(styleClass).PseudoNormal().Modulate(palette.Element),
            E<TControl>().MaybeClass(styleClass).PseudoHovered().Modulate(palette.HoveredElement),
            E<TControl>().MaybeClass(styleClass).PseudoPressed().Modulate(palette.PressedElement),
            E<TControl>().MaybeClass(styleClass).PseudoDisabled().Modulate(palette.DisabledElement),
        ]);
    }

    public static void MakeButtonRules(
        List<StyleRule> rules,
        ColorPalette palette,
        string? styleClass)
    {
        rules.AddRange([
            CButton()
                .MaybeClass(styleClass)
                .PseudoNormal()
                .Prop(Control.StylePropertyModulateSelf, palette.Element),
            CButton()
                .MaybeClass(styleClass)
                .PseudoHovered()
                .Prop(Control.StylePropertyModulateSelf, palette.HoveredElement),
            CButton()
                .MaybeClass(styleClass)
                .PseudoPressed()
                .Prop(Control.StylePropertyModulateSelf, palette.PressedElement),
            CButton()
                .MaybeClass(styleClass)
                .PseudoDisabled()
                .Prop(Control.StylePropertyModulateSelf, palette.DisabledElement),
        ]);
    }

    private static MutableSelectorElement CButton()
    {
        return E<ContainerButton>().Class(ContainerButton.StyleClassButton);
    }
}

// This is currently the only other "helper" type class. If any more crop up, consider making a specific directory.
public static class StyleBoxHelpers
{
    // TODO: Figure out a nicer way to store/represent these hardcoded margins. This is icky.
    public static StyleBoxTexture BaseStyleBox(IResourceCache resourceCache, ButtonConfig config)
    {
        var baseBox = new StyleBoxTexture
        {
            Texture = resourceCache.GetTexture(config.BaseButtonPath),
        };
        baseBox.SetPatchMargin(StyleBox.Margin.All, 10);
        baseBox.SetPadding(StyleBox.Margin.All, 1);
        baseBox.SetContentMarginOverride(StyleBox.Margin.Vertical, 2);
        baseBox.SetContentMarginOverride(StyleBox.Margin.Horizontal, 14);
        return baseBox;
    }

    public static StyleBoxTexture OpenLeftStyleBox(IResourceCache resourceCache, ButtonConfig config)
    {
        var openLeftBox = new StyleBoxTexture(BaseStyleBox(resourceCache, config))
        {
            Texture = new AtlasTexture(resourceCache.GetTexture(config.OpenLeftButtonPath),
                UIBox2.FromDimensions(new Vector2(10, 0), new Vector2(14, 24))),
        };
        openLeftBox.SetPatchMargin(StyleBox.Margin.Left, 0);
        openLeftBox.SetContentMarginOverride(StyleBox.Margin.Left, 8);
        return openLeftBox;
    }

    public static StyleBoxTexture OpenRightStyleBox(IResourceCache resourceCache, ButtonConfig config)
    {
        var openRightBox = new StyleBoxTexture(BaseStyleBox(resourceCache, config))
        {
            Texture = new AtlasTexture(
                resourceCache.GetTexture(config.OpenRightButtonPath),
                UIBox2.FromDimensions(new Vector2(0, 0), new Vector2(14, 24))),
        };
        openRightBox.SetPatchMargin(StyleBox.Margin.Right, 0);
        openRightBox.SetContentMarginOverride(StyleBox.Margin.Right, 8);
        openRightBox.SetPadding(StyleBox.Margin.Right, 1);
        return openRightBox;
    }

    public static StyleBoxTexture SquareStyleBox(IResourceCache resourceCache, ButtonConfig config)
    {
        var openBothBox = new StyleBoxTexture(BaseStyleBox(resourceCache, config))
        {
            Texture = new AtlasTexture(
                resourceCache.GetTexture(config.OpenBothButtonPath),
                UIBox2.FromDimensions(new Vector2(10, 0), new Vector2(3, 24))),
        };
        openBothBox.SetPatchMargin(StyleBox.Margin.Horizontal, 0);
        openBothBox.SetContentMarginOverride(StyleBox.Margin.Horizontal, 8);
        openBothBox.SetPadding(StyleBox.Margin.Horizontal, 1);
        return openBothBox;
    }

    public static StyleBoxTexture SmallStyleBox(IResourceCache resourceCache, ButtonConfig config)
    {
        return new StyleBoxTexture
        {
            Texture = resourceCache.GetTexture(config.SmallButtonPath),
        };
    }
}
