using System.Runtime.InteropServices;

namespace ActionTimelineReplacement.Base.Structs;

[StructLayout(LayoutKind.Explicit)]
public struct WeaponTimelineData
{
    //This is a String so.. uh.. yeah
    //[FieldOffset(0x0)]
    //public ushort Path;

    [FieldOffset(0x4)]
    public ushort NextWeaponTimeline;

    //aaand a boolean
    //[FieldOffset(0x0)]
    //public bool UnkPackedBool;

}
