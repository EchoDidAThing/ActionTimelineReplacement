using ActionTimelineReplacement;
using ActionTimelineReplacement.Base.Setups;
using ActionTimelineReplacement.Interface;
using ActionTimelineReplacement.Sheets;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;
using FFXIVClientStructs.FFXIV.Common.Lua;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.InteropServices;
using static FFXIVClientStructs.FFXIV.Client.UI.Misc.AozNoteModule;

namespace ActionTimelineReplacement.Base.Global;

public class UiGlobals
{
    public static void DrawAddItem(string searchname)
    {

        using (ImRaii.PushFont(UiBuilder.IconFont))
        {
            if (ImGui.Button($"{FontAwesomeIcon.Plus.ToIconString()}"))
            {
                ImGui.OpenPopup(searchname);
            }
            else
            {
                return;
            }
        }
    }

    public static void DrawItemSeparator()
    {
        ImGui.NewLine();
        ImGui.Separator();
        ImGui.NewLine();
    }

    public static void DrawSheetButton(string mainkey, ref string activesheet, Configuration.ReplacementSet activeset)
    {
        uint countvalue = GetSheetCounts(mainkey, activeset);
        string buttontext = mainkey;
        if (countvalue >=1)
        {
            buttontext += " [" + countvalue + "]"; 
        }
        if (activesheet == mainkey)
        {
            buttontext = "<<" + buttontext + ">>";
        }
        if (ImGui.Button(buttontext))
        {
            activesheet = mainkey;
        }
    }


    public static void DrawString(string name, string type, uint key, bool changesenabled, ref string value,
    string defaultvalue, bool sheethasindirects = false, string indirecttype = "NO")
    {
        string refname = name.Replace(" ", "");
        if (ImGui.InputText("##" + refname + key, ref value, 512, ImGuiInputTextFlags.EnterReturnsTrue))
        {
            if (changesenabled) { Setup.SetupByType(key, type); }
            Service.Config.Save();
        }
        ImGui.SameLine();

        using (ImRaii.PushFont(UiBuilder.IconFont))
        {
            if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{refname}{key}"))
            {
                value = defaultvalue;
                if (changesenabled) { Setup.SetupByType(key, type); }
                Service.Config.Save();
            }
        }
        ImGui.SameLine();
        ImGui.TextUnformatted(name);
    }

    public static void DrawSByte(string name, string type, uint key, bool changesenabled, ref sbyte value,
    sbyte defaultvalue, bool sheethasindirects = false, string indirecttype = "NO")
    {
        string refname = name.Replace(" ", "");
        if (ImGui.InputSByte("##" + refname + key, ref value, 0, 0, default, ImGuiInputTextFlags.EnterReturnsTrue))
        {
            if (changesenabled) { Setup.SetupByType(key, type); }
            Service.Config.Save();
        }
        ImGui.SameLine();

        using (ImRaii.PushFont(UiBuilder.IconFont))
        {
            if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{refname}{key}"))
            {
                value = defaultvalue;
                if (changesenabled) { Setup.SetupByType(key, type); }
                Service.Config.Save();
            }
        }
        ImGui.SameLine();
        ImGui.TextUnformatted(name);
        if (sheethasindirects)
        {
            if (indirecttype == "NO")
            {
                ImGui.Text("");
            }
            else
            {
                ImGui.Text(GetIndirectString(indirecttype, (uint)value));
            }
            ImGui.SameLine();
            using (ImRaii.PushFont(UiBuilder.IconFont))
            {
                if (ImGui.Button($"{FontAwesomeIcon.ClipboardList.ToIconString()}##{refname}{key}"))
                {
                    ImGui.SetClipboardText(GetIndirectString(indirecttype, (uint)value));
                }
            }
        }
    }


