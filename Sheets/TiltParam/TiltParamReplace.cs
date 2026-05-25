using ActionTimelineReplacement.Base.Global;
using ActionTimelineReplacement.Base.Structs;
using System;

namespace ActionTimelineReplacement.Sheets;

public class TiltParamConfig(TiltParamReplace replace, bool enabled)
{
    public bool Enabled = enabled;
    public TiltParamReplace Replacement = replace;


    public static TiltParamConfig CreateEntry(uint key)
    {
        TiltParamReplace originalvalues = TiltParamManager.GetOriginal(key);
        return new TiltParamConfig(originalvalues, false);
    }
}

public class TiltParamReplace(
    float unknown0,
    byte unknown1,
    byte unknown2,
    byte unknown3,
    byte unknown4,
    bool mouseReverse)
{
    public float Unknown0 = unknown0;
    public byte Unknown1 = unknown1;
    public byte Unknown2 = unknown2;
    public byte Unknown3 = unknown3;
    public byte Unknown4 = unknown4;
    public bool Unknown5 = mouseReverse;
    public byte UnknownBitfield1 = ProcessingGlobals.PackBools(mouseReverse);
    public unsafe void WriteToPointer(TiltParamData* ptr)
    {
        ptr->TiltRate = Unknown0;
        ptr->RotationOriginOffset = Unknown1;
        ptr->MaxAngle = Unknown2;
        ptr->Unknown3 = Unknown3;
        ptr->Unknown4 = Unknown4;
        ptr->UnknownBitfield1 = UnknownBitfield1;
    }
}