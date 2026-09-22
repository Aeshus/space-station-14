using Robust.Shared.Utility;

namespace Content.Client.Stylesheets.SheetletConfigs;

[SheetletConfig]
public sealed partial class SwitchButtonConfig : SheetletConfig
{
    [DataField]
    public ResPath SwitchButtonTrackFillPath { get; set; }

    [DataField]
    public ResPath SwitchButtonTrackOutlinePath { get; set; }

    [DataField]
    public ResPath SwitchButtonThumbFillPath { get; set; }

    [DataField]
    public ResPath SwitchButtonThumbOutlinePath { get; set; }

    [DataField]
    public ResPath SwitchButtonSymbolOffPath { get; set; }

    [DataField]
    public ResPath SwitchButtonSymbolOnPath { get; set; }
}
