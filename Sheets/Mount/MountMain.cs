using ActionTimelineReplacement.Base.Global;
using ActionTimelineReplacement.Base.Setups;
using ActionTimelineReplacement.Interface;
using Dalamud.Bindings.ImGui;
using Dalamud.Game.Config;
using Dalamud.Interface;
using Dalamud.Interface.Utility.Raii;
using FFXIVClientStructs.FFXIV.Common.Lua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using static FFXIVClientStructs.FFXIV.Client.UI.Misc.AozNoteModule;
#pragma warning disable CA1416 // Validate platform compatibility

namespace ActionTimelineReplacement.Sheets;

#region Main
public class MountMain
{
    const string type = "Mount";
    const string typename = "Mount";
    const string typenameplural = "Mounts";
    public static void Draw(string mainkey, ref Configuration.ReplacementSet _activeSet, ref string search) {

        Dictionary<uint, MountConfig> Writer = _activeSet.MountWriter;
        var GetName = MountManager.GetName;
        var CreateEntry = MountConfig.CreateEntry;

        using var subList = ImRaii.Child(mainkey, CalcGlobals.BodyScale(), true);
        if (subList) {
            const string searchPopup = "Search " + typenameplural;
            UiGlobals.DrawAddItem(searchPopup);

            foreach (var key in Writer.Keys) {
                if (ImGui.CollapsingHeader($"#{key:D5} - " + GetName(key))) {
                    var LocalWriter = Writer[key];
                    var old = MountManager.GetOriginal(key);
                    bool enablechanges = UiGlobals.CheckIsEnabled(Service.Config.EnableReplacement, _activeSet.Enabled, LocalWriter.Enabled);

                    using (ImRaii.PushFont(UiBuilder.IconFont)) {
                        if (UiGlobals.DrawDeleteEntryButton($"{FontAwesomeIcon.Trash.ToIconString()}##{key}")) {
                            Setup.SetupByType(key, type, true);
                            Writer.Remove(key);
                            Service.Config.Save();
                        }
                    }
                    if (ImGui.Checkbox("##" + key, ref LocalWriter.Enabled)) {
                        enablechanges = UiGlobals.CheckIsEnabled(Service.Config.EnableReplacement, _activeSet.Enabled, LocalWriter.Enabled);
                        if (enablechanges) Setup.SetupByType(key, type); 
                        else Setup.SetupByType(key, type, true);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();
                    ImGui.TextUnformatted("Entry Enabled");
                    UiGlobals.DrawItemSeparator();




                    #region Datainputs
                    UiGlobals.DrawUShort("Ride BGM ID", type, key, enablechanges, ref LocalWriter.Replacement.RideBGM, old.RideBGM, true, ["BGM_Path"]);
                    UiGlobals.DrawUShort("Ground Tilt ID", type, key, enablechanges, ref LocalWriter.Replacement.TiltParam1, old.TiltParam1, true, ["TiltParam_Index"]);
                    UiGlobals.DrawUShort("Fly/Swim Tilt ID", type, key, enablechanges, ref LocalWriter.Replacement.TiltParam2, old.TiltParam2, true, ["TiltParam_Index"]);
                    UiGlobals.DrawUShort("Unknown Tilt3 ID", type, key, enablechanges, ref LocalWriter.Replacement.TiltParam3, old.TiltParam3, true, ["TiltParam_Index"]);
                    UiGlobals.DrawUShort("Unknown Tilt4 ID", type, key, enablechanges, ref LocalWriter.Replacement.TiltParam4, old.TiltParam4, true, ["TiltParam_Index"]);
                    UiGlobals.DrawUShort("Fly Up/Down Tilt", type, key, enablechanges, ref LocalWriter.Replacement.FlyUpDownTilt, old.FlyUpDownTilt);
                    UiGlobals.DrawUShort("Unknown 6", type, key, enablechanges, ref LocalWriter.Replacement.Unknown6, old.Unknown6);
                    UiGlobals.DrawUShort("Unknown 7", type, key, enablechanges, ref LocalWriter.Replacement.Unknown7, old.Unknown7);
                    UiGlobals.DrawUShort("Unknown 8", type, key, enablechanges, ref LocalWriter.Replacement.Unknown8, old.Unknown8);
                    UiGlobals.DrawUShort("Mount Customize ID", type, key, enablechanges, ref LocalWriter.Replacement.MountCustomize, old.MountCustomize);
                    UiGlobals.DrawUShort("Swim Animation Speed", type, key, enablechanges, ref LocalWriter.Replacement.SwimAnimSpeed, old.SwimAnimSpeed);
                    //UiGlobals.DrawByte("MountBoolSet1", "Mount Bools 1 [raw]", ref replace.MountBoolSet1, i => i.MountBoolSet1);
                    UiGlobals.DrawItemSeparator();
                    continue;

                    #endregion

                }
            }


#endregion
            #region Search/Set

            using var searchMenu = ImRaii.Popup(searchPopup);
            if (searchMenu) {
                ImGui.SetNextItemWidth(CalcGlobals.XY());
                ImGui.InputText("##Search " + typenameplural.ToLower(), ref search, 256);
                var localsearch = search;

                using var popupChild = ImRaii.Child(searchPopup, CalcGlobals.SearchPopScale(), true);
                foreach (var pair in UiGlobals.CreateSearchList(type).OrderBy(i => {
                    if (string.IsNullOrEmpty(localsearch)) return 0;
                    return Math.Min(ProcessingGlobals.ScoreString(i.Value, localsearch),
                        ProcessingGlobals.ScoreString(i.Key.ToString(), localsearch));
                }))
                {
                    if (ImGui.Selectable($"#{pair.Key:D5} {pair.Value}"))
                    {
                        Writer[pair.Key] =  CreateEntry(pair.Key);
                    }
                }
            }
            #endregion
        }
    }
}
