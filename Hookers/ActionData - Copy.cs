using System.Runtime.InteropServices;

namespace ActionTimelineReplacement.Hookers;

[StructLayout(LayoutKind.Explicit)]
public struct ActionData
{
    [FieldOffset(0xA)]
    public ushort CastVfx;

    [FieldOffset(0xC)]
    public ushort ActionTimelineHit;

    [FieldOffset(0x20)]
    public ushort AnimationEnd;

    [FieldOffset(0x24)]
    public ushort AnimationStart;
}
