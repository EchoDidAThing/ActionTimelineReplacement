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
    public class MountSubSheet
    {
        private static float Scale => ImGuiHelpers.GlobalScale;
        public static void Draw(string mainkey, ref Configuration.ReplacementSet _activeSet, ref Dictionary<string, float>  _AllItemWidths,ref string _searchMount)
        {
            using (var subList = ImRaii.Child(mainkey + "SubList", new Vector2(-1f, (float)(_activeSet.MountReplacements.Count() * 30) + 30), false))
            {
                if (subList)
                {
                    foreach (var key in _activeSet.MountReplacements.Keys)
                    {   //SPECIFICSTART
                        var replacement = _activeSet.MountReplacements[key].Replacement;

                        if (ImGui.Checkbox("##" + key, ref _activeSet.MountReplacements[key].Enabled))
                        {
                            Methods.SetupMount(key);
                            Service.Config.Save();
                        }
                        ImGui.SameLine();
                        if (ImGui.Button(" - ##" + key))
                        {
                            _activeSet.MountReplacements.Remove(key);
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
                        ImGui.TextWrapped(MountReplacementsManager.GetName(key));
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
                        DrawItem("RideBGM", ref replacement.RideBGM, i => i.RideBGM);
                        ImGui.SameLine();

                        DrawItem("TiltGround", ref replacement.TiltParam1, i => i.TiltParam1);

                        ImGui.SameLine();
                        DrawItem("TiltFlySwim", ref replacement.TiltParam2, i => i.TiltParam2);

                        ImGui.SameLine();
                        DrawItem("Tilt3", ref replacement.TiltParam3, i => i.TiltParam3);

                        ImGui.SameLine();
                        DrawItem("Tilt4", ref replacement.TiltParam4, i => i.TiltParam4);

                        ImGui.SameLine();
                        DrawItem("FlyUpDownTilt", ref replacement.Unk1, i => i.Unk1);

                        ImGui.SameLine();
                        DrawItem("Unk2", ref replacement.Unk2, i => i.Unk2);

                        ImGui.SameLine();
                        DrawItem("Unk3", ref replacement.Unk3, i => i.Unk3);

                        ImGui.SameLine();
                        DrawItem("Unk4", ref replacement.Unk4, i => i.Unk4);

                        ImGui.SameLine();
                        DrawItem("MountCustomize", ref replacement.MountCustomize, i => i.MountCustomize);

                        ImGui.SameLine();
                        DrawItem("Unk5", ref replacement.Unk5, i => i.Unk5);

                        ImGui.SameLine();
                        DrawItem("SwimAnimSpeed", ref replacement.Unk6, i => i.Unk6);

                        ImGui.SameLine();
                        _AllItemWidths[mainkey] = ImGui.GetCursorPosX() - startwidth;
                        ImGui.NewLine();
                        continue;

                        //SPECIFICSTART
                        void DrawItem(string name, ref ushort value,
                            Func<Configurations.MountReplacement, ushort> getDefault)
                        {
                            ImGui.SetNextItemWidth(60 * Scale);
                            int relay = value;
                            if (ImGui.DragInt("##" + name + key, ref relay))
                            {
                                value = (ushort)relay;
                                Methods.SetupMount(key);
                                Service.Config.Save();
                            }

                            ImGui.SameLine();
                            using (ImRaii.PushFont(UiBuilder.IconFont))
                            {
                                if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{name}{key}"))
                                {
                                    value = getDefault(MountReplacementsManager.GetOriginalReplacement(key));
                                    Methods.SetupMount(key);
                                    Service.Config.Save();
                                }
                            }
                        }
                        //SPECIFICEND
                    }
                    //SPECIFICSTART
                    const string searchMountsPopup = "Search mounts";
                    if (ImGui.Button(" + "))
                    {
                        ImGui.OpenPopup(searchMountsPopup);
                    }

                    using var searchPopup = ImRaii.Popup(searchMountsPopup);
                    if (searchPopup)
                    {
                        var width = 200 * Scale;

                        ImGui.SetNextItemWidth(width);
                        ImGui.InputText("##Search Mount", ref _searchMount, 256);
                        var localsearch = _searchMount;

                        using var popUpChild = ImRaii.Child(searchMountsPopup, new Vector2(width, 200 * Scale), true);
                        foreach (var pair in MountReplacementsManager.MountNames.OrderBy(i =>
                        {
                            if (string.IsNullOrEmpty(localsearch)) return 0;
                            return Math.Min(ConfigWindow.ScoreString(i.Value, localsearch),
                                ConfigWindow.ScoreString(i.Key.ToString(), localsearch));
                        }))
                        {
                            if (ImGui.Selectable($"#{pair.Key:D5} {pair.Value}"))
                            {
                                var original = MountReplacementsManager.GetOriginalReplacement(pair.Key);
                                _activeSet.MountReplacements[pair.Key] =
                                    new MountReplacementConfig(new Configurations.MountReplacement(
                                            original.RideBGM,
                                            original.TiltParam1,
                                            original.TiltParam2,
                                            original.TiltParam3,
                                            original.TiltParam4,
                                            original.Unk1,
                                            original.Unk2,
                                            original.Unk3,
                                            original.Unk4,
                                            original.MountCustomize,
                                            original.Unk5,
                                            original.Unk6),
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
