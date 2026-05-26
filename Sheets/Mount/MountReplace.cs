using ActionTimelineReplacement.Base.Global;
using ActionTimelineReplacement.Base.Structs;

namespace ActionTimelineReplacement.Sheets;

public class MountConfig(MountReplace replace, bool enabled)
{
    public bool Enabled = enabled;
    public MountReplace Replacement = replace;
    public static MountConfig CreateEntry(uint key)
    {
        MountReplace originalvalues = MountManager.GetOriginal(key);
        return new MountConfig(originalvalues, false);
    }
}

public class MountReplace(
    ushort rideBGM,
    ushort tiltGround,
    ushort tiltFlySwim,
    ushort tiltParam3, 
    ushort tiltParam4,
    byte flyUpDownTilt,
    byte unknown6,
    byte unknown7,
    byte unknown8,
    byte mountCustomize,
    byte swimAnimSpeed,
    bool enableHeadgear,
    bool unk18,
    bool unk19)
    //byte mountBoolSet1
{
    public ushort RideBGM = rideBGM;
    public ushort TiltParam1 = tiltGround;
    public ushort TiltParam2 = tiltFlySwim;
    public ushort TiltParam3 = tiltParam3;
    public ushort TiltParam4 = tiltParam4;
    public byte FlyUpDownTilt = flyUpDownTilt;
    public byte Unknown6 = unknown6;
    public byte Unknown7 = unknown7;
    public byte Unknown8 = unknown8;
    public byte MountCustomize = mountCustomize;
    public byte SwimAnimSpeed = swimAnimSpeed;
    public bool EnableHeadgear = enableHeadgear;
    public bool Unk18 = unk18;
    public bool Unk19 = unk19;
    public byte UnknownBitfield1 = ProcessingGlobals.PackBools(enableHeadgear, unk18, unk19);
    public unsafe void WriteToPointer(MountData* ptr)
    {
        ptr->RideBGM = RideBGM;
        ptr->TiltGround = TiltParam1;
        ptr->TiltFlySwim = TiltParam2;
        ptr->TiltParam3 = TiltParam3;
        ptr->TiltParam4 = TiltParam4;
        ptr->FlyUpDownTilt = FlyUpDownTilt;
        ptr->Unknown6 = Unknown6;
        ptr->Unknown7 = Unknown7;
        ptr->Unknown8 = Unknown8;
        ptr->MountCustomize = MountCustomize;
        ptr->UnderwaterAnimSpeed = SwimAnimSpeed;
        //ptr->MountBoolSet1 = MountBoolSet1;
        ptr->BitField2 = UnknownBitfield1;
    }
}