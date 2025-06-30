using ActionTimelineReplacement.Hookers;

namespace ActionTimelineReplacement.Configurations;

public class ActionReplacementConfig(ActionReplacement replacement, bool enabled)
{
    public bool Enabled = enabled;
    public ActionReplacement Replacement { get; } = replacement;
}

public class ActionReplacement(
    ushort animationStart,
    ushort animationEnd,
    ushort actionTimelineHit,
    ushort castVfx,
    byte Unknown1,
    byte Unknown2,
    byte Unknown4,
    byte Unknown_70)
{
    public ushort AnimationStart = animationStart;
    public ushort AnimationEnd = animationEnd;
    public ushort ActionTimelineHit = actionTimelineHit;
    public ushort CastVfx = castVfx;
    public byte Unknown1 = Unknown1;
    public byte Unknown2 = Unknown2;
    public byte Unknown4 = Unknown4;
    public byte Unknown_70 = Unknown_70;
    public unsafe void WriteToPointer(ActionData* pointer)
    {
        pointer->CastVfx = CastVfx;
        pointer->AnimationStart = AnimationStart;
        pointer->AnimationEnd = AnimationEnd;
        pointer->ActionTimelineHit = ActionTimelineHit;
        pointer->Unknown1 = Unknown1;
        pointer->Unknown2 = Unknown2;
        pointer->Unknown4 = Unknown4;
        pointer->Unknown_70 = Unknown_70;
    }
}