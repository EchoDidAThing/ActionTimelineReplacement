using System;
using System.Runtime.InteropServices;

namespace ActionTimelineReplacement.Hookers;

[StructLayout(LayoutKind.Explicit)]
public struct TiltData
{
    [FieldOffset(0x0)]
    public Single TiltRate;

    [FieldOffset(0x4)]
    public Byte RotationOriginOffset;

    [FieldOffset(0x5)]
    public Byte MaxAngle;

    [FieldOffset(0x6)]
    public Byte Unknown3;

    [FieldOffset(0x7)]
    public Byte Unknown4;

    //There is a packed boolean but I dunno how to work with them yet
    [FieldOffset(0x8)]
    public bool Unknown5;
}
