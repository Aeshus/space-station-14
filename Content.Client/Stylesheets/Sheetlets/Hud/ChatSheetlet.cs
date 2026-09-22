using Content.Client.Resources;
using Content.Client.Stylesheets.SheetletConfigs;
using Content.Client.UserInterface.Systems.Chat.Controls;
using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using static Content.Client.Stylesheets.StylesheetHelpers;

namespace Content.Client.Stylesheets.Sheetlets.Hud;

[Sheetlet]
public sealed partial class ChatSheetlet : ISheetlet
{
    [Dependency] private IResourceCache _resCache = default!;

    public StyleRule[] Generate(SheetletConfigRegistry configs)
    {
        var buttons = configs.GetConfig<ButtonConfig>();
        var palettes = configs.GetConfig<PaletteConfig>();

        var chatColor = palettes.SecondaryPalette.Background.WithAlpha(221.0f / 255.0f);
        var chatBg = new StyleBoxFlat(chatColor);

        var chatChannelButtonTex =
            _resCache.GetTexture(buttons.RoundedButtonBorderedPath);
        var chatChannelButton = new StyleBoxTexture
        {
            Texture = chatChannelButtonTex,
        };
        chatChannelButton.SetPatchMargin(StyleBox.Margin.All, 5);
        chatChannelButton.SetPadding(StyleBox.Margin.All, 2);

        var chatFilterButtonTex =
            _resCache.GetTexture(buttons.RoundedButtonBorderedPath);
        var chatFilterButton = new StyleBoxTexture
        {
            Texture = chatFilterButtonTex,
        };
        chatFilterButton.SetPatchMargin(StyleBox.Margin.All, 5);
        chatFilterButton.SetPadding(StyleBox.Margin.All, 2);

        return
        [
            E<PanelContainer>()
                .Class(ChatInputBox.StyleClassChatPanel)
                .Panel(chatBg),
            E<LineEdit>()
                .Class(ChatInputBox.StyleClassChatLineEdit)
                .Prop(LineEdit.StylePropertyStyleBox, new StyleBoxEmpty()),
            E<Button>().Class(ChatInputBox.StyleClassChatFilterOptionButton).Box(chatChannelButton),
            E<ContainerButton>().Class(ChatInputBox.StyleClassChatFilterOptionButton).Box(chatFilterButton),
        ];
    }
}
