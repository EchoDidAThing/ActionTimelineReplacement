using ActionTimelineReplacement.Hookers.HookersData;

namespace ActionTimelineReplacement.Configurations;

public class GlassesReplacementConfig(GlassesReplacement replacement, bool enabled)
{
    public bool Enabled = enabled;
    public GlassesReplacement Replacement { get; } = replacement;
}

public class GlassesReplacement(
    sbyte Unknown70_1,
    sbyte Unknown70_2,
    sbyte Unknown70_3,
    sbyte Unknown70_4,
    sbyte Unknown70_5,
    sbyte Unknown70_6,
    uint Unknown70_7,
    ushort Unknown70_8)
{
    public sbyte Unknown70_1 = Unknown70_1;
    public sbyte Unknown70_2 = Unknown70_2;
    public sbyte Unknown70_3 = Unknown70_3;
    public sbyte Unknown70_4 = Unknown70_4;
    public sbyte Unknown70_5 = Unknown70_5;
    public sbyte Unknown70_6 = Unknown70_6;
    public uint Unknown70_7 = Unknown70_7;
    public ushort Unknown70_8 = Unknown70_8;
    public unsafe void WriteToPointer(GlassesData* pointer)
    {
        pointer->Unknown70_1 = Unknown70_1;
        pointer->Unknown70_2 = Unknown70_2;
        pointer->Unknown70_3 = Unknown70_3;
        pointer->Unknown70_4 = Unknown70_4;
        pointer->Unknown70_5 = Unknown70_5;
        pointer->Unknown70_6 = Unknown70_6;
        pointer->Unknown70_7 = Unknown70_7;
        pointer->Unknown70_8 = Unknown70_8;
    }
}