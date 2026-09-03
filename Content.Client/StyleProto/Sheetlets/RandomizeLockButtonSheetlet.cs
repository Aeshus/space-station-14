using System.Numerics;
using Content.Client.Resources;
using JetBrains.Annotations;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Utility;
using static Content.Client.Stylesheets.StylesheetHelpers;

namespace Content.Client.StyleProto.Sheetlets;

[Sheetlet]
[UsedImplicitly]
public sealed partial class RandomizeLockButtonSheetlet : ISheetlet
{
    [Dependency] private IResourceCache _resourceCache = default!;

    public StyleRule[] Generate(SheetletConfigRegistry configs)
    {
        var textureLocked = _resourceCache.GetTexture(
            new ResPath("/Textures/Interface/VerbIcons/lock.svg.192dpi.png"));
        var textureUnlocked = _resourceCache.GetTexture(
            new ResPath("/Textures/Interface/VerbIcons/unlock.svg.192dpi.png"));

        return
        [
            E<TextureButton>()
                .Identifier("RandomizerLockButton")
                .Modulate(Color.FromHsl(new Vector4(0f, 0f, .65f, 1f)))
                .Prop(TextureButton.StylePropertyTexture, textureUnlocked)
                .Margin(new Thickness(0f, 0f, 3f, 0f)),

            E<TextureButton>()
                .Identifier("RandomizerLockButton")
                .PseudoHovered()
                .Modulate(Color.WhiteSmoke)
                .Prop(TextureButton.StylePropertyTexture, textureLocked),

            E<TextureButton>()
                .Identifier("RandomizerLockButton")
                .PseudoPressed()
                .Modulate(Color.WhiteSmoke)
                .Prop(TextureButton.StylePropertyTexture, textureLocked),
        ];
    }
}
