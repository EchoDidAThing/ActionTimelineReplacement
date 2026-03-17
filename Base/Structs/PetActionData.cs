using System;
using System.Runtime.InteropServices;

namespace ActionTimelineReplacement.Base.Structs;

[StructLayout(LayoutKind.Explicit)]
public struct PetActionData
{
    //ALL NEW!! SETUP
    [FieldOffset(0x0)]
    public ushort NameOffset;

    [FieldOffset(0x4)]
    public ushort DescriptionOffset;

    [FieldOffset(0x8)]
    public Int32 Icon;

    //seems to be a dead field
    //[FieldOffset(0x8)]
    //public Int32 Action;
}
