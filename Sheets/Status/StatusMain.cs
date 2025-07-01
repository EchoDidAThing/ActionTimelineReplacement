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
public class StatusMain
{
    private static float Scale => ImGuiHelpers.GlobalScale;
    public static void Draw(string mainkey, ref Configuration.ReplacementSet _activeSet, ref string search)
    {
        var itemHeight = ImGui.CalcTextSize("").Y + ImGui.GetStyle().FramePadding.Y * 2 + ImGui.GetStyle().WindowPadding.Y;

        //to fix: scale height according to item count
        using var subList = ImRaii.Child(mainkey, new Vector2(-1, ImGui.GetWindowSize().Y - ImGui.GetCursorPosY() - itemHeight - ImGui.GetStyle().WindowPadding.Y), false);

        if (subList)
        {
            const string searchPopup = "Search statuses";
            if (ImGui.Button(" + "))
            {
                ImGui.OpenPopup(searchPopup);
            }

            foreach (var key in _activeSet.StatusWriter.Keys)
            {
                var replace = _activeSet.StatusWriter[key].Replacement;

                if (ImGui.Checkbox("##" + key, ref _activeSet.StatusWriter[key].Enabled))
                {
                    Setup.SetStatus(key);
                    Service.Config.Save();
                }
                ImGui.SameLine();

                if (ImGui.Button(" - ##" + key))
                {
                    _activeSet.StatusWriter.Remove(key);
                }

                ImGui.SameLine();
                ImGui.Text($"#{key:D5}");

                ImGui.SameLine();
                ImGui.TextWrapped(StatusManager.GetName(key));

                //to do: streamline this
                //to do: show loop vfx and hit effect as strings
                ImGui.TextUnformatted("Parameter Modifier");
                DrawInt("ParamModifier", ref replace.ParamModifier, i => i.ParamModifier);

                ImGui.TextUnformatted("Status Loop VFX ID");
                DrawUShort("StatusLoopVFX", ref replace.StatusLoopVFX, i => i.StatusLoopVFX);

                ImGui.TextUnformatted("Unknown 0");
                DrawByte("Unknown0", ref replace.Unknown0, i => i.Unknown0);

                ImGui.TextUnformatted("Status Category");
                DrawByte("StatusCategory", ref replace.StatusCategory, i => i.StatusCategory);

                ImGui.TextUnformatted("Status Hit Effect ID");
                DrawByte("StatusHitEffect", ref replace.StatusHitEffect, i => i.StatusHitEffect);

                ImGui.TextUnformatted("Target Type");
                DrawByte("TargetType", ref replace.TargetType, i => i.TargetType);

                ImGui.TextUnformatted("Flag 1");
                DrawByte("Flags", ref replace.Flags, i => i.Flags);

                ImGui.TextUnformatted("Flag 2");
                DrawByte("Flag2", ref replace.Flag2, i => i.Flag2);

                ImGui.TextUnformatted("Unknown 70_1");
                DrawByte("Unknown_70_1", ref replace.Unknown_70_1, i => i.Unknown_70_1);

                ImGui.TextUnformatted("Show Attack Type");
                DrawSByte("AtkType", ref replace.Unknown2, i => i.Unknown2);

                ImGui.NewLine();
                ImGui.Separator();
                ImGui.NewLine();
                continue;

                #endregion
                #region Items

                void DrawInt(string name, ref int value,
                    Func<StatusReplace, int> getDefault)
                {
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * Scale);
                    int relay = value;
                    if (ImGui.InputInt("##" + name + key, ref relay))
                    {
                        value = relay;
                        Setup.SetStatus(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{name}{key}"))
                        {
                            value = getDefault(StatusManager.GetOriginal(key));
                            Setup.SetStatus(key);
                            Service.Config.Save();
                        }
                    }
                }

                void DrawUShort(string name, ref ushort value,
                    Func<StatusReplace, ushort> getDefault)
                {
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * Scale);
                    int relay = value;
                    if (ImGui.InputInt("##" + name + key, ref relay))
                    {
                        value = (ushort)relay;
                        Setup.SetStatus(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{name}{key}"))
                        {
                            value = getDefault(StatusManager.GetOriginal(key));
                            Setup.SetStatus(key);
                            Service.Config.Save();
                        }
                    }
                }

                void DrawByte(string name, ref byte value,
                     Func<StatusReplace, byte> getDefault)
                {
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * Scale);
                    int relay = value;
                    if (ImGui.InputInt("##" + name + key, ref relay))
                    {
                        value = (byte)relay;
                        Setup.SetStatus(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{name}{key}"))
                        {
                            value = getDefault(StatusManager.GetOriginal(key));
                            Setup.SetStatus(key);
                            Service.Config.Save();
                        }
                    }
                }

                void DrawSByte(string name, ref sbyte value,
                     Func<StatusReplace, sbyte> getDefault)
                {
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * Scale);
                    int relay = value;
                    if (ImGui.InputInt("##" + name + key, ref relay))
                    {
                        value = (sbyte)relay;
                        Setup.SetStatus(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{name}{key}"))
                        {
                            value = getDefault(StatusManager.GetOriginal(key));
                            Setup.SetStatus(key);
                            Service.Config.Save();
                        }
                    }
                }
            }

            #endregion
            #region Search/Set

            using var searchStatus = ImRaii.Popup(searchPopup);
            if (searchStatus)
            {
                var width = 200 * Scale;
                var height = 200 * Scale;

                ImGui.SetNextItemWidth(width);
                ImGui.InputText("##Search statuses", ref search, 256);
                var localsearch = search;

                using var popupChild = ImRaii.Child(searchPopup, new Vector2(width, height), true);
                foreach (var pair in StatusManager.Names.OrderBy(i =>
                {
                    if (string.IsNullOrEmpty(localsearch)) return 0;
                    return Math.Min(ConfigWindow.ScoreString(i.Value, localsearch),
                        ConfigWindow.ScoreString(i.Key.ToString(), localsearch));
                }))
                {
                    if (ImGui.Selectable($"#{pair.Key:D5} {pair.Value}"))
                    {
                        var original = StatusManager.GetOriginal(pair.Key);
                        _activeSet.StatusWriter[pair.Key] =
                            new StatusConfig(new StatusReplace(
                                    original.ParamModifier,
                                    original.StatusLoopVFX,
                                    original.Unknown0,
                                    original.StatusCategory,
                                    original.StatusHitEffect,
                                    original.TargetType,
                                    original.Flags,
                                    original.Flag2,
                                    original.Unknown_70_1,
                                    original.Unknown2),
                                false);
                        Service.Config.Save();
                    }
                }
            }
            #endregion
        }
    }
}
