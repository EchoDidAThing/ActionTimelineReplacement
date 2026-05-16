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
public class StatusLoopVFXMainOLD
{
    public static void Draw(string mainkey, ref Configuration.ReplacementSet _activeSet, ref string search)
    {

        const string type = "StatusLoopVFX";
        const string typename = "Status Loop VFX";
        const string typenameplural = "Status Loop VFXs";
        Dictionary<uint, StatusLoopVFXConfig> Writer = _activeSet.StatusLoopVFXWriter;
        var GetName = StatusLoopVFXManager.GetName;
        var CreateEntry = StatusLoopVFXConfig.CreateEntry;

        using var subList = ImRaii.Child(mainkey, CalcGlobals.BodyScale(), true);
        if (subList)
        {
            const string searchPopup = "Search " + typenameplural;
            UiGlobals.DrawAddItem(searchPopup);

            foreach (var key in Writer.Keys)
            {
                if (ImGui.CollapsingHeader($"#{key:D5} - " + GetName(key)))
                {
                    var GetOriginal = StatusLoopVFXManager.old[key];
                    var LocalWriter = Writer[key];
                    bool enablechanges = false;

                    if (Service.Config.EnableReplacement && _activeSet.Enabled && LocalWriter.Enabled )
                    {
                        enablechanges = true;
                    }
                    else
                    {
                        enablechanges = false;
                    }

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Trash.ToIconString()}##{key}"))
                        {
                            Setup.SetupByType(key, type, true);
                            Writer.Remove(key);
                            Service.Config.Save();
                        }
                    }

                    if (ImGui.Checkbox("##" + key, ref LocalWriter.Enabled))
                    {
                        if (enablechanges)
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
                    ImGui.TextUnformatted("Entry Enabled");
                    UiGlobals.DrawItemSeparator();

                    //UiGlobals.DrawUShort("Friendly VFX ID", type, key, _activeSet.StatusLoopVFXWriter[key].Enabled, ref LocalWriter.Replacement.FriendlyVFX, GetOriginal.FriendlyVFX, true, "VFX");
                    //UiGlobals.DrawUShort("Stack VFX 1 ID", type, key, _activeSet.StatusLoopVFXWriter[key].Enabled, ref LocalWriter.Replacement.StackVFX1, GetOriginal.StackVFX1, true, "VFX");
                    //UiGlobals.DrawUShort("Stack VFX 2 ID", type, key, _activeSet.StatusLoopVFXWriter[key].Enabled, ref LocalWriter.Replacement.StackVFX2, GetOriginal.StackVFX2, true, "VFX");
                    //UiGlobals.DrawUShort("Enemy VFX ID", type, key, _activeSet.StatusLoopVFXWriter[key].Enabled, ref LocalWriter.Replacement.EnemyVFX, GetOriginal.EnemyVFX, true, "VFX");
                    UiGlobals.DrawByte("Stack 1 Trigger", type, key, _activeSet.StatusLoopVFXWriter[key].Enabled, ref LocalWriter.Replacement.StackTrigger1, GetOriginal.StackTrigger1);
                    UiGlobals.DrawByte("Stack 2 Trigger", type, key, _activeSet.StatusLoopVFXWriter[key].Enabled, ref LocalWriter.Replacement.StackTrigger2, GetOriginal.StackTrigger2);
                    UiGlobals.DrawByte("Unknown 1", type, key, _activeSet.StatusLoopVFXWriter[key].Enabled, ref LocalWriter.Replacement.Unknown1, GetOriginal.Unknown1);
                    UiGlobals.DrawByte("Unknown 2", type, key, _activeSet.StatusLoopVFXWriter[key].Enabled, ref LocalWriter.Replacement.Unknown2, GetOriginal.Unknown2);
                    UiGlobals.DrawBool("Unknown 3", type, key, _activeSet.StatusLoopVFXWriter[key].Enabled, ref LocalWriter.Replacement.Unknown3, GetOriginal.Unknown3);
                    UiGlobals.DrawBool("Unknown 4", type, key, _activeSet.StatusLoopVFXWriter[key].Enabled, ref LocalWriter.Replacement.Unknown4, GetOriginal.Unknown4);
                    UiGlobals.DrawBool("Unknown 5", type, key, _activeSet.StatusLoopVFXWriter[key].Enabled, ref LocalWriter.Replacement.Unknown5, GetOriginal.Unknown5);
                    UiGlobals.DrawItemSeparator();
                    continue;

                    #endregion
                    #region Items

                }
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
                        Writer[pair.Key] =  CreateEntry(pair.Key);
                    }
                }
            }
            #endregion
        }
    }
}
