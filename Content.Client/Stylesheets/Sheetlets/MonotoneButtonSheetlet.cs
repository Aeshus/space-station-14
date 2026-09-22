using System.Numerics;
using Content.Client.Resources;
using Content.Client.Stylesheets.SheetletConfigs;
using Content.Client.UserInterface.Controls;
using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using static Content.Client.Stylesheets.StylesheetHelpers;

namespace Content.Client.Stylesheets.Sheetlets;

[Sheetlet]
public sealed partial class MonotoneButtonSheetlet : ISheetlet
{
    [Dependency] private IResourceCache _resCache = default!;

    public StyleRule[] Generate(SheetletConfigRegistry configs)
    {
        var buttons = configs.GetConfig<ButtonConfig>();

        // Monotone (unfilled)
        var monotoneButton = new StyleBoxTexture
        {
            Texture = _resCache.GetTexture(buttons.MonotoneBaseButtonPath),
        };
        monotoneButton.SetPatchMargin(StyleBox.Margin.All, 11);
        monotoneButton.SetPadding(StyleBox.Margin.All, 1);
        monotoneButton.SetContentMarginOverride(StyleBox.Margin.Vertical, 2);
        monotoneButton.SetContentMarginOverride(StyleBox.Margin.Horizontal, 14);

        var monotoneButtonOpenLeft = new StyleBoxTexture(monotoneButton)
        {
            Texture = _resCache.GetTexture(buttons.MonotoneOpenLeftButtonPath),
        };

        var monotoneButtonOpenRight = new StyleBoxTexture(monotoneButton)
        {
            Texture = _resCache.GetTexture(buttons.MonotoneOpenRightButtonPath),
        };

        var monotoneButtonOpenBoth = new StyleBoxTexture(monotoneButton)
        {
            Texture = _resCache.GetTexture(buttons.MonotoneOpenBothButtonPath),
        };

        // Monotone (filled)
        var buttonTex = _resCache.GetTexture(buttons.OpenLeftButtonPath);
        var monotoneFilledButton = new StyleBoxTexture(monotoneButton)
        {
            Texture = buttonTex
        };

        var monotoneFilledButtonOpenLeft = new StyleBoxTexture(monotoneButton)
        {
            Texture = new AtlasTexture(buttonTex, UIBox2.FromDimensions(new Vector2(10, 0), new Vector2(14, 24))),
        };
        monotoneFilledButtonOpenLeft.SetPatchMargin(StyleBox.Margin.Left, 0);

        var monotoneFilledButtonOpenRight = new StyleBoxTexture(monotoneButton)
        {
            Texture = new AtlasTexture(buttonTex, UIBox2.FromDimensions(new Vector2(0, 0), new Vector2(14, 24))),
        };
        monotoneFilledButtonOpenRight.SetPatchMargin(StyleBox.Margin.Right, 0);

        var monotoneFilledButtonOpenBoth = new StyleBoxTexture(monotoneButton)
        {
            Texture = new AtlasTexture(buttonTex, UIBox2.FromDimensions(new Vector2(10, 0), new Vector2(3, 24))),
        };
        monotoneFilledButtonOpenBoth.SetPatchMargin(StyleBox.Margin.Horizontal, 0);


        return
        [
            // Unfilled
            E<MonotoneButton>()
                .Box(monotoneButton),
            E<MonotoneButton>()
                .Class(StyleClass.ButtonOpenLeft)
                .Box(monotoneButtonOpenLeft),
            E<MonotoneButton>()
                .Class(StyleClass.ButtonOpenRight)
                .Box(monotoneButtonOpenRight),
            E<MonotoneButton>()
                .Class(StyleClass.ButtonOpenBoth)
                .Box(monotoneButtonOpenBoth),

            // Filled
            E<MonotoneButton>()
                .PseudoPressed()
                .Box(monotoneFilledButton)
                .Prop(Button.StylePropertyModulateSelf, Color.White),
            E<MonotoneButton>()
                .Class(StyleClass.ButtonOpenLeft)
                .PseudoPressed()
                .Box(monotoneFilledButtonOpenLeft)
                .Prop(Button.StylePropertyModulateSelf, Color.White),
            E<MonotoneButton>()
                .Class(StyleClass.ButtonOpenRight)
                .PseudoPressed()
                .Box(monotoneFilledButtonOpenRight)
                .Prop(Button.StylePropertyModulateSelf, Color.White),
            E<MonotoneButton>()
                .Class(StyleClass.ButtonOpenBoth)
                .PseudoPressed()
                .Box(monotoneFilledButtonOpenBoth)
                .Prop(Button.StylePropertyModulateSelf, Color.White),
        ];
    }
}
