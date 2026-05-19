using ActionTimelineReplacement.Base.Structs;

namespace ActionTimelineReplacement.Sheets
{
    public class GlassesConfig(GlassesReplace replace, bool enabled)
    {
        public bool Enabled = enabled;
        public GlassesReplace Replacement = replace;
        public static GlassesConfig CreateEntry(uint key)
        {
            GlassesReplace originalvalues = GlassesManager.GetOriginal(key);
            return new GlassesConfig(originalvalues, false);
        }
    }

    public class GlassesReplace(
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
        public unsafe void WriteToPointer(GlassesData* ptr)
        {
            ptr->Unknown70_1 = Unknown70_1;
            ptr->Unknown70_2 = Unknown70_2;
            ptr->Unknown70_3 = Unknown70_3;
            ptr->Unknown70_4 = Unknown70_4;
            ptr->Unknown70_5 = Unknown70_5;
            ptr->Unknown70_6 = Unknown70_6;
            ptr->Unknown70_7 = Unknown70_7;
            ptr->Unknown70_8 = Unknown70_8;
        }
    }
}
