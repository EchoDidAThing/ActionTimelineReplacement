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
    const string type = "ActionTimeline";
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
                var DefaultValues = ActionTimelineManager.GetOriginal(key);

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


                UiGlobals.DrawByte("Type", type, key, ref replace.Type, DefaultValues.Type);
                UiGlobals.DrawByte("Priority", type, key, ref replace.Priority, DefaultValues.Priority);
                UiGlobals.DrawByte("Stance", type, key, ref replace.Stance, DefaultValues.Stance);
                UiGlobals.DrawByte("Slot", type, key, ref replace.Slot, DefaultValues.Slot);
                UiGlobals.DrawByte("LookAt Mode", type, key, ref replace.LookAtMode, DefaultValues.LookAtMode);
                UiGlobals.DrawByte("Action Tomeline ID Mode", type, key, ref replace.ActionTimelineIDMode, DefaultValues.ActionTimelineIDMode);
                UiGlobals.DrawByte("Load Type", type, key, ref replace.LoadType, DefaultValues.LoadType);
                UiGlobals.DrawByte("Start Attach", type, key, ref replace.StartAttach, DefaultValues.StartAttach);
                UiGlobals.DrawByte("Resident PAP", type, key, ref replace.ResidentPap, DefaultValues.ResidentPap);
                UiGlobals.DrawByte("PAP Type", type, key, ref replace.Unknown6, DefaultValues.Unknown6);
                UiGlobals.DrawByte("Unknown 1", type, key, ref replace.Unknown1, DefaultValues.Unknown1);
                UiGlobals.DrawByte("Viper Blade State[Uses fake original]", type, key, ref replace.VPRBladeState, DefaultValues.VPRBladeState);

                UiGlobals.DrawItemSeparator();
                continue;

                #endregion
                #region Items

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
