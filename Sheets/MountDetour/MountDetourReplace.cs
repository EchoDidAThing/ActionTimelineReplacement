using ActionTimelineReplacement.Base.Global;
using ActionTimelineReplacement.Base.Structs;
using ActionTimelineReplacement.Sheets;
using System.Collections;
using System.Runtime.InteropServices;
using static ActionTimelineReplacement.Base.Structs.EffectContainerFAFO;

namespace ActionTimelineReplacement.Sheets;

public class RacialScalingSet (float malemidlander, float femalemidlander, float malehighlander, float femalehighlander, float maleelezen, float femaleelezen, float malelalafell, float femalelalafell, 
    float malemiqote, float femalemiqote, float maleroegadyn, float femaleroegadyn, float maleaura, float femaleaura, float malehrothgar, float femalehrothgar, float maleviera, float femaleviera)
{
    public float MaleMidlander = malemidlander;
    public float FemaleMidlander = femalemidlander;
    public float MaleHighlander = malehighlander;
    public float FemaleHighlander = femalehighlander;
    public float MaleElezen = maleelezen;
    public float FemaleElezen = femaleelezen;
    public float MaleLalafell = malelalafell;
    public float FemaleLalafell = femalelalafell;
    public float MaleMiqote = malemiqote;
    public float FemaleMiqote = femalemiqote;
    public float MaleRoegadyn = maleroegadyn;
    public float FemaleRoegadyn = femaleroegadyn;
    public float MaleAura = maleaura;
    public float FemaleAura = femaleaura;
    public float MaleHrothgar = malehrothgar;
    public float FemaleHrothgar = femalehrothgar;
    public float MaleViera = maleviera;
    public float FemaleViera = femaleviera;

    public float GetByRaceAndSex(byte Race, byte Sex)
    {
        float output = 0f;
        switch (Sex)
        {     
            case 0:
                switch (Race)
                {
                    case 100:
                        return MaleMidlander;
                    case 101:
                        return MaleMidlander;
                    case 102:
                        return MaleElezen;
                    case 103:
                        return MaleLalafell;
                    case 104:
                        return MaleMiqote;
                    case 105:
                        return MaleRoegadyn;
                    case 6:
                        return MaleAura;
                    case 107:
                        return MaleHrothgar;
                    case 108:
                        return MaleViera;
                    default:
                        Service.Log.Error(Race + "is not defined in GetByRaceAndSex -> Male");
                        return output;
                }
            case 1:
                switch (Race)
                {
                    case 100:
                        return FemaleMidlander;
                    case 101:
                        return FemaleMidlander;
                    case 102:
                        return FemaleElezen;
                    case 103:
                        return FemaleLalafell;
                    case 104:
                        return FemaleMiqote;
                    case 105:
                        return FemaleRoegadyn;
                    case 6:
                        return FemaleAura;
                    case 107:
                        return FemaleHrothgar;
                    case 108:
                        return FemaleViera;
                    default:
                        Service.Log.Error(Race + "is not defined in GetByRaceAndSex -> Female");
                        return output;
                }
            default:
                Service.Log.Error(Sex + "is not defined in GetByRaceAndSex");
                return output;
        }
    }
}
public class TiltSet( byte tiltOrigin, byte unk1, byte unk2, float tiltAngle, float tiltSpeed, bool reverseRotation)
{
    public byte TiltOrigin = tiltOrigin;
    public byte Unk1 = unk1;
    public byte Unk2 = unk2;
    public float TiltAngle = tiltAngle;
    public float TiltSpeed = tiltSpeed;
    public bool ReverseRotation = reverseRotation;
}

public class MountDetourConfig(MountDetourReplace replace, bool enabled)
{
    public bool Enabled = enabled;
    public MountDetourReplace Replacement = replace;
    public static MountDetourConfig CreateEntry(uint key)
    {
        MountDetourReplace originalvalues = MountDetourManager.GetOriginal(key);
        return new MountDetourConfig(originalvalues, false);
    }
}

public class MountDetourReplace(
    uint bgmIndex,
    string bgmPath, 
    //int floatHeight,
    int riseSinkTilt,
    int groundAnimationSpeed,
    int flightAnimationSpeed,
    int swimAnimationSpeed,
    RacialScalingSet modelScale,
    RacialScalingSet cameraScale,
    TiltSet mountGroundTilt,
    TiltSet mountFlightTilt,
    TiltSet riderGroundTilt,
    TiltSet riderFlightTilt)
{
    public uint BGMIndex = bgmIndex;
    public string BGMPath = bgmPath;
    //int FloatHeight = floatHeight;
    public int RiseSinkTilt = riseSinkTilt;
    public int GroundAnimationSpeed = groundAnimationSpeed;
    public int FlightAnimationSpeed = flightAnimationSpeed;
    public int SwimAnimationSpeed = swimAnimationSpeed;
    public RacialScalingSet Scale = modelScale;
    public RacialScalingSet CameraScale = cameraScale;
    public TiltSet MountGroundTilt = mountGroundTilt;
    public TiltSet MountFlightTilt = mountFlightTilt;
    public TiltSet RiderGroundTilt = riderGroundTilt;
    public TiltSet RiderFlightTilt = riderFlightTilt;
}