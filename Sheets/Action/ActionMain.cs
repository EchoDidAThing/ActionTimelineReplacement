using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface;
using ImGuiNET;
using System;
using System.Linq;
using ActionTimelineReplacement.Base.Setups;
using ActionTimelineReplacement.Windows;
using ActionTimelineReplacement.Base.Items.Global;
#pragma warning disable CA1416 // Validate platform compatibility

namespace ActionTimelineReplacement.Sheets;

#region Main
public class ActionMain
{
    public static void Draw(string mainkey, ref Configuration.ReplacementSet _activeSet, ref string search)
    {
        using var subList = ImRaii.Child(mainkey, CalcGlobals.BodyScale(), false);
        if (subList)
        {
            const string searchPopup = "Search actions";
            UiGlobals.DrawAddItem(searchPopup);

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

                //to do: show values as strings from their subsheets
                ImGui.SameLine();
                ImGui.Text($"#{key:D5}");

                ImGui.SameLine();
                ImGui.TextWrapped(ActionManager.GetName(key));

                DrawUShort("Cast", "Cast", ref replace.CastVfx, i => i.CastVfx);

                DrawUShort("Start", "Start", ref replace.AnimationStart, i => i.AnimationStart);

                DrawUShort("End", "End", ref replace.AnimationEnd, i => i.AnimationEnd);

                DrawUShort("Hit", "Hit", ref replace.ActionTimelineHit, i => i.ActionTimelineHit);

                DrawUShort("ActionCategory", "Action Category ID", ref replace.ActionCategory, i => i.ActionCategory);

                DrawByte("Unknown1", "Unknown 1", ref replace.Unknown1, i => i.Unknown1);

                DrawByte("Unknown2", "Unknown 2", ref replace.Unknown2, i => i.Unknown2);

                DrawByte("Unknown4", "Unknown 4", ref replace.Unknown4, i => i.Unknown4);

                DrawByte("Unknown_70", "Unknown_70", ref replace.Unknown_70, i => i.Unknown_70);

                UiGlobals.DrawItemSeparator();
                continue;

                #endregion
                #region Items

                void DrawUShort(string refname, string text, ref ushort value,
                    Func<ActionReplace, ushort> getDefault)
                {
                    ImGui.TextUnformatted(text);
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * CalcGlobals.GlobalScale());
                    int relay = value;
                    if (ImGui.InputInt("##" + refname + key, ref relay))
                    {
                        value = (ushort)relay;
                        Setup.SetAction(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{refname}{key}"))
                        {
                            value = getDefault(ActionManager.GetOriginal(key));
                            Setup.SetAction(key);
                            Service.Config.Save();
                        }
                    }
                }

                void DrawByte(string refname, string text, ref byte value,
                    Func<ActionReplace, byte> getDefault)
                {
                    ImGui.TextUnformatted(text);
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * CalcGlobals.GlobalScale());
                    int relay = value;
                    if (ImGui.InputInt("##" + refname + key, ref relay))
                    {
                        value = (byte)relay;
                        Setup.SetAction(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{refname}{key}"))
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
                ImGui.SetNextItemWidth(CalcGlobals.XY());
                ImGui.InputText("##Search actions", ref search, 256);
                var localsearch = search;

                using var popupChild = ImRaii.Child(searchPopup, CalcGlobals.SearchPopScale(), true);
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
                                    original.ActionCategory,
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
