using Robust.Shared.Utility;

namespace Content.Client.Stylesheets.SheetletConfigs;

[SheetletConfig]
public sealed partial class NanoHeadingConfig : SheetletConfig
{
    [DataField]
    public ResPath NanoHeadingPath { get; set; }
}
