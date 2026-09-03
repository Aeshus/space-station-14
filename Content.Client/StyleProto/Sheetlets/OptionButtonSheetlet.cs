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
public sealed partial class OptionButtonSheetlet : ISheetlet
{
    [Dependency] private IResourceCache _resourceCache = default!;

    public StyleRule[] Generate(SheetletConfigRegistry configs)
    {
        var icon = configs.GetConfig<IconConfig>();
        var palette = configs.GetConfig<PaletteConfig>();
        var invertedTriangleTex = _resourceCache.GetTexture(icon.InvertedTriangleIconPath);

        return
        [
            E<TextureRect>()
                .Class(OptionButton.StyleClassOptionTriangle)
                .Prop(TextureRect.StylePropertyTexture, invertedTriangleTex),
            E<Label>().Class(OptionButton.StyleClassOptionButton).AlignMode(Label.AlignMode.Center),
            E<PanelContainer>()
                .Class(OptionButton.StyleClassOptionsBackground)
                .Panel(new StyleBoxFlat(palette.PrimaryPalette.Background)),
        ];
    }
}
