using ActionTimelineReplacement.Base.Global;
using ActionTimelineReplacement.Base.Setups;
using ActionTimelineReplacement.Interface;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface;
using Dalamud.Interface.Utility.Raii;
using FFXIVClientStructs.FFXIV.Common.Lua;
using System;
using System.Collections.Generic;
using System.Linq;
using static FFXIVClientStructs.FFXIV.Client.UI.Misc.AozNoteModule;
#pragma warning disable CA1416 // Validate platform compatibility

namespace ActionTimelineReplacement.Sheets;

#region Main
public class ActionCastVFXMain
{
    public static void Draw(string mainkey, ref Configuration.ReplacementSet _activeSet, ref string search)
    {

        const string type = "ActionCastVFX";
        const string typename = "Action Cast VFX";
        const string typenameplural = "Action Cast VFXs";
        Dictionary<uint, ActionCastVFXConfig> LocalWriter = _activeSet.ActionCastVFXWriter;
        var LocalGetOriginal = ActionCastVFXManager.GetOriginal;
        var LocalGetName = ActionCastVFXManager.GetName;
        var LocalCreateEntry = ActionCastVFXConfig.CreateEntry;

        using var subList = ImRaii.Child(mainkey, CalcGlobals.BodyScale(), false);
        if (subList)
        {
            const string searchPopup = "Search " + typenameplural;
            UiGlobals.DrawAddItem(searchPopup);

            foreach (var key in LocalWriter.Keys)
            {
                var replace = LocalWriter[key].Replacement;
                var DefaultValues = LocalGetOriginal(key);
                var EntryEnabled = LocalWriter[key].Enabled;

                if (ImGui.Checkbox("##" + key, ref LocalWriter[key].Enabled))
                {
                    if (LocalWriter[key].Enabled)
                    {
                        Setup.SetupByType(key, type);
                    }
                    else
                    {
                        Setup.SetupByType(key, type, true);
                    }
                    Service.Config.Save();
                }
                ImGui.SameLine();

                if (ImGui.Button(" - ##" + key))
                {
                    Setup.SetupByType(key, type, true);
                    LocalWriter.Remove(key);
                    Service.Config.Save();
                }

                ImGui.SameLine();
                ImGui.Text($"#{key:D5}");

                ImGui.SameLine();
                ImGui.TextWrapped(LocalGetName(key));



                //REENABLE
                //UiGlobals.DrawUShort("Action Cast VFX", type, key, EntryEnabled, ref replace.CastVfx, DefaultValues.CastVfx);
                continue;

                #endregion
                #region Items

            }

            #endregion
            #region Search/Set

            using var searchMenu = ImRaii.Popup(searchPopup);
            if (searchMenu)
            {
                ImGui.SetNextItemWidth(CalcGlobals.XY());
                ImGui.InputText("##Search " + typenameplural.ToLower(), ref search, 256);
                var localsearch = search;

                using var popupChild = ImRaii.Child(searchPopup, CalcGlobals.SearchPopScale(), true);
                foreach (var pair in UiGlobals.CreateSearchList(type).OrderBy(i =>
                {
                    if (string.IsNullOrEmpty(localsearch)) return 0;
                    return Math.Min(ProcessingGlobals.ScoreString(i.Value, localsearch),
                        ProcessingGlobals.ScoreString(i.Key.ToString(), localsearch));
                }))
                {
                    if (ImGui.Selectable($"#{pair.Key:D5} {pair.Value}"))
                    {
                        LocalWriter[pair.Key] =  LocalCreateEntry(pair.Key);
                    }
                }
            }
            #endregion
        }
    }
}
