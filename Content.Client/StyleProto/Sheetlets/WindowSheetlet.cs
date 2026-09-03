using Content.Client.Resources;
using Content.Client.StyleProto.SheetletConfigs;
using Content.Client.Stylesheets.Fonts;
using Content.Client.Stylesheets.Palette;
using Content.Client.UserInterface.Controls;
using JetBrains.Annotations;
using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.CustomControls;
using static Content.Client.Stylesheets.StylesheetHelpers;
using StyleClass = Content.Client.Stylesheets.StyleClass;

namespace Content.Client.StyleProto.Sheetlets;

[Sheetlet]
[UsedImplicitly]
public sealed partial class WindowSheetlet : ISheetlet
{
    [Dependency] private IResourceCache _resourceCache = default!;

    public StyleRule[] Generate(SheetletConfigRegistry configs)
    {
        var button = configs.GetConfig<ButtonConfig>();
        var window = configs.GetConfig<WindowConfig>();
        var icon = configs.GetConfig<IconConfig>();
        var font = configs.GetConfig<FontConfig>();
        var palette = configs.GetConfig<PaletteConfig>();

        var headerStylebox = new StyleBoxTexture
        {
            Texture = _resourceCache.GetTexture(window.WindowHeaderTexturePath),
            PatchMarginBottom = 3,
            ExpandMarginBottom = 3,
            ContentMarginBottomOverride = 0,
        };
        var headerAlertStylebox = new StyleBoxTexture
        {
            Texture = _resourceCache.GetTexture(window.WindowHeaderAlertTexturePath),
            PatchMarginBottom = 3,
            ExpandMarginBottom = 3,
            ContentMarginBottomOverride = 0,
        };
        var backgroundBox = new StyleBoxTexture
        {
            Texture = _resourceCache.GetTexture(window.WindowBackgroundPath),
        };
        backgroundBox.SetPatchMargin(StyleBox.Margin.Horizontal | StyleBox.Margin.Bottom, 2);
        backgroundBox.SetExpandMargin(StyleBox.Margin.Horizontal | StyleBox.Margin.Bottom, 2);
        var borderedBackgroundBox = new StyleBoxTexture
        {
            Texture = _resourceCache.GetTexture(window.WindowBackgroundBorderedPath),
        };
        borderedBackgroundBox.SetPatchMargin(StyleBox.Margin.All, 2);
        var closeButtonTex = _resourceCache.GetTexture(icon.CrossIconPath);

        var leftPanel = StyleBoxHelpers.OpenLeftStyleBox(_resourceCache, button);
        leftPanel.SetPadding(StyleBox.Margin.All, 0.0f);

        return
        [
            E<Label>()
                .Class(DefaultWindow.StyleClassWindowTitle)
                .FontColor(palette.HighlightPalette.Text)
                .Font(font.BaseFont.GetFont(14, FontKind.Bold)),
            E<Label>()
                .Class("windowTitleAlert")
                .FontColor(Color.White)
                .Font(font.BaseFont.GetFont(14, FontKind.Bold)),
            E()
                .Class(DefaultWindow.StyleClassWindowPanel)
                .Panel(backgroundBox),
            E()
                .Class(DefaultWindow.StyleClassWindowHeader)
                .Panel(headerStylebox),
            E()
                .Class(StyleClass.AlertWindowHeader)
                .Panel(headerAlertStylebox),
            E()
                .Class(StyleClass.BorderedWindowPanel)
                .Panel(borderedBackgroundBox),

            E<TextureButton>()
                .Class(DefaultWindow.StyleClassWindowCloseButton)
                .Prop(TextureButton.StylePropertyTexture, closeButtonTex)
                .Margin(3),
            E<TextureButton>()
                .Class(DefaultWindow.StyleClassWindowCloseButton)
                .PseudoNormal()
                .Modulate(Palettes.Neutral.Element),
            E<TextureButton>()
                .Class(DefaultWindow.StyleClassWindowCloseButton)
                .PseudoHovered()
                .Modulate(Palettes.Red.HoveredElement),
            E<TextureButton>()
                .Class(DefaultWindow.StyleClassWindowCloseButton)
                .PseudoPressed()
                .Modulate(Palettes.Red.PressedElement),
            E<TextureButton>()
                .Class(DefaultWindow.StyleClassWindowCloseButton)
                .PseudoDisabled()
                .Modulate(Palettes.Red.DisabledElement),

            E<Label>()
                .Class("FancyWindowTitle")
                .Font(font.DecorativeFont.GetFont(13))
                .FontColor(palette.HighlightPalette.Text),

            E<TextureButton>()
                .Class(FancyWindow.StyleClassWindowHelpButton)
                .Prop(TextureButton.StylePropertyTexture, _resourceCache.GetTexture(icon.HelpIconPath))
                .Prop(Control.StylePropertyModulateSelf, palette.PrimaryPalette.Element),
            E<TextureButton>()
                .Class(FancyWindow.StyleClassWindowHelpButton)
                .Pseudo(ContainerButton.StylePseudoClassHover)
                .Prop(Control.StylePropertyModulateSelf, palette.PrimaryPalette.HoveredElement),
            E<TextureButton>()
                .Class(FancyWindow.StyleClassWindowHelpButton)
                .Pseudo(ContainerButton.StylePseudoClassPressed)
                .Prop(Control.StylePropertyModulateSelf, palette.PrimaryPalette.PressedElement),

            E<Label>()
                .Class("WindowFooterText")
                .Prop(Label.StylePropertyFont, font.BaseFont.GetFont(8))
                .Prop(Label.StylePropertyFontColor, Color.FromHex("#757575")),
        ];
    }
}
