using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface;
using Dalamud.Bindings.ImGui;
using System;
using System.Linq;
using ActionTimelineReplacement.Base.Setups;
using ActionTimelineReplacement.Windows;
using ActionTimelineReplacement.Base.Global;
#pragma warning disable CA1416 // Validate platform compatibility

namespace ActionTimelineReplacement.Sheets;

#region Main
public class ActionTimelineMain
{
    public static void Draw(string mainkey, ref Configuration.ReplacementSet _activeSet, ref string search)
    {
        using var subList = ImRaii.Child(mainkey, CalcGlobals.BodyScale(), false);
        if (subList)
        {
            const string searchPopup = "Search action timelines";
            UiGlobals.DrawAddItem(searchPopup);

            foreach (var key in _activeSet.ActionTimelineWriter.Keys)
            {
                var replace = _activeSet.ActionTimelineWriter[key].Replacement;

                if (ImGui.Checkbox("##" + key, ref _activeSet.ActionTimelineWriter[key].Enabled))
                {
                    if (_activeSet.ActionTimelineWriter[key].Enabled)
                    {
                        Setup.SetActionTimeline(key);
                    }
                    else
                    {
                        Setup.SetActionTimeline(key, true);
                    }
                    Service.Config.Save();
                }
                ImGui.SameLine();

                if (ImGui.Button(" - ##" + key))
                {
                    Setup.SetActionTimeline(key, true);
                    _activeSet.ActionTimelineWriter.Remove(key);
                    Service.Config.Save();
                }

                ImGui.SameLine();
                ImGui.Text($"#{key:D5}");

                ImGui.SameLine();
                ImGui.TextWrapped(ActionTimelineManager.GetName(key));

                DrawByte("Type", "Type", ref replace.Type, i => i.Type);

                DrawByte("Priority", "Priority", ref replace.Priority, i => i.Priority);

                DrawByte("Stance", "Stance", ref replace.Stance, i => i.Stance);

                DrawByte("Slot", "Slot", ref replace.Slot, i => i.Slot);

                DrawByte("LookAtMode", "Look-At Mode", ref replace.LookAtMode, i => i.LookAtMode);                

                DrawByte("ActionTimelineIDMode", "Action Timeline ID Mode", ref replace.ActionTimelineIDMode, i => i.ActionTimelineIDMode);

                DrawByte("LoadType", "Load Type", ref replace.LoadType, i => i.LoadType);

                DrawByte("StartAttach", "Start Attach", ref replace.StartAttach, i => i.StartAttach);

                DrawByte("ResidentPap", "Resident PAP", ref replace.ResidentPap, i => i.ResidentPap);

                DrawByte("Unknown6", "PAP Type", ref replace.Unknown6, i => i.Unknown6);

                DrawByte("Unknown1", "Unknown 1", ref replace.Unknown1, i => i.Unknown1);

                DrawByte("VPRBladeState", "VPR Blade State [uses fake original]", ref replace.VPRBladeState, i => i.VPRBladeState);

                UiGlobals.DrawItemSeparator();
                continue;

                #endregion
                #region Items

                void DrawByte(string refname, string text, ref byte value,
                    Func<ActionTimelineReplace, byte> getDefault)
                {
                    ImGui.TextUnformatted(text);
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * CalcGlobals.GlobalScale());
                    int relay = value;
                    if (ImGui.InputInt("##" + refname + key, ref relay))
                    {
                        value = (byte)relay;
                        Setup.SetActionTimeline(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{refname}{key}"))
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

                ImGui.SetNextItemWidth(CalcGlobals.XY());
                ImGui.InputText("##Search action timelines", ref search, 256);
                var localsearch = search;

                using var popupChild = ImRaii.Child(searchPopup, CalcGlobals.SearchPopScale(), true);
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
