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
    public class ActionSubSheet
    {
        private static float Scale => ImGuiHelpers.GlobalScale;
        public static void Draw(string mainkey, ref Configuration.ReplacementSet _activeSet, ref string _searchAction)
        {
            var itemHeight = ImGui.CalcTextSize("").Y + ImGui.GetStyle().FramePadding.Y * 2 + ImGui.GetStyle().WindowPadding.Y;

            //to fix: scale height according to item count
            using var subList = ImRaii.Child(mainkey, new Vector2(-1, ImGui.GetWindowSize().Y - ImGui.GetCursorPosY() - itemHeight - ImGui.GetStyle().WindowPadding.Y), false);
            
            if (subList)
            {
                const string searchActionsPopup = "Search actions";
                if (ImGui.Button(" + "))
                {
                    ImGui.OpenPopup(searchActionsPopup);
                }

                foreach (var key in _activeSet.ActionReplacements.Keys)
                {   //SPECIFICSTART
                    var replacement = _activeSet.ActionReplacements[key].Replacement;

                    if (ImGui.Checkbox("##" + key, ref _activeSet.ActionReplacements[key].Enabled))
                    {
                        Methods.SetupAction(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();
                    if (ImGui.Button(" - ##" + key))
                    {
                        _activeSet.ActionReplacements.Remove(key);
                    }
                    //SPECIFICEND
                    ImGui.SameLine();
                    ImGui.Text($"#{key:D5}");

                    ImGui.SameLine();
                    ImGui.TextWrapped(ActionReplacementsManager.GetName(key));

                    ImGui.TextUnformatted("Cast");
                    DrawItem("Cast", ref replacement.CastVfx, i => i.CastVfx);

                    ImGui.TextUnformatted("Start");
                    DrawItem("Start", ref replacement.AnimationStart, i => i.AnimationStart);

                    ImGui.TextUnformatted("End");
                    DrawItem("End", ref replacement.AnimationEnd, i => i.AnimationEnd);

                    ImGui.TextUnformatted("Hit");
                    DrawItem("Hit", ref replacement.ActionTimelineHit, i => i.ActionTimelineHit);

                    ImGui.TextUnformatted("Unknown1");
                    DrawItemByte("Unknown1", ref replacement.Unknown1, i => i.Unknown1);

                    ImGui.TextUnformatted("Unknown2");
                    DrawItemByte("Unknown2", ref replacement.Unknown2, i => i.Unknown2);

                    ImGui.TextUnformatted("Unknown4");
                    DrawItemByte("Unknown4", ref replacement.Unknown4, i => i.Unknown4);

                    ImGui.TextUnformatted("Unknown_70");
                    DrawItemByte("Unknown_70", ref replacement.Unknown_70, i => i.Unknown_70);

                    ImGui.NewLine();
                    ImGui.Separator();
                    ImGui.NewLine();
                    continue;
                    //SPECIFICSTART
                    void DrawItem(string name, ref ushort value,
                        Func<ActionReplacement, ushort> getDefault)
                    {
                        ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 20);
                        ImGui.SetNextItemWidth(60 * Scale);
                        int relay = value;
                        if (ImGui.DragInt("##" + name + key, ref relay))
                        {
                            value = (ushort)relay;
                            Methods.SetupAction(key);
                            Service.Config.Save();
                        }

                        ImGui.SameLine();
                        using (ImRaii.PushFont(UiBuilder.IconFont))
                        {
                            if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{name}{key}"))
                            {
                                value = getDefault(ActionReplacementsManager.GetOriginalReplacement(key));
                                Methods.SetupAction(key);
                                Service.Config.Save();
                            }
                        }
                    }
                    void DrawItemByte(string name, ref byte value,
                        Func<ActionReplacement, byte> getDefault)
                    {
                        ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 20);
                        ImGui.SetNextItemWidth(60 * Scale);
                        int relay = value;
                        if (ImGui.DragInt("##" + name + key, ref relay))
                        {
                            value = (byte)relay;
                            Methods.SetupAction(key);
                            Service.Config.Save();
                        }

                        ImGui.SameLine();
                        using (ImRaii.PushFont(UiBuilder.IconFont))
                        {
                            if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{name}{key}"))
                            {
                                value = getDefault(ActionReplacementsManager.GetOriginalReplacement(key));
                                Methods.SetupAction(key);
                                Service.Config.Save();
                            }
                        }
                    }
                    //SPECIFICEND
                }
                //SPECIFICSTART
                using var searchPopup = ImRaii.Popup(searchActionsPopup);
                if (searchPopup)
                {
                    var width = 200 * Scale;

                    ImGui.SetNextItemWidth(width);
                    ImGui.InputText("##Search Action", ref _searchAction, 256);
                    var localsearch = _searchAction;

                    using var popUpChild = ImRaii.Child(searchActionsPopup, new Vector2(width, 200 * Scale), true);
                    foreach (var pair in ActionReplacementsManager.ActionNames.OrderBy(i =>
                    {
                        if (string.IsNullOrEmpty(localsearch)) return 0;
                        return Math.Min(ConfigWindow.ScoreString(i.Value, localsearch),
                            ConfigWindow.ScoreString(i.Key.ToString(), localsearch));
                    }))
                    {
                        if (ImGui.Selectable($"#{pair.Key:D5} {pair.Value}"))
                        {
                            var original = ActionReplacementsManager.GetOriginalReplacement(pair.Key);
                            _activeSet.ActionReplacements[pair.Key] =
                                new ActionReplacementConfig(new ActionReplacement(
                                        original.AnimationStart,
                                        original.AnimationEnd,
                                        original.ActionTimelineHit,
                                        original.CastVfx,
                                        original.Unknown1,
                                        original.Unknown2,
                                        original.Unknown4,
                                        original.Unknown_70),
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
