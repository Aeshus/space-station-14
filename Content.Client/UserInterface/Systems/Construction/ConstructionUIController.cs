using Content.Client.Construction;
using Content.Client.Gameplay;
using Content.Client.UserInterface.Controls;
using Content.Client.UserInterface.Systems.Construction.Windows;
using Content.Client.UserInterface.Systems.Gameplay;
using Content.Client.UserInterface.Systems.MenuBar.Widgets;
using Content.Shared.Input;
using JetBrains.Annotations;
using Robust.Client.Placement;
using Robust.Client.Player;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controllers;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Input.Binding;
using Robust.Shared.Prototypes;

namespace Content.Client.UserInterface.Systems.Construction;

/// <summary>
/// Handles loading and unloading the <see cref="ConstructionWindow"/>.
/// </summary>
[UsedImplicitly]
public sealed class ConstructionUIController : UIController, IOnStateChanged<GameplayState>
{
    private ConstructionWindow? _window;

    private MenuButton? CraftingButton => UIManager.GetActiveUIWidgetOrNull<GameTopMenuBar>()?.CraftingButton;

    public override void Initialize()
    {
        base.Initialize();

        var gameplayStateLoad = UIManager.GetUIController<GameplayStateLoadController>();
        gameplayStateLoad.OnScreenLoad += OnScreenLoad;
        gameplayStateLoad.OnScreenUnload += OnScreenUnload;
    }

    private void OnScreenLoad()
    {
        LoadGui();
    }

    private void OnScreenUnload()
    {
        UnloadGui();
    }

    private void UnloadGui()
    {
        if (_window == null)
            return;

        _window.OnOpen -= OnWindowOpened;
        _window.OnClose -= OnWindowClosed;

        _window.Dispose();
        _window = null;
    }

    private void LoadGui()
    {
        UnloadGui();

        _window = UIManager.CreateWindow<ConstructionWindow>();
        LayoutContainer.SetAnchorPreset(_window, LayoutContainer.LayoutPreset.CenterTop);

        _window.OnOpen += OnWindowOpened;
        _window.OnClose += OnWindowClosed;
    }

    private void OnWindowOpened()
    {
        CraftingButton?.SetClickPressed(true);
    }

    private void OnWindowClosed()
    {
        CraftingButton?.SetClickPressed(false);
    }

    public void OnStateEntered(GameplayState state)
    {
        var builder = CommandBinds.Builder;

        builder
            .Bind(ContentKeyFunctions.OpenCraftingMenu,
                InputCmdHandler.FromDelegate(_ => ToggleWindow()))
            .Register<ConstructionUIController>();
    }

    public void OnStateExited(GameplayState state)
    {
        UnloadGui();

        CommandBinds.Unregister<ConstructionUIController>();
    }

    internal void UnloadButton()
    {
        if (CraftingButton == null)
            return;

        CraftingButton.Pressed = false;
        CraftingButton.OnToggled -= CraftingButtonToggled;
    }

    internal void LoadButton()
    {
        if (CraftingButton == null)
            return;

        CraftingButton.OnToggled += CraftingButtonToggled;
    }

    private void CraftingButtonToggled(BaseButton.ButtonEventArgs args)
    {
        ToggleWindow();
    }

    private void ToggleWindow()
    {
        if (_window == null)
            return;

        if (_window.IsOpen)
            _window.Close();
        else
            _window.Open();
    }
}
