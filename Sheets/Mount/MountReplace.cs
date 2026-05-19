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
    ushort flyUpDownTilt,
    ushort unknown6,
    ushort unknown7,
    ushort unknown8,
    ushort mountCustomize,
    ushort swimAnimSpeed)
    //byte mountBoolSet1
{
    public ushort RideBGM = rideBGM;
    public ushort TiltParam1 = tiltGround;
    public ushort TiltParam2 = tiltFlySwim;
    public ushort TiltParam3 = tiltParam3;
    public ushort TiltParam4 = tiltParam4;
    public ushort FlyUpDownTilt = flyUpDownTilt;
    public ushort Unknown6 = unknown6;
    public ushort Unknown7 = unknown7;
    public ushort Unknown8 = unknown8;
    public ushort MountCustomize = mountCustomize;
    public ushort SwimAnimSpeed = swimAnimSpeed;
    //public byte MountBoolSet1 = mountBoolSet1;
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
    }
}