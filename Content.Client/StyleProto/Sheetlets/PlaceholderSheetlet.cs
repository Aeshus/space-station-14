using Content.Client.Resources;
using Content.Client.StyleProto.SheetletConfigs;
using Content.Client.UserInterface.Controls;
using JetBrains.Annotations;
using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using static Content.Client.Stylesheets.StylesheetHelpers;

namespace Content.Client.StyleProto.Sheetlets;

[Sheetlet]
[UsedImplicitly]
public sealed partial class PlaceholderSheetlet : ISheetlet
{
    [Dependency] private IResourceCache _resourceCache = default!;

    public StyleRule[] Generate(SheetletConfigRegistry configs)
    {
        var placeholder = configs.GetConfig<PlaceholderConfig>();
        var font = configs.GetConfig<FontConfig>();
        var placeholderBox = _resourceCache.GetTexture(placeholder.PlaceholderPath)
            .IntoPatch(StyleBox.Margin.All, 19);
        placeholderBox.SetExpandMargin(StyleBox.Margin.All, -5);
        placeholderBox.Mode = StyleBoxTexture.StretchMode.Tile;

        return
        [
            E<Placeholder>()
                .Prop(Placeholder.StylePropertyPanel, placeholderBox),
            E<Label>()
                .Class(Placeholder.StyleClassPlaceholderText)
                .Font(font.BaseFont.GetFont(16))
                .FontColor(new Color(103, 103, 103, 128)),
        ];
    }
}
