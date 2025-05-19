using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using ActionTimelineReplacement.Hookers;
using Newtonsoft.Json;

namespace ActionTimelineReplacement.Configurations;

public class TiltReplacementSet(
    Dictionary<uint, TiltReplacementConfig> replacements)
{
    public Dictionary<uint, TiltReplacementConfig> Replacements { get; } = replacements;
}

public class TiltReplacementConfig(TiltReplacement replacement, bool enabled)
{
    public bool Enabled = enabled;
    public TiltReplacement Replacement { get; } = replacement;
}

public class TiltReplacement(
    ushort unknown0,
    byte unknown1,
    byte unknown2,
    byte unknown3,
    byte unknown4,
    bool mouseReverse)
{
    public ushort Unknown0 = unknown0;
    public byte Unknown1 = unknown1;
    public byte Unknown2 = unknown2;
    public byte Unknown3 = unknown3;
    public byte Unknown4 = unknown4;
    public bool Unknown5 = mouseReverse;
    public unsafe void WriteToPointer(TiltData* pointer)
    {
        pointer->TiltRate = Unknown0;
        pointer->RotationOriginOffset = Unknown1;
        pointer->MaxAngle = Unknown2;
        pointer->Unknown3 = Unknown3;
        pointer->Unknown4 = Unknown4;
        pointer->MouseReverse = Unknown5;
    }
}