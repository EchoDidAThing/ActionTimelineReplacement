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
public class ActionMain
{
    private static float Scale => ImGuiHelpers.GlobalScale;
    public static void Draw(string mainkey, ref Configuration.ReplacementSet _activeSet, ref string search)
    {
        var itemHeight = ImGui.CalcTextSize("").Y + ImGui.GetStyle().FramePadding.Y * 2 + ImGui.GetStyle().WindowPadding.Y;

        //to fix: scale height according to item count
        using var subList = ImRaii.Child(mainkey, new Vector2(-1, ImGui.GetWindowSize().Y - ImGui.GetCursorPosY() - itemHeight - ImGui.GetStyle().WindowPadding.Y), false);

        if (subList)
        {
            const string searchPopup = "Search actions";
            if (ImGui.Button(" + "))
            {
                ImGui.OpenPopup(searchPopup);
            }

            foreach (var key in _activeSet.ActionWriter.Keys)
            {
                var replace = _activeSet.ActionWriter[key].Replacement;

                if (ImGui.Checkbox("##" + key, ref _activeSet.ActionWriter[key].Enabled))
                {
                    Setup.SetAction(key);
                    Service.Config.Save();
                }
                ImGui.SameLine();

                if (ImGui.Button(" - ##" + key))
                {
                    _activeSet.ActionWriter.Remove(key);
                }

                ImGui.SameLine();
                ImGui.Text($"#{key:D5}");

                ImGui.SameLine();
                ImGui.TextWrapped(ActionManager.GetName(key));

                ImGui.TextUnformatted("Cast");
                DrawUShort("Cast", ref replace.CastVfx, i => i.CastVfx);

                ImGui.TextUnformatted("Start");
                DrawUShort("Start", ref replace.AnimationStart, i => i.AnimationStart);

                ImGui.TextUnformatted("End");
                DrawUShort("End", ref replace.AnimationEnd, i => i.AnimationEnd);

                ImGui.TextUnformatted("Hit");
                DrawUShort("Hit", ref replace.ActionTimelineHit, i => i.ActionTimelineHit);

                ImGui.TextUnformatted("Unknown1");
                DrawByte("Unknown1", ref replace.Unknown1, i => i.Unknown1);

                ImGui.TextUnformatted("Unknown2");
                DrawByte("Unknown2", ref replace.Unknown2, i => i.Unknown2);

                ImGui.TextUnformatted("Unknown4");
                DrawByte("Unknown4", ref replace.Unknown4, i => i.Unknown4);

                ImGui.TextUnformatted("Unknown_70");
                DrawByte("Unknown_70", ref replace.Unknown_70, i => i.Unknown_70);

                ImGui.NewLine();
                ImGui.Separator();
                ImGui.NewLine();
                continue;

                #endregion
                #region Items

                void DrawUShort(string name, ref ushort value,
                    Func<ActionReplace, ushort> getDefault)
                {
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * Scale);
                    int relay = value;
                    if (ImGui.InputInt("##" + name + key, ref relay))
                    {
                        value = (ushort)relay;
                        Setup.SetAction(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{name}{key}"))
                        {
                            value = getDefault(ActionManager.GetOriginal(key));
                            Setup.SetAction(key);
                            Service.Config.Save();
                        }
                    }
                }

                void DrawByte(string name, ref byte value,
                    Func<ActionReplace, byte> getDefault)
                {
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * Scale);
                    int relay = value;
                    if (ImGui.InputInt("##" + name + key, ref relay))
                    { 
                        value = (byte)relay;
                        Setup.SetAction(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{name}{key}"))
                        {
                            value = getDefault(ActionManager.GetOriginal(key));
                            Setup.SetAction(key);
                            Service.Config.Save();
                        }
                    }
                }
            }
            
            #endregion
            #region Search/Set

            using var searchAction = ImRaii.Popup(searchPopup);
            if (searchAction)
            {
                var width = 200 * Scale;
                var height = 200 * Scale;

                ImGui.SetNextItemWidth(width);
                ImGui.InputText("##Search actions", ref search, 256);
                var localsearch = search;

                using var popupChild = ImRaii.Child(searchPopup, new Vector2(width, height), true);
                foreach (var pair in ActionManager.Names.OrderBy(i =>
                {
                    if (string.IsNullOrEmpty(localsearch)) return 0;
                    return Math.Min(ConfigWindow.ScoreString(i.Value, localsearch),
                        ConfigWindow.ScoreString(i.Key.ToString(), localsearch));
                }))
                {
                    if (ImGui.Selectable($"#{pair.Key:D5} {pair.Value}"))
                    {
                        var original = ActionManager.GetOriginal(pair.Key);
                        _activeSet.ActionWriter[pair.Key] =
                            new ActionConfig(new ActionReplace(
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
            #endregion
        }
    }
}
