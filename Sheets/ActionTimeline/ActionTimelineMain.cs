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
public class ActionTimelineMain
{
    const string type = "ActionTimeline";
    const string typename = "Action Timeline";
    const string typenameplural = "Action Timelines";
    private static string lastsearch = "";
    private static string localsearch = "";
    private static Dictionary<uint, string> SearchList = UiGlobals.CreateSearchList(type);
    public static void Draw(string mainkey, ref Configuration.ReplacementSet _activeSet, ref string search) {

        Dictionary<uint, ActionTimelineConfig> Writer = _activeSet.ActionTimelineWriter;
        var GetName = ActionTimelineManager.GetName;
        var CreateEntry = ActionTimelineConfig.CreateEntry;

        using var subList = ImRaii.Child(mainkey, CalcGlobals.BodyScale(), true);
        if (subList) {
            const string searchPopup = "Search " + typenameplural;
            UiGlobals.DrawAddItem(searchPopup);

            foreach (var key in Writer.Keys) {
                if (ImGui.CollapsingHeader($"#{key:D5} - " + GetName(key))) {
                    var LocalWriter = Writer[key];
                    var old = ActionTimelineManager.GetOriginal(key);
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


                    UiGlobals.DrawString("Animation Path", type, key, enablechanges, ref LocalWriter.Replacement.Animation, old.Animation);
                    UiGlobals.DrawUShort("Weapon Timeline", type, key, enablechanges, ref LocalWriter.Replacement.WeaponTimelineOffset, old.WeaponTimelineOffset, true, ["WeaponTimeline_Path"]);
                    UiGlobals.DrawByte("Type", type, key, enablechanges, ref LocalWriter.Replacement.Type, old.Type);
                    UiGlobals.DrawByte("Priority", type, key, enablechanges, ref LocalWriter.Replacement.Priority, old.Priority);
                    UiGlobals.DrawByte("Stance", type, key, enablechanges, ref LocalWriter.Replacement.Stance, old.Stance);
                    UiGlobals.DrawByte("Slot", type, key, enablechanges, ref LocalWriter.Replacement.Slot, old.Slot);
                    UiGlobals.DrawByte("LookAt Mode", type, key, enablechanges, ref LocalWriter.Replacement.LookAtMode, old.LookAtMode);
                    UiGlobals.DrawByte("Action Timeline ID Mode", type, key, enablechanges, ref LocalWriter.Replacement.ActionTimelineIDMode, old.ActionTimelineIDMode);
                    UiGlobals.DrawByte("Load Type", type, key, enablechanges, ref LocalWriter.Replacement.LoadType, old.LoadType);
                    UiGlobals.DrawByte("Start Attach", type, key, enablechanges, ref LocalWriter.Replacement.StartAttach, old.StartAttach);
                    UiGlobals.DrawByte("Resident PAP", type, key, enablechanges, ref LocalWriter.Replacement.ResidentPap, old.ResidentPap);
                    UiGlobals.DrawByte("PAP Type", type, key, enablechanges, ref LocalWriter.Replacement.Unknown6, old.Unknown6);
                    UiGlobals.DrawByte("Unknown 1", type, key, enablechanges, ref LocalWriter.Replacement.Unknown1, old.Unknown1);
                    UiGlobals.DrawByte("Viper Blade State[Uses fake original]", type, key, enablechanges, ref LocalWriter.Replacement.VPRBladeState, old.VPRBladeState);
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
