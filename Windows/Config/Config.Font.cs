using Dalamud.Interface.Windowing;
using System;
using ImGuiNET;
using Dalamud.Interface.GameFonts;
#pragma warning disable CA1416 // Validate platform compatibility

namespace ActionTimelineReplacement.Windows;

public sealed partial class ConfigWindow : Window
{
    private static unsafe ImFontPtr GetFont(float size, GameFontFamily fontFamily = GameFontFamily.Axis)
    {
        var style = new GameFontStyle(GameFontStyle.GetRecommendedFamilyAndSize(fontFamily, size));

        var handle = Service.PluginInterface.UiBuilder.FontAtlas.NewGameFontHandle(style);

        try
        {
            var font = handle.Lock().ImFont;
            if ((IntPtr)font.NativePtr == IntPtr.Zero)
            {
                return ImGui.GetFont();
            }

            font.Scale = size / font.FontSize;
            return font;
        }
        catch
        {
            return ImGui.GetFont();
        }
    }
}