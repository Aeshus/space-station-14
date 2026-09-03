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
public sealed partial class LineEditSheetlet : ISheetlet
{
    [Dependency] private IResourceCache _resourceCache = default!;

    public StyleRule[] Generate(SheetletConfigRegistry configs)
    {
        var config = configs.GetConfig<LineEditConfig>();
        var lineEditStylebox = _resourceCache.GetTexture(config.LineEditPath)
            .IntoPatch(StyleBox.Margin.All, 3);
        lineEditStylebox.SetContentMarginOverride(StyleBox.Margin.Horizontal, 5);

        return
        [
            E<LineEdit>()
                .Prop(LineEdit.StylePropertyStyleBox, lineEditStylebox),
            E<LineEdit>()
                .Class(LineEdit.StyleClassLineEditNotEditable)
                .Prop("font-color", new Color(192, 192, 192)),
            E<LineEdit>()
                .Pseudo(LineEdit.StylePseudoClassPlaceholder)
                .Prop("font-color", Color.Gray),
            E<TextEdit>()
                .Pseudo(TextEdit.StylePseudoClassPlaceholder)
                .Prop("font-color", Color.Gray),
        ];
    }
}
