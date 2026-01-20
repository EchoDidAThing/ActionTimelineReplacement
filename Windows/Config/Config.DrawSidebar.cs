using System;
using System.Numerics;
using System.Runtime.InteropServices;
using ActionTimelineReplacement.Base.Global;
using ActionTimelineReplacement.Base.Setups;
using ActionTimelineReplacement.Sheets;
using Dalamud.Interface.Colors;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Windowing;
using Dalamud.Bindings.ImGui;
#pragma warning disable CA1416 // Validate platform compatibility

namespace ActionTimelineReplacement.Windows;

public sealed partial class ConfigWindow : Window
{
    private void DrawSidebar()
    {
        using var windowPadding = ImRaii.PushStyle(ImGuiStyleVar.WindowPadding, Vector2.Zero);

        using var child = ImRaii.Child("Sidebar", -Vector2.One, true);
        if (!child) return;

        using (ImRaii.Child("Items", CalcGlobals.BodyScale(), false))
        {
            var span = CollectionsMarshal.AsSpan(Service.Config.ReplacementSets);
            for (var i = 0; i < span.Length; i++)
            {
                var set = span[i];
                // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
                if (set is null) continue;
                using var style = !Service.Config.ReplacementSets[i].Enabled ? ImRaii.PushColor(ImGuiCol.Text, ImGuiColors.DalamudGrey) : null;
                if (ImGui.Selectable($"{Service.Config.ReplacementSets[i].Name}## {i}", _activeSet == Service.Config.ReplacementSets[i]))
                {
                    _activeSet = Service.Config.ReplacementSets[i];
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

                    if (ImGui.Selectable("Export"))
                    {
                        _dialogManager.SaveFileDialog("Save", ".json", Service.Config.ReplacementSets[i].Name, ".json", (b, file) =>
                        {
                            if (!b) return;
                            //FIXME
                            //set.Save(file);
                        });
                    }

                    if (ImGui.Selectable("Delete"))
                    {
                        if (_activeSet == Service.Config.ReplacementSets[i]) _activeSet = null;
                        Service.Config.ReplacementSets.Remove(set);
                        Service.Config.Save();
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
                var randomset = new Random().Next(1, 100).ToString();
                //TOSETUP: add new config bracket set here
                Service.Config.ReplacementSets.Add(new Configuration.ReplacementSet("Set " + randomset, true, 0, [], [], [], [], [], [], [], [], [], []));
                Service.Config.Save();
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
    }
}
