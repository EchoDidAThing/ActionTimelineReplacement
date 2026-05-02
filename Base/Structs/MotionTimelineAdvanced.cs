using System.Runtime.InteropServices;

namespace ActionTimelineReplacement.Base.Structs;

[StructLayout(LayoutKind.Explicit)]
public struct MotionTimelineAdvanced
{
    //ALL NEW!! SETUP
    [FieldOffset(0x0)]
    public byte SourceStringOffset;

    [FieldOffset(0x4)]
    public byte DestStringOffset;
}