    public static void DrawByte(string name, string type, uint key, bool changesenabled, ref byte value,
    byte defaultvalue, bool sheethasindirects = false, string indirecttype = "NO")
    {
        string indirecttext = "";
        string refname = name.Replace(" ", "");
        if (ImGui.InputByte("##" + refname + key, ref value, 0, 0, default, ImGuiInputTextFlags.EnterReturnsTrue))
        {
            if (changesenabled) { Setup.SetupByType(key, type); }
            Service.Config.Save();
        }
        ImGui.SameLine();

        using (ImRaii.PushFont(UiBuilder.IconFont))
        {
            if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{refname}{key}"))
            {
                value = defaultvalue;
                if (changesenabled) { Setup.SetupByType(key, type); }
                Service.Config.Save();
            }
        }
        ImGui.SameLine();
        ImGui.TextUnformatted(name);
        if (sheethasindirects)
        {
            if (indirecttype == "NO")
            {
                ImGui.Text("");
            }
            else
            {
                ImGui.Text(GetIndirectString(indirecttype, value));
            }
            ImGui.SameLine();
            using (ImRaii.PushFont(UiBuilder.IconFont))
            {
                if (ImGui.Button($"{FontAwesomeIcon.ClipboardList.ToIconString()}##{refname}{key}"))
                {
                    ImGui.SetClipboardText(GetIndirectString(indirecttype, value));
                }
            }
        }
    }
    public static void DrawShort(string name, string type, uint key, bool changesenabled, ref short value,
    short defaultvalue, bool sheethasindirects = false, string indirecttype = "NO")
    {
        string refname = name.Replace(" ", "");
        short relay = value;
        if (ImGui.InputShort("##" + refname + key, ref relay, 0, 0, default, ImGuiInputTextFlags.EnterReturnsTrue))
        {
            value = relay;
            if (changesenabled) { Setup.SetupByType(key, type); }
            Service.Config.Save();
        }
        ImGui.SameLine();

        using (ImRaii.PushFont(UiBuilder.IconFont))
        {
            if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{refname}{key}"))
            {
                value = defaultvalue;
                if (changesenabled) { Setup.SetupByType(key, type); }
                Service.Config.Save();
            }
        }
        ImGui.SameLine();
        ImGui.TextUnformatted(name);
        if (sheethasindirects)
        {
            if (indirecttype == "NO")
            {
                ImGui.Text("");
            }
            else
            {
                ImGui.Text(GetIndirectString(indirecttype, (uint)value));
            }
            ImGui.SameLine();
            using (ImRaii.PushFont(UiBuilder.IconFont))
            {
                if (ImGui.Button($"{FontAwesomeIcon.ClipboardList.ToIconString()}##{refname}{key}"))
                {
                    ImGui.SetClipboardText(GetIndirectString(indirecttype, (uint)value));
                }
            }
        }
    }
    public static void DrawUShort(string name, string type, uint key, bool changesenabled, ref ushort value, ushort defaultvalue, bool sheethasindirects = false, string indirecttype = "NO")
    {
        string refname = name.Replace(" ", "");
        ushort relay = value;
        if (ImGui.InputUShort("##" + refname + key, ref relay, 0, 0, default, ImGuiInputTextFlags.EnterReturnsTrue))
        {
            value = relay;
            if (changesenabled) { Setup.SetupByType(key, type); }
            Service.Config.Save();
        }
        ImGui.SameLine();

        using (ImRaii.PushFont(UiBuilder.IconFont))
        {
            if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{refname}{key}"))
            { 
                value = defaultvalue;

                if (changesenabled) { Setup.SetupByType(key, type); }
                Service.Config.Save();
            }
        }
        ImGui.SameLine();
        ImGui.TextUnformatted(name);
        if (sheethasindirects)
        {
            using (ImRaii.PushFont(UiBuilder.IconFont))
            ImGui.Text($"{FontAwesomeIcon.ArrowTurnUp.ToIconString()}");
            ImGui.SameLine();
            if (indirecttype == "NO")
            {
                ImGui.Text("");
            }
            else
            {
                ImGui.Text(GetIndirectString(indirecttype, (uint)value));
            }
            ImGui.SameLine();
            using (ImRaii.PushFont(UiBuilder.IconFont))
            {
                if (ImGui.Button($"{FontAwesomeIcon.ClipboardList.ToIconString()}##{refname}{key}"))
                {
                    ImGui.SetClipboardText(GetIndirectString(indirecttype, (uint)value));
                }
            }
        }
    }
    public static void DrawInt(string name, string type, uint key, bool changesenabled, ref int value,
    int defaultvalue, bool sheethasindirects = false, string indirecttype = "NO")
    {
        string refname = name.Replace(" ", "");
        if (ImGui.InputInt("##" + refname + key, ref value, 0, 0, default, ImGuiInputTextFlags.EnterReturnsTrue))
        {
            if (changesenabled) { Setup.SetupByType(key, type); }
            Service.Config.Save();
        }
        ImGui.SameLine();

        using (ImRaii.PushFont(UiBuilder.IconFont))
        {
            if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{refname}{key}"))
            {
                value = defaultvalue;
                if (changesenabled) { Setup.SetupByType(key, type); }
                Service.Config.Save();
            }
        }
        ImGui.SameLine();
        ImGui.TextUnformatted(name);
        if (sheethasindirects)
        {
            if (indirecttype == "NO")
            {
                ImGui.Text("");
            }
            else
            {
                ImGui.Text(GetIndirectString(indirecttype, (uint)value));
            }
            ImGui.SameLine();
            using (ImRaii.PushFont(UiBuilder.IconFont))
            {
                if (ImGui.Button($"{FontAwesomeIcon.ClipboardList.ToIconString()}##{refname}{key}"))
                {
                    ImGui.SetClipboardText(GetIndirectString(indirecttype, (uint)value));
                }
            }
        }
    }
    public static void DrawUInt(string name, string type, uint key, bool changesenabled, ref uint value,
    uint defaultvalue, bool sheethasindirects = false, string indirecttype = "NO")
    {
        string refname = name.Replace(" ", "");
        if (ImGui.InputUInt("##" + refname + key, ref value, 0, 0, default, ImGuiInputTextFlags.EnterReturnsTrue))
        {
            if (changesenabled) { Setup.SetupByType(key, type); }
            Service.Config.Save();
        }
        ImGui.SameLine();

        using (ImRaii.PushFont(UiBuilder.IconFont))
        {
            if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{refname}{key}"))
            {
                value = defaultvalue;
                if (changesenabled) { Setup.SetupByType(key, type); }
                Service.Config.Save();
            }
        }
        ImGui.SameLine();
        ImGui.TextUnformatted(name);
        if (sheethasindirects)
        {
            if (indirecttype == "NO")
            {
                ImGui.Text("");
            }
            else
            {
                ImGui.Text(GetIndirectString(indirecttype, (uint)value));
            }
            ImGui.SameLine();
            using (ImRaii.PushFont(UiBuilder.IconFont))
            {
                if (ImGui.Button($"{FontAwesomeIcon.ClipboardList.ToIconString()}##{refname}{key}"))
                {
                    ImGui.SetClipboardText(GetIndirectString(indirecttype, (uint)value));
                }
            }
        }
    }
    public static void DrawBool(string name, string type, uint key, bool changesenabled, ref bool value,
    bool defaultvalue, bool sheethasindirects = false, string indirecttype = "NO")
    {
        string refname = name.Replace(" ", "");
        if (ImGui.Checkbox("##" + refname + key, ref value))
        {
            if (changesenabled) { Setup.SetupByType(key, type); }
            Service.Config.Save();
        }
        ImGui.SameLine();

        using (ImRaii.PushFont(UiBuilder.IconFont))
        {
            if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{refname}{key}"))
            {
                value = defaultvalue;
                if (changesenabled) { Setup.SetupByType(key, type); }
                Service.Config.Save();
            }
        }
        ImGui.SameLine();
        ImGui.TextUnformatted(name);
    }


