using ActionTimelineReplacement.Configurations;
using ActionTimelineReplacement.Hookers;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface;
using System.Numerics;
using ImGuiNET;
using System;
using System.Linq;
using Dalamud.Interface.Utility;
using Lumina.Excel.Sheets;

namespace ActionTimelineReplacement.Windows.SubSheets
{
    public class StatusSubSheet
    {
        private static float Scale => ImGuiHelpers.GlobalScale;
        public static void Draw(string mainkey, ref Configuration.ReplacementSet _activeSet, ref string _searchStatus)
        {
            var itemHeight = ImGui.CalcTextSize("").Y + ImGui.GetStyle().FramePadding.Y * 2 + ImGui.GetStyle().WindowPadding.Y;

            //to fix: scale height according to item count
            using var subList = ImRaii.Child(mainkey, new Vector2(-1, ImGui.GetWindowSize().Y - ImGui.GetCursorPosY() - itemHeight - ImGui.GetStyle().WindowPadding.Y), false);
            if (subList)
            {
                const string searchStatusPopup = "Search statuses";
                if (ImGui.Button(" + "))
                {
                    ImGui.OpenPopup(searchStatusPopup);
                }

                foreach (var key in _activeSet.StatusReplacements.Keys)
                {   //SPECIFICSTART
                    var replacement = _activeSet.StatusReplacements[key].Replacement;

                    if (ImGui.Checkbox("##" + key, ref _activeSet.StatusReplacements[key].Enabled))
                    {
                        Methods.SetupStatus(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();
                    if (ImGui.Button(" - ##" + key))
                    {
                        _activeSet.StatusReplacements.Remove(key);
                    }
                    //SPECIFICEND
                    ImGui.SameLine();
                    ImGui.Text($"#{key:D5}");

                    ImGui.SameLine();
                    ImGui.TextWrapped(StatusReplacementsManager.GetName(key));

                    //to do: streamline this
                    //to do: show loop vfx and hit effect as strings
                    ImGui.TextUnformatted("Parameter Modifier");
                    DrawItemInt("ParamModifier", ref replacement.ParamModifier, i => i.ParamModifier);

                    ImGui.TextUnformatted("Status Loop VFX ID");
                    DrawItemUShort("StatusLoopVFX", ref replacement.StatusLoopVFX, i => i.StatusLoopVFX);

                    ImGui.TextUnformatted("Unknown 0");
                    DrawItemByte("Unknown0", ref replacement.Unknown0, i => i.Unknown0);

                    ImGui.TextUnformatted("Status Category");
                    DrawItemByte("StatusCategory", ref replacement.StatusCategory, i => i.StatusCategory);

                    ImGui.TextUnformatted("Status Hit Effect ID");
                    DrawItemByte("StatusHitEffect", ref replacement.StatusHitEffect, i => i.StatusHitEffect);

                    ImGui.TextUnformatted("Target Type");
                    DrawItemByte("TargetType", ref replacement.TargetType, i => i.TargetType);

                    ImGui.TextUnformatted("Flag 1");
                    DrawItemByte("Flags", ref replacement.Flags, i => i.Flags);

                    ImGui.TextUnformatted("Flag 2");
                    DrawItemByte("Flag2", ref replacement.Flag2, i => i.Flag2);

                    ImGui.TextUnformatted("Unknown 70_1");
                    DrawItemByte("Unknown_70_1", ref replacement.Unknown_70_1, i => i.Unknown_70_1);

                    ImGui.TextUnformatted("Show Attack Type");
                    DrawItemSByte("AtkType", ref replacement.Unknown2, i => i.Unknown2);

                    ImGui.NewLine();
                    ImGui.Separator();
                    ImGui.NewLine();

                    continue;
                    //SPECIFICSTART
                    void DrawItemInt(string name, ref int value,
                        Func<StatusReplacement, int> getDefault)
                    {
                        ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 20);
                        ImGui.SetNextItemWidth(60 * Scale);
                        int relay = value;
                        if (ImGui.DragInt("##" + name + key, ref relay))
                        {
                            value = relay;
                            Methods.SetupStatus(key);
                            Service.Config.Save();
                        }

                        ImGui.SameLine();
                        using (ImRaii.PushFont(UiBuilder.IconFont))
                        {
                            if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{name}{key}"))
                            {
                                value = getDefault(StatusReplacementsManager.GetOriginalReplacement(key));
                                Methods.SetupStatus(key);
                                Service.Config.Save();
                            }
                        }
                    }
                    void DrawItemUShort(string name, ref ushort value,
                        Func<StatusReplacement, ushort> getDefault)
                    {
                        ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 20);
                        ImGui.SetNextItemWidth(60 * Scale);
                        int relay = value;
                        if (ImGui.DragInt("##" + name + key, ref relay))
                        {
                            value = (ushort)relay;
                            Methods.SetupStatus(key);
                            Service.Config.Save();
                        }

                        ImGui.SameLine();
                        using (ImRaii.PushFont(UiBuilder.IconFont))
                        {
                            if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{name}{key}"))
                            {
                                value = getDefault(StatusReplacementsManager.GetOriginalReplacement(key));
                                Methods.SetupStatus(key);
                                Service.Config.Save();
                            }
                        }
                    }
                    void DrawItemByte(string name, ref byte value,
                         Func<StatusReplacement, byte> getDefault)
                    {
                        ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 20);
                        ImGui.SetNextItemWidth(60 * Scale);
                        int relay = value;
                        if (ImGui.DragInt("##" + name + key, ref relay))
                        {
                            value = (byte)relay;
                            Methods.SetupStatus(key);
                            Service.Config.Save();
                        }

                        ImGui.SameLine();
                        using (ImRaii.PushFont(UiBuilder.IconFont))
                        {
                            if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{name}{key}"))
                            {
                                value = getDefault(StatusReplacementsManager.GetOriginalReplacement(key));
                                Methods.SetupStatus(key);
                                Service.Config.Save();
                            }
                        }
                    }
                    void DrawItemSByte(string name, ref sbyte value,
                         Func<StatusReplacement, sbyte> getDefault)
                    {
                        ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 20);
                        ImGui.SetNextItemWidth(60 * Scale);
                        int relay = value;
                        if (ImGui.DragInt("##" + name + key, ref relay))
                        {
                            value = (sbyte)relay;
                            Methods.SetupStatus(key);
                            Service.Config.Save();
                        }

                        ImGui.SameLine();
                        using (ImRaii.PushFont(UiBuilder.IconFont))
                        {
                            if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{name}{key}"))
                            {
                                value = getDefault(StatusReplacementsManager.GetOriginalReplacement(key));
                                Methods.SetupStatus(key);
                                Service.Config.Save();
                            }
                        }
                    }
                    //SPECIFICEND
                }
                //SPECIFICSTART
                using var searchPopup = ImRaii.Popup(searchStatusPopup);
                if (searchPopup)
                {
                    var width = 200 * Scale;

                    ImGui.SetNextItemWidth(width);
                    ImGui.InputText("##Search Status", ref _searchStatus, 256);
                    var localsearch = _searchStatus;

                    using var popUpChild = ImRaii.Child(searchStatusPopup, new Vector2(width, 200 * Scale), true);
                    foreach (var pair in StatusReplacementsManager.StatusNames.OrderBy(i =>
                    {
                        if (string.IsNullOrEmpty(localsearch)) return 0;
                        return Math.Min(ConfigWindow.ScoreString(i.Value, localsearch),
                            ConfigWindow.ScoreString(i.Key.ToString(), localsearch));
                    }))
                    {
                        if (ImGui.Selectable($"#{pair.Key:D5} {pair.Value}"))
                        {
                            var original = StatusReplacementsManager.GetOriginalReplacement(pair.Key);
                            _activeSet.StatusReplacements[pair.Key] =
                                new StatusReplacementConfig(new StatusReplacement(
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
                //SPECIFICEND
            }
        }
    }
}
