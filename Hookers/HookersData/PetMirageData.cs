using System.Runtime.InteropServices;

namespace ActionTimelineReplacement.Hookers.HookersData;

[StructLayout(LayoutKind.Explicit)]
public struct PetMirageData
{

    //dumb amount of entries in this sheet. mostly sets of four (three shorts, one byte). truncating the list for now

    //[FieldOffset(0x0)]
    //public string PetMirageName;

    [FieldOffset(0x4)]
    public ushort PetMirageUnk0;

    [FieldOffset(0x6)]
    public ushort PetMirageUnk1;

    [FieldOffset(0x8)]
    public ushort PetMirageUnk2;

    [FieldOffset(0xA)]
    public byte PetMirageUnk3;

    [FieldOffset(0xC)]
    public ushort PetMirageUnk4;

    [FieldOffset(0xE)]
    public ushort PetMirageUnk5;

    [FieldOffset(0x10)]
    public ushort PetMirageUnk6;

    [FieldOffset(0x12)]
    public byte PetMirageUnk7;

    /*[FieldOffset(0x14)]
    public ushort PetMirageUnk8;

    [FieldOffset(0x16)]
    public ushort PetMirageUnk9;

    [FieldOffset(0x18)]
    public ushort PetMirageUnk10;

    [FieldOffset(0x1A)]
    public byte PetMirageUnk11;

    [FieldOffset(0x1C)]
    public ushort PetMirageUnk12;

    [FieldOffset(0x1E)]
    public ushort PetMirageUnk13;

    [FieldOffset(0x20)]
    public ushort PetMirageUnk14;

    [FieldOffset(0x22)]
    public byte PetMirageUnk15;

    [FieldOffset(0x24)]
    public ushort PetMirageUnk16;

    [FieldOffset(0x26)]
    public ushort PetMirageUnk17;

    [FieldOffset(0x28)]
    public ushort PetMirageUnk18;

    [FieldOffset(0x2A)]
    public byte PetMirageUnk19;

    [FieldOffset(0x2C)]
    public ushort PetMirageUnk20;

    [FieldOffset(0x2E)]
    public ushort PetMirageUnk21;

    [FieldOffset(0x30)]
    public ushort PetMirageUnk22;

    [FieldOffset(0x32)]
    public byte PetMirageUnk23;

    [FieldOffset(0x34)]
    public ushort PetMirageUnk24;

    [FieldOffset(0x36)]
    public ushort PetMirageUnk25;

    [FieldOffset(0x38)]
    public ushort PetMirageUnk26;

    [FieldOffset(0x3A)]
    public byte PetMirageUnk27;

    [FieldOffset(0x3C)]
    public ushort PetMirageUnk28;

    [FieldOffset(0x3E)]
    public ushort PetMirageUnk29;

    [FieldOffset(0x40)]
    public ushort PetMirageUnk30;

    [FieldOffset(0x42)]
    public byte PetMirageUnk31;

    [FieldOffset(0x44)]
    public ushort PetMirageUnk32;

    [FieldOffset(0x46)]
    public ushort PetMirageUnk33;

    [FieldOffset(0x48)]
    public ushort PetMirageUnk34;

    [FieldOffset(0x4A)]
    public byte PetMirageUnk35;

    [FieldOffset(0x4C)]
    public ushort PetMirageUnk36;

    [FieldOffset(0x4E)]
    public ushort PetMirageUnk37;

    [FieldOffset(0x50)]
    public ushort PetMirageUnk38;

    [FieldOffset(0x52)]
    public byte PetMirageUnk39;

    [FieldOffset(0x54)]
    public ushort PetMirageUnk40;

    [FieldOffset(0x56)]
    public ushort PetMirageUnk41;

    [FieldOffset(0x58)]
    public ushort PetMirageUnk42;

    [FieldOffset(0x5A)]
    public byte PetMirageUnk43;

    [FieldOffset(0x5C)]
    public ushort PetMirageUnk44;

    [FieldOffset(0x5E)]
    public ushort PetMirageUnk45;

    [FieldOffset(0x60)]
    public ushort PetMirageUnk46;

    [FieldOffset(0x62)]
    public byte PetMirageUnk47;

    [FieldOffset(0x64)]
    public ushort PetMirageUnk48;

    [FieldOffset(0x66)]
    public ushort PetMirageUnk49;

    [FieldOffset(0x68)]
    public ushort PetMirageUnk50;

    [FieldOffset(0x6A)]
    public byte PetMirageUnk51;

    [FieldOffset(0x6C)]
    public ushort PetMirageUnk52;

    [FieldOffset(0x6E)]
    public ushort PetMirageUnk53;

    [FieldOffset(0x70)]
    public ushort PetMirageUnk54;

    [FieldOffset(0x72)]
    public byte PetMirageUnk55;

    [FieldOffset(0x74)]
    public ushort PetMirageUnk56;

    [FieldOffset(0x76)]
    public ushort PetMirageUnk57;

    [FieldOffset(0x78)]
    public ushort PetMirageUnk58;

    [FieldOffset(0x7A)]
    public byte PetMirageUnk59;*/

    [FieldOffset(0x7C)]
    public float PetMirageScale;

    //[FieldOffset(0x80)]
    //public int PetMirageModelChara;
}
