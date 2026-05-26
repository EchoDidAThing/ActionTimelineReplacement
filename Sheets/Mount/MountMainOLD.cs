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
public class MountMainOLD

{
    const string type = "Mount";
    public static void Draw(string mainkey, ref Configuration.ReplacementSet _activeSet, ref string search)
    {
        using var subList = ImRaii.Child(mainkey, CalcGlobals.BodyScale(), false);
        if (subList)
        {
            const string searchPopup = "Search mounts";
            UiGlobals.DrawAddItem(searchPopup);

            foreach (var key in _activeSet.MountWriter.Keys)
            {
                var replace = _activeSet.MountWriter[key].Replacement;
                var DefaultValues = MountManager.GetOriginal(key);

                if (ImGui.Checkbox("##" + key, ref _activeSet.MountWriter[key].Enabled))
                {
                    if (_activeSet.MountWriter[key].Enabled)
                    {
                        Setup.SetMount(key);
                    }
                    else
                    {
                        Setup.SetMount(key, true);
                    }
                    Service.Config.Save();
                }
                ImGui.SameLine();

                if (ImGui.Button(" - ##" + key))
                {
                    Setup.SetMount(key, true);
                    _activeSet.MountWriter.Remove(key);
                    Service.Config.Save();
                }

                ImGui.SameLine();
                ImGui.Text($"#{key:D5}");

                ImGui.SameLine();
                ImGui.TextWrapped(MountManager.GetName(key));



                //REENABLE
                //UiGlobals.DrawUShort("Ride BGM ID", type, key, _activeSet.MountWriter[key].Enabled, ref replace.RideBGM, DefaultValues.RideBGM);

                //REENABLE
                //UiGlobals.DrawUShort("Ground Tilt ID", type, key, _activeSet.MountWriter[key].Enabled, ref replace.TiltParam1, DefaultValues.TiltParam1);

                //REENABLE
                //UiGlobals.DrawUShort("Fly/Swim Tilt ID", type, key, _activeSet.MountWriter[key].Enabled, ref replace.TiltParam2, DefaultValues.TiltParam2);

                //REENABLE
                //UiGlobals.DrawUShort("Unknown Tilt3 ID", type, key, _activeSet.MountWriter[key].Enabled, ref replace.TiltParam3, DefaultValues.TiltParam3);

                //REENABLE
                //UiGlobals.DrawUShort("Unknown Tilt4 ID", type, key, _activeSet.MountWriter[key].Enabled, ref replace.TiltParam4, DefaultValues.TiltParam4);

                //REENABLE
                //UiGlobals.DrawUShort("Fly Up/Down Tilt", type, key, _activeSet.MountWriter[key].Enabled, ref replace.FlyUpDownTilt, DefaultValues.FlyUpDownTilt);

                //REENABLE
                //UiGlobals.DrawUShort("Unknown 6", type, key, _activeSet.MountWriter[key].Enabled, ref replace.Unknown6, DefaultValues.Unknown6);

                //REENABLE
                //UiGlobals.DrawUShort("Unknown 7", type, key, _activeSet.MountWriter[key].Enabled, ref replace.Unknown7, DefaultValues.Unknown7);

                //REENABLE
                //UiGlobals.DrawUShort("Unknown 8", type, key, _activeSet.MountWriter[key].Enabled, ref replace.Unknown8, DefaultValues.Unknown8);

                //REENABLE
                //UiGlobals.DrawUShort("Mount Customize ID", type, key, _activeSet.MountWriter[key].Enabled, ref replace.MountCustomize, DefaultValues.MountCustomize);

                //REENABLE
                //UiGlobals.DrawUShort("Swim Animation Speed", type, key, _activeSet.MountWriter[key].Enabled, ref replace.SwimAnimSpeed, DefaultValues.SwimAnimSpeed);


                //DrawByte("MountBoolSet1", "Mount Bools 1 [raw]", ref replace.MountBoolSet1, i => i.MountBoolSet1);

                UiGlobals.DrawItemSeparator();
                continue;

                #endregion
                #region Items

             
            }

            #endregion
            #region Search/Set

            using var searchMount = ImRaii.Popup(searchPopup);
            if (searchMount)
            {
                ImGui.SetNextItemWidth(CalcGlobals.XY());
                ImGui.InputText("##Search mounts", ref search, 256);
                var localsearch = search;

                using var popupChild = ImRaii.Child(searchPopup, CalcGlobals.SearchPopScale(), true);
                foreach (var pair in MountManager.Names.OrderBy(i =>
                {
                    if (string.IsNullOrEmpty(localsearch)) return 0;
                    return Math.Min(ProcessingGlobals.ScoreString(i.Value, localsearch),
                        ProcessingGlobals.ScoreString(i.Key.ToString(), localsearch));
                }))
                {
                    if (ImGui.Selectable($"#{pair.Key:D5} {pair.Value}"))
                    {
                        var original = MountManager.GetOriginal(pair.Key);
                        _activeSet.MountWriter[pair.Key] =
                            new MountConfig(new MountReplace(
                                    original.RideBGM,
                                    original.TiltParam1,
                                    original.TiltParam2,
                                    original.TiltParam3,
                                    original.TiltParam4,
                                    original.FlyUpDownTilt,
                                    original.Unknown6,
                                    original.Unknown7,
                                    original.Unknown8,
                                    original.MountCustomize,
                                    original.SwimAnimSpeed,
                                    original.EnableHeadgear,
                                    original.Unk18,
                                    original.Unk19),
                                false);
                        Service.Config.Save();
                    }
                }
            }
            #endregion
        }
    }
}
