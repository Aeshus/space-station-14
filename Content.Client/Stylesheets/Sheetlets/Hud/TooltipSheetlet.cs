using Content.Client.Examine;
using Content.Client.Resources;
using Content.Client.Stylesheets.SheetletConfigs;
using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.CustomControls;
using static Content.Client.Stylesheets.StylesheetHelpers;

namespace Content.Client.Stylesheets.Sheetlets.Hud;

[Sheetlet]
public sealed partial class TooltipSheetlet<T> : ISheetlet
{
    [Dependency] private IResourceCache _resCache = default!;
    public StyleRule[] Generate(SheetletConfigRegistry configs)
    {
        var tooltips = configs.GetConfig<TooltipConfig>();
        var fonts = configs.GetConfig<FontConfig>();

        var tooltipBox = _resCache.GetTexture(tooltips.TooltipBoxPath)
            .IntoPatch(StyleBox.Margin.All, 2);
        tooltipBox.SetContentMarginOverride(StyleBox.Margin.Horizontal, 7);

        var whisperBox = _resCache.GetTexture(tooltips.WhisperBoxPath)
            .IntoPatch(StyleBox.Margin.All, 2);
        whisperBox.SetContentMarginOverride(StyleBox.Margin.Horizontal, 7);

        return
        [
            E<PanelContainer>()
                .Class(StyleClass.TooltipPanel)
                .Modulate(Color.Gray.WithAlpha(0.9f)) // TODO: you know the drill by now
                .Panel(tooltipBox),
            E<RichTextLabel>()
                .Class(StyleClass.TooltipTitle)
                .Font(fonts.Main.GetFont(14, FontWeight.Bold)),
            E<RichTextLabel>()
                .Class(StyleClass.TooltipDesc)
                .Font(fonts.Main.GetFont(12)),

            E<Tooltip>()
                // ReSharper disable once AccessToStaticMemberViaDerivedType
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
                .Prop(Label.StylePropertyFont, fonts.Main.GetFont(12, slant: FontSlant.Italic)),
            E<PanelContainer>()
                .Class("speechBox", "emoteBox")
                .ParentOf(E<RichTextLabel>().Class("bubbleContent"))
                .Prop(Label.StylePropertyFont, fonts.Main.GetFont(12, slant: FontSlant.Italic)),
        ];
    }
}
