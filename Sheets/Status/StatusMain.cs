using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface;
using Dalamud.Bindings.ImGui;
using System;
using System.Linq;
using ActionTimelineReplacement.Base.Setups;
using ActionTimelineReplacement.Windows;
using ActionTimelineReplacement.Base.Global;
#pragma warning disable CA1416 // Validate platform compatibility

namespace ActionTimelineReplacement.Sheets;

#region Main
public class StatusMain
{
    const string type = "Status";
    public static void Draw(string mainkey, ref Configuration.ReplacementSet _activeSet, ref string search)
    {
        using var subList = ImRaii.Child(mainkey, CalcGlobals.BodyScale(), false);
        if (subList)
        {
            const string searchPopup = "Search statuses";
            UiGlobals.DrawAddItem(searchPopup);

            foreach (var key in _activeSet.StatusWriter.Keys)
            {
                var replace = _activeSet.StatusWriter[key].Replacement;
                var DefaultValues = StatusManager.GetOriginal(key);

                if (ImGui.Checkbox("##" + key, ref _activeSet.StatusWriter[key].Enabled))
                {
                    if (_activeSet.StatusWriter[key].Enabled)
                    {
                        Setup.SetStatus(key);
                    }
                    else
                    {
                        Setup.SetStatus(key, true);
                    }
                    Service.Config.Save();
                }
                ImGui.SameLine();

                if (ImGui.Button(" - ##" + key))
                {
                    Setup.SetStatus(key, true);
                    _activeSet.StatusWriter.Remove(key);
                    Service.Config.Save();
                }

                ImGui.SameLine();
                ImGui.Text($"#{key:D5}");

                ImGui.SameLine();
                ImGui.TextWrapped(StatusManager.GetName(key));

                //to do: show loop vfx and hit effect as strings

                UiGlobals.DrawInt("Parameter Modifier", type, key, _activeSet.StatusWriter[key].Enabled, ref replace.ParamModifier, DefaultValues.ParamModifier);
                UiGlobals.DrawUShort("Status Loop VFX ID", type, key, _activeSet.StatusWriter[key].Enabled, ref replace.StatusLoopVFX, DefaultValues.StatusLoopVFX);
                UiGlobals.DrawByte("Unknown 0", type, key, _activeSet.StatusWriter[key].Enabled, ref replace.Unknown0, DefaultValues.Unknown0);
                UiGlobals.DrawByte("Status Category", type, key, _activeSet.StatusWriter[key].Enabled, ref replace.StatusCategory, DefaultValues.StatusCategory);
                UiGlobals.DrawByte("Status Hit Effect ID", type, key, _activeSet.StatusWriter[key].Enabled, ref replace.StatusHitEffect, DefaultValues.StatusHitEffect);
                UiGlobals.DrawByte("Parameter Effect", type, key, _activeSet.StatusWriter[key].Enabled, ref replace.ParamEffect, DefaultValues.ParamEffect);
                UiGlobals.DrawByte("Target Type", type, key, _activeSet.StatusWriter[key].Enabled, ref replace.TargetType, DefaultValues.TargetType);
                UiGlobals.DrawByte("Flag 1", type, key, _activeSet.StatusWriter[key].Enabled, ref replace.Flags, DefaultValues.Flags);
                UiGlobals.DrawByte("Flag 2", type, key, _activeSet.StatusWriter[key].Enabled, ref replace.Flag2, DefaultValues.Flag2);
                UiGlobals.DrawByte("Unknown_70_1", type, key, _activeSet.StatusWriter[key].Enabled, ref replace.Unknown_70_1, DefaultValues.Unknown_70_1);
                UiGlobals.DrawSByte("AtkType", type, key, _activeSet.StatusWriter[key].Enabled, ref replace.Unknown2, DefaultValues.Unknown2);
                UiGlobals.DrawItemSeparator();
                continue;

                #endregion
                #region Items

            }

            #endregion
            #region Search/Set

            using var searchStatus = ImRaii.Popup(searchPopup);
            if (searchStatus)
            {
                ImGui.SetNextItemWidth(CalcGlobals.XY());
                ImGui.InputText("##Search statuses", ref search, 256);
                var localsearch = search;

                using var popupChild = ImRaii.Child(searchPopup, CalcGlobals.SearchPopScale(), true);
                foreach (var pair in StatusManager.Names.OrderBy(i =>
                {
                    if (string.IsNullOrEmpty(localsearch)) return 0;
                    return Math.Min(ConfigWindow.ScoreString(i.Value, localsearch),
                        ConfigWindow.ScoreString(i.Key.ToString(), localsearch));
                }))
                {
                    if (ImGui.Selectable($"#{pair.Key:D5} {pair.Value}"))
                    {
                        var original = StatusManager.GetOriginal(pair.Key);
                        _activeSet.StatusWriter[pair.Key] =
                            new StatusConfig(new StatusReplace(
                                    original.ParamModifier,
                                    original.StatusLoopVFX,
                                    original.Unknown0,
                                    original.StatusCategory,
                                    original.StatusHitEffect,
                                    original.ParamEffect,
                                    original.TargetType,
                                    original.Flags,
                                    original.Flag2,
                                    original.Unknown_70_1,
                                    original.Unknown2),
                                false);
                        Service.Config.Save();
                    }
                }
            }
            #endregion
        }
    }
}
