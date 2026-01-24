using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface;
using Dalamud.Bindings.ImGui;
using System;
using System.Linq;
using ActionTimelineReplacement.Base.Setups;
using ActionTimelineReplacement.Windows;
using ActionTimelineReplacement.Base.Global;

namespace ActionTimelineReplacement.Sheets;

#region Main
public class GlassesMain
{
    public static void Draw(string mainkey, ref Configuration.ReplacementSet _activeSet, ref string search)
    {
        using var subList = ImRaii.Child(mainkey, CalcGlobals.BodyScale(), false);
        if (subList)
        {
            const string searchPopup = "Search glasses";
            UiGlobals.DrawAddItem(searchPopup);

            foreach (var key in _activeSet.GlassesWriter.Keys)
            {
                var replace = _activeSet.GlassesWriter[key].Replacement;

                if (ImGui.Checkbox("##" + key, ref _activeSet.GlassesWriter[key].Enabled))
                {
                    if (_activeSet.GlassesWriter[key].Enabled)
                    {
                        Setup.SetGlasses(key);
                    }
                    else
                    {
                        Setup.SetGlasses(key, true);
                    }
                    Service.Config.Save();
                }
                ImGui.SameLine();
                if (ImGui.Button(" - ##" + key))
                {
                    Setup.SetGlasses(key, true);
                    _activeSet.GlassesWriter.Remove(key);
                    Service.Config.Save();
                }

                ImGui.SameLine();
                ImGui.Text($"#{key:D5}");

                ImGui.SameLine();
                ImGui.TextWrapped(GlassesManager.GetName(key));

                DrawSByte("Unknown70_1", "Unknown70_1", ref replace.Unknown70_1, i => i.Unknown70_1);

                DrawSByte("Unknown70_2", "Unknown70_2", ref replace.Unknown70_2, i => i.Unknown70_2);

                DrawSByte("Unknown70_3", "Unknown70_3", ref replace.Unknown70_3, i => i.Unknown70_3);

                DrawSByte("Unknown70_4", "Unknown70_4", ref replace.Unknown70_4, i => i.Unknown70_4);

                DrawSByte("Unknown70_5", "Unknown70_5", ref replace.Unknown70_5, i => i.Unknown70_5);

                DrawSByte("Unknown70_6", "Unknown70_6",ref replace.Unknown70_6, i => i.Unknown70_6);

                DrawUInt("Unknown70_7", "Unknown70_7", ref replace.Unknown70_7, i => i.Unknown70_7);

                DrawUShort("Unknown70_8", "Unknown70_8", ref replace.Unknown70_8, i => i.Unknown70_8);

                UiGlobals.DrawItemSeparator();
                continue;

                #endregion
                #region Items
                
                void DrawUShort(string refname, string text, ref ushort value,
                    Func<GlassesReplace, ushort> getDefault)
                {
                    ImGui.TextUnformatted(text);
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * CalcGlobals.GlobalScale());
                    int relay = value;
                    if (ImGui.InputInt("##" + refname + key, ref relay))
                    {
                        value = (ushort)relay;
                        Setup.SetGlasses(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{refname}{key}"))
                        {
                            value = getDefault(GlassesManager.GetOriginal(key));
                            Setup.SetGlasses(key);
                            Service.Config.Save();
                        }
                    }
                }

                void DrawSByte(string refname, string text, ref sbyte value,
                     Func<GlassesReplace, sbyte> getDefault)
                {
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * CalcGlobals.GlobalScale());
                    int relay = value;
                    if (ImGui.InputInt("##" + refname + key, ref relay))
                    {
                        value = (sbyte)relay;
                        Setup.SetGlasses(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{refname}{key}"))
                        {
                            value = getDefault(GlassesManager.GetOriginal(key));
                            Setup.SetGlasses(key);
                            Service.Config.Save();
                        }
                    }
                }

                void DrawUInt(string refname, string text, ref uint value,
                    Func<GlassesReplace, uint> getDefault)
                {
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * CalcGlobals.GlobalScale());
                    int relay = (int)value;
                    if (ImGui.DragInt("##" + refname + key, ref relay))
                    {
                        value = (uint)relay;
                        Setup.SetGlasses(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{refname}{key}"))
                        {
                            value = getDefault(GlassesManager.GetOriginal(key));
                            Setup.SetGlasses(key);
                            Service.Config.Save();
                        }
                    }
                }
            }
                            
            #endregion
            #region Search/Set

            using var searchGlasses = ImRaii.Popup(searchPopup);
            if (searchGlasses)
            {
                ImGui.SetNextItemWidth(CalcGlobals.XY());
                ImGui.InputText("##Search glasses", ref search, 256);
                var localsearch = search;

                using var popupChild = ImRaii.Child(searchPopup, CalcGlobals.SearchPopScale(), true);
                foreach (var pair in GlassesManager.Names.OrderBy(i =>
                {
                    if (string.IsNullOrEmpty(localsearch)) return 0;
                    return Math.Min(ConfigWindow.ScoreString(i.Value, localsearch),
                        ConfigWindow.ScoreString(i.Key.ToString(), localsearch));
                }))
                {
                    if (ImGui.Selectable($"#{pair.Key:D5} {pair.Value}"))
                    {
                        var original = GlassesManager.GetOriginal(pair.Key);
                        _activeSet.GlassesWriter[pair.Key] =
                            new GlassesConfig(new GlassesReplace(
                                    original.Unknown70_1,
                                    original.Unknown70_2,
                                    original.Unknown70_3,
                                    original.Unknown70_4,
                                    original.Unknown70_5,
                                    original.Unknown70_6,
                                    original.Unknown70_7,
                                    original.Unknown70_8),
                                false);
                        Service.Config.Save();
                    }
                }
            }
            #endregion
        }
    }
}
