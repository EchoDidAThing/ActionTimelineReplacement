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
public class OrnamentMain
{
    const string type = "Ornament";
    public static void Draw(string mainkey, ref Configuration.ReplacementSet _activeSet, ref string search)
    {
        using var subList = ImRaii.Child(mainkey, CalcGlobals.BodyScale(), false);
        if (subList)
        {
            const string searchPopup = "Search ornament data";
            UiGlobals.DrawAddItem(searchPopup);

            foreach (var key in _activeSet.OrnamentWriter.Keys)
            {
                var replace = _activeSet.OrnamentWriter[key].Replacement;
                var DefaultValues = OrnamentManager.GetOriginal(key);

                if (ImGui.Checkbox("##" + key, ref _activeSet.OrnamentWriter[key].Enabled))
                {
                    if (_activeSet.OrnamentWriter[key].Enabled)
                    {
                        Setup.SetOrnament(key);
                    }
                    else
                    {
                        Setup.SetOrnament(key, true);
                    }
                    Service.Config.Save();
                }
                ImGui.SameLine();
                if (ImGui.Button(" - ##" + key))
                {
                    Setup.SetOrnament(key);
                    _activeSet.OrnamentWriter.Remove(key);
                    Service.Config.Save();
                }

                ImGui.SameLine();
                ImGui.Text($"#{key:D5}");

                ImGui.SameLine();
                ImGui.TextWrapped(OrnamentManager.GetName(key));


                UiGlobals.DrawSByte("Unknown 0", type, key, _activeSet.OrnamentWriter[key].Enabled, ref replace.Unknown0, DefaultValues.Unknown0);
                UiGlobals.DrawUShort("Model", type, key, _activeSet.OrnamentWriter[key].Enabled, ref replace.Model, DefaultValues.Model);UiGlobals.DrawUShort("Action Row ID", type, key, _activeSet.OrnamentWriter[key].Enabled, ref replace.Action, DefaultValues.Action);UiGlobals.DrawUShort("Transient [unknown]", type, key, _activeSet.OrnamentWriter[key].Enabled, ref replace.Transient, DefaultValues.Transient);
                UiGlobals.DrawByte("Attachment Point", type, key, _activeSet.OrnamentWriter[key].Enabled, ref replace.AttachmentPoint, DefaultValues.AttachmentPoint);
                UiGlobals.DrawByte("Ornament Hide State", type, key, _activeSet.OrnamentWriter[key].Enabled, ref replace.Unknown3, DefaultValues.Unknown3);
                UiGlobals.DrawByte("Idle Pose Group", type, key, _activeSet.OrnamentWriter[key].Enabled, ref replace.Unknown4, DefaultValues.Unknown4);

                UiGlobals.DrawItemSeparator();
                continue;

                #endregion
                #region Items
                
               
            }
                            
            #endregion
            #region Search/Set

            using var searchOrnament = ImRaii.Popup(searchPopup);
            if (searchOrnament)
            {
                ImGui.SetNextItemWidth(CalcGlobals.XY());
                ImGui.InputText("##Search ornament data", ref search, 256);
                var localsearch = search;

                using var popupChild = ImRaii.Child(searchPopup, CalcGlobals.SearchPopScale(), true);
                foreach (var pair in OrnamentManager.Names.OrderBy(i =>
                {
                    if (string.IsNullOrEmpty(localsearch)) return 0;
                    return Math.Min(ProcessingGlobals.ScoreString(i.Value, localsearch),
                        ProcessingGlobals.ScoreString(i.Key.ToString(), localsearch));
                }))
                {
                    if (ImGui.Selectable($"#{pair.Key:D5} {pair.Value}"))
                    {
                        var original = OrnamentManager.GetOriginal(pair.Key);
                        _activeSet.OrnamentWriter[pair.Key] =
                            new OrnamentConfig(new OrnamentReplace(
                                    original.Unknown0,
                                    original.Model,
                                    original.Action,
                                    original.Transient,
                                    original.AttachmentPoint,
                                    original.Unknown3,
                                    original.Unknown4),
                                false);
                        Service.Config.Save();
                    }
                }
            }
            #endregion
        }
    }
}
