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
public class OrnamentCustomizeMain
{
    public static void Draw(string mainkey, ref Configuration.ReplacementSet _activeSet, ref string search)
    {
        using var subList = ImRaii.Child(mainkey, CalcGlobals.BodyScale(), false);
        if (subList)
        {
            const string searchPopup = "Search ornament customizable data";
            UiGlobals.DrawAddItem(searchPopup);

            foreach (var key in _activeSet.OrnamentCustomizeWriter.Keys)
            {
                var replace = _activeSet.OrnamentCustomizeWriter[key].Replacement;

                if (ImGui.Checkbox("##" + key, ref _activeSet.OrnamentCustomizeWriter[key].Enabled))
                {
                    if (_activeSet.OrnamentCustomizeWriter[key].Enabled)
                    {
                        Setup.SetOrnamentCustomize(key);
                    }
                    else
                    {
                        Setup.SetOrnamentCustomize(key, true);
                    }
                    Service.Config.Save();
                }
                ImGui.SameLine();
                if (ImGui.Button(" - ##" + key))
                {
                    Setup.SetOrnamentCustomize(key, true);
                    _activeSet.OrnamentCustomizeWriter.Remove(key);
                    Service.Config.Save();
                }

                ImGui.SameLine();
                ImGui.Text($"#{key:D5}");

                ImGui.SameLine();
                ImGui.TextWrapped(OrnamentCustomizeManager.GetName(key));

                DrawUShort("Unknown0","Unknown 0", ref replace.Unknown0, i => i.Unknown0);

                DrawShort("Unknown1", "Unknown 1", ref replace.Unknown1, i => i.Unknown1);

                DrawShort("Unknown2", "Unknown 2", ref replace.Unknown2, i => i.Unknown2);

                DrawShort("Unknown3", "Unknown 3", ref replace.Unknown3, i => i.Unknown3);

                DrawShort("Unknown4", "Unknown 4", ref replace.Unknown4, i => i.Unknown4);

                DrawShort("Unknown5", "Unknown 5", ref replace.Unknown5, i => i.Unknown5);

                DrawShort("Unknown6", "Unknown 6", ref replace.Unknown6, i => i.Unknown6);

                UiGlobals.DrawItemSeparator();
                continue;

                #endregion
                #region Items
                
                void DrawShort(string refname, string text, ref short value,
                    Func<OrnamentCustomizeReplace, short> getDefault)
                {
                    ImGui.TextUnformatted(text);
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * CalcGlobals.GlobalScale());
                    int relay = value;
                    if (ImGui.InputInt("##" + refname + key, ref relay))
                    {
                        value = (short)relay;
                        Setup.SetOrnamentCustomize(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{refname}{key}"))
                        {
                            value = getDefault(OrnamentCustomizeManager.GetOriginal(key));
                            Setup.SetOrnamentCustomize(key);
                            Service.Config.Save();
                        }
                    }
                }

                void DrawUShort(string refname, string text, ref ushort value,
                    Func<OrnamentCustomizeReplace, ushort> getDefault)
                {
                    ImGui.TextUnformatted(text);
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * CalcGlobals.GlobalScale());
                    int relay = value;
                    if (ImGui.InputInt("##" + refname + key, ref relay))
                    {
                        value = (ushort)relay;
                        Setup.SetOrnamentCustomize(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{refname}{key}"))
                        {
                            value = getDefault(OrnamentCustomizeManager.GetOriginal(key));
                            Setup.SetOrnamentCustomize(key);
                            Service.Config.Save();
                        }
                    }
                }
            }
                            
            #endregion
            #region Search/Set

            using var searchOrnamentCustomize = ImRaii.Popup(searchPopup);
            if (searchOrnamentCustomize)
            {
                ImGui.SetNextItemWidth(CalcGlobals.XY());
                ImGui.InputText("##Search ornament customize data", ref search, 256);
                var localsearch = search;

                using var popupChild = ImRaii.Child(searchPopup, CalcGlobals.SearchPopScale(), true);
                foreach (var pair in OrnamentCustomizeManager.Names.OrderBy(i =>
                {
                    if (string.IsNullOrEmpty(localsearch)) return 0;
                    return Math.Min(ConfigWindow.ScoreString(i.Value, localsearch),
                        ConfigWindow.ScoreString(i.Key.ToString(), localsearch));
                }))
                {
                    if (ImGui.Selectable($"#{pair.Key:D5} {pair.Value}"))
                    {
                        var original = OrnamentCustomizeManager.GetOriginal(pair.Key);
                        _activeSet.OrnamentCustomizeWriter[pair.Key] =
                            new OrnamentCustomizeConfig(new OrnamentCustomizeReplace(
                                    original.Unknown0,
                                    original.Unknown1,
                                    original.Unknown2,
                                    original.Unknown3,
                                    original.Unknown4,
                                    original.Unknown5,
                                    original.Unknown6),
                                false);
                        Service.Config.Save();
                    }
                }
            }
            #endregion
        }
    }
}
