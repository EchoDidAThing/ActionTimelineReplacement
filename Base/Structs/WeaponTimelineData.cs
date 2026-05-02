using System;
using System.Runtime.InteropServices;

namespace ActionTimelineReplacement.Base.Structs;

[StructLayout(LayoutKind.Explicit)]
public struct WeaponTimelineData
{
    //ALL NEW!! SETUP
    [FieldOffset(0x0)]
    public uint Animation;

    [FieldOffset(0x4)]
    public Int16 NextWeaponTimeline;

    [FieldOffset(0x6)]
    public byte UnknownBitfield1;

    [FieldOffset(0x6)]
    public byte Unknown3;

    [FieldOffset(0x6)]
    public byte Unknown4;

}
