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
    public class ActionSubSheet
    {
        private static float Scale => ImGuiHelpers.GlobalScale;
        public static void Draw(string mainkey, ref Configuration.ReplacementSet _activeSet, ref Dictionary<string, float>  _AllItemWidths, string _searchAction)
        {
            using (var subList = ImRaii.Child(mainkey + "SubList", new Vector2(-1f, (float)(_activeSet.ActionReplacements.Count() * 30) + 30), false))
            {
                if (subList)
                {
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
                        if (_AllItemWidths[mainkey] != 0 && Service.Config.AdvancedMode)
                        {
                            var widthRest = ImGui.GetWindowWidth() - _AllItemWidths[mainkey] - ImGui.GetCursorPosX() - 5 * Scale;
                            ImGui.PushTextWrapPos(Math.Max(widthRest, 60 * Scale) + ImGui.GetCursorPosX());
                        }
                        ImGui.TextWrapped(ActionReplacementsManager.GetName(key));
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
                        //SPECIFICSTART
                        DrawItem("Cast", ref replacement.CastVfx, i => i.CastVfx);
                        ImGui.SameLine();

                        DrawItem("Start", ref replacement.AnimationStart, i => i.AnimationStart);

                        ImGui.SameLine();
                        DrawItem("End", ref replacement.AnimationEnd, i => i.AnimationEnd);

                        ImGui.SameLine();
                        DrawItem("Hit", ref replacement.ActionTimelineHit, i => i.ActionTimelineHit);
                        //SPECIFICEND
                        ImGui.SameLine();
                        _AllItemWidths[mainkey] = ImGui.GetCursorPosX() - startwidth;
                        ImGui.NewLine();
                        continue;
                        //SPECIFICSTART
                        void DrawItem(string name, ref ushort value,
                            Func<Configurations.ActionReplacement, ushort> getDefault)
                        {
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
                        //SPECIFICEND
                    }
                    //SPECIFICSTART
                    const string searchActionsPopup = "Search actions";
                    if (ImGui.Button(" + "))
                    {
                        ImGui.OpenPopup(searchActionsPopup);
                    }

                    using var searchPopup = ImRaii.Popup(searchActionsPopup);
                    if (searchPopup)
                    {
                        var width = 200 * Scale;

                        ImGui.SetNextItemWidth(width);
                        ImGui.InputText("##Search Action", ref _searchAction, 256);

                        using var popUpChild = ImRaii.Child(searchActionsPopup, new Vector2(width, 200 * Scale), true);
                        foreach (var pair in ActionReplacementsManager.ActionNames.OrderBy(i =>
                        {
                            if (string.IsNullOrEmpty(_searchAction)) return 0;
                            return Math.Min(ConfigWindow.ScoreString(i.Value, _searchAction),
                                ConfigWindow.ScoreString(i.Key.ToString(), _searchAction));
                        }))
                        {
                            if (ImGui.Selectable($"#{pair.Key:D5} {pair.Value}"))
                            {
                                var original = ActionReplacementsManager.GetOriginalReplacement(pair.Key);
                                _activeSet.ActionReplacements[pair.Key] =
                                    new ActionReplacementConfig(new Configurations.ActionReplacement(
                                            original.AnimationStart,
                                            original.AnimationEnd,
                                            original.ActionTimelineHit,
                                            original.CastVfx),
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
