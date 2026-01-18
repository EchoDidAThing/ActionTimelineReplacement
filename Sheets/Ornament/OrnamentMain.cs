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
public class OrnamentMain
{
    public static void Draw(string mainkey, ref Configuration.ReplacementSet _activeSet, ref string search)
    {
        using var subList = ImRaii.Child(mainkey, CalcGlobals.BodyScale(), false);
        if (subList)
        {
            const string searchPopup = "Search ornament data";
            UiGlobals.DrawAddItem(searchPopup);

            foreach (var key in _activeSet.OrnamentWriter.Keys)
            {
                var replace = _activeSet.OrnamentWriter[key].Replacement;

                if (ImGui.Checkbox("##" + key, ref _activeSet.OrnamentWriter[key].Enabled))
                {
                    Setup.SetOrnament(key);
                    Service.Config.Save();
                }
                ImGui.SameLine();
                if (ImGui.Button(" - ##" + key))
                {
                    _activeSet.OrnamentWriter.Remove(key);
                }

                ImGui.SameLine();
                ImGui.Text($"#{key:D5}");

                ImGui.SameLine();
                ImGui.TextWrapped(OrnamentManager.GetName(key));

                DrawSByte("Unknown0", "Unknown0", ref replace.Unknown0, i => i.Unknown0);

                DrawUShort("Model", "Model", ref replace.Model, i => i.Model);

                DrawUShort("Action", "Action Row ID", ref replace.Action, i => i.Action);

                DrawUShort("Transient", "Transient [unknown]", ref replace.Transient, i => i.Transient);

                DrawByte("AttachmentPoint", "Attachment Point", ref replace.AttachmentPoint, i => i.AttachmentPoint);

                DrawByte("Unknown3", "Ornament Hide State", ref replace.Unknown3, i => i.Unknown3);

                DrawByte("Unknown4", "Idle Pose Group", ref replace.Unknown4, i => i.Unknown4);

                UiGlobals.DrawItemSeparator();
                continue;

                #endregion
                #region Items
                
                void DrawSByte(string refname, string text, ref sbyte value,
                    Func<OrnamentReplace, sbyte> getDefault)
                {
                    ImGui.TextUnformatted(text);
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * CalcGlobals.GlobalScale());
                    int relay = value;
                    if (ImGui.InputInt("##" + refname + key, ref relay))
                    {
                        value = (sbyte)relay;
                        Setup.SetOrnament(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{refname}{key}"))
                        {
                            value = getDefault(OrnamentManager.GetOriginal(key));
                            Setup.SetOrnament(key);
                            Service.Config.Save();
                        }
                    }
                }

                void DrawByte(string refname, string text, ref byte value,
                    Func<OrnamentReplace, byte> getDefault)
                {
                    ImGui.TextUnformatted(text);
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * CalcGlobals.GlobalScale());
                    int relay = value;
                    if (ImGui.InputInt("##" + refname + key, ref relay))
                    {
                        value = (byte)relay;
                        Setup.SetOrnament(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{refname}{key}"))
                        {
                            value = getDefault(OrnamentManager.GetOriginal(key));
                            Setup.SetOrnament(key);
                            Service.Config.Save();
                        }
                    }
                }

                void DrawUShort(string refname, string text, ref ushort value,
                    Func<OrnamentReplace, ushort> getDefault)
                {
                    ImGui.TextUnformatted(text);
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * CalcGlobals.GlobalScale());
                    int relay = value;
                    if (ImGui.InputInt("##" + refname + key, ref relay))
                    {
                        value = (ushort)relay;
                        Setup.SetOrnament(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{refname}{key}"))
                        {
                            value = getDefault(OrnamentManager.GetOriginal(key));
                            Setup.SetOrnament(key);
                            Service.Config.Save();
                        }
                    }
                }
            }
                            
            #endregion
            #region Search/Set

            using var searchOrnament = ImRaii.Popup(searchPopup);
            if (searchOrnament)
            {
                ImGui.SetNextItemWidth(CalcGlobals.XY());
                ImGui.InputText("##Search ornament data", ref search, 256);
                var localsearch = search;

                using var popupChild = ImRaii.Child(searchPopup, CalcGlobals.SearchPopScale(), true);
                foreach (var pair in OrnamentManager.Names.OrderBy(i =>
                {
                    if (string.IsNullOrEmpty(localsearch)) return 0;
                    return Math.Min(ConfigWindow.ScoreString(i.Value, localsearch),
                        ConfigWindow.ScoreString(i.Key.ToString(), localsearch));
                }))
                {
                    if (ImGui.Selectable($"#{pair.Key:D5} {pair.Value}"))
                    {
                        var original = OrnamentManager.GetOriginal(pair.Key);
                        _activeSet.OrnamentWriter[pair.Key] =
                            new OrnamentConfig(new OrnamentReplace(
                                    original.Unknown0,
                                    original.Model,
                                    original.Action,
                                    original.Transient,
                                    original.AttachmentPoint,
                                    original.Unknown3,
                                    original.Unknown4),
                                false);
                        Service.Config.Save();
                    }
                }
            }
            #endregion
        }
    }
}
