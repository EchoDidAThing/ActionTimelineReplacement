using FFXIVClientStructs.FFXIV.Client.Game.Character;
using InteropGenerator.Runtime.Attributes;
using System;
using System.Runtime.InteropServices;

namespace ActionTimelineReplacement.Base.Structs;

[StructLayout(LayoutKind.Explicit, Size = 0x80)]
public unsafe partial struct EffectContainerFAFO
{
    [FieldOffset(8)]
    public unsafe CharacterContainerFAFO* OwnerObject;
    [FieldOffset(0x10)] public float CurrentFloatHeight;
    [FieldOffset(0x14)] public float TargetFloatHeight;
    [FieldOffset(0x18)] public float FloatHeightChangeSpeed;
    // [FieldOffset(0x1C)] private float UnkSpeed;
    // [FieldOffset(0x20)] private int UnkTime;
    // [FieldOffset(0x24)] private byte UnkRidingPillionFlag;
    // [FieldOffset(0x26)] private short UnkRidingPillionValue;
    // [FieldOffset(0x2C)] private float UnkTime2;
    [FieldOffset(0x30)] public StatusEffect StatusEffects;
    [FieldOffset(0x31)] public byte UnkTilt1;
    [FieldOffset(0x32)] public byte Unktilt2;
    [FieldOffset(0x33)] public byte Unktilt3;
    [FieldOffset(0x34)] public int MountTiltSetupState1;
    [FieldOffset(0x38)] public int MountTiltSetupState2;

    [FieldOffset(0x3C)] public TiltOrigin GroundTiltOrigin;
    [FieldOffset(0x3D)] public byte GroundUnk1;
    [FieldOffset(0x3E)] public byte GroundUnk2;
    [FieldOffset(0x40)] public float GroundTiltAngle;
    [FieldOffset(0x44)] public float GroundTiltSpeed;
    [FieldOffset(0x48)] public TiltFlags GroundTiltFlags;

    [FieldOffset(0x4C)] public TiltOrigin FlightTiltOrigin;
    [FieldOffset(0x4D)] public byte FlightUnk1;
    [FieldOffset(0x4E)] public byte FlightUnk;
    [FieldOffset(0x50)] public float FlightTiltAngle;
    [FieldOffset(0x54)] public float FlightTiltSpeed;
    [FieldOffset(0x58)] public TiltFlags FlightTiltFlags;

    //Set 3 and 4 are loaded into the ownerobject.
    [FieldOffset(0x5C)] public TiltOrigin Set3TiltOrigin;
    [FieldOffset(0x5D)] public byte Set3Unk1;
    [FieldOffset(0x5E)] public byte Set3Unk2;
    [FieldOffset(0x60)] public float Set3TiltAngle;
    [FieldOffset(0x64)] public float Set3TiltSpeed;
    [FieldOffset(0x68)] public TiltFlags Set3TiltFlags;

    [FieldOffset(0x6C)] public TiltOrigin Set4TiltOrigin;
    [FieldOffset(0x6D)] public byte Set4Unk1;
    [FieldOffset(0x6E)] public ushort Set4Unk2;
    [FieldOffset(0x70)] public float Set4TiltAngle;
    [FieldOffset(0x74)] public float Set4TiltSpeed;
    [FieldOffset(0x78)] public TiltFlags Set4ReverseTilt;

    [FieldOffset(0x40), Obsolete("Invalid since 7.1")] public byte TiltParam1Type;
    [FieldOffset(0x44), Obsolete("Invalid since 7.1")] public float TiltParam1Value;
    [FieldOffset(0x48), Obsolete("Invalid since 7.1")] public byte TiltParam2Type;
    [FieldOffset(0x4C), Obsolete("Invalid since 7.1")] public float TiltParam2Value;


    [Flags]
    public enum StatusEffect : byte
    {
        IsGPoseWet = 0x01,
    }

    [Flags]
    public enum TiltFlags : byte
    {
        Clockwise = 0x00,
        CounterClockwise = 0x01,
    }

    public enum TiltOrigin : byte
    {
        Ground = 0x01,
        Center = 0x02,
        //really have no idea what to call this.
        Legsstraightupperbodymove = 0x03,
        Unk1 = 0x04,
    }
}