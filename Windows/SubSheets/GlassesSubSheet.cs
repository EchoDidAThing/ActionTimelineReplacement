using ActionTimelineReplacement.Configurations;
using ActionTimelineReplacement.Hookers;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface;
using System.Numerics;
using ImGuiNET;
using System;
using System.Linq;
using Dalamud.Interface.Utility;

namespace ActionTimelineReplacement.Windows.SubSheets
{
    public class GlassesSubSheet
    {
        private static float Scale => ImGuiHelpers.GlobalScale;
        public static void Draw(string mainkey, ref Configuration.ReplacementSet _activeSet, ref string _searchGlasses)
        {
            var itemHeight = ImGui.CalcTextSize("").Y + ImGui.GetStyle().FramePadding.Y * 2 + ImGui.GetStyle().WindowPadding.Y;

            //to fix: scale height according to item count
            using var subList = ImRaii.Child(mainkey, new Vector2(-1, ImGui.GetWindowSize().Y - ImGui.GetCursorPosY() - itemHeight - ImGui.GetStyle().WindowPadding.Y), false);
            if (subList)
            {
                const string searchGlassesPopup = "Search glasses";
                if (ImGui.Button(" + "))
                {
                    ImGui.OpenPopup(searchGlassesPopup);
                }

                foreach (var key in _activeSet.GlassesReplacements.Keys)
                {   //SPECIFICSTART
                    var replacement = _activeSet.GlassesReplacements[key].Replacement;

                    if (ImGui.Checkbox("##" + key, ref _activeSet.GlassesReplacements[key].Enabled))
                    {
                        Methods.SetupGlasses(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();
                    if (ImGui.Button(" - ##" + key))
                    {
                        _activeSet.GlassesReplacements.Remove(key);
                    }
                    //SPECIFICEND
                    ImGui.SameLine();
                    ImGui.Text($"#{key:D5}");

                    ImGui.SameLine();
                    ImGui.TextWrapped(GlassesReplacementsManager.GetName(key));

                    //to do: streamline this
                    ImGui.TextUnformatted("Unknown70_1");
                    DrawItemSByte("Unknown70_1", ref replacement.Unknown70_1, i => i.Unknown70_1);

                    ImGui.TextUnformatted("Unknown70_2");
                    DrawItemSByte("Unknown70_2", ref replacement.Unknown70_2, i => i.Unknown70_2);

                    ImGui.TextUnformatted("Unknown70_3");
                    DrawItemSByte("Unknown70_3", ref replacement.Unknown70_3, i => i.Unknown70_3);

                    ImGui.TextUnformatted("Unknown70_4");
                    DrawItemSByte("Unknown70_4", ref replacement.Unknown70_4, i => i.Unknown70_4);

                    ImGui.TextUnformatted("Unknown70_5");
                    DrawItemSByte("Unknown70_5", ref replacement.Unknown70_5, i => i.Unknown70_5);

                    ImGui.TextUnformatted("Unknown70_6");
                    DrawItemSByte("Unknown70_6", ref replacement.Unknown70_6, i => i.Unknown70_6);

                    ImGui.TextUnformatted("Unknown70_7");
                    DrawItemUInt("Unknown70_7", ref replacement.Unknown70_7, i => i.Unknown70_7);

                    ImGui.TextUnformatted("Unknown70_8");
                    DrawItemUShort("Unknown70_8", ref replacement.Unknown70_8, i => i.Unknown70_8);

                    ImGui.NewLine();
                    ImGui.Separator();
                    ImGui.NewLine();
                    continue;
                    //SPECIFICSTART
                    void DrawItemUShort(string name, ref ushort value,
                        Func<GlassesReplacement, ushort> getDefault)
                    {
                        ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 20);
                        ImGui.SetNextItemWidth(60 * Scale);
                        int relay = value;
                        if (ImGui.DragInt("##" + name + key, ref relay))
                        {
                            value = (ushort)relay;
                            Methods.SetupGlasses(key);
                            Service.Config.Save();
                        }

                        ImGui.SameLine();
                        using (ImRaii.PushFont(UiBuilder.IconFont))
                        {
                            if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{name}{key}"))
                            {
                                value = getDefault(GlassesReplacementsManager.GetOriginalReplacement(key));
                                Methods.SetupGlasses(key);
                                Service.Config.Save();
                            }
                        }
                    }
                    void DrawItemSByte(string name, ref sbyte value,
                         Func<GlassesReplacement, sbyte> getDefault)
                    {
                        ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 20);
                        ImGui.SetNextItemWidth(60 * Scale);
                        int relay = value;
                        if (ImGui.DragInt("##" + name + key, ref relay))
                        {
                            value = (sbyte)relay;
                            Methods.SetupGlasses(key);
                            Service.Config.Save();
                        }

                        ImGui.SameLine();
                        using (ImRaii.PushFont(UiBuilder.IconFont))
                        {
                            if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{name}{key}"))
                            {
                                value = getDefault(GlassesReplacementsManager.GetOriginalReplacement(key));
                                Methods.SetupGlasses(key);
                                Service.Config.Save();
                            }
                        }
                    }

                    void DrawItemUInt(string name, ref uint value,
                        Func<GlassesReplacement, uint> getDefault)
                    {
                        ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 20);
                        ImGui.SetNextItemWidth(60 * Scale);
                        int relay = (int)value;
                        if (ImGui.DragInt("##" + name + key, ref relay))
                        {
                            value = (uint)relay;
                            Methods.SetupGlasses(key);
                            Service.Config.Save();
                        }

                        ImGui.SameLine();
                        using (ImRaii.PushFont(UiBuilder.IconFont))
                        {
                            if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{name}{key}"))
                            {
                                value = getDefault(GlassesReplacementsManager.GetOriginalReplacement(key));
                                Methods.SetupGlasses(key);
                                Service.Config.Save();
                            }
                        }
                    }
                    //SPECIFICEND
                }
                //SPECIFICSTART
                using var searchPopup = ImRaii.Popup(searchGlassesPopup);
                if (searchPopup)
                {
                    var width = 200 * Scale;

                    ImGui.SetNextItemWidth(width);
                    ImGui.InputText("##Search glasses", ref _searchGlasses, 256);
                    var localsearch = _searchGlasses;

                    using var popUpChild = ImRaii.Child(searchGlassesPopup, new Vector2(width, 200 * Scale), true);
                    foreach (var pair in GlassesReplacementsManager.GlassesNames.OrderBy(i =>
                    {
                        if (string.IsNullOrEmpty(localsearch)) return 0;
                        return Math.Min(ConfigWindow.ScoreString(i.Value, localsearch),
                            ConfigWindow.ScoreString(i.Key.ToString(), localsearch));
                    }))
                    {
                        if (ImGui.Selectable($"#{pair.Key:D5} {pair.Value}"))
                        {
                            var original = GlassesReplacementsManager.GetOriginalReplacement(pair.Key);
                            _activeSet.GlassesReplacements[pair.Key] =
                                new GlassesReplacementConfig(new GlassesReplacement(
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
                //SPECIFICEND
            }
        }
    }
}
