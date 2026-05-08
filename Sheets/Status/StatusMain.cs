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
    public static void Draw(string mainkey, ref Configuration.ReplacementSet _activeSet, ref string search)
    {

        const string type = "Status";
        const string typename = "Status";
        const string typenameplural = "Statuses";
        Dictionary<uint, StatusConfig> Writer = _activeSet.StatusWriter;
        var GetName = StatusManager.GetName;
        var CreateEntry = StatusConfig.CreateEntry;

        using var subList = ImRaii.Child(mainkey, CalcGlobals.BodyScale(), true);
        if (subList)
        {
            const string searchPopup = "Search " + typenameplural;
            UiGlobals.DrawAddItem(searchPopup);

            foreach (var key in Writer.Keys)
            {
                if (ImGui.CollapsingHeader($"#{key:D5} - " + GetName(key)))
                {
                    var GetOriginal = StatusManager.old[key];
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

                    //to do: show loop vfx and hit effect as strings

                    //Service.Log.Error("value is: [{value}] and defaultvalue is [{defaultvalue}]", _activeSet.StatusWriter[key].Replacement.StatusLoopVFX, StatusManager.GetOriginal(key).StatusLoopVFX);
                    UiGlobals.DrawInt("Parameter Modifier", type, key, enablechanges, ref LocalWriter.Replacement.ParamModifier, GetOriginal.ParamModifier);
                    UiGlobals.DrawUShort("Status Loop VFX ID", type, key, enablechanges, ref LocalWriter.Replacement.StatusLoopVFX, GetOriginal.StatusLoopVFX, true, "StatusLoopVFX-VFX");
                    UiGlobals.DrawByte("Unknown 0", type, key, enablechanges, ref LocalWriter.Replacement.Unknown0, GetOriginal.Unknown0);
                    UiGlobals.DrawByte("Status Category", type, key, enablechanges, ref LocalWriter.Replacement.StatusCategory, GetOriginal.StatusCategory);
                    UiGlobals.DrawByte("Status Hit Effect ID", type, key, enablechanges, ref LocalWriter.Replacement.StatusHitEffect, GetOriginal.StatusHitEffect, true, "StatusHitEffect-VFX");
                    UiGlobals.DrawByte("Parameter Effect", type, key, enablechanges, ref LocalWriter.Replacement.ParamEffect, GetOriginal.ParamEffect);
                    UiGlobals.DrawByte("Target Type", type, key, enablechanges, ref LocalWriter.Replacement.TargetType, GetOriginal.TargetType);
                    UiGlobals.DrawByte("Flag 1", type, key, enablechanges, ref LocalWriter.Replacement.Flags, GetOriginal.Flags);
                    UiGlobals.DrawByte("Flag 2", type, key, enablechanges, ref LocalWriter.Replacement.Flag2, GetOriginal.Flag2);
                    UiGlobals.DrawByte("Unknown_70_1", type, key, enablechanges, ref LocalWriter.Replacement.Unknown_70_1, GetOriginal.Unknown_70_1);
                    UiGlobals.DrawSByte("AtkType", type, key, enablechanges, ref LocalWriter.Replacement.Unknown2, GetOriginal.Unknown2);
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
