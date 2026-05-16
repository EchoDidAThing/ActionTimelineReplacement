using System.Runtime.InteropServices;

namespace ActionTimelineReplacement.Base.Structs;

[StructLayout(LayoutKind.Explicit)]
public struct GlassesStyleData
{
    [FieldOffset(0xC)]// Adjective
    public sbyte Unknown70_1;

    [FieldOffset(0xD)] //Plural
    public sbyte Unknown70_2;

    [FieldOffset(0xE)] //StartsWithVowel
    public sbyte Unknown70_3;

    [FieldOffset(0xF)] 
    public sbyte Unknown70_4;

    [FieldOffset(0x10)] //Pronoun
    public sbyte Unknown70_5;

    [FieldOffset(0x11)] //Article
    public sbyte Unknown70_6;

    [FieldOffset(0x32)]//GlassesSlot
    public short Unknown70_7; //enable selection in UI
}
