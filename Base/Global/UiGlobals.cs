using ActionTimelineReplacement.Sheets;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface;
using Dalamud.Interface.Utility.Raii;
using ActionTimelineReplacement.Base.Setups;
using FFXIVClientStructs.FFXIV.Common.Lua;
using System;

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

    public static void DrawString(string name, string type, uint key, ref string value,
    string defaultvalue)
    {
        string refname = name.Replace(" ", "");
        ImGui.TextUnformatted(name);
        ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
        ImGui.SetNextItemWidth(110 * CalcGlobals.GlobalScale());
        if (ImGui.InputText("##" + refname + key, ref value))
        {
            Setup.SetupByType(key, type);
            Service.Config.Save();
        }
        ImGui.SameLine();

        using (ImRaii.PushFont(UiBuilder.IconFont))
        {
            if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{refname}{key}"))
            {
                value = defaultvalue;
                Setup.SetupByType(key, type);
                Service.Config.Save();
            }
        }
    }

    public static void DrawSByte(string name, string type, uint key, ref sbyte value,
    sbyte defaultvalue)
    {
        string refname = name.Replace(" ", "");
        ImGui.TextUnformatted(name);
        ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
        ImGui.SetNextItemWidth(110 * CalcGlobals.GlobalScale());
        if (ImGui.InputSByte("##" + refname + key, ref value))
        {
            Setup.SetupByType(key, type);
            Service.Config.Save();
        }
        ImGui.SameLine();

        using (ImRaii.PushFont(UiBuilder.IconFont))
        {
            if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{refname}{key}"))
            {
                value = defaultvalue;
                Setup.SetupByType(key, type);
                Service.Config.Save();
            }
        }
    }


    public static void DrawByte(string name, string type, uint key, ref byte value,
    byte defaultvalue)
    {
        string refname = name.Replace(" ", "");
        ImGui.TextUnformatted(name);
        ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
        ImGui.SetNextItemWidth(110 * CalcGlobals.GlobalScale());
        if (ImGui.InputByte("##" + refname + key, ref value))
        {
            Setup.SetupByType(key, type);
            Service.Config.Save();
        }
        ImGui.SameLine();

        using (ImRaii.PushFont(UiBuilder.IconFont))
        {
            if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{refname}{key}"))
            {
                value = defaultvalue;
                Setup.SetupByType(key, type);
                Service.Config.Save();
            }
        }
    }
    public static void DrawShort(string name, string type, uint key, ref short value,
    short defaultvalue)
    {
        string refname = name.Replace(" ", "");
        ImGui.TextUnformatted(name);
        ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
        ImGui.SetNextItemWidth(110 * CalcGlobals.GlobalScale());
        if (ImGui.InputShort("##" + refname + key, ref value))
        {
            Setup.SetupByType(key, type);
            Service.Config.Save();
        }
        ImGui.SameLine();

        using (ImRaii.PushFont(UiBuilder.IconFont))
        {
            if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{refname}{key}"))
            {
                value = defaultvalue;
                Setup.SetupByType(key, type);
                Service.Config.Save();
            }
        }
    }
    public static void DrawUShort(string name, string type, uint key, ref ushort value,
    ushort defaultvalue)
    {
        string refname = name.Replace(" ", "");
        ImGui.TextUnformatted(name);
        ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
        ImGui.SetNextItemWidth(110 * CalcGlobals.GlobalScale());
        if (ImGui.InputUShort("##" + refname + key, ref value))
        {
            Setup.SetupByType(key, type);
            Service.Config.Save();
        }
        ImGui.SameLine();

        using (ImRaii.PushFont(UiBuilder.IconFont))
        {
            if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{refname}{key}"))
            {
                value = defaultvalue;
                Setup.SetupByType(key, type);
                Service.Config.Save();
            }
        }
    }
    public static void DrawInt(string name, string type, uint key, ref int value,
    int defaultvalue)
    {
        string refname = name.Replace(" ", "");
        ImGui.TextUnformatted(name);
        ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
        ImGui.SetNextItemWidth(110 * CalcGlobals.GlobalScale());
        if (ImGui.InputInt("##" + refname + key, ref value))
        {
            Setup.SetupByType(key, type);
            Service.Config.Save();
        }
        ImGui.SameLine();

        using (ImRaii.PushFont(UiBuilder.IconFont))
        {
            if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{refname}{key}"))
            {
                value = defaultvalue;
                Setup.SetupByType(key, type);
                Service.Config.Save();
            }
        }
    }
    public static void DrawUInt(string name, string type, uint key, ref uint value,
    uint defaultvalue)
    {
        string refname = name.Replace(" ", "");
        ImGui.TextUnformatted(name);
        ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
        ImGui.SetNextItemWidth(110 * CalcGlobals.GlobalScale());
        if (ImGui.InputUInt("##" + refname + key, ref value))
        {
            Setup.SetupByType(key, type);
            Service.Config.Save();
        }
        ImGui.SameLine();

        using (ImRaii.PushFont(UiBuilder.IconFont))
        {
            if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{refname}{key}"))
            {
                value = defaultvalue;
                Setup.SetupByType(key, type);
                Service.Config.Save();
            }
        }
    }
    public static void DrawBool(string name, string type, uint key, ref bool value,
    bool defaultvalue)
    {
        string refname = name.Replace(" ", "");
        ImGui.TextUnformatted(name);
        ImGui.SetCursorPosX(ImGui.GetCursorPosX() + 15);
        ImGui.SetNextItemWidth(110 * CalcGlobals.GlobalScale());
        if (ImGui.Checkbox("##" + refname + key, ref value))
        {
            Setup.SetupByType(key, type);
            Service.Config.Save();
        }
        ImGui.SameLine();

        using (ImRaii.PushFont(UiBuilder.IconFont))
        {
            if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{refname}{key}"))
            {
                value = defaultvalue;
                Setup.SetupByType(key, type);
                Service.Config.Save();
            }
        }
    }



}