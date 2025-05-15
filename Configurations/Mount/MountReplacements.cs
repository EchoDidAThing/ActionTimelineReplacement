using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using ActionTimelineReplacement.Hookers;
using Newtonsoft.Json;

namespace ActionTimelineReplacement.Configurations;

public class MountReplacementSet(
    Dictionary<uint, MountReplacementConfig> replacements)
{
    public Dictionary<uint, MountReplacementConfig> Replacements { get; } = replacements;
}

public class MountReplacementConfig(MountReplacement replacement, bool enabled)
{
    public bool Enabled = enabled;
    public MountReplacement Replacement { get; } = replacement;
}

public class MountReplacement(
    ushort rideBGM,
    ushort tiltParam1,
    ushort tiltParam2,
    ushort tiltParam3,
    ushort tiltParam4,
    ushort mountCustomize)
{
    public ushort RideBGM = rideBGM;
    public ushort TiltParam1 = tiltParam1;
    public ushort TiltParam2 = tiltParam2;
    public ushort TiltParam3 = tiltParam3;
    public ushort TiltParam4 = tiltParam4;
    public ushort MountCustomize = mountCustomize;
    public unsafe void WriteToPointer(MountData* pointer)
    {
        pointer->RideBGM = RideBGM;
        pointer->TiltParam1 = TiltParam1;
        pointer->TiltParam2 = TiltParam2;
        pointer->TiltParam3 = TiltParam3;
        pointer->TiltParam4 = TiltParam4;
        pointer->MountCustomize = MountCustomize;
    }
}