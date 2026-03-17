using System.Runtime.InteropServices;

namespace ActionTimelineReplacement.Base.Structs;

[StructLayout(LayoutKind.Explicit)]
public struct BGMData
{
    //ALL NEW!! SETUP
    [FieldOffset(0x0)]
    public ushort PathOffset;

    [FieldOffset(0x08)]
    public ushort Priority;
}
