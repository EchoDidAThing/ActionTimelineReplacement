using ActionTimelineReplacement.Base.Structs;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ActionTimelineReplacement.Sheets;

public class ActionTimelineConfig(ActionTimelineReplace replace, bool enabled)
{
    public bool Enabled = enabled;
    public ActionTimelineReplace Replacement => replace;
}

public class ActionTimelineReplace(
    uint rowid,
    string animation,
    ushort weapontimelineoffset,
    byte type,
    byte priority,
    byte stance,
    byte slot,
    byte lookAtMode,
    byte actionTimelineIDMode,
    byte loadType,
    byte startAttach,
    byte residentPap,
    byte unknown6,
    byte unknown1,
    byte viperBladeState)
{
    public uint RowId = rowid;
    public string Animation = animation;
    public ushort WeaponTimelineOffset = weapontimelineoffset;
    public byte Type = type;
    public byte Priority = priority;
    public byte Stance = stance;
    public byte Slot = slot;
    public byte LookAtMode = lookAtMode;
    public byte ActionTimelineIDMode = actionTimelineIDMode;
    public byte LoadType = loadType;
    public byte StartAttach = startAttach;
    public byte ResidentPap = residentPap;
    public byte Unknown6 = unknown6;
    public byte Unknown1 = unknown1;
    public byte VPRBladeState = viperBladeState;
    public unsafe void WriteToPointer(ActionTimelineData* ptr)
    {
        ptr->Type = Type;
        ptr->Priority = Priority;
        ptr->Stance = Stance;
        ptr->Slot = Slot;
        ptr->LookAtMode = LookAtMode;
        ptr->ActionTimelineIDMode = ActionTimelineIDMode;
        ptr->LoadType = LoadType;
        ptr->StartAttach = StartAttach;
        ptr->ResidentPap = ResidentPap;
        ptr->Unknown6 = Unknown6;
        ptr->Unknown1 = Unknown1;
        ptr->VPRBladeState = VPRBladeState;
    }


    public unsafe void WriteSEString(IntPtr ptr)
    {
        var data = ptr;
        var stringbytes = Encoding.UTF8.GetBytes(Animation);
        var offset = Marshal.ReadByte(data);
        Marshal.Copy(stringbytes, 0, data + offset, stringbytes.Length);
        Marshal.WriteByte(data + offset + stringbytes.Length, 0);
    }
}
