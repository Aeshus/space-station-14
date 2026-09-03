using Content.Client.StyleProto.SheetletConfigs;
using Content.Client.Stylesheets.Fonts;
using JetBrains.Annotations;
using Robust.Client.UserInterface;
using static Content.Client.Stylesheets.StylesheetHelpers;
using StyleClass = Content.Client.Stylesheets.StyleClass;

namespace Content.Client.StyleProto.Sheetlets;

/// These are not in <see cref="LabelSheetlet"/> because a label is not the only thing you might want to be monospaced.
[Sheetlet]
[UsedImplicitly]
public sealed partial class TextSheetlet : ISheetlet
{
    public StyleRule[] Generate(SheetletConfigRegistry configs)
    {
        var config = configs.GetConfig<FontConfig>();
        var mono = config.MonoFont.GetFont(12);

        return
        [
            E().Class(StyleClass.Monospace).Font(mono),
            E().Class(StyleClass.Italic).Font(config.BaseFont.GetFont(12, FontKind.Italic)),
            E().Class(StyleClass.FontLarge).Font(config.BaseFont.GetFont(14)),
            E().Class(StyleClass.FontSmall).Font(config.BaseFont.GetFont(10)),
        ];
    }
}
