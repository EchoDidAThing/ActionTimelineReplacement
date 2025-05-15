using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using ActionTimelineReplacement.Hookers;
using Newtonsoft.Json;

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
    ushort castVfx)
{
    public ushort AnimationStart = animationStart;
    public ushort AnimationEnd = animationEnd;
    public ushort ActionTimelineHit = actionTimelineHit;
    public ushort CastVfx = castVfx;
    public unsafe void WriteToPointer(ActionData* pointer)
    {
        pointer->CastVfx = CastVfx;
        pointer->AnimationStart = AnimationStart;
        pointer->AnimationEnd = AnimationEnd;
        pointer->ActionTimelineHit = ActionTimelineHit;
    }
}