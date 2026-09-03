using Robust.Shared.Utility;

namespace Content.Client.StyleProto.SheetletConfigs;

[SheetletConfig]
public sealed partial class IconConfig : SheetletConfig
{
    [DataField]
    public ResPath HelpIconPath { get; set; }

    [DataField]
    public ResPath CrossIconPath { get; set; }

    [DataField]
    public ResPath RefreshIconPath { get; set; }

    [DataField]
    public ResPath InvertedTriangleIconPath { get; set; }
}
