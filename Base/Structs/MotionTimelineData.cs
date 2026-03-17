using System.Runtime.InteropServices;

namespace ActionTimelineReplacement.Base.Structs;

[StructLayout(LayoutKind.Explicit)]
public struct MotionTimelineData
{
    //NEW!! SETUP
    [FieldOffset(0x0)]
    public string FileOffset;

    [FieldOffset(0x4)]
    public byte BlendGroup;

    [FieldOffset(0x5)]
    public byte Unknown_70_1;

    [FieldOffset(0x6)]
    public byte Unknown_70_2;

    //NEW!! SETUP
    [FieldOffset(0x7)]
    public ushort BitField1;

    //NEW!! SETUP
    [FieldOffset(0x7)]
    public ushort IsLoop;

    //NEW!! SETUP
    [FieldOffset(0x7)]
    public ushort IsBlinkEnable;

    //NEW!! SETUP
    [FieldOffset(0x7)]
    public ushort IsLipEnable;

    /* //packed bool 7&08
    [FieldOffset(0x7)]
    public bool Unknown0;
    */
}