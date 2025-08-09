using ActionTimelineReplacement.Base.Setups;
using Dalamud.Interface.Windowing;
using Dalamud.Bindings.ImGui;
#pragma warning disable CA1416 // Validate platform compatibility

namespace ActionTimelineReplacement.Windows;

public sealed partial class ConfigWindow : Window
{
    private static void DrawHeader()
    {
        if (ImGui.Checkbox("Enable", ref Service.Config.EnableReplacement))
        {
            //SETUP ALL PROCESSING
            Setup.SetupAll();
            Service.Config.Save();
        }
        ImGui.SameLine();

        if (ImGui.Button("Redraw"))
        {
            //SETUP ALL PROCESSING
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
