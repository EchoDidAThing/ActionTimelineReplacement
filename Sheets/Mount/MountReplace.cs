using ActionTimelineReplacement.Base.Structs;

namespace ActionTimelineReplacement.Sheets;

public class MountConfig(MountReplace replace, bool enabled)
{
    public bool Enabled = enabled;
    public MountReplace Replacement => replace;
}

public class MountReplace(
    ushort rideBGM,
    ushort tiltGround,
    ushort tiltFlySwim,
    ushort tiltParam3,
    ushort tiltParam4,
    ushort unk1,
    ushort unk2,
    ushort unk3,
    ushort unk4,
    ushort mountCustomize,
    ushort unk5,
    ushort swimAnimSpeed)
{
    public ushort RideBGM = rideBGM;
    public ushort TiltParam1 = tiltGround;
    public ushort TiltParam2 = tiltFlySwim;
    public ushort TiltParam3 = tiltParam3;
    public ushort TiltParam4 = tiltParam4;
    public ushort Unk1 = unk1;
    public ushort Unk2 = unk2;
    public ushort Unk3 = unk3;
    public ushort Unk4 = unk4;
    public ushort MountCustomize = mountCustomize;
    public ushort Unk5 = unk5;
    public ushort Unk6 = swimAnimSpeed;
    public unsafe void WriteToPointer(MountData* ptr)
    {
        ptr->RideBGM = RideBGM;
        ptr->TiltGround = TiltParam1;
        ptr->TiltFlySwim = TiltParam2;
        ptr->TiltParam3 = TiltParam3;
        ptr->TiltParam4 = TiltParam4;
        ptr->FlyUpDownTilt = Unk1;
        ptr->Unk2 = Unk2;
        ptr->Unk3 = Unk3;
        ptr->Unk4 = Unk4;
        ptr->MountCustomize = MountCustomize;
        ptr->Unk5 = Unk5;
        ptr->UnderwaterAnimSpeed = Unk6;
    }
}