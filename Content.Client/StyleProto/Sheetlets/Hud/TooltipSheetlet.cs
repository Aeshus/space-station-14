using Content.Client.Examine;
using Content.Client.Resources;
using Content.Client.StyleProto.SheetletConfigs;
using Content.Client.Stylesheets.Fonts;
using JetBrains.Annotations;
using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.CustomControls;
using static Content.Client.Stylesheets.StylesheetHelpers;
using StyleClass = Content.Client.Stylesheets.StyleClass;

namespace Content.Client.StyleProto.Sheetlets.Hud;

[Sheetlet]
[UsedImplicitly]
public sealed partial class TooltipSheetlet : ISheetlet
{
    [Dependency] private IResourceCache _resourceCache = default!;

    public StyleRule[] Generate(SheetletConfigRegistry configs)
    {
        var tooltip = configs.GetConfig<TooltipConfig>();
        var font = configs.GetConfig<FontConfig>();
        var tooltipBox = _resourceCache.GetTexture(tooltip.TooltipBoxPath)
            .IntoPatch(StyleBox.Margin.All, 2);
        tooltipBox.SetContentMarginOverride(StyleBox.Margin.Horizontal, 7);

        var whisperBox = _resourceCache.GetTexture(tooltip.WhisperBoxPath)
            .IntoPatch(StyleBox.Margin.All, 2);
        whisperBox.SetContentMarginOverride(StyleBox.Margin.Horizontal, 7);

        return
        [
            E<PanelContainer>()
                .Class(StyleClass.TooltipPanel)
                .Modulate(Color.Gray.WithAlpha(0.9f))
                .Panel(tooltipBox),
            E<RichTextLabel>()
                .Class(StyleClass.TooltipTitle)
                .Font(font.BaseFont.GetFont(14, FontKind.Bold)),
            E<RichTextLabel>()
                .Class(StyleClass.TooltipDesc)
                .Font(font.BaseFont.GetFont(12)),

            E<Tooltip>()
                .Prop(Tooltip.StylePropertyPanel, tooltipBox),
            E<PanelContainer>()
                .Class(ExamineSystem.StyleClassEntityTooltip)
                .Panel(tooltipBox),
            E<PanelContainer>()
                .Class("speechBox", "sayBox")
                .Panel(tooltipBox),
            E<PanelContainer>()
                .Class("speechBox", "whisperBox")
                .Panel(whisperBox),

            E<PanelContainer>()
                .Class("speechBox", "whisperBox")
                .ParentOf(E<RichTextLabel>().Class("bubbleContent"))
                .Prop(Label.StylePropertyFont, font.BaseFont.GetFont(12, FontKind.Italic)),
            E<PanelContainer>()
                .Class("speechBox", "emoteBox")
                .ParentOf(E<RichTextLabel>().Class("bubbleContent"))
                .Prop(Label.StylePropertyFont, font.BaseFont.GetFont(12, FontKind.Italic)),
        ];
    }
}
