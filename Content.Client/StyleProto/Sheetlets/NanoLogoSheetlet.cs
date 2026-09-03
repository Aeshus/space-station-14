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
public sealed partial class NanoLogoSheetlet : ISheetlet
{
    [Dependency] private IResourceCache _resourceCache = default!;

    public StyleRule[] Generate(SheetletConfigRegistry configs)
    {
        return
        [
            E<TextureRect>()
                .Class("NTLogoDark")
                .Prop(TextureRect.StylePropertyTexture,
                    _resourceCache.GetTexture(new ResPath("/Textures/Interface/Nano/ntlogo.svg.png")))
                .Prop(Control.StylePropertyModulateSelf, Color.FromHex("#757575")),
        ];
    }
}
