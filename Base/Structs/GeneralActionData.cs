using System.Runtime.InteropServices;

namespace ActionTimelineReplacement.Base.Structs;

[StructLayout(LayoutKind.Explicit)]
public struct GeneralActionData
{
    //ALL NEW!! SETUP
    [FieldOffset(0x0)]
    public ushort NameOffset;

    [FieldOffset(0x4)]
    public ushort Description;

    [FieldOffset(0xC)]
    public ushort Action;

    [FieldOffset(0x08)]
    public ushort Icon;
}
