using Robust.Shared.Utility;

namespace Content.Client.StyleProto.SheetletConfigs;

[SheetletConfig]
public sealed partial class LineEditConfig : SheetletConfig
{
    [DataField]
    public ResPath LineEditPath { get; set; }
}
