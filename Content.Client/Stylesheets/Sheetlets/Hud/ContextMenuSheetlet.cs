using Content.Client.ContextMenu.UI;
using Content.Client.Resources;
using Content.Client.Stylesheets.Palette;
using Content.Client.Stylesheets.SheetletConfigs;
using Content.Client.Verbs.UI;
using Content.Shared.Verbs;
using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using static Content.Client.Stylesheets.StylesheetHelpers;

namespace Content.Client.Stylesheets.Sheetlets.Hud;

[Sheetlet]
public sealed partial class ContextMenuSheetlet<T> : ISheetlet
{
    [Dependency] private IResourceCache _resCache = default!;

    // TODO: make this not hardcoded (I am too scared to change the context menu colors)
    private static readonly ColorPalette ContextButtonPalette = ColorPalette.FromHexBase("#000000") with
    {
        HoveredElement = Color.DarkSlateGray,
        Element = Color.FromHex("#1119"),
        PressedElement = Color.LightSlateGray,
    };

    public StyleRule[] Generate(SheetletConfigRegistry configs)
    {
        var windows = configs.GetConfig<WindowConfig>();
        var fonts = configs.GetConfig<FontConfig>();
        var palettes = configs.GetConfig<PaletteConfig>();

        var borderedWindowBackground = new StyleBoxTexture
        {
            Texture = _resCache.GetTexture(windows.WindowBackgroundBorderedPath),
        };
        borderedWindowBackground.SetPatchMargin(StyleBox.Margin.All, ContextMenuElement.ElementMargin);
        var buttonContext = new StyleBoxTexture { Texture = Texture.White };
        var contextMenuExpansionTexture = _resCache.GetTexture("/Textures/Interface/VerbIcons/group.svg.192dpi.png");
        var verbMenuConfirmationTexture = _resCache.GetTexture("/Textures/Interface/VerbIcons/group.svg.192dpi.png");

        var rules = new List<StyleRule>
        {
            // Context Menu window
            E<PanelContainer>()
                .Class(ContextMenuPopup.StyleClassContextMenuPopup)
                .Panel(borderedWindowBackground),

            // Context menu buttons
            E<ContextMenuElement>()
                .Class(ContextMenuElement.StyleClassContextMenuButton)
                .Prop(ContainerButton.StylePropertyStyleBox, buttonContext),

            // Context Menu Labels
            E<RichTextLabel>()
                .Class(InteractionVerb.DefaultTextStyleClass)
                .Font(fonts.Main.GetFont(12, FontWeight.Bold, FontSlant.Italic)),
            E<RichTextLabel>()
                .Class(ActivationVerb.DefaultTextStyleClass)
                .Font(fonts.Main.GetFont(12, FontWeight.Bold)),
            E<RichTextLabel>()
                .Class(AlternativeVerb.DefaultTextStyleClass)
                .Font(fonts.Main.GetFont(12, slant: FontSlant.Italic)),
            E<RichTextLabel>()
                .Class(Verb.DefaultTextStyleClass)
                .Font(fonts.Main.GetFont(12)),
            E<TextureRect>()
                .Class(ContextMenuElement.StyleClassContextMenuExpansionTexture)
                .Prop(TextureRect.StylePropertyTexture, contextMenuExpansionTexture),
            E<TextureRect>()
                .Class(VerbMenuElement.StyleClassVerbMenuConfirmationTexture)
                .Prop(TextureRect.StylePropertyTexture, verbMenuConfirmationTexture),

            // Context menu confirm buttons
            E<ContextMenuElement>()
                .Class(ConfirmationMenuElement.StyleClassConfirmationContextMenuButton)
                .Prop(ContainerButton.StylePropertyStyleBox, buttonContext),
        };

        ButtonSheetlet.MakeButtonRules<ContextMenuElement>(rules,
            ContextButtonPalette,
            ContextMenuElement.StyleClassContextMenuButton);
        ButtonSheetlet.MakeButtonRules<ContextMenuElement>(rules,
            palettes.NegativePalette,
            ConfirmationMenuElement.StyleClassConfirmationContextMenuButton);

        return rules.ToArray();
    }
}
