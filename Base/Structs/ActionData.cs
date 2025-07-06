using System.Runtime.InteropServices;

namespace ActionTimelineReplacement.Base.Structs;

[StructLayout(LayoutKind.Explicit)]
public struct ActionData
{
    [FieldOffset(0xA)]
    public ushort CastVfx;

    [FieldOffset(0xC)]
    public ushort ActionTimelineHit;

    [FieldOffset(0x20)]
    public ushort AnimationEnd;

    [FieldOffset(0x22)]
    public ushort ActionCategory;

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