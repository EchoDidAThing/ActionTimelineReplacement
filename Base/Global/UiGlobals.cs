using ActionTimelineReplacement;
using ActionTimelineReplacement.Base.Setups;
using ActionTimelineReplacement.Interface;
using ActionTimelineReplacement.Sheets;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;
using FFXIVClientStructs.FFXIV.Common.Lua;
using Lumina.Excel.Sheets;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using static FFXIVClientStructs.FFXIV.Client.UI.Misc.AozNoteModule;

namespace ActionTimelineReplacement.Base.Global;

public class UiGlobals
{
    public static readonly Vector4 RED_COLOR = new(0.89098039216f, 0.30549019608f, 0.28980392157f, 1.0f);

    public static readonly Vector4 GREEN_COLOR = new(0.36078431373f, 0.72156862745f, 0.36078431373f, 1.0f);

    public static readonly Vector4 DARK_GREEN = new(0.0f, 0.493f, 0.019f, 1.0f);
    public static readonly Vector4 FADED_DARK_GREEN = new(0.0f, 0.493f, 0.019f, .5f);

    public static readonly Vector4 DARK_GRAY = new(0.21764705882f, 0.21764705882f, 0.21764705882f, 1);

    public static readonly Vector4 FADED_WHITE = new(1f, 1f, 1f, .25f);
    public static readonly Vector4 LESS_FADED_WHITE = new(1f, 1f, 1f, .5f);
    public static readonly Vector4 FULL_WHITE = new(1f, 1f, 1f, 1f);
    public static readonly Vector4 NO_ALPHA = new(1f, 1f, 1f, 0f);
    public static readonly Vector4 BASE_ALPHA = new(1f, 1f, 1f, .1f);

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

