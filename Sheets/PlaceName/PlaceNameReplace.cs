using ActionTimelineReplacement.Base.Structs;

namespace ActionTimelineReplacement.Sheets;

public class PlaceNameConfig(PlaceNameReplace replace, bool enabled)
{
    public bool Enabled = enabled;
    public PlaceNameReplace Replacement => replace;
}

public class PlaceNameReplace(
    //string name,
    //string nameNoArticle,
    //string unknown0,
    sbyte unknown1,
    sbyte unknown2,
    sbyte unknown3,
    sbyte unknown4,
    sbyte unknown5,
    sbyte unknown6,
    ushort unknown7,
    byte unknown8
    //uint mapCondition
    )
{
    //public string placeNameName = name;
    //public string placeNameNoArticle = nameNoArticle;
    //public string placeNameUnk0 = unknown0;
    public sbyte placeNameUnk1 = unknown1;
    public sbyte placeNameUnk2 = unknown2;
    public sbyte placeNameUnk3 = unknown3;
    public sbyte placeNameUnk4 = unknown4;
    public sbyte placeNameUnk5 = unknown5;
    public sbyte placeNameUnk6 = unknown6;
    public ushort placeNameUnk7 = unknown7;
    public byte placeNameUnk8 = unknown8;
    //public uint placeNameMapCondition = mapCondition;

    public unsafe void WriteToPointer(PlaceNameData* ptr)
    {
        //pointer->PlaceName_Name = placeNameName;
        //pointer->PlaceName_NameNoArticle = placeNameNoArticle;
        //pointer->PlaceNameUnk0 = placeNameUnk0;
        ptr->PlaceNameUnk1 = placeNameUnk1;
        ptr->PlaceNameUnk2 = placeNameUnk2;
        ptr->PlaceNameUnk3 = placeNameUnk3;
        ptr->PlaceNameUnk4 = placeNameUnk4;
        ptr->PlaceNameUnk5 = placeNameUnk5;
        ptr->PlaceNameUnk6 = placeNameUnk6;
        ptr->PlaceNameUnk7 = placeNameUnk7;
        ptr->PlaceNameUnk8 = placeNameUnk8;
    }
}
