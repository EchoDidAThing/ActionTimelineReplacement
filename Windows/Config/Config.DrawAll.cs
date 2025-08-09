using System;
using System.Numerics;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Windowing;
using ActionTimelineReplacement.Base.Global;
using Dalamud.Bindings.ImGui;
#pragma warning disable CA1416 // Validate platform compatibility

namespace ActionTimelineReplacement.Windows;

public sealed partial class ConfigWindow : Window
{
    public override void Draw()
    {
        DrawHeader();

        using var table = ImRaii.Table("Main table", 2, ImGuiTableFlags.Resizable);
        if (!table) return;

        ImGui.TableSetupColumn("Sidebar", ImGuiTableColumnFlags.WidthFixed, 100 * CalcGlobals.GlobalScale());
        ImGui.TableNextColumn();

        try
        {
            using var style = ImRaii.PushStyle(ImGuiStyleVar.SelectableTextAlign, new Vector2(0.5f, 0.5f));

            DrawSidebar();
        }
        catch (Exception ex)
        {
            Service.Log.Warning(ex, "Sidebar error");
        }
        ImGui.TableNextColumn();

        try
        {
            DrawBodyMain();
        }
        catch (Exception ex)
        {
            Service.Log.Warning(ex, "Main body error");
        }

        try
        {
            DrawSheets();
        }
        catch (Exception ex)
        {
            Service.Log.Warning(ex, "Sheets error");
        }

        _dialogManager.Draw();
    }
}