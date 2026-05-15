using System;
using System.Runtime.InteropServices;

namespace ActionTimelineReplacement.Base.Structs;

[StructLayout(LayoutKind.Explicit)]
public struct TraitData
{
    //NEW!! SETUP
    [FieldOffset(0x00)]
    public ushort NameOffset;


    //NEW!! SETUP
    [FieldOffset(0x00)]
    public int Icon;
}