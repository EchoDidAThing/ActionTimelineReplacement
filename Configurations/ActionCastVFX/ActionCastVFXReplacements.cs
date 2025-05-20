using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using ActionTimelineReplacement.Hookers;
using Newtonsoft.Json;

namespace ActionTimelineReplacement.Configurations;


public class ActionCastVFXReplacementConfig(ActionCastVFXReplacement replacement, bool enabled)
{
    public bool Enabled = enabled;
    public ActionCastVFXReplacement Replacement { get; } = replacement;
}

public class ActionCastVFXReplacement(
    ushort castVfx)
{
    public ushort CastVfx = castVfx;
    public unsafe void WriteToPointer(ActionCastVFXData* pointer)
    {
        pointer->Vfx = CastVfx;
    }
}