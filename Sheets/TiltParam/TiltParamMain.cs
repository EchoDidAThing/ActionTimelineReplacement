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
public class TiltParamMain
{
    public static void Draw(string mainkey, ref Configuration.ReplacementSet _activeSet, ref string search)
    {
        using var subList = ImRaii.Child(mainkey, CalcGlobals.BodyScale(), false);
        if (subList)
        {
            const string searchPopup = "Search tilts";
            UiGlobals.DrawAddItem(searchPopup);

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

                DrawUShort("TiltRate", "Tilt Rate", ref replace.Unknown0, i => i.Unknown0);

                DrawByte("RotOriginOffset", "Rotation Origin Offset", ref replace.Unknown1, i => i.Unknown1);

                DrawByte("MaxAngle", "Max Angle", ref replace.Unknown2, i => i.Unknown2);

                DrawByte("Unknown3", "Unknown 3", ref replace.Unknown3, i => i.Unknown3);

                DrawByte("Unknown4", "Unknown 4", ref replace.Unknown4, i => i.Unknown4);

                DrawBool("MouseReverse", "Reverse Mouse Direction", ref replace.Unknown5, i => i.Unknown5);

                UiGlobals.DrawItemSeparator();
                continue;

                #endregion
                #region Items

                void DrawUShort(string refname, string text, ref ushort value,
                    Func<TiltParamReplace, ushort> getDefault)
                {
                    ImGui.TextUnformatted(text);
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * CalcGlobals.GlobalScale());
                    int relay = value;
                    if (ImGui.InputInt("##" + refname + key, ref relay))
                    {
                        value = (ushort)relay;
                        Setup.SetTiltParam(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{refname}{key}"))
                        {
                            value = getDefault(TiltParamManager.GetOriginal(key));
                            Setup.SetTiltParam(key);
                            Service.Config.Save();
                        }
                    }
                }

                void DrawByte(string refname, string text, ref byte value,
                    Func<TiltParamReplace, byte> getDefault)
                {
                    ImGui.TextUnformatted(text);
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * CalcGlobals.GlobalScale());
                    int relay = value;
                    if (ImGui.InputInt("##" + refname + key, ref relay))
                    {
                        value = (byte)relay;
                        Setup.SetTiltParam(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{refname}{key}"))
                        {
                            value = getDefault(TiltParamManager.GetOriginal(key));
                            Setup.SetTiltParam(key);
                            Service.Config.Save();
                        }
                    }
                }

                void DrawBool(string refname, string text, ref bool value,
                    Func<TiltParamReplace, bool> getDefault)
                {
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * CalcGlobals.GlobalScale());
                    bool relay = value;
                    if (ImGui.Checkbox("##" + refname + key, ref relay))
                    {
                        value = relay;
                        Setup.SetTiltParam(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{refname}{key}"))
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
                ImGui.SetNextItemWidth(CalcGlobals.XY());
                ImGui.InputText("##Search tilt parameters", ref search, 256);
                var localsearch = search;

                using var popupChild = ImRaii.Child(searchPopup, CalcGlobals.SearchPopScale(), true);
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
