using Content.Client.Resources;
using Content.Client.StyleProto.SheetletConfigs;
using JetBrains.Annotations;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using static Content.Client.Stylesheets.StylesheetHelpers;

namespace Content.Client.StyleProto.Sheetlets;

[Sheetlet]
[UsedImplicitly]
public sealed partial class RadialMenuSheetlet : ISheetlet
{
    [Dependency] private IResourceCache _resourceCache = default!;

    public StyleRule[] Generate(SheetletConfigRegistry configs)
    {
        var config = configs.GetConfig<RadialMenuConfig>();
        var btnNormalTex = _resourceCache.GetTexture(config.ButtonNormalPath);
        var btnHoverTex = _resourceCache.GetTexture(config.ButtonHoverPath);
        var closeNormalTex = _resourceCache.GetTexture(config.CloseNormalPath);
        var closeHoverTex = _resourceCache.GetTexture(config.CloseHoverPath);
        var backNormalTex = _resourceCache.GetTexture(config.BackNormalPath);
        var backHoverTex = _resourceCache.GetTexture(config.BackHoverPath);

        return
        [
            E<TextureButton>()
                .Class("RadialMenuButton")
                .Prop(TextureButton.StylePropertyTexture, btnNormalTex),
            E<TextureButton>()
                .Class("RadialMenuButton")
                .Pseudo(TextureButton.StylePseudoClassHover)
                .Prop(TextureButton.StylePropertyTexture, btnHoverTex),

            E<TextureButton>()
                .Class("RadialMenuCloseButton")
                .Prop(TextureButton.StylePropertyTexture, closeNormalTex),
            E<TextureButton>()
                .Class("RadialMenuCloseButton")
                .Pseudo(TextureButton.StylePseudoClassHover)
                .Prop(TextureButton.StylePropertyTexture, closeHoverTex),

            E<TextureButton>()
                .Class("RadialMenuBackButton")
                .Prop(TextureButton.StylePropertyTexture, backNormalTex),
            E<TextureButton>()
                .Class("RadialMenuBackButton")
                .Pseudo(TextureButton.StylePseudoClassHover)
                .Prop(TextureButton.StylePropertyTexture, backHoverTex),
        ];
    }
}
