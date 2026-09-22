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
public sealed partial class PlaceholderSheetlet : ISheetlet
{
    [Dependency] private IResourceCache _resCache = default!;

    public StyleRule[] Generate(SheetletConfigRegistry configs)
    {
        var placeholders = configs.GetConfig<PlaceholderConfig>();
        var fonts = configs.GetConfig<FontConfig>();

        var placeholderBox = _resCache.GetTexture(placeholders.PlaceholderPath)
            .IntoPatch(StyleBox.Margin.All, 19);
        placeholderBox.SetExpandMargin(StyleBox.Margin.All, -5);
        placeholderBox.Mode = StyleBoxTexture.StretchMode.Tile;

        return
        [
            E<Placeholder>()
                // ReSharper disable once AccessToStaticMemberViaDerivedType
                .Prop(Placeholder.StylePropertyPanel, placeholderBox),
            E<Label>()
                .Class(Placeholder.StyleClassPlaceholderText)
                .Font(fonts.BaseFont.GetFont(16))
                .FontColor(new Color(103, 103, 103, 128)), // TODO: fix hardcoded color
        ];
    }
}
