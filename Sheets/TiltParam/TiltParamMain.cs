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
public class TiltParamMain
{
    const string type = "TiltParam";
    public static void Draw(string mainkey, ref Configuration.ReplacementSet _activeSet, ref string search)
    {
        using var subList = ImRaii.Child(mainkey, CalcGlobals.BodyScale(), false);
        if (subList)
        {
            const string searchPopup = "Search tilts";
            UiGlobals.DrawAddItem(searchPopup);

            foreach (var key in _activeSet.TiltParamWriter.Keys)
            {
                var replace = _activeSet.TiltParamWriter[key].Replacement;
                var DefaultValues = TiltParamManager.GetOriginal(key);

                if (ImGui.Checkbox("##" + key, ref _activeSet.TiltParamWriter[key].Enabled))
                {
                    Setup.SetTiltParam(key);
                    Service.Config.Save();
                }
                ImGui.SameLine();

                if (ImGui.Button(" - ##" + key))
                {
                    _activeSet.TiltParamWriter.Remove(key);
                }

                ImGui.SameLine();
                ImGui.Text($"#{key:D5}");

                ImGui.SameLine();
                ImGui.TextWrapped(TiltParamManager.GetName(key));
                //REENABLE
                //UiGlobals.DrawUShort("Tilt Rate",type, key, _activeSet.TiltParamWriter[key].Enabled, ref replace.Unknown0, DefaultValues.Unknown0);
                UiGlobals.DrawByte("Rotation Origin Offset", type, key, _activeSet.TiltParamWriter[key].Enabled, ref replace.Unknown1, DefaultValues.Unknown1);
                UiGlobals.DrawByte("Max Angle", type, key, _activeSet.TiltParamWriter[key].Enabled, ref replace.Unknown2, DefaultValues.Unknown2);
                UiGlobals.DrawByte("Unknown 3", type, key, _activeSet.TiltParamWriter[key].Enabled, ref replace.Unknown3, DefaultValues.Unknown3);
                UiGlobals.DrawByte("Unknown 4", type, key, _activeSet.TiltParamWriter[key].Enabled, ref replace.Unknown4, DefaultValues.Unknown4);
                UiGlobals.DrawBool("Reverse Mouse Direction", type, key, _activeSet.TiltParamWriter[key].Enabled, ref replace.Unknown5, DefaultValues.Unknown5);

                UiGlobals.DrawItemSeparator();
                continue;

                #endregion
                #region Items

                
            }

            #endregion
            #region Search/Set

            using var searchTiltParam = ImRaii.Popup(searchPopup);
            if (searchTiltParam)
            {
                ImGui.SetNextItemWidth(CalcGlobals.XY());
                ImGui.InputText("##Search tilt parameters", ref search, 256);
                var localsearch = search;

                using var popupChild = ImRaii.Child(searchPopup, CalcGlobals.SearchPopScale(), true);
                foreach (var pair in TiltParamManager.Names.OrderBy(i =>
                {
                    if (string.IsNullOrEmpty(localsearch)) return 0;
                    return Math.Min(ProcessingGlobals.ScoreString(i.Value, localsearch),
                        ProcessingGlobals.ScoreString(i.Key.ToString(), localsearch));
                }))
                {
                    if (ImGui.Selectable($"#{pair.Key:D5} {pair.Value}"))
                    {
                        var original = TiltParamManager.GetOriginal(pair.Key);
                        _activeSet.TiltParamWriter[pair.Key] =
                            new TiltParamConfig(new TiltParamReplace(
                                    original.Unknown0,
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
