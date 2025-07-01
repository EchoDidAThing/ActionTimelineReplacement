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
public class MountMain
{
    private static float Scale => ImGuiHelpers.GlobalScale;
    public static void Draw(string mainkey, ref Configuration.ReplacementSet _activeSet, ref string search)
    {
        var itemHeight = ImGui.CalcTextSize("").Y + ImGui.GetStyle().FramePadding.Y * 2 + ImGui.GetStyle().WindowPadding.Y;

        //to fix: scale height according to item count
        using var subList = ImRaii.Child(mainkey, new Vector2(-1, ImGui.GetWindowSize().Y - ImGui.GetCursorPosY() - itemHeight - ImGui.GetStyle().WindowPadding.Y), false);
        if (subList)
        {
            const string searchPopup = "Search mounts";
            if (ImGui.Button(" + "))
            {
                ImGui.OpenPopup(searchPopup);
            }

            foreach (var key in _activeSet.MountWriter.Keys)
            {
                var replace = _activeSet.MountWriter[key].Replacement;

                if (ImGui.Checkbox("##" + key, ref _activeSet.MountWriter[key].Enabled))
                {
                    Setup.SetMount(key);
                    Service.Config.Save();
                }
                ImGui.SameLine();

                if (ImGui.Button(" - ##" + key))
                {
                    _activeSet.MountWriter.Remove(key);
                }

                ImGui.SameLine();
                ImGui.Text($"#{key:D5}");

                ImGui.SameLine();
                ImGui.TextWrapped(MountManager.GetName(key));

                //to do: streamline this
                ImGui.TextUnformatted("Ride BGM ID");
                DrawUShort("RideBGM", ref replace.RideBGM, i => i.RideBGM);

                ImGui.TextUnformatted("Ground Tilt ID");
                DrawUShort("GroundTilt", ref replace.TiltParam1, i => i.TiltParam1);

                ImGui.TextUnformatted("Fly/Swim Tilt ID");
                DrawUShort("FlySwimTilt", ref replace.TiltParam2, i => i.TiltParam2);

                ImGui.TextUnformatted("Unknown Tilt3 ID");
                DrawUShort("Tilt3", ref replace.TiltParam3, i => i.TiltParam3);

                ImGui.TextUnformatted("Unknown Tilt4 ID");
                DrawUShort("Tilt4", ref replace.TiltParam4, i => i.TiltParam4);

                ImGui.TextUnformatted("Fly Up/Down Tilt");
                DrawUShort("FlyUpDownTilt", ref replace.Unk1, i => i.Unk1);

                ImGui.TextUnformatted("Unknown 2");
                DrawUShort("Unknown 2", ref replace.Unk2, i => i.Unk2);

                ImGui.TextUnformatted("Unknown 3");
                DrawUShort("Unknown3", ref replace.Unk3, i => i.Unk3);

                ImGui.TextUnformatted("Unknown 4");
                DrawUShort("Unknown4", ref replace.Unk4, i => i.Unk4);

                ImGui.TextUnformatted("Mount Customize ID");
                DrawUShort("MountCustomize", ref replace.MountCustomize, i => i.MountCustomize);

                ImGui.TextUnformatted("Unknown 5");
                DrawUShort("Unknown5", ref replace.Unk5, i => i.Unk5);

                ImGui.TextUnformatted("Swim Animation Speed");
                DrawUShort("SwimAnimSpeed", ref replace.Unk6, i => i.Unk6);

                ImGui.NewLine();
                ImGui.Separator();
                ImGui.NewLine();
                continue;

                #endregion
                #region Items

                void DrawUShort(string name, ref ushort value,
                    Func<MountReplace, ushort> getDefault)
                {
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * Scale);
                    int relay = value;
                    if (ImGui.InputInt("##" + name + key, ref relay))
                    {
                        value = (ushort)relay;
                        Setup.SetMount(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{name}{key}"))
                        {
                            value = getDefault(MountManager.GetOriginal(key));
                            Setup.SetMount(key);
                            Service.Config.Save();
                        }
                    }
                }
            }

            #endregion
            #region Search/Set

            using var searchMount = ImRaii.Popup(searchPopup);
            if (searchMount)
            {
                var width = 200 * Scale;
                var height = 200 * Scale;

                ImGui.SetNextItemWidth(width);
                ImGui.InputText("##Search mounts", ref search, 256);
                var localsearch = search;

                using var popUpChild = ImRaii.Child(searchPopup, new Vector2(width, height), true);
                foreach (var pair in MountManager.Names.OrderBy(i =>
                {
                    if (string.IsNullOrEmpty(localsearch)) return 0;
                    return Math.Min(ConfigWindow.ScoreString(i.Value, localsearch),
                        ConfigWindow.ScoreString(i.Key.ToString(), localsearch));
                }))
                {
                    if (ImGui.Selectable($"#{pair.Key:D5} {pair.Value}"))
                    {
                        var original = MountManager.GetOriginal(pair.Key);
                        _activeSet.MountWriter[pair.Key] =
                            new MountConfig(new MountReplace(
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
            #endregion
        }
    }
}
