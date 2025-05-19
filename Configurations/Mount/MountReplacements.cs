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
    ushort mountCustomize,
    ushort Unk1,
    ushort Unk2,
    ushort Unk3,
    ushort Unk4,
    ushort Unk5,
    ushort Unk6)
{
    public ushort RideBGM = rideBGM;
    public ushort TiltParam1 = tiltParam1;
    public ushort TiltParam2 = tiltParam2;
    public ushort TiltParam3 = tiltParam3;
    public ushort TiltParam4 = tiltParam4;
    public ushort MountCustomize = mountCustomize;
    public ushort Unk1 = Unk1;
    public ushort Unk2 = Unk2;
    public ushort Unk3 = Unk3;
    public ushort Unk4 = Unk4;
    public ushort Unk5 = Unk5;
    public ushort Unk6 = Unk6;
    public unsafe void WriteToPointer(MountData* pointer)
    {
        pointer->RideBGM = RideBGM;
        pointer->TiltGround = TiltParam1;
        pointer->TiltFlySwim = TiltParam2;
        pointer->TiltParam3 = TiltParam3;
        pointer->TiltParam4 = TiltParam4;
        pointer->MountCustomize = MountCustomize;
        pointer->Unk1 = Unk1;
        pointer->Unk2 = Unk2;
        pointer->Unk3 = Unk3;
        pointer->Unk4 = Unk4;
        pointer->Unk5 = Unk5;
        pointer->Unk6 = Unk6;
    }
}