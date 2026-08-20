using Content.Client.Stylesheets.Fonts;

namespace Content.Client.Stylesheets.SheetletConfigs;

public interface IFontConfig : ISheetletConfig
{
    List<(string?, int)> CommonFontSizes { get; }
    FontFamily BaseFont { get; }
    FontFamily MonoFont { get; }
    FontFamily DisplayFont { get; }
    FontFamily DecorativeFont { get; }
}
