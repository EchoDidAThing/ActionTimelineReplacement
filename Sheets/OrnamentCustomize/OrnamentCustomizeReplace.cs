using ActionTimelineReplacement.Base.Structs;

namespace ActionTimelineReplacement.Sheets;

public class OrnamentCustomizeConfig(OrnamentCustomizeReplace replace, bool enabled)
{
    public bool Enabled = enabled;
    public OrnamentCustomizeReplace Replacement => replace;
    public static OrnamentCustomizeConfig CreateEntry(uint key)
    {
        OrnamentCustomizeReplace originalvalues = OrnamentCustomizeManager.GetOriginal(key);
        return new OrnamentCustomizeConfig(originalvalues, false);
    }
}

public class OrnamentCustomizeReplace(
    ushort unknown0,
    short unknown1,
    short unknown2,
    short unknown3,
    short unknown4,
    short unknown5,
    short unknown6)
{
    public ushort Unknown0 = unknown0;
    public short Unknown1 = unknown1;
    public short Unknown2 = unknown2;
    public short Unknown3 = unknown3;
    public short Unknown4 = unknown4;
    public short Unknown5 = unknown5;
    public short Unknown6 = unknown6;
    public unsafe void WriteToPointer(OrnamentCustomizeData* ptr)
    {
        ptr->Unknown0 = Unknown0;
        ptr->Unknown1 = Unknown1;
        ptr->Unknown2 = Unknown2;
        ptr->Unknown3 = Unknown3;
        ptr->Unknown4 = Unknown4;
        ptr->Unknown5 = Unknown5;
        ptr->Unknown6 = Unknown6;
    }
}
