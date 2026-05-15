using System.Runtime.InteropServices;

namespace ActionTimelineReplacement.Base.Structs;

[StructLayout(LayoutKind.Explicit)]
public struct OrnamentCustomizeData
{
    //legitimately have no idea what any of this does to ornaments or how it's tied in
    //Echos note: need to check these, possible these are scaling values.

    [FieldOffset(0x0)]
    public ushort Unknown0;

    [FieldOffset(0x2)]
    public short Unknown1;

    [FieldOffset(0x4)]
    public short Unknown2;

    [FieldOffset(0x6)]
    public short Unknown3;

    [FieldOffset(0x8)]
    public short Unknown4;

    [FieldOffset(0xA)]
    public short Unknown5;

    [FieldOffset(0xC)]
    public short Unknown6;
}
