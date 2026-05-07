using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface;
using Dalamud.Bindings.ImGui;
using System;
using System.Linq;
using ActionTimelineReplacement.Base.Setups;
using ActionTimelineReplacement.Interface;
using ActionTimelineReplacement.Base.Global;

namespace ActionTimelineReplacement.Sheets;

#region Main
public class GlassesMain
{
    const string type = "Glasses";
    public static void Draw(string mainkey, ref Configuration.ReplacementSet _activeSet, ref string search)
    {
        using var subList = ImRaii.Child(mainkey, CalcGlobals.BodyScale(), false);
        if (subList)
        {
            const string searchPopup = "Search glasses";
            UiGlobals.DrawAddItem(searchPopup);

            foreach (var key in _activeSet.GlassesWriter.Keys)
            {
                var replace = _activeSet.GlassesWriter[key].Replacement;
                var DefaultValues = GlassesManager.GetOriginal(key);

                if (ImGui.Checkbox("##" + key, ref _activeSet.GlassesWriter[key].Enabled))
                {
                    if (_activeSet.GlassesWriter[key].Enabled)
                    {
                        Setup.SetGlasses(key);
                    }
                    else
                    {
                        Setup.SetGlasses(key, true);
                    }
                    Service.Config.Save();
                }
                ImGui.SameLine();
                if (ImGui.Button(" - ##" + key))
                {
                    Setup.SetGlasses(key, true);
                    _activeSet.GlassesWriter.Remove(key);
                    Service.Config.Save();
                }

                ImGui.SameLine();
                ImGui.Text($"#{key:D5}");

                ImGui.SameLine();
                ImGui.TextWrapped(GlassesManager.GetName(key));


                UiGlobals.DrawSByte("Unknown70_1", type, key, _activeSet.GlassesWriter[key].Enabled, ref replace.Unknown70_1, DefaultValues.Unknown70_1);
                UiGlobals.DrawSByte("Unknown70_2", type, key, _activeSet.GlassesWriter[key].Enabled, ref replace.Unknown70_2, DefaultValues.Unknown70_2);
                UiGlobals.DrawSByte("Unknown70_3", type, key, _activeSet.GlassesWriter[key].Enabled, ref replace.Unknown70_3, DefaultValues.Unknown70_3);
                UiGlobals.DrawSByte("Unknown70_4", type, key, _activeSet.GlassesWriter[key].Enabled, ref replace.Unknown70_4, DefaultValues.Unknown70_4);
                UiGlobals.DrawSByte("Unknown70_5", type, key, _activeSet.GlassesWriter[key].Enabled, ref replace.Unknown70_5, DefaultValues.Unknown70_5);
                UiGlobals.DrawSByte("Unknown70_6", type, key, _activeSet.GlassesWriter[key].Enabled, ref replace.Unknown70_6, DefaultValues.Unknown70_6);
                UiGlobals.DrawUInt("Unknown70_8", type, key, _activeSet.GlassesWriter[key].Enabled, ref replace.Unknown70_7, DefaultValues.Unknown70_7);

                //REENABLE
                //UiGlobals.DrawUShort("Unknown70_8", type, key, _activeSet.GlassesWriter[key].Enabled, ref replace.Unknown70_8, DefaultValues.Unknown70_8);

                UiGlobals.DrawItemSeparator();
                continue;

                #endregion
                #region Items
                
             
            }
                            
            #endregion
            #region Search/Set

            using var searchGlasses = ImRaii.Popup(searchPopup);
            if (searchGlasses)
            {
                ImGui.SetNextItemWidth(CalcGlobals.XY());
                ImGui.InputText("##Search glasses", ref search, 256);
                var localsearch = search;

                using var popupChild = ImRaii.Child(searchPopup, CalcGlobals.SearchPopScale(), true);
                foreach (var pair in GlassesManager.Names.OrderBy(i =>
                {
                    if (string.IsNullOrEmpty(localsearch)) return 0;
                    return Math.Min(ProcessingGlobals.ScoreString(i.Value, localsearch),
                        ProcessingGlobals.ScoreString(i.Key.ToString(), localsearch));
                }))
                {
                    if (ImGui.Selectable($"#{pair.Key:D5} {pair.Value}"))
                    {
                        var original = GlassesManager.GetOriginal(pair.Key);
                        _activeSet.GlassesWriter[pair.Key] = // can this be moved out into a cased thing? 
                            new GlassesConfig(new GlassesReplace(
                                    original.Unknown70_1,
                                    original.Unknown70_2,
                                    original.Unknown70_3,
                                    original.Unknown70_4,
                                    original.Unknown70_5,
                                    original.Unknown70_6,
                                    original.Unknown70_7,
                                    original.Unknown70_8),
                                false);
                        Service.Config.Save();
                    }
                }
            }
            #endregion
        }
    }
}
