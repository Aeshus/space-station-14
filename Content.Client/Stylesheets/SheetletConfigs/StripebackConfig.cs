using Robust.Shared.Utility;

namespace Content.Client.Stylesheets.SheetletConfigs;

[SheetletConfig]
public sealed partial class StripebackConfig : SheetletConfig
{
    [DataField]
    public ResPath StripebackPath { get; set; }
}
