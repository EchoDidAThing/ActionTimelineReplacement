using System.Runtime.InteropServices;

namespace ActionTimelineReplacement.Base.Structs;

[StructLayout(LayoutKind.Explicit)]
public struct MotionTimelineBlendTableData
{
    //ALL NEW!! SETUP
    [FieldOffset(0x0)]
    public byte DestBlendGroup;

    [FieldOffset(0x1)]
    public byte SourceBlendGroup;

    [FieldOffset(0x2)]
    public byte BlendFramePC;

    [FieldOffset(0x3)]
    public byte BlendFrameTypeA;

    [FieldOffset(0x4)]
    public byte BlendFrameTypeB;

    [FieldOffset(0x5)]
    public byte BlendFrameTypeC;
}