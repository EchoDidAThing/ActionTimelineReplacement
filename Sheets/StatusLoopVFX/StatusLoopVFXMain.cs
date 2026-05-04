using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface;
using Dalamud.Bindings.ImGui;
using System;
using System.Linq;
using ActionTimelineReplacement.Base.Setups;
using ActionTimelineReplacement.Interface;
using ActionTimelineReplacement.Base.Global;
#pragma warning disable CA1416 // Validate platform compatibility

namespace ActionTimelineReplacement.Sheets;

#region Main
public class StatusLoopVFXMain
{
    const string type = "StatusLoopVFX";
    public static void Draw(string mainkey, ref Configuration.ReplacementSet _activeSet, ref string search)
    {
        using var subList = ImRaii.Child(mainkey, CalcGlobals.BodyScale(), false);
        if (subList)
        {
            const string searchPopup = "Search statusloopvfxs";
            UiGlobals.DrawAddItem(searchPopup);

            foreach (var key in _activeSet.StatusLoopVFXWriter.Keys)
            {
                var replace = _activeSet.StatusLoopVFXWriter[key].Replacement;
                var DefaultValues = StatusLoopVFXManager.GetOriginal(key);

                if (ImGui.Checkbox("##" + key, ref _activeSet.StatusLoopVFXWriter[key].Enabled))
                {
                    if (_activeSet.StatusLoopVFXWriter[key].Enabled)
                    {
                        Setup.SetStatusLoopVFX(key);
                    }
                    else 
                    {
                        Setup.SetStatusLoopVFX(key, true);
                    }
                    Service.Config.Save();
                }
                ImGui.SameLine();

                if (ImGui.Button(" - ##" + key))
                {
                    Setup.SetStatusLoopVFX(key, true);
                    _activeSet.StatusLoopVFXWriter.Remove(key);
                    Service.Config.Save();
                }

                ImGui.SameLine();
                ImGui.Text($"#{key:D5}");

                ImGui.SameLine();
                ImGui.TextWrapped(StatusLoopVFXManager.GetName(key));

                //to do: show loop vfx and hit effect as strings

                UiGlobals.DrawUShort("Friendly VFX ID", type, key, _activeSet.StatusLoopVFXWriter[key].Enabled, ref replace.FriendlyVFX, DefaultValues.FriendlyVFX);
                UiGlobals.DrawUShort("Stack VFX 1 ID", type, key, _activeSet.StatusLoopVFXWriter[key].Enabled, ref replace.StackVFX1, DefaultValues.StackVFX1);
                UiGlobals.DrawUShort("Stack VFX 2 ID", type, key, _activeSet.StatusLoopVFXWriter[key].Enabled, ref replace.StackVFX2, DefaultValues.StackVFX2);
                UiGlobals.DrawUShort("Enemy VFX ID", type, key, _activeSet.StatusLoopVFXWriter[key].Enabled, ref replace.EnemyVFX, DefaultValues.EnemyVFX);
                UiGlobals.DrawByte("Stack 1 Trigger", type, key, _activeSet.StatusLoopVFXWriter[key].Enabled, ref replace.StackTrigger1, DefaultValues.StackTrigger1);
                UiGlobals.DrawByte("Stack 2 Trigger", type, key, _activeSet.StatusLoopVFXWriter[key].Enabled, ref replace.StackTrigger2, DefaultValues.StackTrigger2);
                UiGlobals.DrawByte("Unknown 1", type, key, _activeSet.StatusLoopVFXWriter[key].Enabled, ref replace.Unknown1, DefaultValues.Unknown1);
                UiGlobals.DrawByte("Unknown 2", type, key, _activeSet.StatusLoopVFXWriter[key].Enabled, ref replace.Unknown2, DefaultValues.Unknown2);
                UiGlobals.DrawBool("Unknown 3", type, key, _activeSet.StatusLoopVFXWriter[key].Enabled, ref replace.Unknown3, DefaultValues.Unknown3);
                UiGlobals.DrawBool("Unknown 4", type, key, _activeSet.StatusLoopVFXWriter[key].Enabled, ref replace.Unknown4, DefaultValues.Unknown4);
                UiGlobals.DrawBool("Unknown 5", type, key, _activeSet.StatusLoopVFXWriter[key].Enabled, ref replace.Unknown5, DefaultValues.Unknown5);

                UiGlobals.DrawItemSeparator();
                continue;

                #endregion
                #region Items

                
                
            }

            #endregion
            #region Search/Set

            using var searchStatusLoopVFX = ImRaii.Popup(searchPopup);
            if (searchStatusLoopVFX)
            {
                ImGui.SetNextItemWidth(CalcGlobals.XY());
                ImGui.InputText("##Search statusLoopVFXs", ref search, 256);
                var localsearch = search;

                using var popupChild = ImRaii.Child(searchPopup, CalcGlobals.SearchPopScale(), true);
                foreach (var pair in StatusLoopVFXManager.Names.OrderBy(i =>
                {
                    if (string.IsNullOrEmpty(localsearch)) return 0;
                    return Math.Min(ProcessingGlobals.ScoreString(i.Value, localsearch),
                        ProcessingGlobals.ScoreString(i.Key.ToString(), localsearch));
                }))
                {
                    if (ImGui.Selectable($"#{pair.Key:D5} {pair.Value}"))
                    {
                        var original = StatusLoopVFXManager.GetOriginal(pair.Key);
                        _activeSet.StatusLoopVFXWriter[pair.Key] =
                            new StatusLoopVFXConfig(new StatusLoopVFXReplace(
                                    original.FriendlyVFX,
                                    original.StackVFX1,
                                    original.StackVFX2,
                                    original.EnemyVFX,
                                    original.StackTrigger1,
                                    original.StackTrigger2,
                                    original.Unknown1,
                                    original.Unknown2,
                                    original.Unknown3,
                                    original.Unknown4,
                                    original.Unknown5),
                                false);
                        Service.Config.Save();
                    }
                }
            }
            #endregion
        }
    }
}
