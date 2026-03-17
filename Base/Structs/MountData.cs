using System;
using System.Dynamic;
using System.Runtime.InteropServices;
using System.Threading;
using FFXIVClientStructs;
using Lumina.Excel.Sheets;

namespace ActionTimelineReplacement.Base.Structs;

[StructLayout(LayoutKind.Explicit)]
public struct MountData
{
    //NEW!! SETUP
    [FieldOffset(0x10)]
    public ushort WhistlePathOffset;
    //NEW!! SETUP
    [FieldOffset(0x14)]
    public ushort WhistleOnlyPathOffset;
    //NEW!! SETUP
    [FieldOffset(0x18)]
    public ushort DismountOnlyOffset;
    //NEW!! SETUP
    [FieldOffset(0x1C)]
    public Int32 ModelChara;

    [FieldOffset(0x32)]
    public ushort RideBGM;

    //NEW!! SETUP
    [FieldOffset(0x34)]
    public ushort Icon;
    //NEW!! SETUP
    [FieldOffset(0x38)]
    public ushort MountAction;

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
    //NEW!! SETUP
    [FieldOffset(0x4B)]
    public byte ExitMoveDistance;
    //NEW!! SETUP
    [FieldOffset(0x4C)]
    public byte ExitMoveSpeed;

    //NEW!! SETUP
    [FieldOffset(0x4D)]
    public byte RadiusRate;
    //NEW!! SETUP
    [FieldOffset(0x4E)]
    public byte BaseMotionSpeedRun;
    //NEW!! SETUP
    [FieldOffset(0x4F)]
    public byte BaseMotionSpeedWalk;

    [FieldOffset(0x50)]
    public ushort UnderwaterAnimSpeed;

    //NEW!! SETUP
    [FieldOffset(0x54)]
    public ushort Bitfield1;

    //NEW!! SETUP
    [FieldOffset(0x54)]
    public ushort IsAirborn;

    //NEW!! SETUP
    [FieldOffset(0x55)]
    public ushort BitField2;

    //NEW!! SETUP
    [FieldOffset(0x55)]
    public ushort HideHeadgear;

    //[FieldOffset(0x50)]
    //public byte MountBoolSet1;
}