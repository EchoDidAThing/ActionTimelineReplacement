using System.Runtime.InteropServices;

namespace ActionTimelineReplacement.Hookers;

[StructLayout(LayoutKind.Explicit)]
public struct MountData
{
    [FieldOffset(0x32)]
    public ushort RideBGM;

    [FieldOffset(0x3A)]
    public ushort TiltGround;

    [FieldOffset(0x3C)]
    public ushort TiltFlySwim;

    [FieldOffset(0x3E)]
    public ushort TiltParam3;

    [FieldOffset(0x40)]
    public ushort TiltParam4;

    [FieldOffset(0x4A)]
    public ushort MountCustomize;

    [FieldOffset(0x45)]
    public ushort FlyUpDownTilt;

    [FieldOffset(0x46)]
    public ushort Unk2;

    [FieldOffset(0x47)]
    public ushort Unk3;

    [FieldOffset(0x49)]
    public ushort Unk4;

    [FieldOffset(0x4F)]
    public ushort Unk5;

    [FieldOffset(0x50)]
    public ushort UnderwaterAnimSpeed;
}
