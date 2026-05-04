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
public class StatusMain
{
    public static void Draw(string mainkey, ref Configuration.ReplacementSet _activeSet, ref string search)
    {

        const string type = "Status";
        const string typename = "Status";
        const string typenameplural = "Statuses";
        Dictionary<uint, StatusConfig> LocalWriter = _activeSet.StatusWriter;
        var LocalGetOriginal = StatusManager.GetOriginal;
        var LocalGetName = StatusManager.GetName;
        var LocalCreateEntry = StatusConfig.CreateEntry;

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

                //to do: show loop vfx and hit effect as strings

                UiGlobals.DrawInt("Parameter Modifier", type, key, EntryEnabled, ref replace.ParamModifier, DefaultValues.ParamModifier);
                UiGlobals.DrawUShort("Status Loop VFX ID", type, key, EntryEnabled, ref replace.StatusLoopVFX, DefaultValues.StatusLoopVFX);
                UiGlobals.DrawByte("Unknown 0", type, key, EntryEnabled, ref replace.Unknown0, DefaultValues.Unknown0);
                UiGlobals.DrawByte("Status Category", type, key, EntryEnabled, ref replace.StatusCategory, DefaultValues.StatusCategory);
                UiGlobals.DrawByte("Status Hit Effect ID", type, key, EntryEnabled, ref replace.StatusHitEffect, DefaultValues.StatusHitEffect);
                UiGlobals.DrawByte("Parameter Effect", type, key, EntryEnabled, ref replace.ParamEffect, DefaultValues.ParamEffect);
                UiGlobals.DrawByte("Target Type", type, key, EntryEnabled, ref replace.TargetType, DefaultValues.TargetType);
                UiGlobals.DrawByte("Flag 1", type, key, EntryEnabled, ref replace.Flags, DefaultValues.Flags);
                UiGlobals.DrawByte("Flag 2", type, key, EntryEnabled, ref replace.Flag2, DefaultValues.Flag2);
                UiGlobals.DrawByte("Unknown_70_1", type, key, EntryEnabled, ref replace.Unknown_70_1, DefaultValues.Unknown_70_1);
                UiGlobals.DrawSByte("AtkType", type, key, EntryEnabled, ref replace.Unknown2, DefaultValues.Unknown2);
                UiGlobals.DrawItemSeparator();
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
