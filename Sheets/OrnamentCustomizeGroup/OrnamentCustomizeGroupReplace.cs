using ActionTimelineReplacement.Base.Structs;

namespace ActionTimelineReplacement.Sheets;

public class OrnamentCustomizeGroupConfig(OrnamentCustomizeGroupReplace replace, bool enabled)
{
    public bool Enabled = enabled;
    public OrnamentCustomizeGroupReplace Replacement => replace;
}

public class OrnamentCustomizeGroupReplace()
{
    /*
    public ushort Unknown0 = unknown0;
    public short Unknown1 = unknown1;
    public short Unknown2 = unknown2;
    public short Unknown3 = unknown3;
    public short Unknown4 = unknown4;
    public short Unknown5 = unknown5;
    public short Unknown6 = unknown6;
    public unsafe void WriteToPointer(OrnamentCustomizeGroupData* ptr)
    {
        ptr->Unknown0 = Unknown0;
        ptr->Unknown1 = Unknown1;
        ptr->Unknown2 = Unknown2;
        ptr->Unknown3 = Unknown3;
        ptr->Unknown4 = Unknown4;
        ptr->Unknown5 = Unknown5;
        ptr->Unknown6 = Unknown6;
    }
    */
}
