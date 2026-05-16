using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface;
using Dalamud.Bindings.ImGui;
using System;
using System.Linq;
using ActionTimelineReplacement.Base.Setups;
using ActionTimelineReplacement.Interface;
using ActionTimelineReplacement.Base.Global;
#pragma warning disable CA1416 // Validate platform compatibility

namespace ActionTimelineReplacement.Sheets;

#region Main
public class GlassesStyleMainOLD
{
    const string type = "GlassesStyle";
    public static void Draw(string mainkey, ref Configuration.ReplacementSet _activeSet, ref string search)
    {
        using var subList = ImRaii.Child(mainkey, CalcGlobals.BodyScale(), false);
        if (subList)
        {
            const string searchPopup = "Search glasses styles";
            UiGlobals.DrawAddItem(searchPopup);

            foreach (var key in _activeSet.GlassesStyleWriter.Keys)
            {
                var replace = _activeSet.GlassesStyleWriter[key].Replacement;
                var DefaultValues = GlassesStyleManager.GetOriginal(key);

                if (ImGui.Checkbox("##" + key, ref _activeSet.GlassesStyleWriter[key].Enabled))
                {
                    if (_activeSet.GlassesStyleWriter[key].Enabled)
                    {
                        Setup.SetGlassesStyle(key);
                    }
                    else
                    {
                        Setup.SetGlassesStyle(key, true);
                    }
                    Service.Config.Save();
                }
                ImGui.SameLine();
                if (ImGui.Button(" - ##" + key))
                {
                    Setup.SetGlassesStyle(key, true);
                    _activeSet.GlassesStyleWriter.Remove(key);
                    Service.Config.Save();
                }

                ImGui.SameLine();
                ImGui.Text($"#{key:D5}");

                ImGui.SameLine();
                ImGui.TextWrapped(GlassesStyleManager.GetName(key));


                UiGlobals.DrawSByte("Unknown70_1", type, key, _activeSet.GlassesStyleWriter[key].Enabled, ref replace.Unknown70_1, DefaultValues.Unknown70_1);
                UiGlobals.DrawSByte("Unknown70_2", type, key, _activeSet.GlassesStyleWriter[key].Enabled, ref replace.Unknown70_2, DefaultValues.Unknown70_2);
                UiGlobals.DrawSByte("Unknown70_3", type, key, _activeSet.GlassesStyleWriter[key].Enabled, ref replace.Unknown70_3, DefaultValues.Unknown70_3);
                UiGlobals.DrawSByte("Unknown70_4", type, key, _activeSet.GlassesStyleWriter[key].Enabled, ref replace.Unknown70_4, DefaultValues.Unknown70_4);
                UiGlobals.DrawSByte("Unknown70_5", type, key, _activeSet.GlassesStyleWriter[key].Enabled, ref replace.Unknown70_5, DefaultValues.Unknown70_5);
                UiGlobals.DrawSByte("Unknown70_6", type, key, _activeSet.GlassesStyleWriter[key].Enabled, ref replace.Unknown70_6, DefaultValues.Unknown70_6);
                UiGlobals.DrawShort("Disable in UI", type, key, _activeSet.GlassesStyleWriter[key].Enabled, ref replace.Unknown70_7, DefaultValues.Unknown70_7);

                UiGlobals.DrawItemSeparator();
                continue;

                #endregion
                #region Items
                
                
            }
                            
            #endregion
            #region Search/Set

            using var searchGlassesStyle = ImRaii.Popup(searchPopup);
            if (searchGlassesStyle)
            {
                ImGui.SetNextItemWidth(CalcGlobals.XY());
                ImGui.InputText("##Search glasses styles", ref search, 256);
                var localsearch = search;

                using var popupChild = ImRaii.Child(searchPopup, CalcGlobals.SearchPopScale(), true);
                foreach (var pair in GlassesStyleManager.Names.OrderBy(i =>
                {
                    if (string.IsNullOrEmpty(localsearch)) return 0;
                    return Math.Min(ProcessingGlobals.ScoreString(i.Value, localsearch),
                        ProcessingGlobals.ScoreString(i.Key.ToString(), localsearch));
                }))
                {
                    if (ImGui.Selectable($"#{pair.Key:D5} {pair.Value}"))
                    {
                        var original = GlassesStyleManager.GetOriginal(pair.Key);
                        _activeSet.GlassesStyleWriter[pair.Key] =
                            new GlassesStyleConfig(new GlassesStyleReplace(
                                    original.Unknown70_1,
                                    original.Unknown70_2,
                                    original.Unknown70_3,
                                    original.Unknown70_4,
                                    original.Unknown70_5,
                                    original.Unknown70_6,
                                    original.Unknown70_7),
                                false);
                        Service.Config.Save();
                    }
                }
            }
            #endregion
        }
    }
}
