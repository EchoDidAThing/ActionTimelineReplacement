using System;
using System.Runtime.InteropServices;

namespace ActionTimelineReplacement.Hookers;

[StructLayout(LayoutKind.Explicit)]
public struct TiltData
{
    [FieldOffset(0x0)]
    public Single Unk1;

    [FieldOffset(0x4)]
    public Byte Unk2;

    [FieldOffset(0x5)]
    public Byte Unk3;

    [FieldOffset(0x6)]
    public Byte Unk4;

    [FieldOffset(0x7)]
    public Byte Unk5;
}
