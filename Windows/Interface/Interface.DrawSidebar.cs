using ActionTimelineReplacement.Base.Global;
using ActionTimelineReplacement.Base.Setups;
using ActionTimelineReplacement.Sheets;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Colors;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Windowing;
using Lumina.Excel.Sheets;
using System;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
#pragma warning disable CA1416 // Validate platform compatibility

namespace ActionTimelineReplacement.Interface;

public sealed partial class ConfigWindow : Window
{

    private string CreateName = string.Empty;
    private void DrawSidebar()
    {

        using var windowPadding = ImRaii.PushStyle(ImGuiStyleVar.WindowPadding, Vector2.Zero);

        using var child = ImRaii.Child("Sidebar", -Vector2.One, true);
        if (!child) return;

        using (ImRaii.Child("Items", CalcGlobals.BodyScale(), false))
        {
            //var span = CollectionsMarshal.AsSpan(Service.Config.ReplacementSets);
            for (var i = 0; i < Service.Config.ReplacementSets.Count(); i++)
            {
                var set = Service.Config.ReplacementSets[i];
                // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
                if (set is null) continue;
                using var style = !Service.Config.ReplacementSets[i].Enabled ? ImRaii.PushColor(ImGuiCol.Text, ImGuiColors.DalamudGrey) : null;
                if (ImGui.Selectable($"{Service.Config.ReplacementSets[i].Name}## {i}", _activeSet == Service.Config.ReplacementSets[i]))
                {
                    _activeSet = Service.Config.ReplacementSets[i];
                    //Service.Log.Error("activeset is " + _activeSet.ActionTransientsWriter[0].Replacement.ActionName);
                }

                var popupId = $"Set{i}Popup";
                if (ImGui.IsItemClicked(ImGuiMouseButton.Right))
                {
                    ImGui.OpenPopup(popupId);
                }

                using var popup = ImRaii.Popup(popupId);
                if (popup)
                {
                    using var popupChild = ImRaii.Child("Popup" + popupId,
                        new Vector2(80 * CalcGlobals.GlobalScale(), 60 * CalcGlobals.GlobalScale()),
                        true);

                    if (ImGui.Selectable("Export##"+i))
                    {
                        var setid = i;
                        _dialogManager.SaveFileDialog("Save", ".json", Service.Config.ReplacementSets[setid].Name, ".json", (b, file) =>
                        {
                            if (!b) return;
                            Configuration.ReplacementSet.Save(file, setid);
                        });
                    }

                    if (ImGui.Selectable("Delete##"+i))
                    {
                        if (_activeSet == Service.Config.ReplacementSets[i]) _activeSet = null;
                        Service.Config.ReplacementSets.Remove(set);
                        Service.Config.Save();
                        Setup.SetupAll(true);
                        Setup.SetupAll();
                        ImGui.CloseCurrentPopup();
                    }
                }
            }
        }
        ImGui.SetCursorPosY(ImGui.GetWindowSize().Y - CalcGlobals.ItemHeight());

        using (ImRaii.PushStyle(ImGuiStyleVar.ItemSpacing, new Vector2(0, 0)))
        {
            using var corner = ImRaii.PushStyle(ImGuiStyleVar.FrameRounding, 0);
            var width = Math.Max(ImGui.GetWindowWidth() / 2, 60 * CalcGlobals.GlobalScale());
            var buttonSize = new Vector2(width, 0);

            ImGui.PushItemWidth(width);

            if (ImGui.Button("Create", buttonSize))
            {
                ImGui.OpenPopup("##CreateSet");
            }
            ImGui.SameLine();

            if (ImGui.Button("Import", buttonSize))
            {
                _dialogManager.OpenFileDialog("Import", ".json", (b, files) =>
                {
                    if (!b) return;
                    foreach (var file in files)
                    {
                        if (Configuration.ReplacementSet.Load(file) is not { } set) continue;
                        Service.Config.ReplacementSets.Add(set);
                        Setup.SetupAll();
                    }
                    Service.Config.Save();
                }, 10, ".");
            }
            ImGui.PopItemWidth();
        }


        using var createMenu = ImRaii.Popup("##CreateSet");
        if (createMenu)
        {
            ImGui.SetNextItemWidth(CalcGlobals.XY());
            if (ImGui.InputText("##Create", ref CreateName, 512, ImGuiInputTextFlags.EnterReturnsTrue))
            {
                var localname = CreateName;
                var exists = false;
                if (string.IsNullOrEmpty(localname)) { return; }
                foreach (var set in Service.Config.ReplacementSets) { if (set.Name == localname) { exists = true; break; } }
                if (exists) { return; }
                Service.Config.ReplacementSets.Add(new Configuration.ReplacementSet((string)localname, true, 0, new Configuration.JobArray(), Service.PlayerState.CharacterName, Service.PlayerState.HomeWorld.RowId, [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], [], []));
                CreateName = "";
                Service.Config.Save();

            }

        }
    }
}


