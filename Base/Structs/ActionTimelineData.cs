using System.Runtime.InteropServices;

namespace ActionTimelineReplacement.Base.Structs;

[StructLayout(LayoutKind.Explicit)]
public struct ActionTimelineData
{
    //NEW!! SETUP
    [FieldOffset(0x0)]
    public ushort AnimationOffset;

    //NEW!! SETUP
    [FieldOffset(0x4)]
    public ushort WeaponTimeline;
    
    [FieldOffset(0x8)]
    public byte Type;

    [FieldOffset(0x9)]
    public byte Priority;

    [FieldOffset(0xA)]
    public byte Stance;

    [FieldOffset(0xB)]
    public byte Slot;

    [FieldOffset(0xC)]
    public byte LookAtMode;

    [FieldOffset(0xD)]
    public byte ActionTimelineIDMode;

    [FieldOffset(0xE)]
    public byte LoadType;

    [FieldOffset(0xF)]
    public byte StartAttach;

    [FieldOffset(0x10)]
    public byte ResidentPap;

    [FieldOffset(0x11)]
    public byte Unknown6; //pap type

    [FieldOffset(0x12)]
    public byte Unknown1;

    [FieldOffset(0x13)]
    public byte VPRBladeState;

}
