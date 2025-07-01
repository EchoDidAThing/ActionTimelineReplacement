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
public class PlaceNameMain
{
    private static float Scale => ImGuiHelpers.GlobalScale;
    public static void Draw(string mainkey, ref Configuration.ReplacementSet _activeSet, ref string search)
    {
        var itemHeight = ImGui.CalcTextSize("").Y + ImGui.GetStyle().FramePadding.Y * 2 + ImGui.GetStyle().WindowPadding.Y;

        using var subList = ImRaii.Child(mainkey, new Vector2(-1, ImGui.GetWindowSize().Y - ImGui.GetCursorPosY() - itemHeight - ImGui.GetStyle().WindowPadding.Y), false);
        if (subList)
        {
            const string searchPopup = "Search place names";
            if (ImGui.Button(" + "))
            {
                ImGui.OpenPopup(searchPopup);
            }

            foreach (var key in _activeSet.PlaceNameWriter.Keys)
            {
                var replace = _activeSet.PlaceNameWriter[key].Replacement;

                if (ImGui.Checkbox("##" + key, ref _activeSet.PlaceNameWriter[key].Enabled))
                {
                    Setup.SetPlaceName(key);
                    Service.Config.Save();
                }
                ImGui.SameLine();
                if (ImGui.Button(" - ##" + key))
                {
                    _activeSet.PlaceNameWriter.Remove(key);
                }

                ImGui.SameLine();
                ImGui.Text($"#{key:D5}");

                ImGui.SameLine();
                ImGui.TextWrapped(PlaceNameManager.GetName(key));

                /*DrawString("Name", ref replacement.placeNameName, i => i.placeNameName);
                ImGui.SameLine();

                DrawString("Name (No Article)", ref replacement.placeNameNoArticle, i => i.placeNameNoArticle);
                ImGui.SameLine();

                DrawString("Name (Unknown)", ref replacement.placeNameUnk0, i => i.placeNameUnk0);
                ImGui.SameLine();*/

                ImGui.TextUnformatted("Unknown 1");
                DrawSByte("Unk1", ref replace.placeNameUnk1, i => i.placeNameUnk1);

                ImGui.TextUnformatted("Unknown 2");
                DrawSByte("Unk2", ref replace.placeNameUnk2, i => i.placeNameUnk2);

                ImGui.TextUnformatted("Unknown 3");
                DrawSByte("Unk3", ref replace.placeNameUnk3, i => i.placeNameUnk3);

                ImGui.TextUnformatted("Unknown 4");
                DrawSByte("Unk4", ref replace.placeNameUnk4, i => i.placeNameUnk4);

                ImGui.TextUnformatted("Unknown 5");
                DrawSByte("Unk5", ref replace.placeNameUnk5, i => i.placeNameUnk5);

                ImGui.TextUnformatted("Unknown 6");
                DrawSByte("Unk6", ref replace.placeNameUnk6, i => i.placeNameUnk6);

                ImGui.TextUnformatted("Unknown 7");
                DrawUShort("Unk7", ref replace.placeNameUnk7, i => i.placeNameUnk7);

                ImGui.TextUnformatted("Unknown 8");
                DrawByte("Unk8", ref replace.placeNameUnk8, i => i.placeNameUnk8);

                ImGui.NewLine();
                ImGui.Separator();
                ImGui.NewLine();
                continue;

                #endregion
                #region Items

                void DrawUShort(string name, ref ushort value,
                    Func<PlaceNameReplace, ushort> getDefault)
                {
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * Scale);
                    int relay = value;
                    if (ImGui.InputInt("##" + name + key, ref relay))
                    {
                        value = (ushort)relay;
                        Setup.SetPlaceName(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{name}{key}"))
                        {
                            value = getDefault(PlaceNameManager.GetOriginal(key));
                            Setup.SetPlaceName(key);
                            Service.Config.Save();
                        }
                    }
                }

                void DrawByte(string name, ref byte value,
                     Func<PlaceNameReplace, byte> getDefault)
                {
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * Scale);
                    int relay = value;
                    if (ImGui.InputInt("##" + name + key, ref relay))
                    {
                        value = (byte)relay;
                        Setup.SetPlaceName(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{name}{key}"))
                        {
                            value = getDefault(PlaceNameManager.GetOriginal(key));
                            Setup.SetPlaceName(key);
                            Service.Config.Save();
                        }
                    }
                }

                void DrawSByte(string name, ref sbyte value,
                     Func<PlaceNameReplace, sbyte> getDefault)
                {
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * Scale);
                    int relay = value;
                    if (ImGui.InputInt("##" + name + key, ref relay))
                    {
                        value = (sbyte)relay;
                        Setup.SetPlaceName(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{name}{key}"))
                        {
                            value = getDefault(PlaceNameManager.GetOriginal(key));
                            Setup.SetPlaceName(key);
                            Service.Config.Save();
                        }
                    }
                }

                /*void DrawString(string name, ref string value,
                    Func<PlaceNameReplacement, string> getDefault)
                {
                    ImGui.SetNextItemWidth(60 * Scale);
                    string relay = value;
                    if (ImGui.InputText("##" + name + key, ref relay, 260))
                    {
                        value = relay;
                        Setup.SetPlaceName(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{name}{key}"))
                        {
                            value = getDefault(PlaceNameManager.GetOriginal(key));
                            Setup.SetPlaceName(key);
                            Service.Config.Save();
                        }
                    }
                }
                */
            }

            #endregion
            #region Search/Set

            using var searchPlaceName = ImRaii.Popup(searchPopup);
            if (searchPlaceName)
            {
                var width = 200 * Scale;
                var height = 200 * Scale;

                ImGui.SetNextItemWidth(width);
                ImGui.InputText("##Search place names", ref search, 256);
                var localsearch = search;

                using var popupChild = ImRaii.Child(searchPopup, new Vector2(width, height), true);
                foreach (var pair in PlaceNameManager.Names.OrderBy(i =>
                {
                    if (string.IsNullOrEmpty(localsearch)) return 0;
                    return Math.Min(ConfigWindow.ScoreString(i.Value, localsearch),
                        ConfigWindow.ScoreString(i.Key.ToString(), localsearch));
                }))
                {
                    if (ImGui.Selectable($"#{pair.Key:D5} {pair.Value}"))
                    {
                        var original = PlaceNameManager.GetOriginal(pair.Key);
                        _activeSet.PlaceNameWriter[pair.Key] =
                            new PlaceNameConfig(new PlaceNameReplace(
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
            #endregion
        }
    }
}
