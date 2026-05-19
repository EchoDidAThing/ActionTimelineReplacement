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
using static FFXIVClientStructs.FFXIV.Client.System.String.Utf8String.Delegates;
using static FFXIVClientStructs.FFXIV.Client.UI.Misc.AozNoteModule;
#pragma warning disable CA1416 // Validate platform compatibility

namespace ActionTimelineReplacement.Sheets;

#region Main
public class MountCustomizeMain
{
    const string type = "MountCustomize";
    const string typename = "Mount Customize";
    const string typenameplural = "Mount Customizations";
    private static string lastsearch = "";
    private static string localsearch = "";
    private static Dictionary<uint, string> SearchList = UiGlobals.CreateSearchList(type);
    public static void Draw(string mainkey, ref Configuration.ReplacementSet _activeSet, ref string search) {

        Dictionary<uint, MountCustomizeConfig> Writer = _activeSet.MountCustomizeWriter;
        var GetName = MountCustomizeManager.GetName;
        var CreateEntry = MountCustomizeConfig.CreateEntry;

        using var subList = ImRaii.Child(mainkey, CalcGlobals.BodyScale(), true);
        if (subList) {
            const string searchPopup = "Search " + typenameplural;
            UiGlobals.DrawAddItem(searchPopup);

            foreach (var key in Writer.Keys) {
                if (ImGui.CollapsingHeader($"#{key:D5} - " + GetName(key))) {
                    var LocalWriter = Writer[key];
                    var old = MountCustomizeManager.GetOriginal(key);
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
                    UiGlobals.DrawUShort("Midlander Male Scale", type, key, enablechanges, ref LocalWriter.Replacement.HyurMidlanderMaleScale, old.HyurMidlanderMaleScale);
                    UiGlobals.DrawUShort("Midlander Female Scale", type, key, enablechanges, ref LocalWriter.Replacement.HyurMidlanderFemaleScale, old.HyurMidlanderFemaleScale);
                    UiGlobals.DrawUShort("Highlander Male Scale", type, key, enablechanges, ref LocalWriter.Replacement.HyurHighlanderMaleScale, old.HyurHighlanderMaleScale);
                    UiGlobals.DrawUShort("Highlander Female Scale", type, key, enablechanges, ref LocalWriter.Replacement.HyurHighlanderFemaleScale, old.HyurHighlanderFemaleScale);
                    UiGlobals.DrawUShort("Elezen Male Scale", type, key, enablechanges, ref LocalWriter.Replacement.ElezenMaleScale, old.ElezenMaleScale);
                    UiGlobals.DrawUShort("Elezen Female Scale", type, key, enablechanges, ref LocalWriter.Replacement.ElezenFemaleScale, old.ElezenFemaleScale);
                    UiGlobals.DrawUShort("Lalafell Male Scale", type, key, enablechanges, ref LocalWriter.Replacement.LalafellMaleScale, old.LalafellMaleScale);
                    UiGlobals.DrawUShort("Lalafell Female Scale", type, key, enablechanges, ref LocalWriter.Replacement.LalafellFemaleScale, old.LalafellFemaleScale);
                    UiGlobals.DrawUShort("Miqote Male Scale", type, key, enablechanges, ref LocalWriter.Replacement.MiqoteMaleScale, old.MiqoteMaleScale);
                    UiGlobals.DrawUShort("Miqote Female Scale", type, key, enablechanges, ref LocalWriter.Replacement.MiqoteFemaleScale, old.MiqoteFemaleScale);
                    UiGlobals.DrawUShort("Roegadyn Male Scale", type, key, enablechanges, ref LocalWriter.Replacement.RoegadynMaleScale, old.RoegadynMaleScale);
                    UiGlobals.DrawUShort("Roegadyn Female Scale", type, key, enablechanges, ref LocalWriter.Replacement.RoegadynFemaleScale, old.RoegadynFemaleScale);
                    UiGlobals.DrawUShort("AuRa Male Scale", type, key, enablechanges, ref LocalWriter.Replacement.AuRaMaleScale, old.AuRaMaleScale);
                    UiGlobals.DrawUShort("AuRa Female Scale", type, key, enablechanges, ref LocalWriter.Replacement.AuRaFemaleScale, old.AuRaFemaleScale);
                    UiGlobals.DrawUShort("Hrothgar Male Scale", type, key, enablechanges, ref LocalWriter.Replacement.HrothgarMaleScale, old.HrothgarMaleScale);
                    UiGlobals.DrawUShort("Hrothgar Female Scale", type, key, enablechanges, ref LocalWriter.Replacement.HrothgarFemaleScale, old.HrothgarFemaleScale);
                    UiGlobals.DrawUShort("Viera Male Scale", type, key, enablechanges, ref LocalWriter.Replacement.VieraMaleScale, old.VieraMaleScale);
                    UiGlobals.DrawUShort("Viera Female Scale", type, key, enablechanges, ref LocalWriter.Replacement.VieraFemaleScale, old.VieraFemaleScale);
                    UiGlobals.DrawUShort("Midlander Male CameraHeight", type, key, enablechanges, ref LocalWriter.Replacement.HyurMidlanderMaleCameraHeight, old.HyurMidlanderMaleCameraHeight);
                    UiGlobals.DrawUShort("Midlander Female CameraHeight", type, key, enablechanges, ref LocalWriter.Replacement.HyurMidlanderFemaleCameraHeight, old.HyurMidlanderFemaleCameraHeight);
                    UiGlobals.DrawUShort("Highlander Male CameraHeight", type, key, enablechanges, ref LocalWriter.Replacement.HyurHighlanderMaleCameraHeight, old.HyurHighlanderMaleCameraHeight);
                    UiGlobals.DrawByte("Highlander Female CameraHeight", type, key, enablechanges, ref LocalWriter.Replacement.HyurHighlanderFemaleCameraHeight, old.HyurHighlanderFemaleCameraHeight);
                    UiGlobals.DrawByte("Elezen Male CameraHeight", type, key, enablechanges, ref LocalWriter.Replacement.ElezenMaleCameraHeight, old.ElezenMaleCameraHeight);
                    UiGlobals.DrawByte("Elezen Female CameraHeight", type, key, enablechanges, ref LocalWriter.Replacement.ElezenFemaleCameraHeight, old.ElezenFemaleCameraHeight);
                    UiGlobals.DrawByte("Lalafell Male CameraHeight", type, key, enablechanges, ref LocalWriter.Replacement.LalafellMaleCameraHeight, old.LalafellMaleCameraHeight);
                    UiGlobals.DrawByte("Lalafell Female CameraHeight", type, key, enablechanges, ref LocalWriter.Replacement.LalafellFemaleCameraHeight, old.LalafellFemaleCameraHeight);
                    UiGlobals.DrawByte("Miqote Male CameraHeight", type, key, enablechanges, ref LocalWriter.Replacement.MiqoteMaleCameraHeight, old.MiqoteMaleCameraHeight);
                    UiGlobals.DrawByte("Miqote Female CameraHeight", type, key, enablechanges, ref LocalWriter.Replacement.MiqoteFemaleCameraHeight, old.MiqoteFemaleCameraHeight);
                    UiGlobals.DrawByte("Roegadyn Male CameraHeight", type, key, enablechanges, ref LocalWriter.Replacement.RoegadynMaleCameraHeight, old.RoegadynMaleCameraHeight);
                    UiGlobals.DrawByte("Roegadyn Female CameraHeight", type, key, enablechanges, ref LocalWriter.Replacement.RoegadynFemaleCameraHeight, old.RoegadynFemaleCameraHeight);
                    UiGlobals.DrawByte("AuRa Male CameraHeight", type, key, enablechanges, ref LocalWriter.Replacement.AuRaMaleCameraHeight, old.AuRaMaleCameraHeight);
                    UiGlobals.DrawByte("AuRa Female CameraHeight", type, key, enablechanges, ref LocalWriter.Replacement.AuRaFemaleCameraHeight, old.AuRaFemaleCameraHeight);
                    UiGlobals.DrawByte("Hrothgar Male CameraHeight", type, key, enablechanges, ref LocalWriter.Replacement.HrothgarMaleCameraHeight, old.HrothgarMaleCameraHeight);
                    UiGlobals.DrawByte("Hrothgar Female CameraHeight", type, key, enablechanges, ref LocalWriter.Replacement.HrothgarFemaleCameraHeight, old.HrothgarFemaleCameraHeight);
                    UiGlobals.DrawByte("Viera Male CameraHeight", type, key, enablechanges, ref LocalWriter.Replacement.VieraMaleCameraHeight, old.VieraMaleCameraHeight);
                    UiGlobals.DrawByte("Viera Female CameraHeight", type, key, enablechanges, ref LocalWriter.Replacement.VieraFemaleCameraHeight, old.VieraFemaleCameraHeight);

                    //DrawByte("MountBoolSet1", "Mount Bools 1 [raw]", ref replace.MountBoolSet1, i => i.MountBoolSet1);

                    #endregion

                }
            }


#endregion
            #region Search/Set

            using var searchMenu = ImRaii.Popup(searchPopup);
            if (searchMenu)
            {
                ImGui.SetNextItemWidth(200);
                if (ImGui.InputText("##Search " + typenameplural.ToLower(), ref search, 256)) { localsearch = search; }
                ImGui.SameLine();
                using (ImRaii.PushFont(UiBuilder.IconFont))
                {
                    if (ImGui.Button($"{FontAwesomeIcon.Undo.ToIconString()}##{typenameplural.ToLower()}searchreset"))
                    {
                        search = "";
                        localsearch = search;
                    }
                }

                using var popupChild = ImRaii.Child(searchPopup, CalcGlobals.SearchPopScale(), true);
                if (localsearch == "" && localsearch != lastsearch) { SearchList = UiGlobals.CreateSearchList(type); }
                if (localsearch != lastsearch)
                {
                    var tempSearchList = UiGlobals.CreateSearchList(type).OrderBy(i => {
                        if (string.IsNullOrEmpty(localsearch)) return 0;
                        return Math.Min(ProcessingGlobals.ScoreString(i.Value, localsearch),
                            ProcessingGlobals.ScoreString(i.Key.ToString(), localsearch));
                    });
                    SearchList = tempSearchList.ToDictionary<uint, string>();
                    lastsearch = localsearch;
                }
                foreach (var pair in SearchList)
                {
                    if (ImGui.Selectable($"#{pair.Key:D5} {pair.Value}"))
                    {
                        Writer[pair.Key] = CreateEntry(pair.Key);
                        Service.Config.Save();
                    }
                }
            }
            #endregion
        }
    }
}
