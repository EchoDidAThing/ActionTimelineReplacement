using System.Runtime.InteropServices;

namespace ActionTimelineReplacement.Hookers.HookersData;

[StructLayout(LayoutKind.Explicit)]
public struct GlassesData
{
    [FieldOffset(0x10)]
    public sbyte Unknown70_1;

    [FieldOffset(0x11)]
    public sbyte Unknown70_2;

    [FieldOffset(0x12)]
    public sbyte Unknown70_3;

    [FieldOffset(0x13)]
    public sbyte Unknown70_4;

    [FieldOffset(0x14)]
    public sbyte Unknown70_5;

    [FieldOffset(0x15)]
    public sbyte Unknown70_6;

    [FieldOffset(0x18)]
    public uint Unknown70_7; //model ID or something

    [FieldOffset(0x20)]
    public ushort Unknown70_8;
}
