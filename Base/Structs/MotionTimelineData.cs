using System.Runtime.InteropServices;

namespace ActionTimelineReplacement.Base.Structs;

[StructLayout(LayoutKind.Explicit)]
public struct MotionTimelineData
{
    /* //SE string
    [FieldOffset(0x0)]
    public string Filename;
    */

    [FieldOffset(0x1)]
    public byte BlendGroup;

    [FieldOffset(0x5)]
    public byte Unknown_70_1;

    [FieldOffset(0x6)]
    public byte Unknown_70_2;

    /* //packed bool 7&08
    [FieldOffset(0x7)]
    public bool Unknown0;
    */
}