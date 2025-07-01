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
public class GlassesStyleMain
{
    private static float Scale => ImGuiHelpers.GlobalScale;
    public static void Draw(string mainkey, ref Configuration.ReplacementSet _activeSet, ref string search)
    {
        var itemHeight = ImGui.CalcTextSize("").Y + ImGui.GetStyle().FramePadding.Y * 2 + ImGui.GetStyle().WindowPadding.Y;

        //to fix: scale height according to item count
        using var subList = ImRaii.Child(mainkey, new Vector2(-1, ImGui.GetWindowSize().Y - ImGui.GetCursorPosY() - itemHeight - ImGui.GetStyle().WindowPadding.Y), false);
        if (subList)
        {
            const string searchPopup = "Search glasses styles";
            if (ImGui.Button(" + "))
            {
                ImGui.OpenPopup(searchPopup);
            }

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
                DrawShort("Unknown70_7", ref replace.Unknown70_7, i => i.Unknown70_7);

                ImGui.NewLine();
                ImGui.Separator();
                ImGui.NewLine();
                continue;

                #endregion
                #region Items
                
                void DrawShort(string name, ref short value,
                    Func<GlassesStyleReplace, short> getDefault)
                {
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * Scale);
                    int relay = value;
                    if (ImGui.InputInt("##" + name + key, ref relay))
                    {
                        value = (short)relay;
                        Setup.SetGlassesStyle(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{name}{key}"))
                        {
                            value = getDefault(GlassesStyleManager.GetOriginal(key));
                            Setup.SetGlasses(key);
                            Service.Config.Save();
                        }
                    }
                }

                void DrawSByte(string name, ref sbyte value,
                     Func<GlassesStyleReplace, sbyte> getDefault)
                {
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * Scale);
                    int relay = value;
                    if (ImGui.InputInt("##" + name + key, ref relay))
                    {
                        value = (sbyte)relay;
                        Setup.SetGlassesStyle(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{name}{key}"))
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
                var width = 200 * Scale;
                var height = 200 * Scale;

                ImGui.SetNextItemWidth(width);
                ImGui.InputText("##Search glasses styles", ref search, 256);
                var localsearch = search;

                using var popupChild = ImRaii.Child(searchPopup, new Vector2(width, height), true);
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
