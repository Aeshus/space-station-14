using System.Numerics;
using Content.Client.Resources;
using Content.Client.StyleProto.SheetletConfigs;
using Content.Client.Stylesheets.Fonts;
using Content.Client.UserInterface.Controls;
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
public sealed partial class MenuButtonSheetlet : ISheetlet
{
    [Dependency] private IResourceCache _resourceCache = default!;

    private static MutableSelectorElement CButton()
    {
        return E<MenuButton>();
    }

    public StyleRule[] Generate(SheetletConfigRegistry configs)
    {
        var button = configs.GetConfig<ButtonConfig>();
        var font = configs.GetConfig<FontConfig>();
        var buttonTex = _resourceCache.GetTexture(button.BaseButtonPath);
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
            CButton().Box(StyleBoxHelpers.BaseStyleBox(_resourceCache, button)),
            CButton()
                .Class(StyleClass.ButtonOpenLeft)
                .Prop(ContainerButton.StylePropertyStyleBox, StyleBoxHelpers.OpenLeftStyleBox(_resourceCache, button)),
            CButton()
                .Class(StyleClass.ButtonOpenRight)
                .Prop(ContainerButton.StylePropertyStyleBox, StyleBoxHelpers.OpenRightStyleBox(_resourceCache, button)),
            CButton()
                .Class(StyleClass.ButtonOpenBoth)
                .Prop(ContainerButton.StylePropertyStyleBox, StyleBoxHelpers.SquareStyleBox(_resourceCache, button)),
            CButton()
                .Class(StyleClass.ButtonSquare)
                .Prop(ContainerButton.StylePropertyStyleBox, StyleBoxHelpers.SquareStyleBox(_resourceCache, button)),
            E<Label>()
                .Class(MenuButton.StyleClassLabelTopButton)
                .Prop(Label.StylePropertyFont, font.BaseFont.GetFont(14, FontKind.Bold)),
        };

        ButtonSheetlet.MakeButtonRules<MenuButton>(rules, button.ButtonPalette, null);
        ButtonSheetlet.MakeButtonRules<MenuButton>(rules, button.PositiveButtonPalette, StyleClass.Positive);
        ButtonSheetlet.MakeButtonRules<MenuButton>(rules, button.NegativeButtonPalette, StyleClass.Negative);

        return rules.ToArray();
    }
}
