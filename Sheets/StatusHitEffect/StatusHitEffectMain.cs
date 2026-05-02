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
public class StatusHitEffectMain
{
    public static void Draw(string mainkey, ref Configuration.ReplacementSet _activeSet, ref string search)
    {
        using var subList = ImRaii.Child(mainkey, CalcGlobals.BodyScale(), false);
        if (subList)
        {
            const string searchPopup = "Search StatusHitEffects";
            UiGlobals.DrawAddItem(searchPopup);

            foreach (var key in _activeSet.StatusHitEffectWriter.Keys)
            {
                var replace = _activeSet.StatusHitEffectWriter[key].Replacement;

                if (ImGui.Checkbox("##" + key, ref _activeSet.StatusHitEffectWriter[key].Enabled))
                {
                    if (_activeSet.StatusHitEffectWriter[key].Enabled)
                    {
                        Setup.SetStatusHitEffect(key);
                    }
                    else 
                    {
                        Setup.SetStatusHitEffect(key, true);
                    }
                    Service.Config.Save();
                }
                ImGui.SameLine();

                if (ImGui.Button(" - ##" + key))
                {
                    Setup.SetStatusHitEffect(key, true);
                    _activeSet.StatusHitEffectWriter.Remove(key);
                    Service.Config.Save();
                }

                ImGui.SameLine();
                ImGui.Text($"#{key:D5}");

                ImGui.SameLine();
                ImGui.TextWrapped(StatusHitEffectManager.GetName(key));

                //to do: show loop vfx and hit effect as strings
                DrawUShort("VFXRow", "VFX Row", ref replace.VFX, i => i.VFX);

                UiGlobals.DrawItemSeparator();
                continue;

                #endregion
                #region Items

                void DrawInt(string refname, string text, ref int value,
                    Func<StatusHitEffectReplace, int> getDefault)
                {
                    ImGui.TextUnformatted(text);
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * CalcGlobals.GlobalScale());
                    int relay = value;
                    if (ImGui.InputInt("##" + refname + key, ref relay))
                    {
                        value = relay;
                        Setup.SetStatusHitEffect(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{refname}{key}"))
                        {
                            value = getDefault(StatusHitEffectManager.GetOriginal(key));
                            Setup.SetStatusHitEffect(key);
                            Service.Config.Save();
                        }
                    }
                }

                void DrawUShort(string refname, string text, ref ushort value,
                    Func<StatusHitEffectReplace, ushort> getDefault)
                {
                    ImGui.TextUnformatted(text);
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * CalcGlobals.GlobalScale());
                    int relay = value;
                    if (ImGui.InputInt("##" + refname + key, ref relay))
                    {
                        value = (ushort)relay;
                        Setup.SetStatusHitEffect(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{refname}{key}"))
                        {
                            value = getDefault(StatusHitEffectManager.GetOriginal(key));
                            Setup.SetStatusHitEffect(key);
                            Service.Config.Save();
                        }
                    }
                }

                void DrawByte(string refname, string text, ref byte value,
                    Func<StatusHitEffectReplace, byte> getDefault)
                {
                    ImGui.TextUnformatted(text);
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * CalcGlobals.GlobalScale());
                    int relay = value;
                    if (ImGui.InputInt("##" + refname + key, ref relay))
                    {
                        value = (byte)relay;
                        Setup.SetStatusHitEffect(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{refname}{key}"))
                        {
                            value = getDefault(StatusHitEffectManager.GetOriginal(key));
                            Setup.SetStatusHitEffect(key);
                            Service.Config.Save();
                        }
                    }
                }

                void DrawBool(string refname, string text, ref bool value,
                    Func<StatusHitEffectReplace, bool> getDefault)
                {
                    ImGui.TextUnformatted(text);
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * CalcGlobals.GlobalScale());
                    bool relay = value;
                    if (ImGui.Checkbox("##" + refname + key, ref relay))
                    {
                        value = (bool)relay;
                        Setup.SetStatusHitEffect(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{refname}{key}"))
                        {
                            value = getDefault(StatusHitEffectManager.GetOriginal(key));
                            Setup.SetStatusHitEffect(key);
                            Service.Config.Save();
                        }
                    }
                }

                void DrawSByte(string refname, string text, ref sbyte value,
                    Func<StatusHitEffectReplace, sbyte> getDefault)
                {
                    ImGui.TextUnformatted(text);
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * CalcGlobals.GlobalScale());
                    int relay = value;
                    if (ImGui.InputInt("##" + refname + key, ref relay))
                    {
                        value = (sbyte)relay;
                        Setup.SetStatusHitEffect(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{refname}{key}"))
                        {
                            value = getDefault(StatusHitEffectManager.GetOriginal(key));
                            Setup.SetStatusHitEffect(key);
                            Service.Config.Save();
                        }
                    }
                }
                
            }

            #endregion
            #region Search/Set

            using var searchStatusHitEffect = ImRaii.Popup(searchPopup);
            if (searchStatusHitEffect)
            {
                ImGui.SetNextItemWidth(CalcGlobals.XY());
                ImGui.InputText("##Search StatusHitEffects", ref search, 256);
                var localsearch = search;

                using var popupChild = ImRaii.Child(searchPopup, CalcGlobals.SearchPopScale(), true);
                foreach (var pair in StatusHitEffectManager.Names.OrderBy(i =>
                {
                    if (string.IsNullOrEmpty(localsearch)) return 0;
                    return Math.Min(ConfigWindow.ScoreString(i.Value, localsearch),
                        ConfigWindow.ScoreString(i.Key.ToString(), localsearch));
                }))
                {
                    if (ImGui.Selectable($"#{pair.Key:D5} {pair.Value}"))
                    {
                        var original = StatusHitEffectManager.GetOriginal(pair.Key);
                        _activeSet.StatusHitEffectWriter[pair.Key] =
                            new StatusHitEffectConfig(new StatusHitEffectReplace(
                                    original.VFX),
                                false);
                        Service.Config.Save();
                    }
                }
            }
            #endregion
        }
    }
}
