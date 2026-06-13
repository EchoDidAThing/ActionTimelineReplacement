using ActionTimelineReplacement.Base.Global;
using ActionTimelineReplacement.Base.Setups;
using ActionTimelineReplacement.Interface;
using Dalamud.Bindings.ImGui;
using Dalamud.Game.Config;
using Dalamud.Interface;
using Dalamud.Interface.Utility.Raii;
using FFXIVClientStructs.FFXIV.Common.Lua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using static FFXIVClientStructs.FFXIV.Client.UI.Misc.AozNoteModule;
#pragma warning disable CA1416 // Validate platform compatibility

namespace ActionTimelineReplacement.Sheets;

#region Main
public class MountDetourMain
{
    const string type = "MountDetour";
    const string typename = "MountDetour";
    const string typenameplural = "MountDetours";
    private static string lastsearch = "";
    private static string localsearch = "";
    private static Dictionary<uint, string> SearchList = UiGlobals.CreateSearchList(type);
    public static void Draw(string mainkey, ref Configuration.ReplacementSet _activeSet, ref string search) {

        Dictionary<uint, MountDetourConfig> Writer = _activeSet.MountDetourWriter;
        var GetName = MountDetourManager.GetName;
        var CreateEntry = MountDetourConfig.CreateEntry;

        using var subList = ImRaii.Child(mainkey, CalcGlobals.BodyScale(), true);
        if (subList) {
            const string searchPopup = "Search " + typenameplural;
            UiGlobals.DrawAddItem(searchPopup);

            foreach (var key in Writer.Keys) {
                if (ImGui.CollapsingHeader($"#{key:D5} - " + GetName(key))) {
                    var LocalWriter = Writer[key];
                    var old = MountDetourManager.GetOriginal(key);
                    bool enablechanges = UiGlobals.CheckIsEnabled(Service.Config.EnableReplacement, _activeSet.Enabled, LocalWriter.Enabled);

                    using (ImRaii.PushFont(UiBuilder.IconFont)) {
                        if (UiGlobals.DrawDeleteEntryButton($"{FontAwesomeIcon.Trash.ToIconString()}##{key}")) {
                            Setup.SetupByType(key, type, true);
                            Writer.Remove(key);
                            Service.Config.Save();
                        }
                    }
                    if (ImGui.Checkbox("##" + key, ref LocalWriter.Enabled)) {
                        enablechanges = UiGlobals.CheckIsEnabled(Service.Config.EnableReplacement, _activeSet.Enabled, LocalWriter.Enabled);
                        if (enablechanges) Setup.SetupByType(key, type); 
                        else Setup.SetupByType(key, type, true);
                        Service.Config.Save();
                    }
                    ImGui.SameLine();
                    ImGui.TextUnformatted("Entry Enabled");
                    UiGlobals.DrawItemSeparator();



                    
                    #region Datainputs
                    UiGlobals.DrawUInt("Ride BGM Index", type, key, enablechanges, ref LocalWriter.Replacement.BGMIndex, old.BGMIndex, true, ["BGM_Path"]);
                    UiGlobals.DrawString("Ride BGM Path", type, key, enablechanges, ref LocalWriter.Replacement.BGMPath, old.BGMPath);
                    UiGlobals.DrawInt("Rise/Sink Tilt Angle", type, key, enablechanges, ref LocalWriter.Replacement.RiseSinkTilt, old.RiseSinkTilt);
                    UiGlobals.DrawInt("Ground Animation Speed", type, key, enablechanges, ref LocalWriter.Replacement.GroundAnimationSpeed, old.GroundAnimationSpeed);
                    UiGlobals.DrawInt("Flight Animation Speed", type, key, enablechanges, ref LocalWriter.Replacement.FlightAnimationSpeed, old.FlightAnimationSpeed);
                    UiGlobals.DrawInt("Swim Animation Speed", type, key, enablechanges, ref LocalWriter.Replacement.SwimAnimationSpeed, old.SwimAnimationSpeed);
                    UiGlobals.DrawItemSeparator();
                    UiGlobals.DrawFloat("Male Midlander Model Scale", type, key, enablechanges, ref LocalWriter.Replacement.Scale.MaleMidlander, old.Scale.MaleMidlander);
                    UiGlobals.DrawFloat("Female Midlander Model Scale", type, key, enablechanges, ref LocalWriter.Replacement.Scale.FemaleMidlander, old.Scale.MaleMidlander);
                    UiGlobals.DrawFloat("Male Highlander Model Scale", type, key, enablechanges, ref LocalWriter.Replacement.Scale.MaleHighlander, old.Scale.MaleHighlander);
                    UiGlobals.DrawFloat("Female Highlander Model Scale", type, key, enablechanges, ref LocalWriter.Replacement.Scale.FemaleHighlander, old.Scale.FemaleHighlander);
                    UiGlobals.DrawFloat("Male Elezen Model Scale", type, key, enablechanges, ref LocalWriter.Replacement.Scale.MaleElezen, old.Scale.MaleElezen);
                    UiGlobals.DrawFloat("Female Elezen Model Scale", type, key, enablechanges, ref LocalWriter.Replacement.Scale.FemaleElezen, old.Scale.FemaleElezen);
                    UiGlobals.DrawFloat("Male Lalafell Model Scale", type, key, enablechanges, ref LocalWriter.Replacement.Scale.MaleLalafell, old.Scale.MaleLalafell);
                    UiGlobals.DrawFloat("Female Lalafell Model Scale", type, key, enablechanges, ref LocalWriter.Replacement.Scale.FemaleLalafell, old.Scale.FemaleLalafell);
                    UiGlobals.DrawFloat("Male Miqote Model Scale", type, key, enablechanges, ref LocalWriter.Replacement.Scale.MaleMiqote, old.Scale.MaleMiqote);
                    UiGlobals.DrawFloat("Female Miqote Model Scale", type, key, enablechanges, ref LocalWriter.Replacement.Scale.FemaleMiqote, old.Scale.FemaleMiqote);
                    UiGlobals.DrawFloat("Male Roegadyn Model Scale", type, key, enablechanges, ref LocalWriter.Replacement.Scale.MaleRoegadyn, old.Scale.MaleRoegadyn);
                    UiGlobals.DrawFloat("Female Roegadyn Model Scale", type, key, enablechanges, ref LocalWriter.Replacement.Scale.FemaleRoegadyn, old.Scale.FemaleRoegadyn);
                    UiGlobals.DrawFloat("Male AuRa Model Scale", type, key, enablechanges, ref LocalWriter.Replacement.Scale.MaleAura, old.Scale.MaleAura);
                    UiGlobals.DrawFloat("Female AuRa Model Scale", type, key, enablechanges, ref LocalWriter.Replacement.Scale.FemaleAura, old.Scale.FemaleAura);
                    UiGlobals.DrawFloat("Male Hrothgar Model Scale", type, key, enablechanges, ref LocalWriter.Replacement.Scale.MaleHrothgar, old.Scale.MaleHrothgar);
                    UiGlobals.DrawFloat("Female Hrothgar Model Scale", type, key, enablechanges, ref LocalWriter.Replacement.Scale.FemaleHrothgar, old.Scale.FemaleHighlander);
                    UiGlobals.DrawFloat("Male Viera Model Scale", type, key, enablechanges, ref LocalWriter.Replacement.Scale.MaleViera, old.Scale.MaleViera);
                    UiGlobals.DrawFloat("Female Viera Model Scale", type, key, enablechanges, ref LocalWriter.Replacement.Scale.FemaleViera, old.Scale.FemaleViera);
                    UiGlobals.DrawItemSeparator();
                    UiGlobals.DrawFloat("Male Midlander Model Camera Scale", type, key, enablechanges, ref LocalWriter.Replacement.CameraScale.MaleMidlander, old.CameraScale.MaleMidlander);
                    UiGlobals.DrawFloat("Female Midlander Model Camera Scale", type, key, enablechanges, ref LocalWriter.Replacement.CameraScale.FemaleMidlander, old.CameraScale.MaleMidlander);
                    UiGlobals.DrawFloat("Male Highlander Model Camera Scale", type, key, enablechanges, ref LocalWriter.Replacement.CameraScale.MaleHighlander, old.CameraScale.MaleHighlander);
                    UiGlobals.DrawFloat("Female Highlander Model Camera Scale", type, key, enablechanges, ref LocalWriter.Replacement.CameraScale.FemaleHighlander, old.CameraScale.FemaleHighlander);
                    UiGlobals.DrawFloat("Male Elezen Model Camera Scale", type, key, enablechanges, ref LocalWriter.Replacement.CameraScale.MaleElezen, old.CameraScale.MaleElezen);
                    UiGlobals.DrawFloat("Female Elezen Model Camera Scale", type, key, enablechanges, ref LocalWriter.Replacement.CameraScale.FemaleElezen, old.CameraScale.FemaleElezen);
                    UiGlobals.DrawFloat("Male Lalafell Model Camera Scale", type, key, enablechanges, ref LocalWriter.Replacement.CameraScale.MaleLalafell, old.CameraScale.MaleLalafell);
                    UiGlobals.DrawFloat("Female Lalafell Model Camera Scale", type, key, enablechanges, ref LocalWriter.Replacement.CameraScale.FemaleLalafell, old.CameraScale.FemaleLalafell);
                    UiGlobals.DrawFloat("Male Miqote Model Camera Scale", type, key, enablechanges, ref LocalWriter.Replacement.CameraScale.MaleMiqote, old.CameraScale.MaleMiqote);
                    UiGlobals.DrawFloat("Female Miqote Model Camera Scale", type, key, enablechanges, ref LocalWriter.Replacement.CameraScale.FemaleMiqote, old.CameraScale.FemaleMiqote);
                    UiGlobals.DrawFloat("Male Roegadyn Model Camera Scale", type, key, enablechanges, ref LocalWriter.Replacement.CameraScale.MaleRoegadyn, old.CameraScale.MaleRoegadyn);
                    UiGlobals.DrawFloat("Female Roegadyn Model Camera Scale", type, key, enablechanges, ref LocalWriter.Replacement.CameraScale.FemaleRoegadyn, old.CameraScale.FemaleRoegadyn);
                    UiGlobals.DrawFloat("Male AuRa Model Camera Scale", type, key, enablechanges, ref LocalWriter.Replacement.CameraScale.MaleAura, old.CameraScale.MaleAura);
                    UiGlobals.DrawFloat("Female AuRa Model Camera Scale", type, key, enablechanges, ref LocalWriter.Replacement.CameraScale.FemaleAura, old.CameraScale.FemaleAura);
                    UiGlobals.DrawFloat("Male Hrothgar Model Camera Scale", type, key, enablechanges, ref LocalWriter.Replacement.CameraScale.MaleHrothgar, old.CameraScale.MaleHrothgar);
                    UiGlobals.DrawFloat("Female Hrothgar Model Camera Scale", type, key, enablechanges, ref LocalWriter.Replacement.CameraScale.FemaleHrothgar, old.CameraScale.FemaleHighlander);
                    UiGlobals.DrawFloat("Male Viera Model Camera Scale", type, key, enablechanges, ref LocalWriter.Replacement.CameraScale.MaleViera, old.CameraScale.MaleViera);
                    UiGlobals.DrawFloat("Female Viera Model Camera Scale", type, key, enablechanges, ref LocalWriter.Replacement.CameraScale.FemaleViera, old.CameraScale.FemaleViera);
                    UiGlobals.DrawItemSeparator();
                    UiGlobals.DrawByte("Mount Ground Tilt Origin", type, key, enablechanges, ref LocalWriter.Replacement.MountGroundTilt.TiltOrigin, old.MountGroundTilt.TiltOrigin);
                    UiGlobals.DrawByte("Mount Ground Unknown1", type, key, enablechanges, ref LocalWriter.Replacement.MountGroundTilt.Unk1, old.MountGroundTilt.Unk1);
                    UiGlobals.DrawByte("Mount Ground Unknown2", type, key, enablechanges, ref LocalWriter.Replacement.MountGroundTilt.Unk2, old.MountGroundTilt.Unk2);
                    UiGlobals.DrawFloat("Mount Ground Tilt Angle", type, key, enablechanges, ref LocalWriter.Replacement.MountGroundTilt.TiltAngle, old.MountGroundTilt.TiltAngle);
                    UiGlobals.DrawFloat("Mount Ground Tilt Speed", type, key, enablechanges, ref LocalWriter.Replacement.MountGroundTilt.TiltSpeed, old.MountGroundTilt.TiltSpeed);
                    UiGlobals.DrawBool("Mount Ground Reverse Tilt", type, key, enablechanges, ref LocalWriter.Replacement.MountGroundTilt.ReverseRotation, old.MountGroundTilt.ReverseRotation);
                    UiGlobals.DrawItemSeparator();
                    UiGlobals.DrawByte("Mount Flight Tilt Origin", type, key, enablechanges, ref LocalWriter.Replacement.MountFlightTilt.TiltOrigin, old.MountFlightTilt.TiltOrigin);
                    UiGlobals.DrawByte("Mount Flight Unknown1", type, key, enablechanges, ref LocalWriter.Replacement.MountFlightTilt.Unk1, old.MountFlightTilt.Unk1);
                    UiGlobals.DrawByte("Mount Flight Unknown2", type, key, enablechanges, ref LocalWriter.Replacement.MountFlightTilt.Unk2, old.MountFlightTilt.Unk2);
                    UiGlobals.DrawFloat("Mount Flight Tilt Angle", type, key, enablechanges, ref LocalWriter.Replacement.MountFlightTilt.TiltAngle, old.MountFlightTilt.TiltAngle);
                    UiGlobals.DrawFloat("Mount Flight Tilt Speed", type, key, enablechanges, ref LocalWriter.Replacement.MountFlightTilt.TiltSpeed, old.MountFlightTilt.TiltSpeed);
                    UiGlobals.DrawBool("Mount Flight Reverse Tilt", type, key, enablechanges, ref LocalWriter.Replacement.MountFlightTilt.ReverseRotation, old.MountFlightTilt.ReverseRotation);
                    UiGlobals.DrawItemSeparator();
                    UiGlobals.DrawByte("Rider Ground Tilt Origin", type, key, enablechanges, ref LocalWriter.Replacement.RiderGroundTilt.TiltOrigin, old.RiderGroundTilt.TiltOrigin);
                    UiGlobals.DrawByte("Rider Ground Unknown1", type, key, enablechanges, ref LocalWriter.Replacement.RiderGroundTilt.Unk1, old.RiderGroundTilt.Unk1);
                    UiGlobals.DrawByte("Rider Ground Unknown2", type, key, enablechanges, ref LocalWriter.Replacement.RiderGroundTilt.Unk2, old.RiderGroundTilt.Unk2);
                    UiGlobals.DrawFloat("Rider Ground Tilt Angle", type, key, enablechanges, ref LocalWriter.Replacement.RiderGroundTilt.TiltAngle, old.RiderGroundTilt.TiltAngle);
                    UiGlobals.DrawFloat("Rider Ground Tilt Speed", type, key, enablechanges, ref LocalWriter.Replacement.RiderGroundTilt.TiltSpeed, old.RiderGroundTilt.TiltSpeed);
                    UiGlobals.DrawBool("Rider Ground Reverse Tilt", type, key, enablechanges, ref LocalWriter.Replacement.RiderGroundTilt.ReverseRotation, old.RiderGroundTilt.ReverseRotation);
                    UiGlobals.DrawItemSeparator();
                    UiGlobals.DrawByte("Rider Flight Tilt Origin", type, key, enablechanges, ref LocalWriter.Replacement.RiderFlightTilt.TiltOrigin, old.RiderFlightTilt.TiltOrigin);
                    UiGlobals.DrawByte("Rider Flight Unknown1", type, key, enablechanges, ref LocalWriter.Replacement.RiderFlightTilt.Unk1, old.RiderFlightTilt.Unk1);
                    UiGlobals.DrawByte("Rider Flight Unknown2", type, key, enablechanges, ref LocalWriter.Replacement.RiderFlightTilt.Unk2, old.RiderFlightTilt.Unk2);
                    UiGlobals.DrawFloat("Rider Flight Tilt Angle", type, key, enablechanges, ref LocalWriter.Replacement.RiderFlightTilt.TiltAngle, old.RiderFlightTilt.TiltAngle);
                    UiGlobals.DrawFloat("Rider Flight Tilt Speed", type, key, enablechanges, ref LocalWriter.Replacement.RiderFlightTilt.TiltSpeed, old.RiderFlightTilt.TiltSpeed);
                    UiGlobals.DrawBool("Rider Flight Reverse Tilt", type, key, enablechanges, ref LocalWriter.Replacement.RiderFlightTilt.ReverseRotation, old.RiderFlightTilt.ReverseRotation);
                    UiGlobals.DrawItemSeparator();

                    UiGlobals.DrawItemSeparator();
                    continue;

                    #endregion

                }
            }


#endregion
            #region Search/Set

            using var searchMenu = ImRaii.Popup(searchPopup);
            if (searchMenu)
            {
                ImGui.SetNextItemWidth(200);
                if (ImGui.InputText("##Search " + typenameplural.ToLower(), ref search, 256)) { localsearch = search; }
                ImGui.SameLine();
                using (ImRaii.PushFont(UiBuilder.IconFont))
                {
                    if (ImGui.Button($"{FontAwesomeIcon.Undo.ToIconString()}##{typenameplural.ToLower()}searchreset"))
                    {
                        search = "";
                        localsearch = search;
                    }
                }

                using var popupChild = ImRaii.Child(searchPopup, CalcGlobals.SearchPopScale(), true);
                if (localsearch == "" && localsearch != lastsearch) { SearchList = UiGlobals.CreateSearchList(type); }
                if (localsearch != lastsearch)
                {
                    var tempSearchList = UiGlobals.CreateSearchList(type).OrderBy(i => {
                        if (string.IsNullOrEmpty(localsearch)) return 0;
                        return Math.Min(ProcessingGlobals.ScoreString(i.Value, localsearch),
                            ProcessingGlobals.ScoreString(i.Key.ToString(), localsearch));
                    });
                    SearchList = tempSearchList.ToDictionary<uint, string>();
                    lastsearch = localsearch;
                }
                foreach (var pair in SearchList)
                {
                    if (ImGui.Selectable($"#{pair.Key:D5} {pair.Value}"))
                    {
                        Writer[pair.Key] = CreateEntry(pair.Key);
                        Service.Config.Save();
                    }
                }
            }
            #endregion
        }
    }
}
