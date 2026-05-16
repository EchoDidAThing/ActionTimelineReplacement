using ActionTimelineReplacement.Base.Structs;

namespace ActionTimelineReplacement.Sheets
{
    public class ActionCastTimelineConfig(ActionCastTimelineReplace replace, bool enabled)
    {
        public bool Enabled = enabled;
        public ActionCastTimelineReplace Replacement => replace;

        public static ActionCastTimelineConfig CreateEntry(uint key)
        {
            return new ActionCastTimelineConfig(ActionCastTimelineManager.GetOriginal(key), false);
        }
    }

    public class ActionCastTimelineReplace(
        ushort actionTimeline, ushort castVfx)
    {
        public ushort ActionTimeline = actionTimeline;
        public ushort CastVfx = castVfx;
        public unsafe void WriteToPointer(ActionCastTimelineData* ptr)
        {
            ptr->ActionTimeline = ActionTimeline;
            ptr->CastVfx = CastVfx;
        }
    }
}