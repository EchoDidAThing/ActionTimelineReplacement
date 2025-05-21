using System;
using System.Runtime.InteropServices;

namespace ActionTimelineReplacement.Hookers.HookersData;

[StructLayout(LayoutKind.Explicit)]
public struct StatusData
{
    //This is a String so.. uh.. yeah
    //[FieldOffset(0x0)]
    //public ushort Name;

    //This is a String so.. uh.. yeah
    //[FieldOffset(0x4)]
    //public ushort Description;

    [FieldOffset(0x10)]
    public ushort StatusLoopVFX;

    [FieldOffset(0x18)]
    public ushort StatusHitEffect;
}
