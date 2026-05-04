using ActionTimelineReplacement;
using ActionTimelineReplacement.Base.Setups;
using ActionTimelineReplacement.Sheets;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface;
using Dalamud.Interface.Utility.Raii;
using FFXIVClientStructs.FFXIV.Common.Lua;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using static FFXIVClientStructs.FFXIV.Client.UI.Misc.AozNoteModule;

namespace ActionTimelineReplacement.Base.Global;

public class UiGlobals
{
    public static void DrawAddItem(string searchname)
    {
        if (ImGui.Button(" + "))
        {
            ImGui.OpenPopup(searchname);
        }
        else
        {
            return;
        }
    }

    public static void DrawItemSeparator()
    {
        ImGui.NewLine();
        ImGui.Separator();
        ImGui.NewLine();
    }

    public static void DrawString(string name, string type, uint key, bool changesenabled, ref string value,
    string defaultvalue)
    {
        string refname = name.Replace(" ", "");
        ImGui.TextUnformatted(name);
        ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
        ImGui.SetNextItemWidth(110 * CalcGlobals.GlobalScale());
        if (ImGui.InputText("##" + refname + key, ref value))
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
    }

    public static void DrawSByte(string name, string type, uint key, bool changesenabled, ref sbyte value,
    sbyte defaultvalue)
    {
        string refname = name.Replace(" ", "");
        ImGui.TextUnformatted(name);
        ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
        ImGui.SetNextItemWidth(110 * CalcGlobals.GlobalScale());
        if (ImGui.InputSByte("##" + refname + key, ref value))
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
    }


    public static void DrawByte(string name, string type, uint key, bool changesenabled, ref byte value,
    byte defaultvalue)
    {
        string refname = name.Replace(" ", "");
        ImGui.TextUnformatted(name);
        ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
        ImGui.SetNextItemWidth(110 * CalcGlobals.GlobalScale());
        if (ImGui.InputByte("##" + refname + key, ref value))
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
    }
    public static void DrawShort(string name, string type, uint key, bool changesenabled, ref short value,
    short defaultvalue)
    {
        string refname = name.Replace(" ", "");
        ImGui.TextUnformatted(name);
        ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
        ImGui.SetNextItemWidth(110 * CalcGlobals.GlobalScale());
        if (ImGui.InputShort("##" + refname + key, ref value))
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
    }
    public static void DrawUShort(string name, string type, uint key, bool changesenabled, ref ushort value,
    ushort defaultvalue)
    {
        string refname = name.Replace(" ", "");
        ImGui.TextUnformatted(name);
        ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
        ImGui.SetNextItemWidth(110 * CalcGlobals.GlobalScale());
        if (ImGui.InputUShort("##" + refname + key, ref value))
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
    }
    public static void DrawInt(string name, string type, uint key, bool changesenabled, ref int value,
    int defaultvalue)
    {
        string refname = name.Replace(" ", "");
        ImGui.TextUnformatted(name);
        ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
        ImGui.SetNextItemWidth(110 * CalcGlobals.GlobalScale());
        if (ImGui.InputInt("##" + refname + key, ref value))
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
    }
    public static void DrawUInt(string name, string type, uint key, bool changesenabled, ref uint value,
    uint defaultvalue)
    {
        string refname = name.Replace(" ", "");
        ImGui.TextUnformatted(name);
        ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
        ImGui.SetNextItemWidth(110 * CalcGlobals.GlobalScale());
        if (ImGui.InputUInt("##" + refname + key, ref value))
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
    }
    public static void DrawBool(string name, string type, uint key, bool changesenabled, ref bool value,
    bool defaultvalue)
    {
        string refname = name.Replace(" ", "");
        ImGui.TextUnformatted(name);
        ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
        ImGui.SetNextItemWidth(110 * CalcGlobals.GlobalScale());
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
    }
    

    public static Dictionary<uint, string> CreateSearchList(string type)
    {
        Dictionary<uint, string> output = [];
        switch (type)
        {
            case "Action":
                output = ActionManager.Names;
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
            case "Vfx":
                output = VfxManager.Names;
                return output;
            default:
                Service.Log.Error("Datasheet type [{type}] is not defined in CreateSearchList", type);
                return output;
        }
    }
}