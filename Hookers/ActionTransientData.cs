using System.Runtime.InteropServices;

namespace ActionTimelineReplacement.Hookers;

[StructLayout(LayoutKind.Explicit)]
public struct ActionTransientData
{
    //This is a String so.. uh.. yeah
    //[FieldOffset(0x0)]
    //public ushort Description;
}
