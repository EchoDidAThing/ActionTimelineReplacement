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
public class TiltParamMain
{
    private static float Scale => ImGuiHelpers.GlobalScale;
    public static void Draw(string mainkey, ref Configuration.ReplacementSet _activeSet, ref string search)
    {
        var itemHeight = ImGui.CalcTextSize("").Y + ImGui.GetStyle().FramePadding.Y * 2 + ImGui.GetStyle().WindowPadding.Y;

        //to fix: scale height according to item count
        using var subList = ImRaii.Child(mainkey, new Vector2(-1, ImGui.GetWindowSize().Y - ImGui.GetCursorPosY() - itemHeight - ImGui.GetStyle().WindowPadding.Y), false);

        if (subList)
        {
            const string searchPopup = "Search tilts";
            if (ImGui.Button(" + "))
            {
                ImGui.OpenPopup(searchPopup);
            }

            foreach (var key in _activeSet.TiltParamWriter.Keys)
            {
                var replace = _activeSet.TiltParamWriter[key].Replacement;

                if (ImGui.Checkbox("##" + key, ref _activeSet.TiltParamWriter[key].Enabled))
                {
                    Setup.SetTiltParam(key);
                    Service.Config.Save();
                }
                ImGui.SameLine();

                if (ImGui.Button(" - ##" + key))
                {
                    _activeSet.TiltParamWriter.Remove(key);
                }

                ImGui.SameLine();
                ImGui.Text($"#{key:D5}");

                ImGui.SameLine();
                ImGui.TextWrapped(TiltParamManager.GetName(key));

                //to do: streamline this
                ImGui.TextUnformatted("Tilt Rate");
                DrawUShort("TiltRate", ref replace.Unknown0, i => i.Unknown0);

                ImGui.TextUnformatted("Rotation Origin Offset");
                DrawByte("RotOriginOffset", ref replace.Unknown1, i => i.Unknown1);

                ImGui.TextUnformatted("Max Angle");
                DrawByte("MaxAngle", ref replace.Unknown2, i => i.Unknown2);

                ImGui.TextUnformatted("Unknown 3");
                DrawByte("Unknown 3", ref replace.Unknown3, i => i.Unknown3);

                ImGui.TextUnformatted("Unknown 4");
                DrawByte("Unknown 4", ref replace.Unknown4, i => i.Unknown4);

                ImGui.TextUnformatted("Reverse Mouse Dir");
                DrawBool("MouseReverse", ref replace.Unknown5, i => i.Unknown5);

                ImGui.NewLine();
                ImGui.Separator();
                ImGui.NewLine();
                continue;

                #endregion
                #region Items

                void DrawUShort(string name, ref ushort value,
                    Func<TiltParamReplace, ushort> getDefault)
                {
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * Scale);
                    int relay = value;
                    if (ImGui.InputInt("##" + name + key, ref relay))
                    {
                        value = (ushort)relay;
                        Setup.SetTiltParam(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{name}{key}"))
                        {
                            value = getDefault(TiltParamManager.GetOriginal(key));
                            Setup.SetTiltParam(key);
                            Service.Config.Save();
                        }
                    }
                }

                void DrawByte(string name, ref byte value,
                    Func<TiltParamReplace, byte> getDefault)
                {
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * Scale);
                    int relay = value;
                    if (ImGui.InputInt("##" + name + key, ref relay))
                    {
                        value = (byte)relay;
                        Setup.SetTiltParam(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{name}{key}"))
                        {
                            value = getDefault(TiltParamManager.GetOriginal(key));
                            Setup.SetTiltParam(key);
                            Service.Config.Save();
                        }
                    }
                }

                void DrawBool(string name, ref bool value,
                    Func<TiltParamReplace, bool> getDefault)
                {
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * Scale);
                    bool relay = value;
                    if (ImGui.Checkbox("##" + name + key, ref relay))
                    {
                        value = relay;
                        Setup.SetTiltParam(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{name}{key}"))
                        {
                            value = getDefault(TiltParamManager.GetOriginal(key));
                            Setup.SetTiltParam(key);
                            Service.Config.Save();
                        }
                    }
                }
            }

            #endregion
            #region Search/Set

            using var searchTiltParam = ImRaii.Popup(searchPopup);
            if (searchTiltParam)
            {
                var width = 200 * Scale;
                var height = 200 * Scale;

                ImGui.SetNextItemWidth(width);
                ImGui.InputText("##Search tilt parameters", ref search, 256);
                var localsearch = search;

                using var popupChild = ImRaii.Child(searchPopup, new Vector2(width, height), true);
                foreach (var pair in TiltParamManager.Names.OrderBy(i =>
                {
                    if (string.IsNullOrEmpty(localsearch)) return 0;
                    return Math.Min(ConfigWindow.ScoreString(i.Value, localsearch),
                        ConfigWindow.ScoreString(i.Key.ToString(), localsearch));
                }))
                {
                    if (ImGui.Selectable($"#{pair.Key:D5} {pair.Value}"))
                    {
                        var original = TiltParamManager.GetOriginal(pair.Key);
                        _activeSet.TiltParamWriter[pair.Key] =
                            new TiltParamConfig(new TiltParamReplace(
                                    original.Unknown0,
                                    original.Unknown1,
                                    original.Unknown2,
                                    original.Unknown3,
                                    original.Unknown4,
                                    original.Unknown5),
                                false);
                        Service.Config.Save();
                    }
                }
            }
            #endregion
        }
    }
}
