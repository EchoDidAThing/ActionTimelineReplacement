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
public class StatusHitEffectMain
{
    const string type = "StatusHitEffect";
    public static void Draw(string mainkey, ref Configuration.ReplacementSet _activeSet, ref string search)
    {
        using var subList = ImRaii.Child(mainkey, CalcGlobals.BodyScale(), false);
        if (subList)
        {
            const string searchPopup = "Search StatusHitEffects";
            UiGlobals.DrawAddItem(searchPopup);

            foreach (var key in _activeSet.StatusHitEffectWriter.Keys)
            {
                var replace = _activeSet.StatusHitEffectWriter[key].Replacement;
                var DefaultValues = StatusHitEffectManager.GetOriginal(key);

                if (ImGui.Checkbox("##" + key, ref _activeSet.StatusHitEffectWriter[key].Enabled))
                {
                    if (_activeSet.StatusHitEffectWriter[key].Enabled)
                    {
                        Setup.SetStatusHitEffect(key);
                    }
                    else 
                    {
                        Setup.SetStatusHitEffect(key, true);
                    }
                    Service.Config.Save();
                }
                ImGui.SameLine();

                if (ImGui.Button(" - ##" + key))
                {
                    Setup.SetStatusHitEffect(key, true);
                    _activeSet.StatusHitEffectWriter.Remove(key);
                    Service.Config.Save();
                }

                ImGui.SameLine();
                ImGui.Text($"#{key:D5}");

                ImGui.SameLine();
                ImGui.TextWrapped(StatusHitEffectManager.GetName(key));

                //to do: show loop vfx and hit effect as strings

                UiGlobals.DrawUShort("VFX Row", type, key, ref replace.VFX, DefaultValues.VFX);

                UiGlobals.DrawItemSeparator();
                continue;

                #endregion
                #region Items

                
            }

            #endregion
            #region Search/Set

            using var searchStatusHitEffect = ImRaii.Popup(searchPopup);
            if (searchStatusHitEffect)
            {
                ImGui.SetNextItemWidth(CalcGlobals.XY());
                ImGui.InputText("##Search StatusHitEffects", ref search, 256);
                var localsearch = search;

                using var popupChild = ImRaii.Child(searchPopup, CalcGlobals.SearchPopScale(), true);
                foreach (var pair in StatusHitEffectManager.Names.OrderBy(i =>
                {
                    if (string.IsNullOrEmpty(localsearch)) return 0;
                    return Math.Min(ConfigWindow.ScoreString(i.Value, localsearch),
                        ConfigWindow.ScoreString(i.Key.ToString(), localsearch));
                }))
                {
                    if (ImGui.Selectable($"#{pair.Key:D5} {pair.Value}"))
                    {
                        var original = StatusHitEffectManager.GetOriginal(pair.Key);
                        _activeSet.StatusHitEffectWriter[pair.Key] =
                            new StatusHitEffectConfig(new StatusHitEffectReplace(
                                    original.VFX),
                                false);
                        Service.Config.Save();
                    }
                }
            }
            #endregion
        }
    }
}
