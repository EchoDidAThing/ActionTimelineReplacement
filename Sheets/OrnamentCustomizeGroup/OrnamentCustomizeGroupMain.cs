using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface;
using System.Numerics;
using Dalamud.Bindings.ImGui;
using System;
using System.Linq;
using Dalamud.Interface.Utility;
using ActionTimelineReplacement.Base.Setups;
using ActionTimelineReplacement.Windows;

namespace ActionTimelineReplacement.Sheets;

#region Main
public class OrnamentCustomizeGroupMain
{
    /*
    private static float Scale => ImGuiHelpers.GlobalScale;
    public static void Draw(string mainkey, ref Configuration.ReplacementSet _activeSet, ref string search)
    {
        var itemHeight = ImGui.CalcTextSize("").Y + ImGui.GetStyle().FramePadding.Y * 2 + ImGui.GetStyle().WindowPadding.Y;

        //to fix: scale height according to item count
        using var subList = ImRaii.Child(mainkey, new Vector2(-1, ImGui.GetWindowSize().Y - ImGui.GetCursorPosY() - itemHeight - ImGui.GetStyle().WindowPadding.Y), false);
        if (subList)
        {
            const string searchPopup = "Search ornament customizable data";
            if (ImGui.Button(" + "))
            {
                ImGui.OpenPopup(searchPopup);
            }

            foreach (var key in _activeSet.OrnamentCustomizeWriter.Keys)
            {
                var replace = _activeSet.OrnamentCustomizeWriter[key].Replacement;

                if (ImGui.Checkbox("##" + key, ref _activeSet.OrnamentCustomizeWriter[key].Enabled))
                {
                    Setup.SetOrnamentCustomize(key);
                    Service.Config.Save();
                }
                ImGui.SameLine();
                if (ImGui.Button(" - ##" + key))
                {
                    _activeSet.OrnamentCustomizeWriter.Remove(key);
                }

                ImGui.SameLine();
                ImGui.Text($"#{key:D5}");

                ImGui.SameLine();
                ImGui.TextWrapped(OrnamentCustomizeManager.GetName(key));

                //to do: streamline this
                ImGui.TextUnformatted("Unknown 0");
                DrawUShort("Unknown0", ref replace.Unknown0, i => i.Unknown0);

                ImGui.TextUnformatted("Unknown 1");
                DrawShort("Unknown1", ref replace.Unknown1, i => i.Unknown1);

                ImGui.TextUnformatted("Unknown 2");
                DrawShort("Unknown2", ref replace.Unknown2, i => i.Unknown2);

                ImGui.TextUnformatted("Unknown 3");
                DrawShort("Unknown3", ref replace.Unknown3, i => i.Unknown3);

                ImGui.TextUnformatted("Unknown 4");
                DrawShort("Unknown4", ref replace.Unknown4, i => i.Unknown4);

                ImGui.TextUnformatted("Unknown 5");
                DrawShort("Unknown5", ref replace.Unknown5, i => i.Unknown5);

                ImGui.TextUnformatted("Unknown 6");
                DrawShort("Unknown6", ref replace.Unknown6, i => i.Unknown6);

                ImGui.NewLine();
                ImGui.Separator();
                ImGui.NewLine();
                continue;

                #endregion
                #region Items
                
                void DrawShort(string name, ref short value,
                    Func<OrnamentCustomizeReplace, short> getDefault)
                {
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * Scale);
                    int relay = value;
                    if (ImGui.InputInt("##" + name + key, ref relay))
                    {
                        value = (short)relay;
                        Setup.SetOrnamentCustomize(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{name}{key}"))
                        {
                            value = getDefault(OrnamentCustomizeManager.GetOriginal(key));
                            Setup.SetOrnamentCustomize(key);
                            Service.Config.Save();
                        }
                    }
                }

                void DrawUShort(string name, ref ushort value,
                    Func<OrnamentCustomizeReplace, ushort> getDefault)
                {
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * Scale);
                    int relay = value;
                    if (ImGui.InputInt("##" + name + key, ref relay))
                    {
                        value = (ushort)relay;
                        Setup.SetOrnamentCustomize(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{name}{key}"))
                        {
                            value = getDefault(OrnamentCustomizeManager.GetOriginal(key));
                            Setup.SetOrnamentCustomize(key);
                            Service.Config.Save();
                        }
                    }
                }
            }
                            
            #endregion
            #region Search/Set

            using var searchOrnamentCustomize = ImRaii.Popup(searchPopup);
            if (searchOrnamentCustomize)
            {
                var width = 200 * Scale;
                var height = 200 * Scale;

                ImGui.SetNextItemWidth(width);
                ImGui.InputText("##Search ornament customize data", ref search, 256);
                var localsearch = search;

                using var popupChild = ImRaii.Child(searchPopup, new Vector2(width, height), true);
                foreach (var pair in OrnamentCustomizeManager.Names.OrderBy(i =>
                {
                    if (string.IsNullOrEmpty(localsearch)) return 0;
                    return Math.Min(ConfigWindow.ScoreString(i.Value, localsearch),
                        ConfigWindow.ScoreString(i.Key.ToString(), localsearch));
                }))
                {
                    if (ImGui.Selectable($"#{pair.Key:D5} {pair.Value}"))
                    {
                        var original = OrnamentCustomizeManager.GetOriginal(pair.Key);
                        _activeSet.OrnamentCustomizeWriter[pair.Key] =
                            new OrnamentCustomizeConfig(new OrnamentCustomizeReplace(
                                    original.Unknown0,
                                    original.Unknown1,
                                    original.Unknown2,
                                    original.Unknown3,
                                    original.Unknown4,
                                    original.Unknown5,
                                    original.Unknown6),
                                false);
                        Service.Config.Save();
                    }
                }
            }
            //#endregion
        }
    }
    */
    #endregion
}
