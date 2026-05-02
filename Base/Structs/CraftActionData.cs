using System.Runtime.InteropServices;

namespace ActionTimelineReplacement.Base.Structs;

[StructLayout(LayoutKind.Explicit)]
public struct CraftActionData
{
    //ALL NEW!! SETUP
    [FieldOffset(0x0)]
    public ushort NameOffset;

    [FieldOffset(0x04)]
    public ushort DescriptionOffset;

    [FieldOffset(0x2C)]
    public ushort StartAnimation;

    [FieldOffset(0x2E)]
    public ushort EndAnimation;

    [FieldOffset(0x30)]
    public ushort Icon;
}
