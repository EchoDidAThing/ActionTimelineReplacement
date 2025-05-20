using System.Runtime.InteropServices;

namespace ActionTimelineReplacement.Hookers;

[StructLayout(LayoutKind.Explicit)]
public struct ActionTimelineData
{
    //This is a String so.. uh.. yeah
    //[FieldOffset(0x0)]
    //public ushort Path;

    [FieldOffset(0xC)]
    public ushort WeaponTimeline;
}
