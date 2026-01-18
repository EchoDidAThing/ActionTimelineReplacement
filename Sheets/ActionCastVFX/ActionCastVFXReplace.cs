using ActionTimelineReplacement.Base.Structs;

namespace ActionTimelineReplacement.Sheets
{
    public class ActionCastVFXConfig(ActionCastVFXReplace replace, bool enabled)
    {
        public bool Enabled = enabled;
        public ActionCastVFXReplace Replacement => replace;
    }

    public class ActionCastVFXReplace(
        ushort castVfx)
    {
        public ushort CastVfx = castVfx;
        public unsafe void WriteToPointer(ActionCastVFXData* ptr)
        {
            ptr->Vfx = CastVfx;
        }
    }
}