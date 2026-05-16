using System;
using System.Runtime.InteropServices;

namespace ActionTimelineReplacement.Base.Structs;

[StructLayout(LayoutKind.Explicit)]
public struct PetData
{
    //Covered by petscale????
    [FieldOffset(0x0E)]
    public ushort SmallScalePercentage;

    [FieldOffset(0xF)]
    public ushort MediumScalePercentage;

    [FieldOffset(0x10)]
    public ushort LargeScalePercentage;
}
