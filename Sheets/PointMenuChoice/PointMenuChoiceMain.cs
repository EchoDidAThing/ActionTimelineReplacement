/*
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface;
using System.Numerics;
using Dalamud.Bindings.ImGui;
using System;
using System.Linq;
using Dalamud.Interface.Utility;
using ActionTimelineReplacement.Base.Setups;
using ActionTimelineReplacement.Windows;

namespace ActionTimelineReplacement.Sheets;

#region Main
public class PointMenuChoiceMain
{
    private static float Scale => ImGuiHelpers.GlobalScale;
    public static void Draw(string mainkey, ref Configuration.ReplacementSet _activeSet, ref string search)
    {
        var itemHeight = ImGui.CalcTextSize("").Y + ImGui.GetStyle().FramePadding.Y * 2 + ImGui.GetStyle().WindowPadding.Y;

        //to fix: scale height according to item count
        using var subList = ImRaii.Child(mainkey, new Vector2(-1, ImGui.GetWindowSize().Y - ImGui.GetCursorPosY() - itemHeight - ImGui.GetStyle().WindowPadding.Y), false);

        if (subList)
        {
            const string searchPopup = "Search point menu choices";
            if (ImGui.Button(" + "))
            {
                ImGui.OpenPopup(searchPopup);
            }

            foreach (var key in _activeSet.PointMenuChoiceWriter.Keys)
            {
                var replace = _activeSet.PointMenuChoiceWriter[key].Replacement;

                if (ImGui.Checkbox("##" + key, ref _activeSet.PointMenuChoiceWriter[key].Enabled))
                {
                    Setup.SetPointMenuChoice(key);
                    Service.Config.Save();
                }
                ImGui.SameLine();

                if (ImGui.Button(" - ##" + key))
                {
                    _activeSet.PointMenuChoiceWriter.Remove(key);
                }

                ImGui.SameLine();
                ImGui.Text($"#{key:D5}");

                ImGui.SameLine();
                ImGui.TextWrapped(PointMenuChoiceManager.GetName(key));

                //to do: streamline this
                ImGui.TextUnformatted("Unknown 0");
                DrawFloat("Unknown0", ref replace.Unknown0, i => i.Unknown0);

                ImGui.TextUnformatted("Unknown 1");
                DrawFloat("Unknown1", ref replace.Unknown1, i => i.Unknown1);

                ImGui.TextUnformatted("Unknown 2");
                DrawFloat("Unknown2", ref replace.Unknown2, i => i.Unknown2);

                ImGui.TextUnformatted("Unknown 3");
                DrawFloat("Unknown3", ref replace.Unknown3, i => i.Unknown3);

                ImGui.TextUnformatted("Unknown 4");
                DrawFloat("Unknown4", ref replace.Unknown4, i => i.Unknown4);

                ImGui.TextUnformatted("Unknown 5");
                DrawFloat("Unknown5", ref replace.Unknown5, i => i.Unknown5);

                ImGui.TextUnformatted("Unknown 6");
                DrawUShort("Unknown6", ref replace.Unknown6, i => i.Unknown6);

                ImGui.TextUnformatted("Unknown 7");
                DrawByte("Unknown7", ref replace.Unknown7, i => i.Unknown7);
                
                ImGui.TextUnformatted("Unknown 8");
                DrawByte("Unknown8", ref replace.Unknown8, i => i.Unknown8);

                ImGui.TextUnformatted("Unknown 9");
                DrawByte("Unknown9", ref replace.Unknown9, i => i.Unknown9);

                ImGui.TextUnformatted("Unknown 10");
                DrawByte("Unknown10", ref replace.Unknown10, i => i.Unknown10);

                ImGui.TextUnformatted("Unknown 11");
                DrawByte("Unknown11", ref replace.Unknown11, i => i.Unknown11);

                ImGui.TextUnformatted("Unknown 12");
                DrawByte("Unknown12", ref replace.Unknown12, i => i.Unknown12);


                ImGui.NewLine();
                ImGui.Separator();
                ImGui.NewLine();
                continue;

                #endregion
                #region Items

                void DrawFloat(string name, ref float value,
                    Func<PointMenuChoiceReplace, float> getDefault)
                {
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * Scale);
                    float relay = value;
                    if (ImGui.InputFloat("##" + name + key, ref relay))
                    {
                        value = relay;
                        Setup.SetPointMenuChoice(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{name}{key}"))
                        {
                            value = getDefault(PointMenuChoiceManager.GetOriginal(key));
                            Setup.SetPointMenuChoice(key);
                            Service.Config.Save();
                        }
                    }
                }

                void DrawUShort(string name, ref ushort value,
                    Func<PointMenuChoiceReplace, ushort> getDefault)
                {
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * Scale);
                    int relay = value;
                    if (ImGui.InputInt("##" + name + key, ref relay))
                    {
                        value = (ushort)relay;
                        Setup.SetPointMenuChoice(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{name}{key}"))
                        {
                            value = getDefault(PointMenuChoiceManager.GetOriginal(key));
                            Setup.SetPointMenuChoice(key);
                            Service.Config.Save();
                        }
                    }
                }

                void DrawByte(string name, ref byte value,
                     Func<PointMenuChoiceReplace, byte> getDefault)
                {
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * Scale);
                    int relay = value;
                    if (ImGui.InputInt("##" + name + key, ref relay))
                    {
                        value = (byte)relay;
                        Setup.SetPointMenuChoice(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{name}{key}"))
                        {
                            value = getDefault(PointMenuChoiceManager.GetOriginal(key));
                            Setup.SetPointMenuChoice(key);
                            Service.Config.Save();
                        }
                    }
                }
            }

            #endregion
            #region Search/Set

            using var searchPointMenuChoice = ImRaii.Popup(searchPopup);
            if (searchPointMenuChoice)
            {
                var width = 200 * Scale;
                var height = 200 * Scale;

                ImGui.SetNextItemWidth(width);
                ImGui.InputText("##Search point menu choices", ref search, 256);
                var localsearch = search;

                using var popupChild = ImRaii.Child(searchPopup, new Vector2(width, height), true);
                foreach (var pair in PointMenuChoiceManager.Names.OrderBy(i =>
                {
                    if (string.IsNullOrEmpty(localsearch)) return 0;
                    return Math.Min(ConfigWindow.ScoreString(i.Value, localsearch),
                        ConfigWindow.ScoreString(i.Key.ToString(), localsearch));
                }))
                {
                    if (ImGui.Selectable($"#{pair.Key:D5} {pair.Value}"))
                    {
                        var original = PointMenuChoiceManager.GetOriginal(pair.Key);
                        _activeSet.PointMenuChoiceWriter[pair.Key] =
                            new PointMenuChoiceConfig(new PointMenuChoiceReplace(
                                    original.Unknown0,
                                    original.Unknown1,
                                    original.Unknown2,
                                    original.Unknown3,
                                    original.Unknown4,
                                    original.Unknown5,
                                    original.Unknown6,
                                    original.Unknown7,
                                    original.Unknown8,
                                    original.Unknown9,
                                    original.Unknown10,
                                    original.Unknown11,
                                    original.Unknown12),
                                false);
                        Service.Config.Save();
                    }
                }
            }
            #endregion
        }
    }
}
*/