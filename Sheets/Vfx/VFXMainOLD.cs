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
public class VfxMainOLD
{
    public static void Draw(string mainkey, ref Configuration.ReplacementSet _activeSet, ref string search)
    {

        const string type = "VFX";
        const string typename = "VFX";
        const string typenameplural = "VFXs";
        Dictionary<uint, VfxConfig> Writer = _activeSet.VfxWriter;
        var GetOriginal = VfxManager.GetOriginal;
        var GetName = VfxManager.GetName;
        var CreateEntry = VfxConfig.CreateEntry;

        using var subList = ImRaii.Child(mainkey, CalcGlobals.BodyScale(), false);
        if (subList)
        {
            const string searchPopup = "Search " + typenameplural;
            UiGlobals.DrawAddItem(searchPopup);

            foreach (var key in Writer.Keys)
            {
                var DefaultValues = GetOriginal(key);
                var LocalWriter = Writer[key];

                if (ImGui.Checkbox("##" + key, ref LocalWriter.Enabled))
                {
                    //_activeSet.VfxWriter[key].Enabled = EntryEnabled;
                    if (LocalWriter.Enabled)
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
                    Writer.Remove(key);
                    Service.Config.Save();
                }

                ImGui.SameLine();
                ImGui.Text($"#{key:D5}");

                ImGui.SameLine();
                ImGui.TextWrapped(GetName(key));

                UiGlobals.DrawString("VFX Path", type, key, LocalWriter.Enabled, ref LocalWriter.Replacement.String1, DefaultValues.String1);
                UiGlobals.DrawItemSeparator();
                continue;

                #endregion
                #region Items

            }

            #endregion
            #region Search/Set

            using var searchMenuVFX = ImRaii.Popup(searchPopup);
            if (searchMenuVFX)
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
