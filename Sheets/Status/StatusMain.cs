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
public class StatusMain
{
    const string type = "Status";
    const string typename = "Status";
    const string typenameplural = "Statuses";
    private static string lastsearch = "";
    private static string localsearch = "";
    private static Dictionary<uint, string> SearchList = UiGlobals.CreateSearchList(type);
    public static void Draw(string mainkey, ref Configuration.ReplacementSet _activeSet, ref string search) {

        Dictionary<uint, StatusConfig> Writer = _activeSet.StatusWriter;
        var GetName = StatusManager.GetName;
        var CreateEntry = StatusConfig.CreateEntry;

        using var subList = ImRaii.Child(mainkey, CalcGlobals.BodyScale(), true);
        if (subList) {
            const string searchPopup = "Search " + typenameplural;
            UiGlobals.DrawAddItem(searchPopup);

            foreach (var key in Writer.Keys) {
                if (ImGui.CollapsingHeader($"#{key:D5} - " + GetName(key))) {
                    var LocalWriter = Writer[key];
                    var old = StatusManager.OldExport(key);
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
                    UiGlobals.DrawInt("Parameter Modifier", type, key, enablechanges, ref LocalWriter.Replacement.ParamModifier, old.ParamModifier);
                    UiGlobals.DrawUShort("Status Loop VFX ID", type, key, enablechanges, ref LocalWriter.Replacement.StatusLoopVFX, old.StatusLoopVFX, true, ["StatusLoopVFX-VFX"]);
                    UiGlobals.DrawByte("Unknown 0", type, key, enablechanges, ref LocalWriter.Replacement.Unknown0, old.Unknown0);
                    UiGlobals.DrawByte("Status Category", type, key, enablechanges, ref LocalWriter.Replacement.StatusCategory, old.StatusCategory);
                    UiGlobals.DrawByte("Status Hit Effect ID", type, key, enablechanges, ref LocalWriter.Replacement.StatusHitEffect, old.StatusHitEffect, true, ["StatusHitEffect-VFX"]);
                    UiGlobals.DrawByte("Parameter Effect", type, key, enablechanges, ref LocalWriter.Replacement.ParamEffect, old.ParamEffect);
                    UiGlobals.DrawByte("Target Type", type, key, enablechanges, ref LocalWriter.Replacement.TargetType, old.TargetType);
                    UiGlobals.DrawByte("Flag 1", type, key, enablechanges, ref LocalWriter.Replacement.Flags, old.Flags);
                    UiGlobals.DrawByte("Flag 2", type, key, enablechanges, ref LocalWriter.Replacement.Flag2, old.Flag2);
                    UiGlobals.DrawByte("Unknown_70_1", type, key, enablechanges, ref LocalWriter.Replacement.Unknown_70_1, old.Unknown_70_1);
                    UiGlobals.DrawSByte("AtkType", type, key, enablechanges, ref LocalWriter.Replacement.Unknown2, old.Unknown2);
                    UiGlobals.DrawItemSeparator();
                    continue;

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
                    if (ImGui.Selectable($"#{pair.Key:D5} {pair.Value}")) { Writer[pair.Key] = CreateEntry(pair.Key); }
                }
            }
            #endregion
        }
    }
}
