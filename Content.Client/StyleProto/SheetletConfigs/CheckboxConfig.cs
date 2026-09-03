using Robust.Shared.Utility;

namespace Content.Client.StyleProto.SheetletConfigs;

[SheetletConfig]
public sealed partial class CheckboxConfig : SheetletConfig
{
    [DataField]
    public ResPath CheckboxUncheckedPath { get; set; }

    [DataField]
    public ResPath CheckboxCheckedPath { get; set; }
}
