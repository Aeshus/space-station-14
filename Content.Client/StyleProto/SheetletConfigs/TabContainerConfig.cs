using Robust.Shared.Utility;

namespace Content.Client.StyleProto.SheetletConfigs;

[SheetletConfig]
public sealed partial class TabContainerConfig : SheetletConfig
{
    [DataField]
    public ResPath TabContainerPanelPath { get; set; }
}
