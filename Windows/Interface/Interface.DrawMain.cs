using System.Numerics;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Windowing;
using ActionTimelineReplacement.Base.Global;
using ActionTimelineReplacement.Base.Setups;
using Dalamud.Bindings.ImGui;
#pragma warning disable CA1416 // Validate platform compatibility

namespace ActionTimelineReplacement.Interface;

public sealed partial class ConfigWindow : Window
{
    private void DrawBodyMain()
    {
        using var child = ImRaii.Child("Main body", new Vector2(-1f, 60f), false);
        if (!child) return;
        if (_activeSet is null) return;

        using (ImRaii.PushFont(GetFont(20)))
        {
            var width = ImGui.CalcTextSize(_activeSet.Name).X + ImGui.GetStyle().FramePadding.X * 2;
            var x = ImGui.GetWindowWidth() / 2 - width / 2;

            ImGui.SetCursorPosX(x);
            ImGui.SetNextItemWidth(width);

            if (ImGui.InputText("##Name", ref _activeSet.Name, 256))
            {
                Service.Config.Save();
            }
        }

        if (ImGui.Checkbox("Enable", ref _activeSet.Enabled))
        {
            Setup.SetupAll();
            Service.Config.Save();
        }
        ImGui.SameLine();
        ImGui.SetNextItemWidth(50 * CalcGlobals.GlobalScale());

        if (ImGui.DragInt("Priority", ref _activeSet.Priority))
        {
            Setup.SetupAll();
            Service.Config.Save();
        }
    }
}