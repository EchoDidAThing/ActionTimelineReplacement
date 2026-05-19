using ActionTimelineReplacement.Base.Setups;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Windowing;
using FFXIVClientStructs.FFXIV.Common.Lua;
#pragma warning disable CA1416 // Validate platform compatibility

namespace ActionTimelineReplacement.Interface;

public sealed partial class ConfigWindow : Window
{
    private static void DrawHeader()
    {
        if (ImGui.Checkbox("Enable", ref Service.Config.EnableReplacement))
        {

            if (Service.Config.EnableReplacement)
            {
                Setup.SetupAll();
            }
            else
            {
                Setup.SetupAll(true);
            }
            Service.Config.Save();
        }
        ImGui.SameLine();

        if (ImGui.Button("Redraw"))
        {
            //Resets: Disables all changes, then enables all changes.
            Setup.SetupAll(true);
            Setup.SetupAll();
        }
        /*
        ImGui.SameLine();
        if (ImGui.Button(Service.Config.AdvancedMode ? "To Simple Mode" : "To Advanced Mode"))
        {
            Service.Config.AdvancedMode = !Service.Config.AdvancedMode;
            Service.Config.Save();
        }
        */
    }
}
