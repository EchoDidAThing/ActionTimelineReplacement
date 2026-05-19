using System;
using System.Numerics;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Windowing;
using ActionTimelineReplacement.Base.Global;
using Dalamud.Bindings.ImGui;
#pragma warning disable CA1416 // Validate platform compatibility

namespace ActionTimelineReplacement.Interface;

public sealed partial class ConfigWindow : Window
{
    public override void Draw()
    {

        using var table = ImRaii.Table("Main table", 2);
        if (!table) return;
        ImGui.TableSetupColumn("Main table", ImGuiTableColumnFlags.NoResize, 25);

        ImGui.TableSetupColumn("Sidebar", ImGuiTableColumnFlags.NoResize, 100 * CalcGlobals.GlobalScale());
        ImGui.TableNextColumn();

        DrawHeader();
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
            DrawSheetButtons();
        }
        catch (Exception ex)
        {
            Service.Log.Warning(ex, "Sheetbuttons error");
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