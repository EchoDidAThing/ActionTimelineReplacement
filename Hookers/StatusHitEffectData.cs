using System.Runtime.InteropServices;

namespace ActionTimelineReplacement.Hookers;

[StructLayout(LayoutKind.Explicit)]
public struct StatusHitEffectData
{
    [FieldOffset(0x0)]
    public ushort Vfx;
}
