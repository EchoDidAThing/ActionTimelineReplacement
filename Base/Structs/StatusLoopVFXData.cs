using System.Runtime.InteropServices;

namespace ActionTimelineReplacement.Base.Structs;

[StructLayout(LayoutKind.Explicit)]
public struct StatusLoopVFXData
{
    [FieldOffset(0x0)]
    public ushort FriendlyVFX;

    [FieldOffset(0x2)]
    public ushort StackVFX1;

    [FieldOffset(0x4)]
    public ushort StackVFX2;

    [FieldOffset(0x6)]
    public ushort EnemyVFX;

    [FieldOffset(0x8)]
    public byte StackTrigger1;

    [FieldOffset(0x9)]
    public byte StackTrigger2;

    [FieldOffset(0xA)]
    public byte Unknown1;

    [FieldOffset(0xB)]
    public byte Unknown2;

    [FieldOffset(0xC)]
    public byte UnknownBitfield1;

    [FieldOffset(0xC)]
    public byte Unknown3;

    [FieldOffset(0xC)]
    public byte Unknown4;
    
    [FieldOffset(0xC)]
    public byte Unknown5;
}