    public static Dictionary<uint, string> CreateSearchList(string type)
    {
        Dictionary<uint, string> output = [];
        switch (type)
        {
            case "Action":
                output = ActionManager.Names;
                return output;
            case "ActionCastVFX":
                output = ActionCastVFXManager.Names;
                return output;
            case "ActionTimeline":
                output = ActionTimelineManager.Names;
                return output;
            case "Glasses":
                output = GlassesManager.Names;
                return output;
            case "GlassesStyle":
                output = GlassesStyleManager.Names;
                return output;
            case "Mount":
                output = MountManager.Names;
                return output;
            case "MountCustomize":
                output = MountCustomizeManager.Names;
                return output;
            case "Ornament":
                output = OrnamentManager.Names;
                return output;
            case "OrnamentCustomize":
                output = OrnamentCustomizeManager.Names;
                return output;
            case "OrnamentCustomizeGroup":
            //output = OrnamentCustomizeGroupManager.Names;
            //return output;
            case "Placename":
            //output = PlaceNameManager.Names;
            //return output;
            case "PointMenuChoice":
            //output = PointMenuManager.Names;
            //return output;
            case "Status":
                output = StatusManager.Names;
                return output;
            case "StatusHitEffect":
                output = StatusHitEffectManager.Names;
                return output;
            case "StatusLoopVFX":
                output = StatusLoopVFXManager.Names;
                return output;
            case "TiltParam":
                output = TiltParamManager.Names;
                return output;
            case "VFX":
                output = VfxManager.Names;
                return output;
            default:
                Service.Log.Error("Datasheet type [{type}] is not defined in CreateSearchList", type);
                return output;
        }
    }
    public static uint GetSheetCounts(string type, Configuration.ReplacementSet activeset)
    {
       uint output = 0;
        switch (type)
        {
            case "Action":
                output = (uint)activeset.ActionWriter.Count();
                return output;
            case "ActionCastVFX":
                output = (uint)activeset.ActionCastVFXWriter.Count();
                return output;
            case "ActionTimeline":
                output = (uint)activeset.ActionTimelineWriter.Count();
                return output;
            case "Glasses":
                output = (uint)activeset.GlassesWriter.Count();
                return output;
            case "GlassesStyle":
                output = (uint)activeset.GlassesStyleWriter.Count();
                return output;
            case "Mount":
                output = (uint)activeset.MountWriter.Count();
                return output;
            case "MountCustomize":
                output = (uint)activeset.MountCustomizeWriter.Count();
                return output;
            case "Ornament":
                output = (uint)activeset.OrnamentWriter.Count();
                return output;
            case "OrnamentCustomize":
                output = (uint)activeset.OrnamentCustomizeWriter.Count();
                return output;
            case "OrnamentCustomizeGroup":
            //output = OrnamentCustomizeGroupManager.Names;
            //return output;
            case "Placename":
            //output = PlaceNameManager.Names;
            //return output;
            case "PointMenuChoice":
            //output = PointMenuManager.Names;
            //return output;
            case "Status":
                output = (uint)activeset.StatusWriter.Count();
                return output;
            case "StatusHitEffect":
                output = (uint)activeset.StatusHitEffectWriter.Count();
                return output;
            case "StatusLoopVFX":
                output = (uint)activeset.StatusLoopVFXWriter.Count();
                return output;
            case "TiltParam":
                output = (uint)activeset.TiltParamWriter.Count();
                return output;
            case "VFX":
                output = (uint)activeset.VfxWriter.Count();
                return output;
            default:
                Service.Log.Error("Datasheet type [{type}] is not defined in GetSheetCounts", type);
                return output;
        }
    }

    public static String GetIndirectString(string type, uint key)
    {
        string output = "";
        switch (type)
        {
            case "VFX":
                output = "vfx/common/" + VfxManager.GetReplacement(key).String1 + ".avfx";
                return output;
            case "ActionCastVFX-VFX":
                output = "vfx/common/" + VfxManager.GetReplacement(ActionCastVFXManager.GetReplacement(key).CastVfx).String1 + ".avfx";
                return output;
            case "StatusHitEffect-VFX":
                output = "vfx/common/" + VfxManager.GetReplacement(StatusHitEffectManager.GetReplacement(key).VFX).String1 + ".avfx";
                return output;
            case "StatusLoopVFX-VFX":
                output = "vfx/common/" + VfxManager.GetReplacement(StatusLoopVFXManager.GetReplacement(key).FriendlyVFX).String1 + ".avfx";
                return output;
            case "ActionTimeline":
                output = ActionTimelineManager.GetReplacement(key).Animation;
                return output;
            default:
                Service.Log.Error("Datasheet type [{type}] is not defined in GetIndirectString", type);
                return output;
        }
    }
}