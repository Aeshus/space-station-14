using Robust.Client.Graphics;
using Robust.Client.UserInterface.RichText;
using Robust.Shared.Prototypes;

namespace Content.Client.Stylesheets.Fonts;

internal sealed partial class FontSelectionManager
{
    private static readonly Dictionary<ProtoId<FontPrototype>, (StandardFontType, FontKind)> FontHijacks = new()
    {
        { "Default", (StandardFontType.Main, FontKind.Regular) },
        { "DefaultItalic", (StandardFontType.Main, FontKind.Italic) },
        { "DefaultBold", (StandardFontType.Main, FontKind.Bold) },
        { "DefaultBoldItalic", (StandardFontType.Main, FontKind.BoldItalic) },

        { "NotoSansDisplay", (StandardFontType.Display, FontKind.Regular) },
        { "NotoSansDisplayItalic", (StandardFontType.Display, FontKind.Italic) },
        { "NotoSansDisplayBold", (StandardFontType.Display, FontKind.Bold) },
        { "NotoSansDisplayBoldItalic", (StandardFontType.Display, FontKind.BoldItalic) },

        { "BoxRound", (StandardFontType.Decorative, FontKind.Regular) },
        { "Monospace", (StandardFontType.Monospace, FontKind.Regular) },
    };

    private Font? HijackFontTag(ProtoId<FontPrototype> protoId, int size)
    {
        if (FontHijacks.TryGetValue(protoId, out var font))
            return GetFont(font.Item1, size, font.Item2);

        return null;
    }
}
