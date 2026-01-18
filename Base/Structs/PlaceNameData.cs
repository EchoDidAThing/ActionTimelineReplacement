using System.Runtime.InteropServices;

namespace ActionTimelineReplacement.Base.Structs;

[StructLayout(LayoutKind.Explicit)]
public struct PlaceNameData
{

    /*[FieldOffset(0x0)]
    public string PlaceName_Name;

    /*[FieldOffset(0x4)]
    public string PlaceName_NameNoArticle;

    [FieldOffset(0x8)]
    public string PlaceNameUnk0;*/

    [FieldOffset(0xC)]
    public sbyte PlaceNameUnk1;

    [FieldOffset(0xD)]
    public sbyte PlaceNameUnk2;

    [FieldOffset(0xE)]
    public sbyte PlaceNameUnk3;

    [FieldOffset(0xF)]
    public sbyte PlaceNameUnk4;

    [FieldOffset(0x10)]
    public sbyte PlaceNameUnk5;

    [FieldOffset(0x11)]
    public sbyte PlaceNameUnk6;

    [FieldOffset(0x14)]
    public ushort PlaceNameUnk7;

    [FieldOffset(0x16)]
    public byte PlaceNameUnk8;

    //[FieldOffset(0x17)]
    //public byte PlaceNameMapCondition;
}
