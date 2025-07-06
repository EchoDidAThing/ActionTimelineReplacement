/*
using ActionTimelineReplacement.Base.Structs;

namespace ActionTimelineReplacement.Sheets;

public class PointMenuChoiceConfig(PointMenuChoiceReplace replace, bool enabled)
{
    public bool Enabled = enabled;
    public PointMenuChoiceReplace Replacement => replace;
}

public class PointMenuChoiceReplace(
    float unknown0,
    float unknown1,
    float unknown2,
    float unknown3,
    float unknown4,
    float unknown5,
    ushort unknown6,
    byte unknown7,
    byte unknown8,
    byte unknown9,
    byte unknown10,
    byte unknown11,
    byte unknown12)
{
    public float Unknown0 = unknown0;
    public float Unknown1 = unknown1;
    public float Unknown2 = unknown2;
    public float Unknown3 = unknown3;
    public float Unknown4 = unknown4;
    public float Unknown5 = unknown5;
    public ushort Unknown6 = unknown6;
    public byte Unknown7 = unknown7;
    public byte Unknown8 = unknown8;
    public byte Unknown9 = unknown9;
    public byte Unknown10 = unknown10;
    public byte Unknown11 = unknown11;
    public byte Unknown12 = unknown12;

    public unsafe void WriteToPointer(PointMenuChoiceData* ptr)
    {
        ptr->Unknown0 = Unknown0;
        ptr->Unknown1 = Unknown1;
        ptr->Unknown2 = Unknown2;
        ptr->Unknown3 = Unknown3;
        ptr->Unknown4 = Unknown4;
        ptr->Unknown5 = Unknown5;
        ptr->Unknown6 = Unknown6;
        ptr->Unknown7 = Unknown7;
        ptr->Unknown8 = Unknown8;
        ptr->Unknown9 = Unknown9;
        ptr->Unknown10 = Unknown10;
        ptr->Unknown11 = Unknown11;
        ptr->Unknown12 = Unknown12;
    }
}
*/