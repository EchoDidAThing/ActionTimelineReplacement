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
public class OrnamentCustomizeMainOLD
{
    const string type = "OrnamentCustomize";
    public static void Draw(string mainkey, ref Configuration.ReplacementSet _activeSet, ref string search)
    {
        using var subList = ImRaii.Child(mainkey, CalcGlobals.BodyScale(), false);
        if (subList)
        {
            const string searchPopup = "Search ornament customizable data";
            UiGlobals.DrawAddItem(searchPopup);

            foreach (var key in _activeSet.OrnamentCustomizeWriter.Keys)
            {
                var replace = _activeSet.OrnamentCustomizeWriter[key].Replacement;
                var DefaultValues = OrnamentCustomizeManager.GetOriginal(key);

                if (ImGui.Checkbox("##" + key, ref _activeSet.OrnamentCustomizeWriter[key].Enabled))
                {
                    if (_activeSet.OrnamentCustomizeWriter[key].Enabled)
                    {
                        Setup.SetOrnamentCustomize(key);
                    }
                    else
                    {
                        Setup.SetOrnamentCustomize(key, true);
                    }
                    Service.Config.Save();
                }
                ImGui.SameLine();
                if (ImGui.Button(" - ##" + key))
                {
                    Setup.SetOrnamentCustomize(key, true);
                    _activeSet.OrnamentCustomizeWriter.Remove(key);
                    Service.Config.Save();
                }

                ImGui.SameLine();
                ImGui.Text($"#{key:D5}");

                ImGui.SameLine();
                ImGui.TextWrapped(OrnamentCustomizeManager.GetName(key));


                //REENABLE
                //UiGlobals.DrawUShort("Unknown 0", type, key, _activeSet.OrnamentCustomizeWriter[key].Enabled, ref replace.Unknown0, DefaultValues.Unknown0);
                UiGlobals.DrawShort("Unknown 1", type, key, _activeSet.OrnamentCustomizeWriter[key].Enabled, ref replace.Unknown1, DefaultValues.Unknown1);
                UiGlobals.DrawShort("Unknown 2", type, key, _activeSet.OrnamentCustomizeWriter[key].Enabled, ref replace.Unknown2, DefaultValues.Unknown2);
                UiGlobals.DrawShort("PUnknown 3", type, key, _activeSet.OrnamentCustomizeWriter[key].Enabled, ref replace.Unknown3, DefaultValues.Unknown3);
                UiGlobals.DrawShort("Unknown 4", type, key, _activeSet.OrnamentCustomizeWriter[key].Enabled, ref replace.Unknown4, DefaultValues.Unknown4);
                UiGlobals.DrawShort("Unknown 5", type, key, _activeSet.OrnamentCustomizeWriter[key].Enabled, ref replace.Unknown5, DefaultValues.Unknown5);
                UiGlobals.DrawShort("Unknown 6", type, key, _activeSet.OrnamentCustomizeWriter[key].Enabled, ref replace.Unknown6, DefaultValues.Unknown6);

                UiGlobals.DrawItemSeparator();
                continue;

#endregion
                #region Items


            }
                            
            #endregion
            #region Search/Set

            using var searchOrnamentCustomize = ImRaii.Popup(searchPopup);
            if (searchOrnamentCustomize)
            {
                ImGui.SetNextItemWidth(CalcGlobals.XY());
                ImGui.InputText("##Search ornament customize data", ref search, 256);
                var localsearch = search;

                using var popupChild = ImRaii.Child(searchPopup, CalcGlobals.SearchPopScale(), true);
                foreach (var pair in OrnamentCustomizeManager.Names.OrderBy(i =>
                {
                    if (string.IsNullOrEmpty(localsearch)) return 0;
                    return Math.Min(ProcessingGlobals.ScoreString(i.Value, localsearch),
                        ProcessingGlobals.ScoreString(i.Key.ToString(), localsearch));
                }))
                {
                    if (ImGui.Selectable($"#{pair.Key:D5} {pair.Value}"))
                    {
                        var original = OrnamentCustomizeManager.GetOriginal(pair.Key);
                        _activeSet.OrnamentCustomizeWriter[pair.Key] =
                            new OrnamentCustomizeConfig(new OrnamentCustomizeReplace(
                                    original.Unknown0,
                                    original.Unknown1,
                                    original.Unknown2,
                                    original.Unknown3,
                                    original.Unknown4,
                                    original.Unknown5,
                                    original.Unknown6),
                                false);
                        Service.Config.Save();
                    }
                }
            }
            #endregion
        }
    }
}
