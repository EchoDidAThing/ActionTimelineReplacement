using System.Dynamic;
using System.Runtime.InteropServices;
using System.Threading;
using FFXIVClientStructs;
using Lumina.Excel.Sheets;

namespace ActionTimelineReplacement.Base.Structs;

[StructLayout(LayoutKind.Explicit)]
public struct MountData
{
    //THESE NEED SESTRING HANDLING
    //[FieldOffset(0x10)]
    //public ushort WhistlePath;

    //[FieldOffset(0x14)]
    //public ushort WhistleOnlyPath;

    //[FieldOffset(0x18)]
    //public ushort ByePath;

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

    [FieldOffset(0x45)]
    public ushort FlyUpDownTilt;

    [FieldOffset(0x46)]
    public ushort Unknown6;

    [FieldOffset(0x47)]
    public ushort Unknown7;

    [FieldOffset(0x49)]
    public ushort Unknown8;

    [FieldOffset(0x4A)]
    public ushort MountCustomize;

    [FieldOffset(0x4F)]
    public ushort Unknown9;

    [FieldOffset(0x50)]
    public ushort UnderwaterAnimSpeed;

    //[FieldOffset(0x50)]
    //public byte MountBoolSet1;
}