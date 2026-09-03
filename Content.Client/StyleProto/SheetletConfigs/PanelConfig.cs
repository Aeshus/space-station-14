using Robust.Shared.Utility;

namespace Content.Client.StyleProto.SheetletConfigs;

[SheetletConfig]
public sealed partial class PanelConfig : SheetletConfig
{
    [DataField]
    public ResPath GeometricPanelBorderPath { get; set; }

    [DataField]
    public ResPath BlackPanelDarkThinBorderPath { get; set; }

    [DataField]
    public Color DeepPanelBackgroundColor { get; set; }

    [DataField]
    public Color DeepPanelBorderColor { get; set; }
}
