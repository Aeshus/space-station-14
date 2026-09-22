using Robust.Shared.Utility;

namespace Content.Client.Stylesheets.SheetletConfigs;

[SheetletConfig]
public sealed partial class PlaceholderConfig : SheetletConfig
{
    [DataField]
    public ResPath PlaceholderPath { get; set; }
}
