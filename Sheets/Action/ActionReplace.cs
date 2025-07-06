using ActionTimelineReplacement.Base.Structs;

namespace ActionTimelineReplacement.Sheets;

public class ActionConfig(ActionReplace replace, bool enabled)
{
    public bool Enabled = enabled;
    public ActionReplace Replacement => replace;
}

public class ActionReplace(
    ushort animationStart,
    ushort animationEnd,
    ushort actionTimelineHit,
    ushort castVfx,
    ushort actionCategory,
    byte unknown1,
    byte unknown2,
    byte unknown4,
    byte unknown_70)
{
    public ushort AnimationStart = animationStart;
    public ushort AnimationEnd = animationEnd;
    public ushort ActionTimelineHit = actionTimelineHit;
    public ushort CastVfx = castVfx;
    public ushort ActionCategory = actionCategory;
    public byte Unknown1 = unknown1;
    public byte Unknown2 = unknown2;
    public byte Unknown4 = unknown4;
    public byte Unknown_70 = unknown_70;
    public unsafe void WriteToPointer(ActionData* ptr)
    {
        ptr->CastVfx = CastVfx;
        ptr->AnimationStart = AnimationStart;
        ptr->AnimationEnd = AnimationEnd;
        ptr->ActionTimelineHit = ActionTimelineHit;
        ptr->ActionCategory = ActionCategory;
        ptr->Unknown1 = Unknown1;
        ptr->Unknown2 = Unknown2;
        ptr->Unknown4 = Unknown4;
        ptr->Unknown_70 = Unknown_70;
    }
}