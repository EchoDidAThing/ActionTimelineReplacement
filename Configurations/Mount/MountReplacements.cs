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
    ushort tiltGround,
    ushort tiltFlySwim,
    ushort tiltParam3,
    ushort tiltParam4,
    ushort Unk1,
    ushort Unk2,
    ushort Unk3,
    ushort Unk4,
    ushort mountCustomize,
    ushort Unk5,
    ushort SwimAnimSpeed)
{
    public ushort RideBGM = rideBGM;
    public ushort TiltParam1 = tiltGround;
    public ushort TiltParam2 = tiltFlySwim;
    public ushort TiltParam3 = tiltParam3;
    public ushort TiltParam4 = tiltParam4;
    public ushort Unk1 = Unk1;
    public ushort Unk2 = Unk2;
    public ushort Unk3 = Unk3;
    public ushort Unk4 = Unk4;
    public ushort MountCustomize = mountCustomize;
    public ushort Unk5 = Unk5;
    public ushort Unk6 = SwimAnimSpeed;
    public unsafe void WriteToPointer(MountData* pointer)
    {
        pointer->RideBGM = RideBGM;
        pointer->TiltGround = TiltParam1;
        pointer->TiltFlySwim = TiltParam2;
        pointer->TiltParam3 = TiltParam3;
        pointer->TiltParam4 = TiltParam4;
        pointer->FlyUpDownTilt = Unk1;
        pointer->Unk2 = Unk2;
        pointer->Unk3 = Unk3;
        pointer->Unk4 = Unk4;
        pointer->MountCustomize = MountCustomize;
        pointer->Unk5 = Unk5;
        pointer->UnderwaterAnimSpeed = Unk6;
    }
}