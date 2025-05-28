using ActionTimelineReplacement.Hookers.HookersData;

namespace ActionTimelineReplacement.Configurations;
public class PlaceNameReplacementConfig(PlaceNameReplacement replacement, bool enabled)
{
    public bool Enabled = enabled;
    public PlaceNameReplacement Replacement { get; } = replacement;
}

public class PlaceNameReplacement(

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
    //uint mapCondition,
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

    public unsafe void WriteToPointer(PlaceNameData* pointer)
    {
        //pointer->PlaceName_Name = placeNameName;
        //pointer->PlaceName_NameNoArticle = placeNameNoArticle;
        //pointer->PlaceNameUnk0 = placeNameUnk0;
        pointer->PlaceNameUnk1 = placeNameUnk1;
        pointer->PlaceNameUnk2 = placeNameUnk2;
        pointer->PlaceNameUnk3 = placeNameUnk3;
        pointer->PlaceNameUnk4 = placeNameUnk4;
        pointer->PlaceNameUnk5 = placeNameUnk5;
        pointer->PlaceNameUnk6 = placeNameUnk6;
        pointer->PlaceNameUnk7 = placeNameUnk7;
        pointer->PlaceNameUnk8 = placeNameUnk8;
    }
}
