using Robust.Shared.Utility;

namespace Content.Client.StyleProto.SheetletConfigs;

[SheetletConfig]
public sealed partial class SliderConfig : SheetletConfig
{
    [DataField]
    public ResPath SliderFillPath { get; set; }

    [DataField]
    public ResPath SliderOutlinePath { get; set; }

    [DataField]
    public ResPath SliderGrabber { get; set; }
}
