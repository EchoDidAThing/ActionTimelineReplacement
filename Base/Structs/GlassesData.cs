using System.Runtime.InteropServices;

namespace ActionTimelineReplacement.Base.Structs;

[StructLayout(LayoutKind.Explicit)]
public struct GlassesData
{
    [FieldOffset(0x10)]//Adjective
    public sbyte Unknown70_1;

    [FieldOffset(0x11)]//POsessivePronoun
    public sbyte Unknown70_2;

    [FieldOffset(0x12)]//StartsWithVowel
    public sbyte Unknown70_3;

    [FieldOffset(0x13)]
    public sbyte Unknown70_4;

    [FieldOffset(0x14)]//Pronoun
    public sbyte Unknown70_5;

    [FieldOffset(0x15)]//Article
    public sbyte Unknown70_6;

    [FieldOffset(0x18)]//Model
    public uint Unknown70_7; //model ID or something

    [FieldOffset(0x20)]
    public ushort Unknown70_8;
}
