using ActionTimelineReplacement.Configurations;
using ActionTimelineReplacement.Hookers;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface;
using System.Numerics;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using Dalamud.Interface.Utility;

namespace ActionTimelineReplacement.Windows.SubSheets
{
    public class TiltSubSheet
    {
        private static float Scale => ImGuiHelpers.GlobalScale;
        public static void Draw(string mainkey, ref Configuration.ReplacementSet _activeSet, ref Dictionary<string, float>  _AllItemWidths,ref string _searchTilt)
        {
            using (var subList = ImRaii.Child(mainkey + "SubList", new Vector2(-1f, (float)(_activeSet.TiltReplacements.Count() * 30) + 30), false))
            {
                if (subList)
                {
                    foreach (var key in _activeSet.TiltReplacements.Keys)
                    {   //SPECIFICSTART
                        var replacement = _activeSet.TiltReplacements[key].Replacement;

                        if (ImGui.Checkbox("##" + key, ref _activeSet.TiltReplacements[key].Enabled))
                        {
                            Methods.SetupTilt(key);
                            Service.Config.Save();
                        }
                        ImGui.SameLine();
                        if (ImGui.Button(" - ##" + key))
                        {
                            _activeSet.TiltReplacements.Remove(key);
                        }
                        //SPECIFICEND
                        ImGui.SameLine();
                        ImGui.Text($"#{key:D5}");

                        ImGui.SameLine();
                        if (_AllItemWidths[mainkey] != 0 && Service.Config.AdvancedMode)
                        {
                            var widthRest = ImGui.GetWindowWidth() - _AllItemWidths[mainkey] - ImGui.GetCursorPosX() - 5 * Scale;
                            ImGui.PushTextWrapPos(Math.Max(widthRest, 60 * Scale) + ImGui.GetCursorPosX());
                        }
                        ImGui.TextWrapped(TiltReplacementsManager.GetName(key));
                        if (_AllItemWidths[mainkey] != 0 && Service.Config.AdvancedMode)
                        {
                            ImGui.PopTextWrapPos();
                        }

                        if (!Service.Config.AdvancedMode)
                        {
                            continue;
                        }

                        ImGui.SameLine();
                        if (_AllItemWidths[mainkey] != 0)
                        {
                            ImGui.SetCursorPosX(ImGui.GetWindowWidth() - _AllItemWidths[mainkey]);
                        }

                        var startwidth = ImGui.GetCursorPosX();
                        DrawItem("TiltRate", ref replacement.Unknown0, i => i.Unknown0);
                        ImGui.SameLine();

                        DrawItemByte("RotationOriginOffset", ref replacement.Unknown1, i => i.Unknown1);

                        ImGui.SameLine();
                        DrawItemByte("MaxAngle", ref replacement.Unknown2, i => i.Unknown2);

                        ImGui.SameLine();
                        DrawItemByte("Unknown3", ref replacement.Unknown3, i => i.Unknown3);

                        ImGui.SameLine();
                        DrawItemByte("Unknown4", ref replacement.Unknown4, i => i.Unknown4);

                        ImGui.SameLine();
                        DrawItemBool("MouseReverse", ref replacement.Unknown5, i => i.Unknown5);

                        ImGui.SameLine();
                        _AllItemWidths[mainkey] = ImGui.GetCursorPosX() - startwidth;
                        ImGui.NewLine();
                        continue;
                        //SPECIFICSTART
                        void DrawItem(string name, ref ushort value,
                            Func<Configurations.TiltReplacement, ushort> getDefault)
                        {
                            ImGui.SetNextItemWidth(60 * Scale);
                            int relay = value;
                            if (ImGui.DragInt("##" + name + key, ref relay))
                            {
                                value = (ushort)relay;
                                Methods.SetupTilt(key);
                                Service.Config.Save();
                            }

                            ImGui.SameLine();
                            using (ImRaii.PushFont(UiBuilder.IconFont))
                            {
                                if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{name}{key}"))
                                {
                                    value = getDefault(TiltReplacementsManager.GetOriginalReplacement(key));
                                    Methods.SetupTilt(key);
                                    Service.Config.Save();
                                }
                            }
                        }
                        void DrawItemByte(string name, ref byte value,
                             Func<Configurations.TiltReplacement, byte> getDefault)
                        {
                            ImGui.SetNextItemWidth(60 * Scale);
                            int relay = value;
                            if (ImGui.DragInt("##" + name + key, ref relay))
                            {
                                value = (byte)relay;
                                Methods.SetupTilt(key);
                                Service.Config.Save();
                            }

                            ImGui.SameLine();
                            using (ImRaii.PushFont(UiBuilder.IconFont))
                            {
                                if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{name}{key}"))
                                {
                                    value = getDefault(TiltReplacementsManager.GetOriginalReplacement(key));
                                    Methods.SetupTilt(key);
                                    Service.Config.Save();
                                }
                            }
                        }
                        void DrawItemBool(string name, ref bool value,
                            Func<Configurations.TiltReplacement, bool> getDefault)
                        {
                            ImGui.SetNextItemWidth(60 * Scale);
                            bool relayBool = value;
                            if (ImGui.Checkbox("##" + name + key, ref relayBool))
                            {
                                value = relayBool;
                                Methods.SetupTilt(key);
                                Service.Config.Save();
                            }

                            ImGui.SameLine();
                            using (ImRaii.PushFont(UiBuilder.IconFont))
                            {
                                if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{name}{key}"))
                                {
                                    value = getDefault(TiltReplacementsManager.GetOriginalReplacement(key));
                                    Methods.SetupTilt(key);
                                    Service.Config.Save();
                                }
                            }
                        }
                        //SPECIFICEND
                    }
                    //SPECIFICSTART
                    const string searchTiltsPopup = "Search tilts";
                    if (ImGui.Button(" + "))
                    {
                        ImGui.OpenPopup(searchTiltsPopup);
                    }

                    using var searchPopup = ImRaii.Popup(searchTiltsPopup);
                    if (searchPopup)
                    {
                        var width = 200 * Scale;

                        ImGui.SetNextItemWidth(width);
                        ImGui.InputText("##Search Tilt", ref _searchTilt, 256);
                        var localsearch = _searchTilt;

                        using var popUpChild = ImRaii.Child(searchTiltsPopup, new Vector2(width, 200 * Scale), true);
                        foreach (var pair in TiltReplacementsManager.TiltNames.OrderBy(i =>
                        {
                            if (string.IsNullOrEmpty(localsearch)) return 0;
                            return Math.Min(ConfigWindow.ScoreString(i.Value, localsearch),
                                ConfigWindow.ScoreString(i.Key.ToString(), localsearch));
                        }))
                        {
                            if (ImGui.Selectable($"#{pair.Key:D5} {pair.Value}"))
                            {
                                var original = TiltReplacementsManager.GetOriginalReplacement(pair.Key);
                                _activeSet.TiltReplacements[pair.Key] =
                                    new TiltReplacementConfig(new Configurations.TiltReplacement(
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
                    //SPECIFICEND
                }
            }
        }
    }
}
