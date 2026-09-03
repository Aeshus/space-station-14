using Content.Client.Stylesheets.Palette;
using Robust.Shared.Utility;

namespace Content.Client.StyleProto.SheetletConfigs;

[SheetletConfig]
public sealed partial class ButtonConfig : SheetletConfig
{
    [DataField]
    public ResPath BaseButtonPath { get; set; }

    [DataField]
    public ResPath OpenLeftButtonPath { get; set; }

    [DataField]
    public ResPath OpenRightButtonPath { get; set; }

    [DataField]
    public ResPath OpenBothButtonPath { get; set; }

    [DataField]
    public ResPath SmallButtonPath { get; set; }

    [DataField]
    public ResPath RoundedButtonPath { get; set; }

    [DataField]
    public ResPath RoundedButtonBorderedPath { get; set; }

    [DataField]
    public ResPath MonotoneBaseButtonPath { get; set; }

    [DataField]
    public ResPath MonotoneOpenLeftButtonPath { get; set; }

    [DataField]
    public ResPath MonotoneOpenRightButtonPath { get; set; }

    [DataField]
    public ResPath MonotoneOpenBothButtonPath { get; set; }

    [DataField]
    public ColorPalette ButtonPalette { get; set; }

    [DataField]
    public ColorPalette PositiveButtonPalette { get; set; }

    [DataField]
    public ColorPalette NegativeButtonPalette { get; set; }
}
