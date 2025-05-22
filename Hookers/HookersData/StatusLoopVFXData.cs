using System.Runtime.InteropServices;

namespace ActionTimelineReplacement.Hookers;

[StructLayout(LayoutKind.Explicit)]
public struct StatusLoopVFXData
{
    [FieldOffset(0x0)]
    public ushort FriendlyVFX;

    [FieldOffset(0x2)]
    public ushort StackVFX1;

    [FieldOffset(0x4)]
    public ushort StackVFX2;

    [FieldOffset(0x8)]
    public byte Stack1Trigger;

    [FieldOffset(0x9)]
    public byte Stack2Trigger;
}
