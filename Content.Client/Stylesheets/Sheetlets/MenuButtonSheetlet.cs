using System.Numerics;
using Content.Client.Resources;
using Content.Client.Stylesheets.Fonts;
using Content.Client.Stylesheets.SheetletConfigs;
using Content.Client.Stylesheets.Stylesheets;
using Content.Client.UserInterface.Controls;
using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using static Content.Client.Stylesheets.StylesheetHelpers;

namespace Content.Client.Stylesheets.Sheetlets;

[Sheetlet]
public sealed partial class MenuButtonSheetlet : ISheetlet
{
    [Dependency] private IResourceCache _resCache = default!;

    public StyleRule[] Generate(SheetletConfigRegistry configs)
    {
        var buttons = configs.GetConfig<ButtonConfig>();
        var fonts = configs.GetConfig<FontConfig>();

        var buttonTex = _resCache.GetTexture(buttons.BaseButtonPath);
        var topButtonBase = new StyleBoxTexture
        {
            Texture = buttonTex,
        };
        topButtonBase.SetPatchMargin(StyleBox.Margin.All, 10);
        topButtonBase.SetPadding(StyleBox.Margin.All, 0);
        topButtonBase.SetContentMarginOverride(StyleBox.Margin.All, 0);

        var topButtonOpenRight = new StyleBoxTexture(topButtonBase)
        {
            Texture = new AtlasTexture(buttonTex, UIBox2.FromDimensions(new Vector2(0, 0), new Vector2(14, 24))),
        };
        topButtonOpenRight.SetPatchMargin(StyleBox.Margin.Right, 0);

        var topButtonOpenLeft = new StyleBoxTexture(topButtonBase)
        {
            Texture = new AtlasTexture(buttonTex, UIBox2.FromDimensions(new Vector2(10, 0), new Vector2(14, 24))),
        };
        topButtonOpenLeft.SetPatchMargin(StyleBox.Margin.Left, 0);

        var topButtonSquare = new StyleBoxTexture(topButtonBase)
        {
            Texture = new AtlasTexture(buttonTex, UIBox2.FromDimensions(new Vector2(10, 0), new Vector2(3, 24))),
        };
        topButtonSquare.SetPatchMargin(StyleBox.Margin.Horizontal, 0);

        var rules = new List<StyleRule>
        {
            CButton().Class(StyleClass.ButtonSquare).Box(topButtonSquare),
            CButton().Class(StyleClass.ButtonOpenLeft).Box(topButtonOpenLeft),
            CButton().Class(StyleClass.ButtonOpenRight).Box(topButtonOpenRight),
            CButton().Box(StyleBoxHelpers.BaseStyleBox(_resCache, buttons)),
            CButton()
                .Class(StyleClass.ButtonOpenLeft)
                .Prop(ContainerButton.StylePropertyStyleBox, StyleBoxHelpers.OpenLeftStyleBox(_resCache, buttons)),
            CButton()
                .Class(StyleClass.ButtonOpenRight)
                .Prop(ContainerButton.StylePropertyStyleBox, StyleBoxHelpers.OpenRightStyleBox(_resCache, buttons)),
            CButton()
                .Class(StyleClass.ButtonOpenBoth)
                .Prop(ContainerButton.StylePropertyStyleBox, StyleBoxHelpers.SquareStyleBox(_resCache, buttons)),
            CButton()
                .Class(StyleClass.ButtonSquare)
                .Prop(ContainerButton.StylePropertyStyleBox, StyleBoxHelpers.SquareStyleBox(_resCache, buttons)),
            E<Label>()
                .Class(MenuButton.StyleClassLabelTopButton)
                .Prop(Label.StylePropertyFont, fonts.BaseFont.GetFont(14, FontWeight.Bold)),
            // new StyleProperty(Label.StylePropertyFont, notoSansDisplayBold14),
        };

        ButtonSheetlet.MakeButtonRules<MenuButton>(rules, buttons.ButtonPalette, null);
        ButtonSheetlet.MakeButtonRules<MenuButton>(rules, buttons.PositiveButtonPalette, StyleClass.Positive);
        ButtonSheetlet.MakeButtonRules<MenuButton>(rules, buttons.NegativeButtonPalette, StyleClass.Negative);

        return rules.ToArray();
    }

    private static MutableSelectorElement CButton()
    {
        return E<MenuButton>();
    }
}
