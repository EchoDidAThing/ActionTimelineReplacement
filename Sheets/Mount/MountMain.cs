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
public class MountMain
{
    public static void Draw(string mainkey, ref Configuration.ReplacementSet _activeSet, ref string search)
    {
        using var subList = ImRaii.Child(mainkey, CalcGlobals.BodyScale(), false);
        if (subList)
        {
            const string searchPopup = "Search mounts";
            UiGlobals.DrawAddItem(searchPopup);

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

                DrawUShort("RideBGM", "Ride BGM ID", ref replace.RideBGM, i => i.RideBGM);

                DrawUShort("GroundTilt", "Ground Tilt ID", ref replace.TiltParam1, i => i.TiltParam1);

                DrawUShort("FlySwimTilt", "Fly/Swim Tilt ID", ref replace.TiltParam2, i => i.TiltParam2);

                DrawUShort("Tilt3", "Unknown Tilt3 ID", ref replace.TiltParam3, i => i.TiltParam3);

                DrawUShort("Tilt4", "Unknown Tilt4 ID", ref replace.TiltParam4, i => i.TiltParam4);

                DrawUShort("FlyUpDownTilt", "Fly Up/Down Tilt", ref replace.FlyUpDownTilt, i => i.FlyUpDownTilt);

                DrawUShort("Unknown6", "Unknown 6", ref replace.Unknown6, i => i.Unknown6);

                DrawUShort("Unknown7", "Unknown 7", ref replace.Unknown7, i => i.Unknown7);

                DrawUShort("Unknown8", "Unknown 8", ref replace.Unknown8, i => i.Unknown8);

                DrawUShort("MountCustomize", "Mount Customize ID", ref replace.MountCustomize, i => i.MountCustomize);

                DrawUShort("Unknown9", "Unknown 9", ref replace.Unknown9, i => i.Unknown9);

                DrawUShort("SwimAnimSpeed", "Swim Animation Speed", ref replace.SwimAnimSpeed, i => i.SwimAnimSpeed);

                //DrawByte("MountBoolSet1", "Mount Bools 1 [raw]", ref replace.MountBoolSet1, i => i.MountBoolSet1);

                UiGlobals.DrawItemSeparator();
                continue;

                #endregion
                #region Items

                //to do: streamline items
                void DrawUShort(string refname, string text, ref ushort value,
                    Func<MountReplace, ushort> getDefault)
                {
                    ImGui.TextUnformatted(text);
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * CalcGlobals.GlobalScale());
                    int relay = value;
                    if (ImGui.InputInt("##" + refname + key, ref relay))
                    {
                        value = (ushort)relay;
                        Setup.SetMount(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{refname}{key}"))
                        {
                            value = getDefault(MountManager.GetOriginal(key));
                            Setup.SetMount(key);
                            Service.Config.Save();
                        }
                    }
                }

                void DrawByte(string refname, string text, ref byte value,
                    Func<MountReplace, byte> getDefault)
                {
                    ImGui.TextUnformatted(text);
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * CalcGlobals.GlobalScale());
                    int relay = value;
                    if (ImGui.InputInt("##" + refname + key, ref relay))
                    {
                        value = (byte)relay;
                        Setup.SetMount(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{refname}{key}"))
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
                ImGui.SetNextItemWidth(CalcGlobals.XY());
                ImGui.InputText("##Search mounts", ref search, 256);
                var localsearch = search;

                using var popupChild = ImRaii.Child(searchPopup, CalcGlobals.SearchPopScale(), true);
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
                                    original.FlyUpDownTilt,
                                    original.Unknown6,
                                    original.Unknown7,
                                    original.Unknown8,
                                    original.MountCustomize,
                                    original.Unknown9,
                                    original.SwimAnimSpeed),
                                false);
                        Service.Config.Save();
                    }
                }
            }
            #endregion
        }
    }
}
