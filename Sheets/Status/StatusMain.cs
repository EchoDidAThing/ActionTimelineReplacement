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
public class StatusMain
{
    public static void Draw(string mainkey, ref Configuration.ReplacementSet _activeSet, ref string search)
    {
        using var subList = ImRaii.Child(mainkey, CalcGlobals.BodyScale(), false);
        if (subList)
        {
            const string searchPopup = "Search statuses";
            UiGlobals.DrawAddItem(searchPopup);

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

                //to do: show loop vfx and hit effect as strings
                DrawInt("ParamModifier", "Parameter Modifier", ref replace.ParamModifier, i => i.ParamModifier);

                DrawUShort("StatusLoopVFX", "Status Loop VFX ID", ref replace.StatusLoopVFX, i => i.StatusLoopVFX);

                DrawByte("Unknown0", "Unknown 0", ref replace.Unknown0, i => i.Unknown0);

                DrawByte("StatusCategory", "Status Category", ref replace.StatusCategory, i => i.StatusCategory);

                DrawByte("StatusHitEffect", "Status Hit Effect ID", ref replace.StatusHitEffect, i => i.StatusHitEffect);

                DrawByte("ParamEffect", "Parameter Effect", ref replace.ParamEffect, i => i.ParamEffect);

                DrawByte("TargetType", "Target Type", ref replace.TargetType, i => i.TargetType);

                DrawByte("Flags", "Flag 1", ref replace.Flags, i => i.Flags);

                DrawByte("Flag2", "Flag 2", ref replace.Flag2, i => i.Flag2);

                DrawByte("Unknown_70_1", "Unknown_70_1", ref replace.Unknown_70_1, i => i.Unknown_70_1);

                DrawSByte("AtkType", "(related to attack type)", ref replace.Unknown2, i => i.Unknown2);

                UiGlobals.DrawItemSeparator();
                continue;

                #endregion
                #region Items

                void DrawInt(string refname, string text, ref int value,
                    Func<StatusReplace, int> getDefault)
                {
                    ImGui.TextUnformatted(text);
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * CalcGlobals.GlobalScale());
                    int relay = value;
                    if (ImGui.InputInt("##" + refname + key, ref relay))
                    {
                        value = relay;
                        Setup.SetStatus(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{refname}{key}"))
                        {
                            value = getDefault(StatusManager.GetOriginal(key));
                            Setup.SetStatus(key);
                            Service.Config.Save();
                        }
                    }
                }

                void DrawUShort(string refname, string text, ref ushort value,
                    Func<StatusReplace, ushort> getDefault)
                {
                    ImGui.TextUnformatted(text);
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * CalcGlobals.GlobalScale());
                    int relay = value;
                    if (ImGui.InputInt("##" + refname + key, ref relay))
                    {
                        value = (ushort)relay;
                        Setup.SetStatus(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{refname}{key}"))
                        {
                            value = getDefault(StatusManager.GetOriginal(key));
                            Setup.SetStatus(key);
                            Service.Config.Save();
                        }
                    }
                }

                void DrawByte(string refname, string text, ref byte value,
                    Func<StatusReplace, byte> getDefault)
                {
                    ImGui.TextUnformatted(text);
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * CalcGlobals.GlobalScale());
                    int relay = value;
                    if (ImGui.InputInt("##" + refname + key, ref relay))
                    {
                        value = (byte)relay;
                        Setup.SetStatus(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{refname}{key}"))
                        {
                            value = getDefault(StatusManager.GetOriginal(key));
                            Setup.SetStatus(key);
                            Service.Config.Save();
                        }
                    }
                }

                void DrawSByte(string refname, string text, ref sbyte value,
                    Func<StatusReplace, sbyte> getDefault)
                {
                    ImGui.TextUnformatted(text);
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * CalcGlobals.GlobalScale());
                    int relay = value;
                    if (ImGui.InputInt("##" + refname + key, ref relay))
                    {
                        value = (sbyte)relay;
                        Setup.SetStatus(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{refname}{key}"))
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
                ImGui.SetNextItemWidth(CalcGlobals.XY());
                ImGui.InputText("##Search statuses", ref search, 256);
                var localsearch = search;

                using var popupChild = ImRaii.Child(searchPopup, CalcGlobals.SearchPopScale(), true);
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
                                    original.ParamEffect,
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
