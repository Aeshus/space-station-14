using Content.Client.Resources;
using Content.Client.Stylesheets.SheetletConfigs;
using Content.Client.UserInterface.Systems.Actions.Controls;
using Content.Client.UserInterface.Systems.Actions.Windows;
using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using static Content.Client.Stylesheets.StylesheetHelpers;

namespace Content.Client.Stylesheets.Sheetlets.Hud;

[Sheetlet]
public sealed partial class ActionSheetlet : ISheetlet
{
    [Dependency] private IResourceCache _resCache = default!;

    public StyleRule[] Generate(SheetletConfigRegistry configs)
    {
        var panels = configs.GetConfig<PanelConfig>();

        // TODO: absolute texture access
        var handSlotHighlightTex = _resCache.GetTexture("/Textures/Interface/Inventory/hand_slot_highlight.png");
        var handSlotHighlight = new StyleBoxTexture
        {
            Texture = handSlotHighlightTex,
        };
        handSlotHighlight.SetPatchMargin(StyleBox.Margin.All, 2);

        var actionSearchBoxTex = _resCache.GetTexture(panels.BlackPanelDarkThinBorderPath);
        var actionSearchBox = new StyleBoxTexture
        {
            Texture = actionSearchBoxTex,
        };
        actionSearchBox.SetPatchMargin(StyleBox.Margin.All, 3);
        actionSearchBox.SetContentMarginOverride(StyleBox.Margin.Horizontal, 5);

        return
        [
            E<PanelContainer>().Class(ActionButton.StyleClassActionHighlightRect).Panel(handSlotHighlight),
            E<LineEdit>().Class(ActionsWindow.StyleClassActionSearchBox).Box(actionSearchBox),
        ];
    }
}
