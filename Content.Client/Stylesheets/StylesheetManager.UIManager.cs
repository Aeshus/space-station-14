using Robust.Client.UserInterface;

namespace Content.Client.Stylesheets;

public sealed partial class StylesheetManager
{
    [Dependency] private IUserInterfaceManager _userInterfaceManager = default!;

    /// <summary>
    /// The style accessor for the default stylesheet.
    /// </summary>
    private IStyleAccessor _defaultAccessor = default!;

    /// <summary>
    /// Sets the default theme to the default stylesheet.
    /// </summary>
    private void SetDefaultTheme()
    {
        _defaultAccessor = GetStyleSubscription(DefaultStylesheet);
        _defaultAccessor.StyleChanged += OnStyleChanged;
    }

    /// <inheritdoc cref="IStyleAccessor.StyleChanged"/>
    private void OnStyleChanged(Stylesheet stylesheet, SheetletConfigRegistry configs)
    {
        _userInterfaceManager.Stylesheet = stylesheet;
    }
}
