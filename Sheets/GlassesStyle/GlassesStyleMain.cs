using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface;
using ImGuiNET;
using System;
using System.Linq;
using ActionTimelineReplacement.Base.Setups;
using ActionTimelineReplacement.Windows;
using ActionTimelineReplacement.Base.Global;
#pragma warning disable CA1416 // Validate platform compatibility

namespace ActionTimelineReplacement.Sheets;

#region Main
public class GlassesStyleMain
{
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

                if (ImGui.Checkbox("##" + key, ref _activeSet.GlassesStyleWriter[key].Enabled))
                {
                    Setup.SetGlassesStyle(key);
                    Service.Config.Save();
                }
                ImGui.SameLine();
                if (ImGui.Button(" - ##" + key))
                {
                    _activeSet.GlassesStyleWriter.Remove(key);
                }

                ImGui.SameLine();
                ImGui.Text($"#{key:D5}");

                ImGui.SameLine();
                ImGui.TextWrapped(GlassesStyleManager.GetName(key));

                DrawSByte("Unknown70_1", "Unknown70_1",ref replace.Unknown70_1, i => i.Unknown70_1);

                DrawSByte("Unknown70_2", "Unknown70_2", ref replace.Unknown70_2, i => i.Unknown70_2);

                DrawSByte("Unknown70_3", "Unknown70_3", ref replace.Unknown70_3, i => i.Unknown70_3);

                DrawSByte("Unknown70_4", "Unknown70_4", ref replace.Unknown70_4, i => i.Unknown70_4);

                DrawSByte("Unknown70_5", "Unknown70_5", ref replace.Unknown70_5, i => i.Unknown70_5);

                DrawSByte("Unknown70_6", "Unknown70_6", ref replace.Unknown70_6, i => i.Unknown70_6);

                DrawShort("Unknown70_7", "Disable in UI", ref replace.Unknown70_7, i => i.Unknown70_7);

                UiGlobals.DrawItemSeparator();
                continue;

                #endregion
                #region Items
                
                void DrawShort(string refname, string text, ref short value,
                    Func<GlassesStyleReplace, short> getDefault)
                {
                    ImGui.TextUnformatted(text);
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * CalcGlobals.GlobalScale());
                    int relay = value;
                    if (ImGui.InputInt("##" + refname + key, ref relay))
                    {
                        value = (short)relay;
                        Setup.SetGlassesStyle(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{refname}{key}"))
                        {
                            value = getDefault(GlassesStyleManager.GetOriginal(key));
                            Setup.SetGlassesStyle(key);
                            Service.Config.Save();
                        }
                    }
                }

                void DrawSByte(string refname, string text, ref sbyte value,
                    Func<GlassesStyleReplace, sbyte> getDefault)
                {
                    ImGui.TextUnformatted(text);
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * CalcGlobals.GlobalScale());
                    int relay = value;
                    if (ImGui.InputInt("##" + refname + key, ref relay))
                    {
                        value = (sbyte)relay;
                        Setup.SetGlassesStyle(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{refname}{key}"))
                        {
                            value = getDefault(GlassesStyleManager.GetOriginal(key));
                            Setup.SetGlassesStyle(key);
                            Service.Config.Save();
                        }
                    }
                }
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
                    return Math.Min(ConfigWindow.ScoreString(i.Value, localsearch),
                        ConfigWindow.ScoreString(i.Key.ToString(), localsearch));
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
