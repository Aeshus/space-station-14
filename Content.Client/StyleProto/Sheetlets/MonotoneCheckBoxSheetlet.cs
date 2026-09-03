using Content.Client.Resources;
using Content.Client.UserInterface.Controls;
using JetBrains.Annotations;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Utility;
using static Content.Client.Stylesheets.StylesheetHelpers;

namespace Content.Client.StyleProto.Sheetlets;

[Sheetlet]
[UsedImplicitly]
public sealed partial class MonotoneCheckBoxSheetlet : ISheetlet
{
    [Dependency] private IResourceCache _resourceCache = default!;

    public StyleRule[] Generate(SheetletConfigRegistry configs)
    {
        var monotoneCheckBoxTextureChecked = _resourceCache.GetTexture(
            new ResPath("/Textures/Interface/Nano/Monotone/monotone_checkbox_checked.svg.96dpi.png"));
        var monotoneCheckBoxTextureUnchecked = _resourceCache.GetTexture(
            new ResPath("/Textures/Interface/Nano/Monotone/monotone_checkbox_unchecked.svg.96dpi.png"));

        return
        [
            E<TextureRect>()
                .Class(MonotoneCheckBox.StyleClassMonotoneCheckBox)
                .Prop(TextureRect.StylePropertyTexture, monotoneCheckBoxTextureUnchecked),
            E<TextureRect>()
                .Class(MonotoneCheckBox.StyleClassMonotoneCheckBox)
                .Class(CheckBox.StyleClassCheckBoxChecked)
                .Prop(TextureRect.StylePropertyTexture, monotoneCheckBoxTextureChecked),
        ];
    }
}
