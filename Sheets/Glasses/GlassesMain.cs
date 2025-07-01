using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface;
using System.Numerics;
using ImGuiNET;
using System;
using System.Linq;
using Dalamud.Interface.Utility;
using ActionTimelineReplacement.Base.Setups;
using ActionTimelineReplacement.Windows;

namespace ActionTimelineReplacement.Sheets;

#region Main
public class GlassesMain
{
    private static float Scale => ImGuiHelpers.GlobalScale;
    public static void Draw(string mainkey, ref Configuration.ReplacementSet _activeSet, ref string search)
    {
        var itemHeight = ImGui.CalcTextSize("").Y + ImGui.GetStyle().FramePadding.Y * 2 + ImGui.GetStyle().WindowPadding.Y;

        //to fix: scale height according to item count
        using var subList = ImRaii.Child(mainkey, new Vector2(-1, ImGui.GetWindowSize().Y - ImGui.GetCursorPosY() - itemHeight - ImGui.GetStyle().WindowPadding.Y), false);
        if (subList)
        {
            const string searchPopup = "Search glasses";
            if (ImGui.Button(" + "))
            {
                ImGui.OpenPopup(searchPopup);
            }

            foreach (var key in _activeSet.GlassesWriter.Keys)
            {
                var replace = _activeSet.GlassesWriter[key].Replacement;

                if (ImGui.Checkbox("##" + key, ref _activeSet.GlassesWriter[key].Enabled))
                {
                    Setup.SetGlasses(key);
                    Service.Config.Save();
                }
                ImGui.SameLine();
                if (ImGui.Button(" - ##" + key))
                {
                    _activeSet.GlassesWriter.Remove(key);
                }

                ImGui.SameLine();
                ImGui.Text($"#{key:D5}");

                ImGui.SameLine();
                ImGui.TextWrapped(GlassesManager.GetName(key));

                //to do: streamline this
                ImGui.TextUnformatted("Unknown70_1");
                DrawSByte("Unknown70_1", ref replace.Unknown70_1, i => i.Unknown70_1);

                ImGui.TextUnformatted("Unknown70_2");
                DrawSByte("Unknown70_2", ref replace.Unknown70_2, i => i.Unknown70_2);

                ImGui.TextUnformatted("Unknown70_3");
                DrawSByte("Unknown70_3", ref replace.Unknown70_3, i => i.Unknown70_3);

                ImGui.TextUnformatted("Unknown70_4");
                DrawSByte("Unknown70_4", ref replace.Unknown70_4, i => i.Unknown70_4);

                ImGui.TextUnformatted("Unknown70_5");
                DrawSByte("Unknown70_5", ref replace.Unknown70_5, i => i.Unknown70_5);

                ImGui.TextUnformatted("Unknown70_6");
                DrawSByte("Unknown70_6", ref replace.Unknown70_6, i => i.Unknown70_6);

                ImGui.TextUnformatted("Unknown70_7");
                DrawUInt("Unknown70_7", ref replace.Unknown70_7, i => i.Unknown70_7);

                ImGui.TextUnformatted("Unknown70_8");
                DrawUShort("Unknown70_8", ref replace.Unknown70_8, i => i.Unknown70_8);

                ImGui.NewLine();
                ImGui.Separator();
                ImGui.NewLine();
                continue;

                #endregion
                #region Items
                
                void DrawUShort(string name, ref ushort value,
                    Func<GlassesReplace, ushort> getDefault)
                {
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * Scale);
                    int relay = value;
                    if (ImGui.InputInt("##" + name + key, ref relay))
                    {
                        value = (ushort)relay;
                        Setup.SetGlasses(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{name}{key}"))
                        {
                            value = getDefault(GlassesManager.GetOriginal(key));
                            Setup.SetGlasses(key);
                            Service.Config.Save();
                        }
                    }
                }

                void DrawSByte(string name, ref sbyte value,
                     Func<GlassesReplace, sbyte> getDefault)
                {
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * Scale);
                    int relay = value;
                    if (ImGui.InputInt("##" + name + key, ref relay))
                    {
                        value = (sbyte)relay;
                        Setup.SetGlasses(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{name}{key}"))
                        {
                            value = getDefault(GlassesManager.GetOriginal(key));
                            Setup.SetGlasses(key);
                            Service.Config.Save();
                        }
                    }
                }

                void DrawUInt(string name, ref uint value,
                    Func<GlassesReplace, uint> getDefault)
                {
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * Scale);
                    int relay = (int)value;
                    if (ImGui.DragInt("##" + name + key, ref relay))
                    {
                        value = (uint)relay;
                        Setup.SetGlasses(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{name}{key}"))
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
                var width = 200 * Scale;
                var height = 200 * Scale;

                ImGui.SetNextItemWidth(width);
                ImGui.InputText("##Search glasses", ref search, 256);
                var localsearch = search;

                using var popupChild = ImRaii.Child(searchPopup, new Vector2(width, height), true);
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
