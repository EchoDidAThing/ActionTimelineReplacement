using System.Runtime.InteropServices;

namespace ActionTimelineReplacement.Base.Structs;

[StructLayout(LayoutKind.Explicit)]
public struct ActionCastVFXData
{
    [FieldOffset(0x0)]
    public ushort Vfx;
}
