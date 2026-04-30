using System;
using System.Runtime.InteropServices;

namespace ActionTimelineReplacement.Base.Structs;

[StructLayout(LayoutKind.Explicit)]
public struct ActionData
{
    //NEW!! SETUP
    [FieldOffset(0x0)]
    public ushort ActionNameOffset;

    [FieldOffset(0x08)]
    public ushort Icon;

    [FieldOffset(0xA)]
    public ushort CastVfx;

    [FieldOffset(0xC)]
    public ushort ActionTimelineHit;

    [FieldOffset(0x20)]
    public Int16 AnimationEnd;

    [FieldOffset(0x23)]
    public byte Unknown1;

    [FieldOffset(0x24)]
    public ushort AnimationStart;

    [FieldOffset(0x25)]
    public byte Unknown2;

    [FieldOffset(0x32)]
    public byte Unknown4;

    [FieldOffset(0x69)]
    public byte Unknown_70;
}