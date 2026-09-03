using Content.Client.StyleProto.SheetletConfigs;
using Content.Client.Stylesheets.Fonts;
using JetBrains.Annotations;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using static Content.Client.Stylesheets.StylesheetHelpers;

namespace Content.Client.StyleProto.Sheetlets;

[Sheetlet]
[UsedImplicitly]
public sealed partial class FontSheetlet : ISheetlet
{
    public StyleRule[] Generate(SheetletConfigRegistry configs)
    {
        var config = configs.GetConfig<FontConfig>();
        var rules = new List<StyleRule>
        {
            // Default font
            E().Prop(Label.StylePropertyFont, config.BaseFont.GetFont(config.CommonFontSizes[0].Item2)),
        };

        foreach (var (name, size) in config.CommonFontSizes)
        {
            foreach (var kind in Enum.GetValues<FontKind>())
            {
                var builder = E().Class(GetFontClass(kind));

                if (name is not null)
                    builder.Class(name);

                builder.Prop(Label.StylePropertyFont, config.BaseFont.GetFont(size, kind));
                rules.Add(builder);
            }
        }

        return rules.ToArray();
    }

    private static string GetFontClass(FontKind kind, string? prefix = null)
    {
        var kindStr = kind.ToString().ToLowerInvariant();
        return prefix is null ? $"font-{kindStr}" : $"{prefix}-{kindStr}";
    }
}
