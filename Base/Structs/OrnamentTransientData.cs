using System.Runtime.InteropServices;

namespace ActionTimelineReplacement.Base.Structs;

[StructLayout(LayoutKind.Explicit)]
public struct OrnamentTransientData
{
    //ALL NEW!! SETUP
    [FieldOffset(0x0)]
    public ushort TooltipDescOffset;
}
