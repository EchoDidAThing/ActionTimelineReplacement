using System.Runtime.InteropServices;

namespace ActionTimelineReplacement.Base.Structs;

[StructLayout(LayoutKind.Explicit)]
public struct ActionTimelineData
{
    //VERIFY THIS WORKS
    [FieldOffset(0x0)]
    public ushort AnimationOffset;

    //VERIFY THIS WORKS
    [FieldOffset(0x4)]
    public ushort WeaponTimeline;
    
    [FieldOffset(0x9)]
    public byte Type;

    [FieldOffset(0xA)]
    public byte Priority;

    [FieldOffset(0xB)]
    public byte Stance;

    [FieldOffset(0xC)]
    public byte Slot;

    [FieldOffset(0xD)]
    public byte LookAtMode;

    [FieldOffset(0xE)]
    public byte ActionTimelineIDMode;

    [FieldOffset(0xF)]
    public byte LoadType;

    [FieldOffset(0x10)]
    public byte StartAttach; //1 or 2 enables for mounts

    [FieldOffset(0x14)]
    public byte ResidentPap;

    [FieldOffset(0x12)]
    public byte Unknown6; //pap type

    [FieldOffset(0x13)]
    public byte Unknown1;

    [FieldOffset(0x14)]
    public byte VPRBladeState;

}
