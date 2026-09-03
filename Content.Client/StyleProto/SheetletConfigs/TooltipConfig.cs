using Robust.Shared.Utility;

namespace Content.Client.StyleProto.SheetletConfigs;

[SheetletConfig]
public sealed partial class TooltipConfig : SheetletConfig
{
    [DataField]
    public ResPath TooltipBoxPath { get; set; }

    [DataField]
    public ResPath WhisperBoxPath { get; set; }
}
