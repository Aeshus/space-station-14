using Robust.Shared.Utility;

namespace Content.Client.StyleProto.SheetletConfigs;

[SheetletConfig]
public sealed partial class RadialMenuConfig : SheetletConfig
{
    [DataField]
    public ResPath ButtonNormalPath { get; set; }

    [DataField]
    public ResPath ButtonHoverPath { get; set; }

    [DataField]
    public ResPath CloseNormalPath { get; set; }

    [DataField]
    public ResPath CloseHoverPath { get; set; }

    [DataField]
    public ResPath BackNormalPath { get; set; }

    [DataField]
    public ResPath BackHoverPath { get; set; }
}
