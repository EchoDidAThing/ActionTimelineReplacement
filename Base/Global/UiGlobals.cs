using ImGuiNET;

namespace ActionTimelineReplacement.Base.Global;

public class UiGlobals
{
    public static void DrawAddItem(string searchname)
    {
        if (ImGui.Button(" + "))
        {
            ImGui.OpenPopup(searchname);
        }
        else
        {
            return;
        }
    }

    public static void DrawItemSeparator()
    {
        ImGui.NewLine();
        ImGui.Separator();
        ImGui.NewLine();
    }
}