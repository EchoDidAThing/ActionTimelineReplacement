using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface;
using Dalamud.Bindings.ImGui;
using System;
using System.Linq;
using ActionTimelineReplacement.Base.Setups;
using ActionTimelineReplacement.Windows;
using ActionTimelineReplacement.Base.Global;
using System.Reflection.Metadata;

#pragma warning disable CA1416 // Validate platform compatibility

namespace ActionTimelineReplacement.Sheets;

#region Main
public class VfxMain
{
    const string type = "VFX";
    public static void Draw(string mainkey, ref Configuration.ReplacementSet _activeSet, ref string search)
    {
        using var subList = ImRaii.Child(mainkey, CalcGlobals.BodyScale(), false);
        if (subList)
        {
            const string searchPopup = "Search Vfxes";
            UiGlobals.DrawAddItem(searchPopup);

            foreach (var key in _activeSet.VfxWriter.Keys)
            {
                var replace = _activeSet.VfxWriter[key].Replacement;
                var DefaultValues = VfxManager.GetOriginal(key);

                if (ImGui.Checkbox("##" + key, ref _activeSet.VfxWriter[key].Enabled))
                {
                    if (_activeSet.VfxWriter[key].Enabled)
                    {
                        Setup.SetVfx(key);
                    }
                    else
                    {
                        Setup.SetVfx(key, true);
                    }
                    Service.Config.Save();
                }
                ImGui.SameLine();

                if (ImGui.Button(" - ##" + key))
                {
                    Setup.SetVfx(key, true);
                    _activeSet.VfxWriter.Remove(key);
                    Service.Config.Save();
                }

                ImGui.SameLine();
                ImGui.Text($"#{key:D5}");

                ImGui.SameLine();
                ImGui.TextWrapped(VfxManager.GetName(key));

                UiGlobals.DrawString("VFX Path", type, key, ref replace.String1, DefaultValues.String1);


                UiGlobals.DrawItemSeparator();
                continue;

                #endregion
                #region Items


                
            }

            #endregion
            #region Search/Set

            using var searchVfx = ImRaii.Popup(searchPopup);
            if (searchVfx)
            {
                ImGui.SetNextItemWidth(CalcGlobals.XY());
                ImGui.InputText("##Search Vfxes", ref search, 256);
                var localsearch = search;

                using var popupChild = ImRaii.Child(searchPopup, CalcGlobals.SearchPopScale(), true);
                foreach (var pair in VfxManager.Names.OrderBy(i =>
                {
                    return Math.Min(ConfigWindow.ScoreString(i.Value, localsearch),
                        ConfigWindow.ScoreString(i.Key.ToString(), localsearch));
                }))
                {
                    if (ImGui.Selectable($"#{pair.Key:D5} {pair.Value}"))
                    {
                        var original = VfxManager.GetOriginal(pair.Key);
                        _activeSet.VfxWriter[pair.Key] =
                            new VfxConfig(new VfxReplace(pair.Key, original.String1),
                                false);
                        Service.Config.Save();
                    }
                }
            }
            #endregion
        }
    }
}
