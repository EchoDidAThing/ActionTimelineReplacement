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
public class MountCustomizeMain
{
    const string type = "MountCustomize";
    public static void Draw(string mainkey, ref Configuration.ReplacementSet _activeSet, ref string search)
    {
        using var subList = ImRaii.Child(mainkey, CalcGlobals.BodyScale(), false);
        if (subList)
        {
            const string searchPopup = "Search mountcustomize entries";
            UiGlobals.DrawAddItem(searchPopup);

            foreach (var key in _activeSet.MountCustomizeWriter.Keys)
            {
                var replace = _activeSet.MountCustomizeWriter[key].Replacement;
                var DefaultValues = MountCustomizeManager.GetOriginal(key);

                if (ImGui.Checkbox("##" + key, ref _activeSet.MountCustomizeWriter[key].Enabled))
                {
                    if (_activeSet.MountCustomizeWriter[key].Enabled)
                    {
                        Setup.SetMountCustomize(key);
                    }
                    else
                    {
                        Setup.SetMountCustomize(key, true);
                    }
                    Service.Config.Save();
                }
                ImGui.SameLine();

                if (ImGui.Button(" - ##" + key))
                {
                    Setup.SetMountCustomize(key, true);
                    _activeSet.MountCustomizeWriter.Remove(key);
                    Service.Config.Save();
                }

                ImGui.SameLine();
                ImGui.Text($"#{key:D5}");

                ImGui.SameLine();
                ImGui.TextWrapped(MountCustomizeManager.GetName(key));


                UiGlobals.DrawUShort("Midlander Male Scale", type, key, _activeSet.MountCustomizeWriter[key].Enabled, ref replace.HyurMidlanderMaleScale, DefaultValues.HyurMidlanderMaleScale);
                UiGlobals.DrawUShort("Midlander Female Scale", type, key, _activeSet.MountCustomizeWriter[key].Enabled, ref replace.HyurMidlanderFemaleScale, DefaultValues.HyurMidlanderFemaleScale);
                UiGlobals.DrawUShort("Highlander Male Scale", type, key, _activeSet.MountCustomizeWriter[key].Enabled, ref replace.HyurHighlanderMaleScale, DefaultValues.HyurHighlanderMaleScale);
                UiGlobals.DrawUShort("Highlander Female Scale", type, key, _activeSet.MountCustomizeWriter[key].Enabled, ref replace.HyurHighlanderFemaleScale, DefaultValues.HyurHighlanderFemaleScale);
                UiGlobals.DrawUShort("Elezen Male Scale", type, key, _activeSet.MountCustomizeWriter[key].Enabled, ref replace.ElezenMaleScale, DefaultValues.ElezenMaleScale);
                UiGlobals.DrawUShort("Elezen Female Scale", type, key, _activeSet.MountCustomizeWriter[key].Enabled, ref replace.ElezenFemaleScale, DefaultValues.ElezenFemaleScale);
                UiGlobals.DrawUShort("Lalafell Male Scale", type, key, _activeSet.MountCustomizeWriter[key].Enabled, ref replace.LalafellMaleScale, DefaultValues.LalafellMaleScale);
                UiGlobals.DrawUShort("Lalafell Female Scale", type, key, _activeSet.MountCustomizeWriter[key].Enabled, ref replace.LalafellFemaleScale, DefaultValues.LalafellFemaleScale);
                UiGlobals.DrawUShort("Miqote Male Scale", type, key, _activeSet.MountCustomizeWriter[key].Enabled, ref replace.MiqoteMaleScale, DefaultValues.MiqoteMaleScale);
                UiGlobals.DrawUShort("Miqote Female Scale", type, key, _activeSet.MountCustomizeWriter[key].Enabled, ref replace.MiqoteFemaleScale, DefaultValues.MiqoteFemaleScale);
                UiGlobals.DrawUShort("Roegadyn Male Scale", type, key, _activeSet.MountCustomizeWriter[key].Enabled, ref replace.RoegadynMaleScale, DefaultValues.RoegadynMaleScale);
                UiGlobals.DrawUShort("Roegadyn Female Scale", type, key, _activeSet.MountCustomizeWriter[key].Enabled, ref replace.RoegadynFemaleScale, DefaultValues.RoegadynFemaleScale);
                UiGlobals.DrawUShort("AuRa Male Scale", type, key, _activeSet.MountCustomizeWriter[key].Enabled, ref replace.AuRaMaleScale, DefaultValues.AuRaMaleScale);
                UiGlobals.DrawUShort("AuRa Female Scale", type, key, _activeSet.MountCustomizeWriter[key].Enabled, ref replace.AuRaFemaleScale, DefaultValues.AuRaFemaleScale);
                UiGlobals.DrawUShort("Hrothgar Male Scale", type, key, _activeSet.MountCustomizeWriter[key].Enabled, ref replace.HrothgarMaleScale, DefaultValues.HrothgarMaleScale);
                UiGlobals.DrawUShort("Hrothgar Female Scale", type, key, _activeSet.MountCustomizeWriter[key].Enabled, ref replace.HrothgarFemaleScale, DefaultValues.HrothgarFemaleScale);
                UiGlobals.DrawUShort("Viera Male Scale", type, key, _activeSet.MountCustomizeWriter[key].Enabled, ref replace.VieraMaleScale, DefaultValues.VieraMaleScale);
                UiGlobals.DrawUShort("Viera Female Scale", type, key, _activeSet.MountCustomizeWriter[key].Enabled, ref replace.VieraFemaleScale, DefaultValues.VieraFemaleScale);
                UiGlobals.DrawUShort("Midlander Male CameraHeight", type, key, _activeSet.MountCustomizeWriter[key].Enabled, ref replace.HyurMidlanderMaleCameraHeight, DefaultValues.HyurMidlanderMaleCameraHeight);
                UiGlobals.DrawUShort("Midlander Female CameraHeight", type, key, _activeSet.MountCustomizeWriter[key].Enabled, ref replace.HyurMidlanderFemaleCameraHeight, DefaultValues.HyurMidlanderFemaleCameraHeight);
                UiGlobals.DrawUShort("Highlander Male CameraHeight", type, key, _activeSet.MountCustomizeWriter[key].Enabled, ref replace.HyurHighlanderMaleCameraHeight, DefaultValues.HyurHighlanderMaleCameraHeight);
                UiGlobals.DrawByte("Highlander Female CameraHeight", type, key, _activeSet.MountCustomizeWriter[key].Enabled, ref replace.HyurHighlanderFemaleCameraHeight, DefaultValues.HyurHighlanderFemaleCameraHeight);
                UiGlobals.DrawByte("Elezen Male CameraHeight", type, key, _activeSet.MountCustomizeWriter[key].Enabled, ref replace.ElezenMaleCameraHeight, DefaultValues.ElezenMaleCameraHeight);
                UiGlobals.DrawByte("Elezen Female CameraHeight", type, key, _activeSet.MountCustomizeWriter[key].Enabled, ref replace.ElezenFemaleCameraHeight, DefaultValues.ElezenFemaleCameraHeight);
                UiGlobals.DrawByte("Lalafell Male CameraHeight", type, key, _activeSet.MountCustomizeWriter[key].Enabled, ref replace.LalafellMaleCameraHeight, DefaultValues.LalafellMaleCameraHeight);
                UiGlobals.DrawByte("Lalafell Female CameraHeight", type, key, _activeSet.MountCustomizeWriter[key].Enabled, ref replace.LalafellFemaleCameraHeight, DefaultValues.LalafellFemaleCameraHeight);
                UiGlobals.DrawByte("Miqote Male CameraHeight", type, key, _activeSet.MountCustomizeWriter[key].Enabled, ref replace.MiqoteMaleCameraHeight, DefaultValues.MiqoteMaleCameraHeight);
                UiGlobals.DrawByte("Miqote Female CameraHeight", type, key, _activeSet.MountCustomizeWriter[key].Enabled, ref replace.MiqoteFemaleCameraHeight, DefaultValues.MiqoteFemaleCameraHeight);
                UiGlobals.DrawByte("Roegadyn Male CameraHeight", type, key, _activeSet.MountCustomizeWriter[key].Enabled, ref replace.RoegadynMaleCameraHeight, DefaultValues.RoegadynMaleCameraHeight);
                UiGlobals.DrawByte("Roegadyn Female CameraHeight", type, key, _activeSet.MountCustomizeWriter[key].Enabled, ref replace.RoegadynFemaleCameraHeight, DefaultValues.RoegadynFemaleCameraHeight);
                UiGlobals.DrawByte("AuRa Male CameraHeight", type, key, _activeSet.MountCustomizeWriter[key].Enabled, ref replace.AuRaMaleCameraHeight, DefaultValues.AuRaMaleCameraHeight);
                UiGlobals.DrawByte("AuRa Female CameraHeight", type, key, _activeSet.MountCustomizeWriter[key].Enabled, ref replace.AuRaFemaleCameraHeight, DefaultValues.AuRaFemaleCameraHeight);
                UiGlobals.DrawByte("Hrothgar Male CameraHeight", type, key, _activeSet.MountCustomizeWriter[key].Enabled, ref replace.HrothgarMaleCameraHeight, DefaultValues.HrothgarMaleCameraHeight);
                UiGlobals.DrawByte("Hrothgar Female CameraHeight", type, key, _activeSet.MountCustomizeWriter[key].Enabled, ref replace.HrothgarFemaleCameraHeight, DefaultValues.HrothgarFemaleCameraHeight);
                UiGlobals.DrawByte("Viera Male CameraHeight", type, key, _activeSet.MountCustomizeWriter[key].Enabled, ref replace.VieraMaleCameraHeight, DefaultValues.VieraMaleCameraHeight);
                UiGlobals.DrawByte("Viera Female CameraHeight", type, key, _activeSet.MountCustomizeWriter[key].Enabled, ref replace.VieraFemaleCameraHeight, DefaultValues.VieraFemaleCameraHeight);

                //DrawByte("MountBoolSet1", "Mount Bools 1 [raw]", ref replace.MountBoolSet1, i => i.MountBoolSet1);

                UiGlobals.DrawItemSeparator();
                continue;

                #endregion
                #region Items

               
            }

            #endregion
            #region Search/Set

            using var searchMountCustomize = ImRaii.Popup(searchPopup);
            if (searchMountCustomize)
            {
                ImGui.SetNextItemWidth(CalcGlobals.XY());
                ImGui.InputText("##Search mountcustomize", ref search, 256);
                var localsearch = search;

                using var popupChild = ImRaii.Child(searchPopup, CalcGlobals.SearchPopScale(), true);
                foreach (var pair in MountCustomizeManager.Names.OrderBy(i =>
                {
                    if (string.IsNullOrEmpty(localsearch)) return 0;
                    return Math.Min(ConfigWindow.ScoreString(i.Value, localsearch),
                        ConfigWindow.ScoreString(i.Key.ToString(), localsearch));
                }))
                {
                    if (ImGui.Selectable($"#{pair.Key:D5} {pair.Value}"))
                    {
                        var original = MountCustomizeManager.GetOriginal(pair.Key);
                        _activeSet.MountCustomizeWriter[pair.Key] =
                            new MountCustomizeConfig(new MountCustomizeReplace(
                                    original.HyurMidlanderMaleScale,
                                    original.HyurMidlanderFemaleScale,
                                    original.HyurHighlanderMaleScale,
                                    original.HyurHighlanderFemaleScale,
                                    original.ElezenMaleScale,
                                    original.ElezenFemaleScale,
                                    original.LalafellMaleScale,
                                    original.LalafellFemaleScale,
                                    original.MiqoteMaleScale,
                                    original.MiqoteFemaleScale,
                                    original.RoegadynMaleScale,
                                    original.RoegadynFemaleScale,
                                    original.AuRaMaleScale,
                                    original.AuRaFemaleScale,
                                    original.HrothgarMaleScale,
                                    original.HrothgarFemaleScale,
                                    original.VieraMaleScale,
                                    original.VieraFemaleScale,
                                    original.HyurMidlanderMaleCameraHeight,
                                    original.HyurMidlanderFemaleCameraHeight,
                                    original.HyurHighlanderMaleCameraHeight,
                                    original.HyurHighlanderFemaleCameraHeight,
                                    original.ElezenMaleCameraHeight,
                                    original.ElezenFemaleCameraHeight,
                                    original.LalafellMaleCameraHeight,
                                    original.LalafellFemaleCameraHeight,
                                    original.MiqoteMaleCameraHeight,
                                    original.MiqoteFemaleCameraHeight,
                                    original.RoegadynMaleCameraHeight,
                                    original.RoegadynFemaleCameraHeight,
                                    original.AuRaMaleCameraHeight,
                                    original.AuRaFemaleCameraHeight,
                                    original.HrothgarMaleCameraHeight,
                                    original.HrothgarFemaleCameraHeight,
                                    original.VieraMaleCameraHeight,
                                    original.VieraFemaleCameraHeight),
                                    false);
                        Service.Config.Save();
                    }
                }
            }
            #endregion
        }
    }
}
