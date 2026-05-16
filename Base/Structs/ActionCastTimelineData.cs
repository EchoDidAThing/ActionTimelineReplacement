using System.Runtime.InteropServices;

namespace ActionTimelineReplacement.Base.Structs;

[StructLayout(LayoutKind.Explicit)]
public struct ActionCastTimelineData
{
    //ALL NEW!! SETUP
    [FieldOffset(0x0)]
    public ushort ActionTimeline;

    [FieldOffset(0x02)]
    public ushort CastVfx;
}