using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface;
using System.Numerics;
using ImGuiNET;
using System;
using System.Linq;
using Dalamud.Interface.Utility;
using ActionTimelineReplacement.Base.Setups;
using ActionTimelineReplacement.Windows;
using Lumina.Excel.Sheets;

namespace ActionTimelineReplacement.Sheets;

#region Main
public class ActionTimelineMain
{
    private static float Scale => ImGuiHelpers.GlobalScale;
    public static void Draw(string mainkey, ref Configuration.ReplacementSet _activeSet, ref string search)
    {
        var itemHeight = ImGui.CalcTextSize("").Y + ImGui.GetStyle().FramePadding.Y * 2 + ImGui.GetStyle().WindowPadding.Y;

        //to fix: scale height according to item count
        using var subList = ImRaii.Child(mainkey, new Vector2(-1, ImGui.GetWindowSize().Y - ImGui.GetCursorPosY() - itemHeight - ImGui.GetStyle().WindowPadding.Y), false);

        if (subList)
        {
            const string searchPopup = "Search action timelines";
            if (ImGui.Button(" + "))
            {
                ImGui.OpenPopup(searchPopup);
            }

            foreach (var key in _activeSet.ActionTimelineWriter.Keys)
            {
                var replace = _activeSet.ActionTimelineWriter[key].Replacement;

                if (ImGui.Checkbox("##" + key, ref _activeSet.ActionTimelineWriter[key].Enabled))
                {
                    Setup.SetActionTimeline(key);
                    Service.Config.Save();
                }
                ImGui.SameLine();

                if (ImGui.Button(" - ##" + key))
                {
                    _activeSet.ActionTimelineWriter.Remove(key);
                }

                ImGui.SameLine();
                ImGui.Text($"#{key:D5}");

                ImGui.SameLine();
                ImGui.TextWrapped(ActionTimelineManager.GetName(key));

                //to do: streamline this
                ImGui.TextUnformatted("Type");
                DrawByte("Type", ref replace.Type, i => i.Type);

                ImGui.TextUnformatted("Priority");
                DrawByte("Priority", ref replace.Priority, i => i.Priority);

                ImGui.TextUnformatted("Stance");
                DrawByte("Stance", ref replace.Stance, i => i.Stance);

                ImGui.TextUnformatted("Slot");
                DrawByte("Slot", ref replace.Slot, i => i.Slot);

                ImGui.TextUnformatted("Look-At Mode");
                DrawByte("LookAtMode", ref replace.LookAtMode, i => i.LookAtMode);                

                ImGui.TextUnformatted("Action Timeline ID Mode");
                DrawByte("ActionTimelineIDMode", ref replace.ActionTimelineIDMode, i => i.ActionTimelineIDMode);

                ImGui.TextUnformatted("Load Type");
                DrawByte("LoadType", ref replace.LoadType, i => i.LoadType);

                ImGui.TextUnformatted("Start Attach");
                DrawByte("StartAttach", ref replace.StartAttach, i => i.StartAttach);

                ImGui.TextUnformatted("Resident PAP");
                DrawByte("ResidentPap", ref replace.ResidentPap, i => i.ResidentPap);

                ImGui.TextUnformatted("PAP Type");
                DrawByte("Unknown6", ref replace.Unknown6, i => i.Unknown6);

                ImGui.TextUnformatted("Unknown 1");
                DrawByte("Unknown1", ref replace.Unknown1, i => i.Unknown1);

                ImGui.TextUnformatted("VPR Blade State [uses fake original]");
                DrawByte("VPRBladeState", ref replace.VPRBladeState, i => i.VPRBladeState);

                ImGui.NewLine();
                ImGui.Separator();
                ImGui.NewLine();
                continue;

                #endregion
                #region Items

                void DrawByte(string name, ref byte value,
                     Func<ActionTimelineReplace, byte> getDefault)
                {
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * Scale);
                    int relay = value;
                    if (ImGui.InputInt("##" + name + key, ref relay))
                    {
                        value = (byte)relay;
                        Setup.SetActionTimeline(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{name}{key}"))
                        {
                            value = getDefault(ActionTimelineManager.GetOriginal(key));
                            Setup.SetActionTimeline(key);
                            Service.Config.Save();
                        }
                    }
                }
            }

            #endregion
            #region Search/Set

            using var searchActionTimeline = ImRaii.Popup(searchPopup);
            if (searchActionTimeline)
            {
                var width = 420 * Scale;
                var height = 300 * Scale;

                ImGui.SetNextItemWidth(width);
                ImGui.InputText("##Search action timelines", ref search, 256);
                var localsearch = search;

                using var popupChild = ImRaii.Child(searchPopup, new Vector2(width, height), true);
                foreach (var pair in ActionTimelineManager.Names.OrderBy(i =>
                {
                    if (string.IsNullOrEmpty(localsearch)) return 0;
                    return Math.Min(ConfigWindow.ScoreString(i.Value, localsearch),
                        ConfigWindow.ScoreString(i.Key.ToString(), localsearch));
                }))
                {
                    if (ImGui.Selectable($"#{pair.Key:D5} {pair.Value}"))
                    {
                        var original = ActionTimelineManager.GetOriginal(pair.Key);
                        _activeSet.ActionTimelineWriter[pair.Key] =
                            new ActionTimelineConfig(new ActionTimelineReplace(
                                    original.Type,
                                    original.Priority,
                                    original.Stance,
                                    original.Slot,
                                    original.LookAtMode,
                                    original.ActionTimelineIDMode,
                                    original.LoadType,
                                    original.StartAttach,
                                    original.ResidentPap,
                                    original.Unknown6,
                                    original.Unknown1,
                                    original.VPRBladeState),
                                false);
                        Service.Config.Save();
                    }
                }
            }
            #endregion
        }
    }
}
