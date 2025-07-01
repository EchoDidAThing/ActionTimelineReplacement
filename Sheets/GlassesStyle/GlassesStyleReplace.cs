using ActionTimelineReplacement.Base.Structs;

namespace ActionTimelineReplacement.Sheets;

public class GlassesStyleConfig(GlassesStyleReplace replace, bool enabled)
{
    public bool Enabled = enabled;
    public GlassesStyleReplace Replacement => replace;
}

public class GlassesStyleReplace(
    sbyte Unknown70_1,
    sbyte Unknown70_2,
    sbyte Unknown70_3,
    sbyte Unknown70_4,
    sbyte Unknown70_5,
    sbyte Unknown70_6,
    short Unknown70_7)
{
    public sbyte Unknown70_1 = Unknown70_1;
    public sbyte Unknown70_2 = Unknown70_2;
    public sbyte Unknown70_3 = Unknown70_3;
    public sbyte Unknown70_4 = Unknown70_4;
    public sbyte Unknown70_5 = Unknown70_5;
    public sbyte Unknown70_6 = Unknown70_6;
    public short Unknown70_7 = Unknown70_7;
    public unsafe void WriteToPointer(GlassesStyleData* ptr)
    {
        ptr->Unknown70_1 = Unknown70_1;
        ptr->Unknown70_2 = Unknown70_2;
        ptr->Unknown70_3 = Unknown70_3;
        ptr->Unknown70_4 = Unknown70_4;
        ptr->Unknown70_5 = Unknown70_5;
        ptr->Unknown70_6 = Unknown70_6;
        ptr->Unknown70_7 = Unknown70_7;
    }
}
