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
public class ActionTransientsMain
{
    const string type = "ActionTransients";
    const string typename = "Action Transient";
    const string typenameplural = "Action Transients";
    const string searchPopup = "Search " + typenameplural;
    public static void Draw(string mainkey, ref Configuration.ReplacementSet _activeSet, ref string search) {

        Dictionary<uint, ActionTransientsConfig> Writer = _activeSet.ActionTransientsWriter;
        var GetName = ActionTransientsManager.GetName;
        var CreateEntry = ActionTransientsConfig.CreateEntry;

        using var subList = ImRaii.Child(mainkey, CalcGlobals.BodyScale(), true);
        if (subList) {
            UiGlobals.DrawAddItem(searchPopup);
            foreach (var key in Writer.Keys)
            {
                var old = ActionTransientsManager.GetOriginal(key);
                if (ImGui.CollapsingHeader($"#{key:D5} - " + GetName(key))) {
                    var LocalWriter = Writer[key];
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
                    UiGlobals.DrawUShort("Icon", type, key, enablechanges, ref LocalWriter.Replacement.Icon, old.Icon, true, ["ICON"]);
                    UiGlobals.DrawString("Name", type, key, enablechanges, ref LocalWriter.Replacement.ActionName, old.ActionName);
                    //This needs better implementation to handle the multiline.. no idea. 
                    //UiGlobals.DrawString("Description", type, key, enablechanges, ref LocalWriter.Replacement.ActionDesc, old.ActionDesc);
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
