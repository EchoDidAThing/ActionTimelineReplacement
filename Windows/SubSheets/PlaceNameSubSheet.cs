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
    public class PlaceNameSubSheet
    {
        private static float Scale => ImGuiHelpers.GlobalScale;
        public static void Draw(string mainkey, ref Configuration.ReplacementSet _activeSet, ref string _searchPlaceName)
        {
            var itemHeight = ImGui.CalcTextSize("").Y + ImGui.GetStyle().FramePadding.Y * 2 + ImGui.GetStyle().WindowPadding.Y;

            using var subList = ImRaii.Child(mainkey, new Vector2(-1, ImGui.GetWindowSize().Y - ImGui.GetCursorPosY() - itemHeight - ImGui.GetStyle().WindowPadding.Y), false);
            if (subList)
            {
                const string searchPlaceNamesPopup = "Search place names";
                if (ImGui.Button(" + "))
                {
                    ImGui.OpenPopup(searchPlaceNamesPopup);
                }

                foreach (var key in _activeSet.PlaceNameReplacements.Keys)
                {   //SPECIFICSTART
                    var replacement = _activeSet.PlaceNameReplacements[key].Replacement;

                    if (ImGui.Checkbox("##" + key, ref _activeSet.PlaceNameReplacements[key].Enabled))
                    {
                        Methods.SetupPlaceName(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();
                    if (ImGui.Button(" - ##" + key))
                    {
                        _activeSet.PlaceNameReplacements.Remove(key);
                    }
                    //SPECIFICEND
                    ImGui.SameLine();
                    ImGui.Text($"#{key:D5}");

                    ImGui.SameLine();
                    ImGui.TextWrapped(PlaceNameReplacementsManager.GetName(key));

                    /*DrawItemString("Name", ref replacement.placeNameName, i => i.placeNameName);
                    ImGui.SameLine();

                    DrawItemString("Name (No Article)", ref replacement.placeNameNoArticle, i => i.placeNameNoArticle);
                    ImGui.SameLine();

                    DrawItemString("Name (Unknown)", ref replacement.placeNameUnk0, i => i.placeNameUnk0);
                    ImGui.SameLine();*/

                    ImGui.TextUnformatted("Unknown 1");
                    DrawItemSByte("Unk1", ref replacement.placeNameUnk1, i => i.placeNameUnk1);

                    ImGui.TextUnformatted("Unknown 2");
                    DrawItemSByte("Unk2", ref replacement.placeNameUnk2, i => i.placeNameUnk2);

                    ImGui.TextUnformatted("Unknown 3");
                    DrawItemSByte("Unk3", ref replacement.placeNameUnk3, i => i.placeNameUnk3);

                    ImGui.TextUnformatted("Unknown 4");
                    DrawItemSByte("Unk4", ref replacement.placeNameUnk4, i => i.placeNameUnk4);

                    ImGui.TextUnformatted("Unknown 5");
                    DrawItemSByte("Unk5", ref replacement.placeNameUnk5, i => i.placeNameUnk5);

                    ImGui.TextUnformatted("Unknown 6");
                    DrawItemSByte("Unk6", ref replacement.placeNameUnk6, i => i.placeNameUnk6);

                    ImGui.TextUnformatted("Unknown 7");
                    DrawItem("Unk7", ref replacement.placeNameUnk7, i => i.placeNameUnk7);

                    ImGui.TextUnformatted("Unknown 8");
                    DrawItemByte("Unk8", ref replacement.placeNameUnk8, i => i.placeNameUnk8);

                    ImGui.NewLine();
                    ImGui.Separator();
                    ImGui.NewLine();
                    continue;
                    //SPECIFICSTART
                    void DrawItem(string name, ref ushort value,
                        Func<PlaceNameReplacement, ushort> getDefault)
                    {
                        ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 20);
                        ImGui.SetNextItemWidth(60 * Scale);
                        int relay = value;
                        if (ImGui.DragInt("##" + name + key, ref relay))
                        {
                            value = (ushort)relay;
                            Methods.SetupPlaceName(key);
                            Service.Config.Save();
                        }

                        ImGui.SameLine();
                        using (ImRaii.PushFont(UiBuilder.IconFont))
                        {
                            if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{name}{key}"))
                            {
                                value = getDefault(PlaceNameReplacementsManager.GetOriginalReplacement(key));
                                Methods.SetupPlaceName(key);
                                Service.Config.Save();
                            }
                        }
                    }
                    void DrawItemByte(string name, ref byte value,
                         Func<PlaceNameReplacement, byte> getDefault)
                    {
                        ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 20);
                        ImGui.SetNextItemWidth(60 * Scale);
                        int relay = value;
                        if (ImGui.DragInt("##" + name + key, ref relay))
                        {
                            value = (byte)relay;
                            Methods.SetupPlaceName(key);
                            Service.Config.Save();
                        }

                        ImGui.SameLine();
                        using (ImRaii.PushFont(UiBuilder.IconFont))
                        {
                            if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{name}{key}"))
                            {
                                value = getDefault(PlaceNameReplacementsManager.GetOriginalReplacement(key));
                                Methods.SetupPlaceName(key);
                                Service.Config.Save();
                            }
                        }
                    }
                    void DrawItemSByte(string name, ref sbyte value,
                         Func<PlaceNameReplacement, sbyte> getDefault)
                    {
                        ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 20);
                        ImGui.SetNextItemWidth(60 * Scale);
                        int relay = value;
                        if (ImGui.DragInt("##" + name + key, ref relay))
                        {
                            value = (sbyte)relay;
                            Methods.SetupPlaceName(key);
                            Service.Config.Save();
                        }

                        ImGui.SameLine();
                        using (ImRaii.PushFont(UiBuilder.IconFont))
                        {
                            if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{name}{key}"))
                            {
                                value = getDefault(PlaceNameReplacementsManager.GetOriginalReplacement(key));
                                Methods.SetupPlaceName(key);
                                Service.Config.Save();
                            }
                        }
                    }
                    /*void DrawItemString(string name, ref string value,
                        Func<PlaceNameReplacement, string> getDefault)
                    {
                        ImGui.SetNextItemWidth(60 * Scale);
                        string relayString = value;
                        if (ImGui.InputText("##" + name + key, ref relayString, 260))
                        {
                            value = relayString;
                            Methods.SetupPlaceName(key);
                            Service.Config.Save();
                        }

                        ImGui.SameLine();
                        using (ImRaii.PushFont(UiBuilder.IconFont))
                        {
                            if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{name}{key}"))
                            {
                                value = getDefault(PlaceNameReplacementsManager.GetOriginalReplacement(key));
                                Methods.SetupPlaceName(key);
                                Service.Config.Save();
                            }
                        }
                    }*/
                    //SPECIFICEND
                }
                //SPECIFICSTART

                using var searchPopup = ImRaii.Popup(searchPlaceNamesPopup);
                if (searchPopup)
                {
                    var width = 200 * Scale;

                    ImGui.SetNextItemWidth(width);
                    ImGui.InputText("##Search Place Name", ref _searchPlaceName, 256);
                    var localsearch = _searchPlaceName;

                    using var popUpChild = ImRaii.Child(searchPlaceNamesPopup, new Vector2(width, 200 * Scale), true);
                    foreach (var pair in PlaceNameReplacementsManager.PlaceNameNames.OrderBy(i =>
                    {
                        if (string.IsNullOrEmpty(localsearch)) return 0;
                        return Math.Min(ConfigWindow.ScoreString(i.Value, localsearch),
                            ConfigWindow.ScoreString(i.Key.ToString(), localsearch));
                    }))
                    {
                        if (ImGui.Selectable($"#{pair.Key:D5} {pair.Value}"))
                        {
                            var original = PlaceNameReplacementsManager.GetOriginalReplacement(pair.Key);
                            _activeSet.PlaceNameReplacements[pair.Key] =
                                new PlaceNameReplacementConfig(new PlaceNameReplacement(
                                        //original.placeNameName,
                                        //original.placeNameNoArticle,
                                        //original.placeNameUnk0,
                                        original.placeNameUnk1,
                                        original.placeNameUnk2,
                                        original.placeNameUnk3,
                                        original.placeNameUnk4,
                                        original.placeNameUnk5,
                                        original.placeNameUnk6,
                                        original.placeNameUnk7,
                                        original.placeNameUnk8
                                        ),
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
