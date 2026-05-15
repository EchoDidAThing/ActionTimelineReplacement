using Lumina.Data.Parsing;
using System.Runtime.InteropServices;

namespace ActionTimelineReplacement.Base.Structs;

[StructLayout(LayoutKind.Explicit)]
public struct ItemData
{
    //ALL NEW!! SETUP
    [FieldOffset(0x0)]
    public ushort SingularOffset;

    [FieldOffset(0x4)]
    public ushort PLuralOffset;

    [FieldOffset(0x8)]
    public ushort DescriptionOffset;

    [FieldOffset(0x8E)]
    public ushort ItemAction;

    [FieldOffset(0x18)]
    public Quad MainModel;

    [FieldOffset(0x20)]
    public Quad SubModel;
}
