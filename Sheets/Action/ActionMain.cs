using ActionTimelineReplacement.Base.Global;
using ActionTimelineReplacement.Base.Setups;
using ActionTimelineReplacement.Interface;
using Dalamud.Bindings.ImGui;
using Dalamud.Game.Config;
using Dalamud.Interface;
using Dalamud.Interface.Utility.Raii;
using FFXIVClientStructs.FFXIV.Client.UI;
using FFXIVClientStructs.FFXIV.Common.Lua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using static FFXIVClientStructs.FFXIV.Client.UI.Misc.AozNoteModule;
#pragma warning disable CA1416 // Validate platform compatibility

namespace ActionTimelineReplacement.Sheets;

#region Main
public class ActionMain
{
    const string type = "Action";
    const string typename = "Action";
    const string typenameplural = "Actions";
    private static string lastsearch = "";
    private static string localsearch = "";
    private static Dictionary<uint, string> SearchList = UiGlobals.CreateSearchList(type);
    public static void Draw(string mainkey, ref Configuration.ReplacementSet _activeSet, ref string search) {

        Dictionary<uint, ActionConfig> Writer = _activeSet.ActionWriter;
        var GetName = ActionManager.GetName;
        var CreateEntry = ActionConfig.CreateEntry;

        using var subList = ImRaii.Child(mainkey, CalcGlobals.BodyScale(), true);
        if (subList) {
            const string searchPopup = "Search " + typenameplural;
            UiGlobals.DrawAddItem(searchPopup);

            foreach (var key in Writer.Keys) {
                if (ImGui.CollapsingHeader($"#{key:D5} - " + GetName(key))) {
                    var LocalWriter = Writer[key];
                    var old = ActionManager.GetOriginal(key);
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
                    UiGlobals.DrawUShort("Cast", type, key, enablechanges, ref LocalWriter.Replacement.CastVfx, old.CastVfx, true, ["ActionCastVFX_Index", "ActionCastVFX-VFX_Path"]);
                    UiGlobals.DrawUShort("Start", type, key, enablechanges, ref LocalWriter.Replacement.AnimationStart, old.AnimationStart, true, ["ActionTimeline_Path"]);
                    UiGlobals.DrawShort("End", type, key, enablechanges, ref LocalWriter.Replacement.AnimationEnd, old.AnimationEnd, true, ["ActionTimeline_Path"]);
                    UiGlobals.DrawUShort("Hit", type, key, enablechanges, ref LocalWriter.Replacement.ActionTimelineHit, old.ActionTimelineHit, true, ["ActionTimeline_Path"]);
                    UiGlobals.DrawByte("Unknown 1", type, key, enablechanges, ref LocalWriter.Replacement.Unknown1, old.Unknown1);
                    UiGlobals.DrawByte("Unknown 2", type, key, enablechanges, ref LocalWriter.Replacement.Unknown2, old.Unknown2);
                    UiGlobals.DrawByte("Unknown 4", type, key, enablechanges, ref LocalWriter.Replacement.Unknown4, old.Unknown4);
                    UiGlobals.DrawByte("Unknown_70", type, key, enablechanges, ref LocalWriter.Replacement.Unknown_70, old.Unknown_70);
                    UiGlobals.DrawItemSeparator();
                    continue;

                    #endregion

                }
            }


#endregion
            #region Search/Set

            using var searchMenu = ImRaii.Popup(searchPopup);
            if (searchMenu) {
                ImGui.SetNextItemWidth(200);
                if (ImGui.InputText("##Search " + typenameplural.ToLower(), ref search, 256)){ localsearch = search;}
                ImGui.SameLine();
                using (ImRaii.PushFont(UiBuilder.IconFont)){
                    if (ImGui.Button($"{FontAwesomeIcon.Undo.ToIconString()}##{typenameplural.ToLower()}searchreset")) {
                        search = "";
                        localsearch = search;
                    }
                }

                using var popupChild = ImRaii.Child(searchPopup, CalcGlobals.SearchPopScale(), true);
                if (localsearch == "" && localsearch != lastsearch) { SearchList = UiGlobals.CreateSearchList(type); }
                if (localsearch != lastsearch){
                    var tempSearchList = UiGlobals.CreateSearchList(type).OrderBy(i => {
                        if (string.IsNullOrEmpty(localsearch)) return 0;
                        return Math.Min(ProcessingGlobals.ScoreString(i.Value, localsearch),
                            ProcessingGlobals.ScoreString(i.Key.ToString(), localsearch));
                    });
                    SearchList = tempSearchList.ToDictionary<uint, string>();
                    lastsearch = localsearch;
                }
                foreach (var pair in SearchList) {
                    if (ImGui.Selectable($"#{pair.Key:D5} {pair.Value}")){ Writer[pair.Key] =  CreateEntry(pair.Key); }
                }
            }
            #endregion
        }
    }
}
