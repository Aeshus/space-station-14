using Content.Client.Resources;
using Content.Client.StyleProto.SheetletConfigs;
using JetBrains.Annotations;
using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Utility;
using static Content.Client.Stylesheets.StylesheetHelpers;

namespace Content.Client.StyleProto.Sheetlets;

[Sheetlet]
[UsedImplicitly]
public sealed partial class PaperSheetlet : ISheetlet
{
    [Dependency] private IResourceCache _resourceCache = default!;

    public StyleRule[] Generate(SheetletConfigRegistry configs)
    {
        var window = configs.GetConfig<WindowConfig>();
        var paperBackground = _resourceCache
            .GetTexture(new ResPath("/Textures/Interface/Paper/paper_background_default.svg.96dpi.png"))
            .IntoPatch(StyleBox.Margin.All, 16);
        var paperBox = new StyleBoxTexture
        {
            Texture = _resourceCache.GetTexture(window.TransparentWindowBackgroundBorderedPath),
        };
        paperBox.SetPatchMargin(StyleBox.Margin.All, 2);

        var borderedTransparentTex = _resourceCache.GetTexture(
            new ResPath("/Textures/Interface/Nano/transparent_window_background_bordered.png"));
        var borderedTransparentBackground = new StyleBoxTexture
        {
            Texture = borderedTransparentTex,
        };
        borderedTransparentBackground.SetPatchMargin(StyleBox.Margin.All, 2);

        return
        [
            E<PanelContainer>().Identifier("PaperContainer").Panel(paperBox),
            E<PanelContainer>()
                .Identifier("PaperDefaultBorder")
                .Prop(PanelContainer.StylePropertyPanel, paperBackground),
            E<PanelContainer>()
                .Identifier("PaperEditBackground")
                .Prop(PanelContainer.StylePropertyPanel, borderedTransparentBackground),
        ];
    }
}
