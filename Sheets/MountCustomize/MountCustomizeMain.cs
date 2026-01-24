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
public class MountCustomizeMain
{
    public static void Draw(string mainkey, ref Configuration.ReplacementSet _activeSet, ref string search)
    {
        using var subList = ImRaii.Child(mainkey, CalcGlobals.BodyScale(), false);
        if (subList)
        {
            const string searchPopup = "Search mountcustomize entries";
            UiGlobals.DrawAddItem(searchPopup);

            foreach (var key in _activeSet.MountCustomizeWriter.Keys)
            {
                var replace = _activeSet.MountCustomizeWriter[key].Replacement;

                if (ImGui.Checkbox("##" + key, ref _activeSet.MountCustomizeWriter[key].Enabled))
                {
                    if (_activeSet.MountCustomizeWriter[key].Enabled)
                    {
                        Setup.SetMountCustomize(key);
                    }
                    else
                    {
                        Setup.SetMountCustomize(key, true);
                    }
                    Service.Config.Save();
                }
                ImGui.SameLine();

                if (ImGui.Button(" - ##" + key))
                {
                    Setup.SetMountCustomize(key, true);
                    _activeSet.MountCustomizeWriter.Remove(key);
                    Service.Config.Save();
                }

                ImGui.SameLine();
                ImGui.Text($"#{key:D5}");

                ImGui.SameLine();
                ImGui.TextWrapped(MountCustomizeManager.GetName(key));

                DrawUShort("HyurMidlanderMaleScale", "Midlander Male Scale", ref replace.HyurMidlanderMaleScale, i => i.HyurMidlanderMaleScale);
                DrawUShort("HyurMidlanderFemaleScale", "Midlander Female Scale", ref replace.HyurMidlanderFemaleScale, i => i.HyurMidlanderFemaleScale);
                DrawUShort("HyurHighlanderMaleScale", "Highlander Male Scale", ref replace.HyurHighlanderMaleScale, i => i.HyurHighlanderMaleScale);
                DrawUShort("HyurHighlanderFemaleScale", "Highlander Female Scale", ref replace.HyurHighlanderFemaleScale, i => i.HyurHighlanderFemaleScale);
                DrawUShort("ElezenMaleScale", "Elezen Male Scale", ref replace.ElezenMaleScale, i => i.ElezenMaleScale);
                DrawUShort("ElezenFemaleScale", "Elezen Female Scale", ref replace.ElezenFemaleScale, i => i.ElezenFemaleScale);
                DrawUShort("LalafellMaleScale", "Lalafell Male Scale", ref replace.LalafellMaleScale, i => i.LalafellMaleScale);
                DrawUShort("LalafellFemaleScale", "Lalafell Female Scale", ref replace.LalafellFemaleScale, i => i.LalafellFemaleScale);
                DrawUShort("MiqoteMaleScale", "Miqote Male Scale", ref replace.MiqoteMaleScale, i => i.MiqoteMaleScale);
                DrawUShort("MiqoteFemaleScale", "Miqote Female Scale", ref replace.MiqoteFemaleScale, i => i.MiqoteFemaleScale);
                DrawUShort("RoegadynMaleScale", "Roegadyn Male Scale", ref replace.RoegadynMaleScale, i => i.RoegadynMaleScale);
                DrawUShort("RoegadynFemaleScale", "Roegadyn Female Scale", ref replace.RoegadynFemaleScale, i => i.RoegadynFemaleScale);
                DrawUShort("AuRaMaleScale", "AuRa Male Scale", ref replace.AuRaMaleScale, i => i.AuRaMaleScale);
                DrawUShort("AuRaFemaleScale", "AuRa Female Scale", ref replace.AuRaFemaleScale, i => i.AuRaFemaleScale);
                DrawUShort("HrothgarMaleScale", "Hrothgar Male Scale", ref replace.HrothgarMaleScale, i => i.HrothgarMaleScale);
                DrawUShort("HrothgarFemaleScale", "Hrothgar Female Scale", ref replace.HrothgarFemaleScale, i => i.HrothgarFemaleScale);
                DrawUShort("VieraMaleScale", "Viera Male Scale", ref replace.VieraMaleScale, i => i.VieraMaleScale);
                DrawUShort("VieraFemaleScale", "Viera Female Scale", ref replace.VieraFemaleScale, i => i.VieraFemaleScale);
                DrawUShort("HyurMidlanderMaleCameraHeight", "Midlander Male CameraHeight", ref replace.HyurMidlanderMaleCameraHeight, i => i.HyurMidlanderMaleCameraHeight);
                DrawUShort("HyurMidlanderFemaleCameraHeight", "Midlander Female CameraHeight", ref replace.HyurMidlanderFemaleCameraHeight, i => i.HyurMidlanderFemaleCameraHeight);
                DrawUShort("HyurHighlanderMaleCameraHeight", "Highlander Male CameraHeight", ref replace.HyurHighlanderMaleCameraHeight, i => i.HyurHighlanderMaleCameraHeight);
                DrawByte("HyurHighlanderFemaleCameraHeight", "Highlander Female CameraHeight", ref replace.HyurHighlanderFemaleCameraHeight, i => i.HyurHighlanderFemaleCameraHeight);
                DrawByte("ElezenMaleCameraHeight", "Elezen Male CameraHeight", ref replace.ElezenMaleCameraHeight, i => i.ElezenMaleCameraHeight);
                DrawByte("ElezenFemaleCameraHeight", "Elezen Female CameraHeight", ref replace.ElezenFemaleCameraHeight, i => i.ElezenFemaleCameraHeight);
                DrawByte("LalafellMaleCameraHeight", "Lalafell Male CameraHeight", ref replace.LalafellMaleCameraHeight, i => i.LalafellMaleCameraHeight);
                DrawByte("LalafellFemaleCameraHeight", "Lalafell Female CameraHeight", ref replace.LalafellFemaleCameraHeight, i => i.LalafellFemaleCameraHeight);
                DrawByte("MiqoteMaleCameraHeight", "Miqote Male CameraHeight", ref replace.MiqoteMaleCameraHeight, i => i.MiqoteMaleCameraHeight);
                DrawByte("MiqoteFemaleCameraHeight", "Miqote Female CameraHeight", ref replace.MiqoteFemaleCameraHeight, i => i.MiqoteFemaleCameraHeight);
                DrawByte("RoegadynMaleCameraHeight", "Roegadyn Male CameraHeight", ref replace.RoegadynMaleCameraHeight, i => i.RoegadynMaleCameraHeight);
                DrawByte("RoegadynFemaleCameraHeight", "Roegadyn Female CameraHeight", ref replace.RoegadynFemaleCameraHeight, i => i.RoegadynFemaleCameraHeight);
                DrawByte("AuRaMaleCameraHeight", "AuRa Male CameraHeight", ref replace.AuRaMaleCameraHeight, i => i.AuRaMaleCameraHeight);
                DrawByte("AuRaFemaleCameraHeight", "AuRa Female CameraHeight", ref replace.AuRaFemaleCameraHeight, i => i.AuRaFemaleCameraHeight);
                DrawByte("HrothgarMaleCameraHeight", "Hrothgar Male CameraHeight", ref replace.HrothgarMaleCameraHeight, i => i.HrothgarMaleCameraHeight);
                DrawByte("HrothgarFemaleCameraHeight", "Hrothgar Female CameraHeight", ref replace.HrothgarFemaleCameraHeight, i => i.HrothgarFemaleCameraHeight);
                DrawByte("VieraMaleCameraHeight", "Viera Male CameraHeight", ref replace.VieraMaleCameraHeight, i => i.VieraMaleCameraHeight);
                DrawByte("VieraFemaleCameraHeight", "Viera Female CameraHeight", ref replace.VieraFemaleCameraHeight, i => i.VieraFemaleCameraHeight);

                //DrawByte("MountBoolSet1", "Mount Bools 1 [raw]", ref replace.MountBoolSet1, i => i.MountBoolSet1);

                UiGlobals.DrawItemSeparator();
                continue;

                #endregion
                #region Items

                //to do: streamline items
                void DrawUShort(string refname, string text, ref ushort value,
                    Func<MountCustomizeReplace, ushort> getDefault)
                {
                    ImGui.TextUnformatted(text);
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * CalcGlobals.GlobalScale());
                    int relay = value;
                    if (ImGui.InputInt("##" + refname + key, ref relay))
                    {
                        value = (ushort)relay;
                        Setup.SetMountCustomize(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{refname}{key}"))
                        {
                            value = getDefault(MountCustomizeManager.GetOriginal(key));
                            Setup.SetMountCustomize(key);
                            Service.Config.Save();
                        }
                    }
                }

                void DrawByte(string refname, string text, ref byte value,
                    Func<MountCustomizeReplace, byte> getDefault)
                {
                    ImGui.TextUnformatted(text);
                    ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
                    ImGui.SetNextItemWidth(110 * CalcGlobals.GlobalScale());
                    int relay = value;
                    if (ImGui.InputInt("##" + refname + key, ref relay))
                    {
                        value = (byte)relay;
                        Setup.SetMountCustomize(key);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();

                    using (ImRaii.PushFont(UiBuilder.IconFont))
                    {
                        if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{refname}{key}"))
                        {
                            value = getDefault(MountCustomizeManager.GetOriginal(key));
                            Setup.SetMountCustomize(key);
                            Service.Config.Save();
                        }
                    }
                }
            }

            #endregion
            #region Search/Set

            using var searchMountCustomize = ImRaii.Popup(searchPopup);
            if (searchMountCustomize)
            {
                ImGui.SetNextItemWidth(CalcGlobals.XY());
                ImGui.InputText("##Search mountcustomize", ref search, 256);
                var localsearch = search;

                using var popupChild = ImRaii.Child(searchPopup, CalcGlobals.SearchPopScale(), true);
                foreach (var pair in MountCustomizeManager.Names.OrderBy(i =>
                {
                    if (string.IsNullOrEmpty(localsearch)) return 0;
                    return Math.Min(ConfigWindow.ScoreString(i.Value, localsearch),
                        ConfigWindow.ScoreString(i.Key.ToString(), localsearch));
                }))
                {
                    if (ImGui.Selectable($"#{pair.Key:D5} {pair.Value}"))
                    {
                        var original = MountCustomizeManager.GetOriginal(pair.Key);
                        _activeSet.MountCustomizeWriter[pair.Key] =
                            new MountCustomizeConfig(new MountCustomizeReplace(
                                    original.HyurMidlanderMaleScale,
                                    original.HyurMidlanderFemaleScale,
                                    original.HyurHighlanderMaleScale,
                                    original.HyurHighlanderFemaleScale,
                                    original.ElezenMaleScale,
                                    original.ElezenFemaleScale,
                                    original.LalafellMaleScale,
                                    original.LalafellFemaleScale,
                                    original.MiqoteMaleScale,
                                    original.MiqoteFemaleScale,
                                    original.RoegadynMaleScale,
                                    original.RoegadynFemaleScale,
                                    original.AuRaMaleScale,
                                    original.AuRaFemaleScale,
                                    original.HrothgarMaleScale,
                                    original.HrothgarFemaleScale,
                                    original.VieraMaleScale,
                                    original.VieraFemaleScale,
                                    original.HyurMidlanderMaleCameraHeight,
                                    original.HyurMidlanderFemaleCameraHeight,
                                    original.HyurHighlanderMaleCameraHeight,
                                    original.HyurHighlanderFemaleCameraHeight,
                                    original.ElezenMaleCameraHeight,
                                    original.ElezenFemaleCameraHeight,
                                    original.LalafellMaleCameraHeight,
                                    original.LalafellFemaleCameraHeight,
                                    original.MiqoteMaleCameraHeight,
                                    original.MiqoteFemaleCameraHeight,
                                    original.RoegadynMaleCameraHeight,
                                    original.RoegadynFemaleCameraHeight,
                                    original.AuRaMaleCameraHeight,
                                    original.AuRaFemaleCameraHeight,
                                    original.HrothgarMaleCameraHeight,
                                    original.HrothgarFemaleCameraHeight,
                                    original.VieraMaleCameraHeight,
                                    original.VieraFemaleCameraHeight),
                                    false);
                        Service.Config.Save();
                    }
                }
            }
            #endregion
        }
    }
}
