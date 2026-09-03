using Content.Client.Resources;
using Content.Client.StyleProto.SheetletConfigs;
using Content.Client.UserInterface.Systems.Actions.Controls;
using Content.Client.UserInterface.Systems.Actions.Windows;
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
public sealed partial class ActionSheetlet : ISheetlet
{
    [Dependency] private IResourceCache _resourceCache = default!;

    public StyleRule[] Generate(SheetletConfigRegistry configs)
    {
        var panel = configs.GetConfig<PanelConfig>();
        var handSlotHighlightTex = _resourceCache.GetTexture(
            new ResPath("/Textures/Interface/Inventory/hand_slot_highlight.png"));
        var handSlotHighlight = new StyleBoxTexture
        {
            Texture = handSlotHighlightTex,
        };
        handSlotHighlight.SetPatchMargin(StyleBox.Margin.All, 2);

        var actionSearchBoxTex = _resourceCache.GetTexture(panel.BlackPanelDarkThinBorderPath);
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
