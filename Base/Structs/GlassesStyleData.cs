using System.Runtime.InteropServices;

namespace ActionTimelineReplacement.Base.Structs;

[StructLayout(LayoutKind.Explicit)]
public struct GlassesStyleData
{
    [FieldOffset(0xC)]
    public sbyte Unknown70_1;

    [FieldOffset(0xD)]
    public sbyte Unknown70_2;

    [FieldOffset(0xE)]
    public sbyte Unknown70_3;

    [FieldOffset(0xF)]
    public sbyte Unknown70_4;

    [FieldOffset(0x10)]
    public sbyte Unknown70_5;

    [FieldOffset(0x11)]
    public sbyte Unknown70_6;

    [FieldOffset(0x32)]//GlassesSlot
    public short Unknown70_7; //enable selection in UI
}