    public static bool CheckIsEnabled(bool global, bool set, bool local)
    {
        if ((global) && (set) && (local))
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public static bool DrawDeleteEntryButton(string label, bool small = false) => ColorButton(label, RED_COLOR, small);

    public static bool ColorButton(string label, Vector4 color, bool small)
    {
        using var style = ImRaii.PushColor(ImGuiCol.Button, color);
        return small ? ImGui.SmallButton(label) : ImGui.Button(label);
    }
    
    public static bool DrawResetCellButton(string label, bool enabled, bool small = false) {
        ImRaii.ColorDisposable styletype = new ImRaii.ColorDisposable();
        styletype.Pop();
        if (!enabled) {
            styletype.Push(ImGuiCol.Text, FADED_WHITE);
            styletype.Push(ImGuiCol.ButtonHovered, NO_ALPHA);
        }
        using var style = styletype;
        return small ? ImGui.SmallButton(label) : ImGui.Button(label);
    }


    public static void DrawSheetButton(string mainkey, ref string activesheet, Configuration.ReplacementSet activeset)
    {
        ImRaii.ColorDisposable styletype = new ImRaii.ColorDisposable();
        styletype.Pop();
        uint countvalue = GetSheetCounts(mainkey, activeset);
        string buttontext = mainkey;
        if (countvalue >=1)
        {
            buttontext += " [" + countvalue + "]";
        }
        else
        {
            styletype.Pop();
            styletype.Push(ImGuiCol.Text, FADED_WHITE);
            styletype.Push(ImGuiCol.Button, NO_ALPHA);
        }
        if (mainkey == activesheet)
        {
            styletype.Pop();
            styletype.Push(ImGuiCol.Button, DARK_GREEN);
            styletype.Push(ImGuiCol.Text, FULL_WHITE);
        }
        else
        {
            styletype.Push(ImGuiCol.Button, BASE_ALPHA);
        }
        styletype.Push(ImGuiCol.ButtonHovered, FADED_DARK_GREEN);
        using var style = styletype;
        if (ImGui.Button(buttontext))
        {
            activesheet = mainkey;
        }
    }

    public static void ResolveIndirectDraw(List<string> indirecttype, string refname, uint key, uint curvalue)
    {
        bool copyenabled = false;
        foreach (string entry in indirecttype)
        {
            using (ImRaii.PushFont(UiBuilder.IconFont))
            ImGui.Text($"{FontAwesomeIcon.ArrowTurnUp.ToIconString()}");
            ImGui.SameLine();
            if (entry == "NO")
            {
                ImGui.Text("");
            }
            else
            {
                ImGui.Text(GetIndirectString(entry, (uint)curvalue, ref copyenabled));
            }
            if (copyenabled)
            {
                ImGui.SameLine();
                using (ImRaii.PushFont(UiBuilder.IconFont))
                {
                    if (ImGui.Button($"{FontAwesomeIcon.ClipboardList.ToIconString()}##{refname}{key}"))
                    {
                        ImGui.SetClipboardText(GetIndirectString(entry, (uint)curvalue, ref copyenabled));
                    }
                }
            }
        }
    }


    public static void DrawString(string name, string type, uint key, bool changesenabled, ref string value,
    string defaultvalue)
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
    sbyte defaultvalue, bool sheethasindirects = false, List<string> indirecttype = null)
    {
        if (indirecttype == null) { indirecttype = ["NO"]; }
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
            ResolveIndirectDraw(indirecttype, refname, key, (uint)value);
        }
    }


    public static void DrawByte(string name, string type, uint key, bool changesenabled, ref byte value,
    byte defaultvalue, bool sheethasindirects = false, List<string> indirecttype = null)
    {
        if (indirecttype == null) { indirecttype = ["NO"]; }
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
            ResolveIndirectDraw(indirecttype, refname, key, (uint)value);
        }
    }
    public static void DrawShort(string name, string type, uint key, bool changesenabled, ref short value,
    short defaultvalue, bool sheethasindirects = false, List<string> indirecttype = null)
    {
        if (indirecttype == null) { indirecttype = ["NO"]; }
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
            ResolveIndirectDraw(indirecttype, refname, key, (uint)value);
        }
    }
    public static void DrawUShort(string name, string type, uint key, bool changesenabled, ref ushort value, ushort defaultvalue, bool sheethasindirects = false, List<string> indirecttype = null)
    {
        if (indirecttype == null) { indirecttype = ["NO"]; }
        ushort DefaultValue = defaultvalue;
        string refname = name.Replace(" ", "");
        if (ImGui.InputUShort("##" + refname + key, ref value, 0, 0, default, ImGuiInputTextFlags.EnterReturnsTrue))
        {
            if (changesenabled) { Setup.SetupByType(key, type); }
            Service.Log.Error("old value is[{new}] new value is[{old}] ", defaultvalue,value);
            Service.Config.Save();
        }
        ImGui.SameLine();
        using (ImRaii.PushFont(UiBuilder.IconFont))
        {
            if (DrawResetCellButton($"{FontAwesomeIcon.Reply.ToIconString()}##{refname}{key}", value==defaultvalue))
            {
                value = DefaultValue;
                if (changesenabled) { Setup.SetupByType(key, type); }
                Service.Config.Save();
            }
        }

        ImGui.SameLine();
        ImGui.TextUnformatted(name);
        if (sheethasindirects)
        {
            ResolveIndirectDraw(indirecttype, refname, key, (uint)value);
        }
    }
    
    public static void DrawInt(string name, string type, uint key, bool changesenabled, ref int value,
    int defaultvalue, bool sheethasindirects = false, List<string> indirecttype = null)
    {
        if (indirecttype == null) { indirecttype = ["NO"]; }
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
            ResolveIndirectDraw(indirecttype, refname, key, (uint)value);
        }
    }
    public static void DrawUInt(string name, string type, uint key, bool changesenabled, ref uint value,
    uint defaultvalue, bool sheethasindirects = false, List<string> indirecttype = null)
    {
        if (indirecttype == null) { indirecttype = ["NO"]; }
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
            ResolveIndirectDraw(indirecttype, refname, key, (uint)value);
        }
    }
    public static void DrawBool(string name, string type, uint key, bool changesenabled, ref bool value,
    bool defaultvalue)
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

    public static String GetIndirectString(string type, uint key, ref bool copyenabled)
    {
        copyenabled = true;
        string output = "";
        switch (type)
        {
            case "VFX_Path":
                output = "vfx/common/" + VfxManager.GetReplacement(key).String1 + ".avfx";
                return output;
            case "ActionCastVFX-VFX_Path":
                output = "vfx/common/" + VfxManager.GetReplacement(ActionCastVFXManager.GetReplacement(key).CastVfx).String1 + ".avfx";
                return output;
            case "ActionCastVFX_Index":
                output = "ActionCastVFX Entry#" + key;
                copyenabled = false;
                return output;
            case "BGM_Path":
                //output = BGMManager.GetReplacement(key).String1;
                output = "BGM Not Implemented";
                return output;
            case "StatusHitEffect-VFX_Path":
                output = "vfx/common/" + VfxManager.GetReplacement(StatusHitEffectManager.GetReplacement(key).VFX).String1 + ".avfx";
                return output;
            case "StatusLoopVFX-VFX_Path":
                output = "vfx/common/" + VfxManager.GetReplacement(StatusLoopVFXManager.GetReplacement(key).FriendlyVFX).String1 + ".avfx";
                return output;
            case "TiltParam_Index":
                output = "TiltParam Entry#" + key;
                copyenabled = false;
                return output;
            case "ActionTimeline_Path":
                output = "chara/action/" + ActionTimelineManager.GetReplacement(key).Animation + ".tmb";
                return output;
            case "WeaponTimeline_Path":
                output = "chara/action/" + ActionTimelineManager.GetReplacement(key).Animation + ".tmb";
                return output;
            default:
                Service.Log.Error("Datasheet type [{type}] is not defined in GetIndirectString", type);
                return output;
        }
    }
}