using System.Runtime.InteropServices;

namespace ActionTimelineReplacement.Base.Structs;

[StructLayout(LayoutKind.Explicit)]
public struct StatusData
{
    //This is a String so.. uh.. yeah
    //[FieldOffset(0x0)]
    //public ushort Name;

    //This is a String so.. uh.. yeah
    //[FieldOffset(0x4)]
    //public ushort Description;

    [FieldOffset(0xC)]
    public int ParamModifier;

    [FieldOffset(0x10)]
    public ushort StatusLoopVFX;

    [FieldOffset(0x14)]
    public byte Unknown0;

    [FieldOffset(0x17)]
    public byte StatusCategory; //buff, debuff, removable

    [FieldOffset(0x18)]
    public byte StatusHitEffect;

    [FieldOffset(0x1C)]
    public byte ParamEffect;

    [FieldOffset(0x1C)]
    public byte TargetType;

    [FieldOffset(0x1D)]
    public byte Flags;
    //affects animation in some way
    //example: Hidden for NIN when changed from 2 will change it from walk to standard run

    [FieldOffset(0x1E)]
    public byte Flag2;

    [FieldOffset(0x1F)]
    public byte Unknown_70_1;

    [FieldOffset(0x20)]
    public sbyte Unknown2;
    //affects if type shows on flytext, but also what type sometimes
    //0 is don't show, 1-5 shows its assigned type, 6 changes it to unaspected
}