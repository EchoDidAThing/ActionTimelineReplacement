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
public class OrnamentMain
{
    private static float Scale => ImGuiHelpers.GlobalScale;
    public static void Draw(string mainkey, ref Configuration.ReplacementSet _activeSet, ref string search)
    {
        var itemHeight = ImGui.CalcTextSize("").Y + ImGui.GetStyle().FramePadding.Y * 2 + ImGui.GetStyle().WindowPadding.Y;

        //to fix: scale height according to item count
        using var subList = ImRaii.Child(mainkey, new Vector2(-1, ImGui.GetWindowSize().Y - ImGui.GetCursorPosY() - itemHeight - ImGui.GetStyle().WindowPadding.Y), false);
        if (subList)
        {
            const string searchPopup = "Search ornament data";
            if (ImGui.Button(" + "))
            {
                ImGui.OpenPopup(searchPopup);
            }

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

                //to do: streamline this
                ImGui.TextUnformatted("Unknown 0");
                DrawSByte("Unknown0", ref replace.Unknown0, i => i.Unknown0);

                ImGui.TextUnformatted("Model ID");
                DrawUShort("Model", ref replace.Model, i => i.Model);

                ImGui.TextUnformatted("Action Row ID");
                DrawUShort("Action", ref replace.Action, i => i.Action);

                ImGui.TextUnformatted("Transient [unknown]");
                DrawUShort("Transient", ref replace.Transient, i => i.Transient);

                ImGui.TextUnformatted("Attachment Point");
                DrawByte("AttachmentPoint", ref replace.AttachmentPoint, i => i.AttachmentPoint);

                ImGui.TextUnformatted("Ornament Hide State");
                DrawByte("Unknown3", ref replace.Unknown3, i => i.Unknown3);

                ImGui.TextUnformatted("Unknown 4");
                DrawByte("Unknown4", ref replace.Unknown4, i => i.Unknown4);

                ImGui.NewLine();
                ImGui.Separator();
                ImGui.NewLine();
                continue;

                #endregion
                #region Items
                
                void DrawSByte(string name, ref sbyte value,
                    Func<OrnamentReplace, sbyte> getDefault)
                {
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * Scale);
                    int relay = value;
                    if (ImGui.InputInt("##" + name + key, ref relay))
                    {
                        value = (sbyte)relay;
                        Setup.SetOrnament(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{name}{key}"))
                        {
                            value = getDefault(OrnamentManager.GetOriginal(key));
                            Setup.SetOrnament(key);
                            Service.Config.Save();
                        }
                    }
                }

                void DrawByte(string name, ref byte value,
                    Func<OrnamentReplace, byte> getDefault)
                {
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * Scale);
                    int relay = value;
                    if (ImGui.InputInt("##" + name + key, ref relay))
                    {
                        value = (byte)relay;
                        Setup.SetOrnament(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{name}{key}"))
                        {
                            value = getDefault(OrnamentManager.GetOriginal(key));
                            Setup.SetOrnament(key);
                            Service.Config.Save();
                        }
                    }
                }

                void DrawUShort(string name, ref ushort value,
                    Func<OrnamentReplace, ushort> getDefault)
                {
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * Scale);
                    int relay = value;
                    if (ImGui.InputInt("##" + name + key, ref relay))
                    {
                        value = (ushort)relay;
                        Setup.SetOrnament(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{name}{key}"))
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
                var width = 200 * Scale;
                var height = 200 * Scale;

                ImGui.SetNextItemWidth(width);
                ImGui.InputText("##Search ornament data", ref search, 256);
                var localsearch = search;

                using var popupChild = ImRaii.Child(searchPopup, new Vector2(width, height), true);
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
