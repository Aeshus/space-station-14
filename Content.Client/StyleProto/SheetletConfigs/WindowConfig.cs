using Robust.Shared.Utility;

namespace Content.Client.StyleProto.SheetletConfigs;

[SheetletConfig]
public sealed partial class WindowConfig : SheetletConfig
{
    [DataField]
    public ResPath WindowHeaderTexturePath { get; set; }

    [DataField]
    public ResPath WindowHeaderAlertTexturePath { get; set; }

    [DataField]
    public ResPath WindowBackgroundPath { get; set; }

    [DataField]
    public ResPath WindowBackgroundBorderedPath { get; set; }

    [DataField]
    public ResPath TransparentWindowBackgroundBorderedPath { get; set; }
}
