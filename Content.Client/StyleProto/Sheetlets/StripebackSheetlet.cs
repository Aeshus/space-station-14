using Content.Client.Resources;
using Content.Client.StyleProto.SheetletConfigs;
using Content.Client.UserInterface.Controls;
using JetBrains.Annotations;
using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface;
using static Content.Client.Stylesheets.StylesheetHelpers;

namespace Content.Client.StyleProto.Sheetlets;

[Sheetlet]
[UsedImplicitly]
public sealed partial class StripebackSheetlet : ISheetlet
{
    [Dependency] private IResourceCache _resourceCache = default!;

    public StyleRule[] Generate(SheetletConfigRegistry configs)
    {
        var config = configs.GetConfig<StripebackConfig>();
        var stripeBack = new StyleBoxTexture
        {
            Texture = _resourceCache.GetTexture(config.StripebackPath),
            Mode = StyleBoxTexture.StretchMode.Tile,
        };

        return
        [
            E<StripeBack>()
                .Prop(StripeBack.StylePropertyBackground, stripeBack),
        ];
    }
}
