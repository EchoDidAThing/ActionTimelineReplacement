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
public class PlaceNameMain
{
    public static void Draw(string mainkey, ref Configuration.ReplacementSet _activeSet, ref string search)
    {
        using var subList = ImRaii.Child(mainkey, CalcGlobals.BodyScale(), false);
        if (subList)
        {
            const string searchPopup = "Search place names";
            UiGlobals.DrawAddItem(searchPopup);

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

                DrawSByte("Unk1", "Unknown 1", ref replace.placeNameUnk1, i => i.placeNameUnk1);

                DrawSByte("Unk2", "Unknown 2", ref replace.placeNameUnk2, i => i.placeNameUnk2);

                DrawSByte("Unk3", "Unknown 3", ref replace.placeNameUnk3, i => i.placeNameUnk3);

                DrawSByte("Unk4", "Unknown 4", ref replace.placeNameUnk4, i => i.placeNameUnk4);

                DrawSByte("Unk5", "Unknown 5", ref replace.placeNameUnk5, i => i.placeNameUnk5);

                DrawSByte("Unk6", "Unknown 6", ref replace.placeNameUnk6, i => i.placeNameUnk6);

                DrawUShort("Unk7", "Unknown 7", ref replace.placeNameUnk7, i => i.placeNameUnk7);

                DrawByte("Unk8", "Unknown 8", ref replace.placeNameUnk8, i => i.placeNameUnk8);

                UiGlobals.DrawItemSeparator();
                continue;

                #endregion
                #region Items

                void DrawUShort(string refname, string text, ref ushort value,
                    Func<PlaceNameReplace, ushort> getDefault)
                {
                    ImGui.TextUnformatted(text);
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * CalcGlobals.GlobalScale());
                    int relay = value;
                    if (ImGui.InputInt("##" + refname + key, ref relay))
                    {
                        value = (ushort)relay;
                        Setup.SetPlaceName(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{refname}{key}"))
                        {
                            value = getDefault(PlaceNameManager.GetOriginal(key));
                            Setup.SetPlaceName(key);
                            Service.Config.Save();
                        }
                    }
                }

                void DrawByte(string refname, string text, ref byte value,
                    Func<PlaceNameReplace, byte> getDefault)
                {
                    ImGui.TextUnformatted(text);
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * CalcGlobals.GlobalScale());
                    int relay = value;
                    if (ImGui.InputInt("##" + refname + key, ref relay))
                    {
                        value = (byte)relay;
                        Setup.SetPlaceName(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{refname}{key}"))
                        {
                            value = getDefault(PlaceNameManager.GetOriginal(key));
                            Setup.SetPlaceName(key);
                            Service.Config.Save();
                        }
                    }
                }

                void DrawSByte(string refname, string text, ref sbyte value,
                    Func<PlaceNameReplace, sbyte> getDefault)
                {
                    ImGui.TextUnformatted(text);
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * CalcGlobals.GlobalScale());
                    int relay = value;
                    if (ImGui.InputInt("##" + refname + key, ref relay))
                    {
                        value = (sbyte)relay;
                        Setup.SetPlaceName(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{refname}{key}"))
                        {
                            value = getDefault(PlaceNameManager.GetOriginal(key));
                            Setup.SetPlaceName(key);
                            Service.Config.Save();
                        }
                    }
                }

                /*
                void DrawString(string name, ref string value,
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
                ImGui.SetNextItemWidth(CalcGlobals.XY());
                ImGui.InputText("##Search place names", ref search, 256);
                var localsearch = search;

                using var popupChild = ImRaii.Child(searchPopup, CalcGlobals.SearchPopScale(), true);
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
