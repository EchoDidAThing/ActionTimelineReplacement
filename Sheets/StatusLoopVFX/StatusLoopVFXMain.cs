using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface;
using Dalamud.Bindings.ImGui;
using System;
using System.Linq;
using ActionTimelineReplacement.Base.Setups;
using ActionTimelineReplacement.Windows;
using ActionTimelineReplacement.Base.Global;
#pragma warning disable CA1416 // Validate platform compatibility

namespace ActionTimelineReplacement.Sheets;

#region Main
public class StatusLoopVFXMain
{
    public static void Draw(string mainkey, ref Configuration.ReplacementSet _activeSet, ref string search)
    {
        using var subList = ImRaii.Child(mainkey, CalcGlobals.BodyScale(), false);
        if (subList)
        {
            const string searchPopup = "Search statusloopvfxs";
            UiGlobals.DrawAddItem(searchPopup);

            foreach (var key in _activeSet.StatusLoopVFXWriter.Keys)
            {
                var replace = _activeSet.StatusLoopVFXWriter[key].Replacement;

                if (ImGui.Checkbox("##" + key, ref _activeSet.StatusLoopVFXWriter[key].Enabled))
                {
                    if (_activeSet.StatusLoopVFXWriter[key].Enabled)
                    {
                        Setup.SetStatusLoopVFX(key);
                    }
                    else 
                    {
                        Setup.SetStatusLoopVFX(key, true);
                    }
                    Service.Config.Save();
                }
                ImGui.SameLine();

                if (ImGui.Button(" - ##" + key))
                {
                    Setup.SetStatusLoopVFX(key, true);
                    _activeSet.StatusLoopVFXWriter.Remove(key);
                    Service.Config.Save();
                }

                ImGui.SameLine();
                ImGui.Text($"#{key:D5}");

                ImGui.SameLine();
                ImGui.TextWrapped(StatusLoopVFXManager.GetName(key));

                //to do: show loop vfx and hit effect as strings
                DrawUShort("FriendlyVFX", "Friendly VFX ID", ref replace.FriendlyVFX, i => i.FriendlyVFX);
                DrawUShort("StackVFX1", "Stack VFX 1 ID", ref replace.StackVFX1, i => i.StackVFX1);
                DrawUShort("StackVFX2", "Stack VFX 2 ID", ref replace.StackVFX2, i => i.StackVFX2);
                DrawUShort("EnemyVFX", "Enemy VFX ID", ref replace.EnemyVFX, i => i.EnemyVFX);
                DrawByte("Stack1Trigger", "Stack 1 Trigger", ref replace.StackTrigger1, i => i.StackTrigger1);
                DrawByte("Stack2Trigger", "Stack 2 Trigger", ref replace.StackTrigger2, i => i.StackTrigger2);
                DrawByte("Unknown1", "Unknown1", ref replace.Unknown1, i => i.Unknown1);
                DrawByte("Unknown2", "Unknown2", ref replace.Unknown2, i => i.Unknown2);
                DrawBool("Unknown3", "Unknown3", ref replace.Unknown3, i => i.Unknown3);
                DrawBool("Unknown4", "Unknown4", ref replace.Unknown4, i => i.Unknown4);
                DrawBool("Unknown5", "Unknown5", ref replace.Unknown5, i => i.Unknown5);

                UiGlobals.DrawItemSeparator();
                continue;

                #endregion
                #region Items

                void DrawInt(string refname, string text, ref int value,
                    Func<StatusLoopVFXReplace, int> getDefault)
                {
                    ImGui.TextUnformatted(text);
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * CalcGlobals.GlobalScale());
                    int relay = value;
                    if (ImGui.InputInt("##" + refname + key, ref relay))
                    {
                        value = relay;
                        Setup.SetStatusLoopVFX(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{refname}{key}"))
                        {
                            value = getDefault(StatusLoopVFXManager.GetOriginal(key));
                            Setup.SetStatusLoopVFX(key);
                            Service.Config.Save();
                        }
                    }
                }

                void DrawUShort(string refname, string text, ref ushort value,
                    Func<StatusLoopVFXReplace, ushort> getDefault)
                {
                    ImGui.TextUnformatted(text);
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * CalcGlobals.GlobalScale());
                    int relay = value;
                    if (ImGui.InputInt("##" + refname + key, ref relay))
                    {
                        value = (ushort)relay;
                        Setup.SetStatusLoopVFX(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{refname}{key}"))
                        {
                            value = getDefault(StatusLoopVFXManager.GetOriginal(key));
                            Setup.SetStatusLoopVFX(key);
                            Service.Config.Save();
                        }
                    }
                }

                void DrawByte(string refname, string text, ref byte value,
                    Func<StatusLoopVFXReplace, byte> getDefault)
                {
                    ImGui.TextUnformatted(text);
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * CalcGlobals.GlobalScale());
                    int relay = value;
                    if (ImGui.InputInt("##" + refname + key, ref relay))
                    {
                        value = (byte)relay;
                        Setup.SetStatusLoopVFX(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{refname}{key}"))
                        {
                            value = getDefault(StatusLoopVFXManager.GetOriginal(key));
                            Setup.SetStatusLoopVFX(key);
                            Service.Config.Save();
                        }
                    }
                }

                void DrawBool(string refname, string text, ref bool value,
                    Func<StatusLoopVFXReplace, bool> getDefault)
                {
                    ImGui.TextUnformatted(text);
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * CalcGlobals.GlobalScale());
                    bool relay = value;
                    if (ImGui.Checkbox("##" + refname + key, ref relay))
                    {
                        value = (bool)relay;
                        Setup.SetStatusLoopVFX(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{refname}{key}"))
                        {
                            value = getDefault(StatusLoopVFXManager.GetOriginal(key));
                            Setup.SetStatusLoopVFX(key);
                            Service.Config.Save();
                        }
                    }
                }

                void DrawSByte(string refname, string text, ref sbyte value,
                    Func<StatusLoopVFXReplace, sbyte> getDefault)
                {
                    ImGui.TextUnformatted(text);
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * CalcGlobals.GlobalScale());
                    int relay = value;
                    if (ImGui.InputInt("##" + refname + key, ref relay))
                    {
                        value = (sbyte)relay;
                        Setup.SetStatusLoopVFX(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{refname}{key}"))
                        {
                            value = getDefault(StatusLoopVFXManager.GetOriginal(key));
                            Setup.SetStatusLoopVFX(key);
                            Service.Config.Save();
                        }
                    }
                }
                
            }

            #endregion
            #region Search/Set

            using var searchStatusLoopVFX = ImRaii.Popup(searchPopup);
            if (searchStatusLoopVFX)
            {
                ImGui.SetNextItemWidth(CalcGlobals.XY());
                ImGui.InputText("##Search statusLoopVFXs", ref search, 256);
                var localsearch = search;

                using var popupChild = ImRaii.Child(searchPopup, CalcGlobals.SearchPopScale(), true);
                foreach (var pair in StatusLoopVFXManager.Names.OrderBy(i =>
                {
                    if (string.IsNullOrEmpty(localsearch)) return 0;
                    return Math.Min(ConfigWindow.ScoreString(i.Value, localsearch),
                        ConfigWindow.ScoreString(i.Key.ToString(), localsearch));
                }))
                {
                    if (ImGui.Selectable($"#{pair.Key:D5} {pair.Value}"))
                    {
                        var original = StatusLoopVFXManager.GetOriginal(pair.Key);
                        _activeSet.StatusLoopVFXWriter[pair.Key] =
                            new StatusLoopVFXConfig(new StatusLoopVFXReplace(
                                    original.FriendlyVFX,
                                    original.StackVFX1,
                                    original.StackVFX2,
                                    original.EnemyVFX,
                                    original.StackTrigger1,
                                    original.StackTrigger2,
                                    original.Unknown1,
                                    original.Unknown2,
                                    original.Unknown3,
                                    original.Unknown4,
                                    original.Unknown5),
                                false);
                        Service.Config.Save();
                    }
                }
            }
            #endregion
        }
    }
}
