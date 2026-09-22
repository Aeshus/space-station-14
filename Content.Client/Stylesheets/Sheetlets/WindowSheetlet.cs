using Content.Client.Resources;
using Content.Client.Stylesheets.Palette;
using Content.Client.Stylesheets.SheetletConfigs;
using Content.Client.UserInterface.Controls;
using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.CustomControls;
using static Content.Client.Stylesheets.StylesheetHelpers;

namespace Content.Client.Stylesheets.Sheetlets;

[Sheetlet]
public sealed partial class WindowSheetlet : ISheetlet
{
    [Dependency] private IResourceCache _resCache = default!;

    public StyleRule[] Generate(SheetletConfigRegistry configs)
    {
        var buttons = configs.GetConfig<ButtonConfig>();
        var windows = configs.GetConfig<WindowConfig>();
        var icons = configs.GetConfig<IconConfig>();
        var palettes = configs.GetConfig<PaletteConfig>();
        var fonts = configs.GetConfig<FontConfig>();

        var headerStylebox = new StyleBoxTexture
        {
            Texture = _resCache.GetTexture(windows.WindowHeaderTexturePath),
            PatchMarginBottom = 3,
            ExpandMarginBottom = 3,
            ContentMarginBottomOverride = 0,
        };
        // TODO: This would probably be better palette-based but we can leave it for now.
        var headerAlertStylebox = new StyleBoxTexture
        {
            Texture = _resCache.GetTexture(windows.WindowHeaderAlertTexturePath),
            PatchMarginBottom = 3,
            ExpandMarginBottom = 3,
            ContentMarginBottomOverride = 0,
        };
        var backgroundBox = new StyleBoxTexture()
        {
            Texture = _resCache.GetTexture(windows.WindowBackgroundPath),
        };
        backgroundBox.SetPatchMargin(StyleBox.Margin.Horizontal | StyleBox.Margin.Bottom, 2);
        backgroundBox.SetExpandMargin(StyleBox.Margin.Horizontal | StyleBox.Margin.Bottom, 2);
        var borderedBackgroundBox = new StyleBoxTexture
        {
            Texture = _resCache.GetTexture(windows.WindowBackgroundBorderedPath),
        };
        borderedBackgroundBox.SetPatchMargin(StyleBox.Margin.All, 2);
        var closeButtonTex = _resCache.GetTexture(icons.CrossIconPath);

        var leftPanel = StyleBoxHelpers.OpenLeftStyleBox(_resCache, buttons);
        leftPanel.SetPadding(StyleBox.Margin.All, 0.0f);

        // TODO: maybe also change everything here to `NanoWindow` or something
        return
        [
            // TODO: KILL DEFAULT WINDOW (in a bit)
            E<Label>()
                .Class(DefaultWindow.StyleClassWindowTitle)
                .FontColor(palettes.HighlightPalette.Text)
                .Font(fonts.Main.GetFont(14, FontWeight.Bold)),
            E<Label>()
                .Class("windowTitleAlert")
                .FontColor(Color.White)
                .Font(fonts.Main.GetFont(14, FontWeight.Bold)),
            // TODO: maybe also change everything here to `NanoWindow` or something
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

            // Close button
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

            // Title
            E<Label>()
                .Class("FancyWindowTitle") // TODO: hardcoding class name
                .Font(fonts.Decorative.GetFont(13)) // TODO: hardcoding font
                .FontColor(palettes.HighlightPalette.Text),

            // Help Button
            E<TextureButton>()
                .Class(FancyWindow.StyleClassWindowHelpButton)
                .Prop(TextureButton.StylePropertyTexture,
                    _resCache.GetTexture(icons.HelpIconPath))
                .Prop(Control.StylePropertyModulateSelf, palettes.PrimaryPalette.Element),
            E<TextureButton>()
                .Class(FancyWindow.StyleClassWindowHelpButton)
                .Pseudo(ContainerButton.StylePseudoClassHover)
                .Prop(Control.StylePropertyModulateSelf, palettes.PrimaryPalette.HoveredElement),
            E<TextureButton>()
                .Class(FancyWindow.StyleClassWindowHelpButton)
                .Pseudo(ContainerButton.StylePseudoClassPressed)
                .Prop(Control.StylePropertyModulateSelf, palettes.PrimaryPalette.PressedElement),

            // Footer
            E<Label>()
                .Class("WindowFooterText") // TODO: hardcoding font
                .Prop(Label.StylePropertyFont, fonts.Main.GetFont(8))
                .Prop(Label.StylePropertyFontColor, Color.FromHex("#757575")),
        ];
    }
}
