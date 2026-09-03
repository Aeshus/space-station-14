using Content.Client.ContextMenu.UI;
using Content.Client.Resources;
using Content.Client.StyleProto.SheetletConfigs;
using Content.Client.Stylesheets.Fonts;
using Content.Client.Stylesheets.Palette;
using Content.Client.Verbs.UI;
using Content.Shared.Verbs;
using JetBrains.Annotations;
using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Utility;
using static Content.Client.Stylesheets.StylesheetHelpers;

namespace Content.Client.StyleProto.Sheetlets.Hud;

[Sheetlet]
[UsedImplicitly]
public sealed partial class ContextMenuSheetlet : ISheetlet
{
    [Dependency] private IResourceCache _resourceCache = default!;

    private static readonly ColorPalette ContextButtonPalette = ColorPalette.FromHexBase("#000000") with
    {
        HoveredElement = Color.DarkSlateGray,
        Element = Color.FromHex("#1119"),
        PressedElement = Color.LightSlateGray,
    };

    public StyleRule[] Generate(SheetletConfigRegistry configs)
    {
        var window = configs.GetConfig<WindowConfig>();
        var font = configs.GetConfig<FontConfig>();
        var palette = configs.GetConfig<PaletteConfig>();
        var borderedWindowBackground = new StyleBoxTexture
        {
            Texture = _resourceCache.GetTexture(window.WindowBackgroundBorderedPath),
        };
        borderedWindowBackground.SetPatchMargin(StyleBox.Margin.All, ContextMenuElement.ElementMargin);
        var buttonContext = new StyleBoxTexture { Texture = Texture.White };
        var contextMenuExpansionTexture = _resourceCache.GetTexture(
            new ResPath("/Textures/Interface/VerbIcons/group.svg.192dpi.png"));
        var verbMenuConfirmationTexture = _resourceCache.GetTexture(
            new ResPath("/Textures/Interface/VerbIcons/group.svg.192dpi.png"));

        var rules = new List<StyleRule>
        {
            E<PanelContainer>()
                .Class(ContextMenuPopup.StyleClassContextMenuPopup)
                .Panel(borderedWindowBackground),

            E<ContextMenuElement>()
                .Class(ContextMenuElement.StyleClassContextMenuButton)
                .Prop(ContainerButton.StylePropertyStyleBox, buttonContext),

            E<RichTextLabel>()
                .Class(InteractionVerb.DefaultTextStyleClass)
                .Font(font.BaseFont.GetFont(12, FontKind.BoldItalic)),
            E<RichTextLabel>()
                .Class(ActivationVerb.DefaultTextStyleClass)
                .Font(font.BaseFont.GetFont(12, FontKind.Bold)),
            E<RichTextLabel>()
                .Class(AlternativeVerb.DefaultTextStyleClass)
                .Font(font.BaseFont.GetFont(12, FontKind.Italic)),
            E<RichTextLabel>()
                .Class(Verb.DefaultTextStyleClass)
                .Font(font.BaseFont.GetFont(12)),
            E<TextureRect>()
                .Class(ContextMenuElement.StyleClassContextMenuExpansionTexture)
                .Prop(TextureRect.StylePropertyTexture, contextMenuExpansionTexture),
            E<TextureRect>()
                .Class(VerbMenuElement.StyleClassVerbMenuConfirmationTexture)
                .Prop(TextureRect.StylePropertyTexture, verbMenuConfirmationTexture),

            E<ContextMenuElement>()
                .Class(ConfirmationMenuElement.StyleClassConfirmationContextMenuButton)
                .Prop(ContainerButton.StylePropertyStyleBox, buttonContext),
        };

        ButtonSheetlet.MakeButtonRules<ContextMenuElement>(rules,
            ContextButtonPalette,
            ContextMenuElement.StyleClassContextMenuButton);
        ButtonSheetlet.MakeButtonRules<ContextMenuElement>(rules,
            palette.NegativePalette,
            ConfirmationMenuElement.StyleClassConfirmationContextMenuButton);

        return rules.ToArray();
    }
}
