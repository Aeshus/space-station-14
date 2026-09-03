using Content.Client.Resources;
using Content.Client.StyleProto.SheetletConfigs;
using JetBrains.Annotations;
using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using static Content.Client.Stylesheets.StylesheetHelpers;

namespace Content.Client.StyleProto.Sheetlets;

[Sheetlet]
[UsedImplicitly]
public sealed partial class TabContainerSheetlet : ISheetlet
{
    [Dependency] private IResourceCache _resourceCache = default!;

    public StyleRule[] Generate(SheetletConfigRegistry configs)
    {
        var tab = configs.GetConfig<TabContainerConfig>();
        var palette = configs.GetConfig<PaletteConfig>();
        var tabContainerPanel = _resourceCache.GetTexture(tab.TabContainerPanelPath)
            .IntoPatch(StyleBox.Margin.All, 2);

        var tabContainerBoxActive = new StyleBoxFlat(palette.SecondaryPalette.Element);
        tabContainerBoxActive.SetContentMarginOverride(StyleBox.Margin.Horizontal, 5);
        var tabContainerBoxInactive = new StyleBoxFlat(palette.SecondaryPalette.Background);
        tabContainerBoxInactive.SetContentMarginOverride(StyleBox.Margin.Horizontal, 5);

        return
        [
            E<TabContainer>()
                .Prop(TabContainer.StylePropertyPanelStyleBox, tabContainerPanel)
                .Prop(TabContainer.StylePropertyTabStyleBox, tabContainerBoxActive)
                .Prop(TabContainer.StylePropertyTabStyleBoxInactive, tabContainerBoxInactive),
        ];
    }
}
