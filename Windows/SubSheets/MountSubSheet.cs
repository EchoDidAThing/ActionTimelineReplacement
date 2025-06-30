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
    public class MountSubSheet
    {
        private static float Scale => ImGuiHelpers.GlobalScale;
        public static void Draw(string mainkey, ref Configuration.ReplacementSet _activeSet, ref string _searchMount)
        {
            var itemHeight = ImGui.CalcTextSize("").Y + ImGui.GetStyle().FramePadding.Y * 2 + ImGui.GetStyle().WindowPadding.Y;
            
            //to fix: scale height according to item count
            using var subList = ImRaii.Child(mainkey, new Vector2(-1, ImGui.GetWindowSize().Y - ImGui.GetCursorPosY() - itemHeight - ImGui.GetStyle().WindowPadding.Y), false);
            if (subList)
            {
                const string searchMountsPopup = "Search mounts";
                if (ImGui.Button(" + "))
                {
                    ImGui.OpenPopup(searchMountsPopup);
                }

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
                    ImGui.TextWrapped(MountReplacementsManager.GetName(key));

                    //to do: streamline this
                    ImGui.TextUnformatted("Ride BGM ID");
                    DrawItem("RideBGM", ref replacement.RideBGM, i => i.RideBGM);

                    ImGui.TextUnformatted("Ground Tilt ID");
                    DrawItem("GroundTilt", ref replacement.TiltParam1, i => i.TiltParam1);

                    ImGui.TextUnformatted("Fly/Swim Tilt ID");
                    DrawItem("FlySwimTilt", ref replacement.TiltParam2, i => i.TiltParam2);

                    ImGui.TextUnformatted("Unknown Tilt3 ID");
                    DrawItem("Tilt3", ref replacement.TiltParam3, i => i.TiltParam3);

                    ImGui.TextUnformatted("Unknown Tilt4 ID");
                    DrawItem("Tilt4", ref replacement.TiltParam4, i => i.TiltParam4);

                    ImGui.TextUnformatted("Fly Up/Down Tilt");
                    DrawItem("FlyUpDownTilt", ref replacement.Unk1, i => i.Unk1);

                    ImGui.TextUnformatted("Unknown 2");
                    DrawItem("Unknown 2", ref replacement.Unk2, i => i.Unk2);

                    ImGui.TextUnformatted("Unknown 3");
                    DrawItem("Unknown3", ref replacement.Unk3, i => i.Unk3);

                    ImGui.TextUnformatted("Unknown 4");
                    DrawItem("Unknown4", ref replacement.Unk4, i => i.Unk4);

                    ImGui.TextUnformatted("Mount Customize ID");
                    DrawItem("MountCustomize", ref replacement.MountCustomize, i => i.MountCustomize);

                    ImGui.TextUnformatted("Unknown 5");
                    DrawItem("Unknown5", ref replacement.Unk5, i => i.Unk5);

                    ImGui.TextUnformatted("Swim Animation Speed");
                    DrawItem("SwimAnimSpeed", ref replacement.Unk6, i => i.Unk6);

                    ImGui.NewLine();
                    ImGui.Separator();
                    ImGui.NewLine();
                    continue;

                    //SPECIFICSTART
                    void DrawItem(string name, ref ushort value,
                        Func<MountReplacement, ushort> getDefault)
                    {
                        ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 20);
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
                    void DrawItemByte(string name, ref byte value,
                        Func<MountReplacement, byte> getDefault)
                    {
                        ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 20);
                        ImGui.SetNextItemWidth(60 * Scale);
                        int relay = value;
                        if (ImGui.DragInt("##" + name + key, ref relay))
                        {
                            value = (byte)relay;
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
                                new MountReplacementConfig(new MountReplacement(
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
