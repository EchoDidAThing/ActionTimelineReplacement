using System.Runtime.InteropServices;

namespace ActionTimelineReplacement.Hookers;

[StructLayout(LayoutKind.Explicit)]
public struct TiltData
{
    [FieldOffset(0x0)]
    public float TiltRate;

    [FieldOffset(0x4)]
    public byte RotationOriginOffset;

    [FieldOffset(0x5)]
    public byte MaxAngle;

    [FieldOffset(0x6)]
    public byte Unknown3;

    [FieldOffset(0x7)]
    public byte Unknown4;

    [FieldOffset(0x8)]
    public bool MouseReverse;
}
