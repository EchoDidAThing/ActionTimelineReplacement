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
public class ActionMain
{
    const string type = "Action";
    public static void Draw(string mainkey, ref Configuration.ReplacementSet _activeSet, ref string search)
    {
        using var subList = ImRaii.Child(mainkey, CalcGlobals.BodyScale(), false);
        if (subList)
        {
            const string searchPopup = "Search actions";
            UiGlobals.DrawAddItem(searchPopup);

            foreach (var key in _activeSet.ActionWriter.Keys)
            { 
                var replace = _activeSet.ActionWriter[key].Replacement;
                var DefaultValues = ActionManager.GetOriginal(key);

                if (ImGui.Checkbox("##" + key, ref _activeSet.ActionWriter[key].Enabled))
                {
                    if (_activeSet.ActionWriter[key].Enabled)
                    {
                        Setup.SetAction(key);
                    }
                    else
                    {
                        Setup.SetAction(key, true);
                    }
                    Service.Config.Save();
                }
                ImGui.SameLine();

                if (ImGui.Button(" - ##" + key))
                {
                    Setup.SetAction(key, true);
                    _activeSet.ActionWriter.Remove(key);
                    Service.Config.Save();
                }

                //to do: show values as strings from their subsheets
                ImGui.SameLine();
                ImGui.Text($"#{key:D5}");

                ImGui.SameLine();
                ImGui.TextWrapped(ActionManager.GetName(key));

                


                UiGlobals.DrawUShort("Cast", type, key, _activeSet.ActionWriter[key].Enabled, ref replace.CastVfx, DefaultValues.CastVfx, true, "ActionCastVFX-VFX");
                UiGlobals.DrawUShort("Start", type, key, _activeSet.ActionWriter[key].Enabled, ref replace.AnimationStart, DefaultValues.AnimationStart, true, "ActionTimeline");
                UiGlobals.DrawShort("End", type, key, _activeSet.ActionWriter[key].Enabled, ref replace.AnimationEnd, DefaultValues.AnimationEnd);
                UiGlobals.DrawUShort("Hit", type, key, _activeSet.ActionWriter[key].Enabled, ref replace.ActionTimelineHit, DefaultValues.ActionTimelineHit);
                UiGlobals.DrawByte("Unknown 1", type, key, _activeSet.ActionWriter[key].Enabled, ref replace.Unknown1, DefaultValues.Unknown1);
                UiGlobals.DrawByte("Unknown 2", type, key, _activeSet.ActionWriter[key].Enabled, ref replace.Unknown2, DefaultValues.Unknown2);
                UiGlobals.DrawByte("Unknown 4", type, key, _activeSet.ActionWriter[key].Enabled, ref replace.Unknown4, DefaultValues.Unknown4);
                UiGlobals.DrawByte("Unknown_70", type, key, _activeSet.ActionWriter[key].Enabled, ref replace.Unknown_70, DefaultValues.Unknown_70);

                UiGlobals.DrawItemSeparator();
                continue;

                #endregion
                #region Items

                
            }
            
            #endregion
            #region Search/Set

            using var searchAction = ImRaii.Popup(searchPopup);
            if (searchAction)
            {
                ImGui.SetNextItemWidth(CalcGlobals.XY());
                ImGui.InputText("##Search actions", ref search, 256);
                var localsearch = search;

                using var popupChild = ImRaii.Child(searchPopup, CalcGlobals.SearchPopScale(), true);
                foreach (var pair in ActionManager.Names.OrderBy(i =>
                {
                    if (string.IsNullOrEmpty(localsearch)) return 0;
                    return Math.Min(ProcessingGlobals.ScoreString(i.Value, localsearch),
                        ProcessingGlobals.ScoreString(i.Key.ToString(), localsearch));
                }))
                {
                    if (ImGui.Selectable($"#{pair.Key:D5} {pair.Value}"))
                    {
                        var original = ActionManager.GetOriginal(pair.Key);
                        _activeSet.ActionWriter[pair.Key] =
                            new ActionConfig(new ActionReplace(
                                    original.AnimationStart,
                                    original.AnimationEnd,
                                    original.ActionTimelineHit,
                                    original.CastVfx,
                                    original.Unknown1,
                                    original.Unknown2,
                                    original.Unknown4,
                                    original.Unknown_70),
                                false);
                        Service.Config.Save();
                    }
                }
            }
            #endregion
        }
    }
}
