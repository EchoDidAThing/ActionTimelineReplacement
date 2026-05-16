using ActionTimelineReplacement.Base.Structs;

namespace ActionTimelineReplacement.Sheets;

public class MountCustomizeConfig(MountCustomizeReplace replace, bool enabled)
{
    public bool Enabled = enabled;
    public MountCustomizeReplace Replacement => replace;
    public static MountCustomizeConfig CreateEntry(uint key)
    {
        MountCustomizeReplace originalvalues = MountCustomizeManager.GetOriginal(key);
        return new MountCustomizeConfig(originalvalues, false);
    }
}

public class MountCustomizeReplace(
    ushort hyurMidlanderMaleScale,
    ushort hyurMidlanderFemaleScale,
    ushort hyurHighlanderMaleScale,
    ushort hyurHighlanderFemaleScale,
    ushort elezenMaleScale,
    ushort elezenFemaleScale,
    ushort lalafellMaleScale,
    ushort lalafellFemaleScale,
    ushort miqoteMaleScale,
    ushort miqoteFemaleScale,
    ushort roegadynMaleScale,
    ushort roegadynFemaleScale,
    ushort auRaMaleScale,
    ushort auRaFemaleScale,
    ushort hrothgarMaleScale,
    ushort hrothgarFemaleScale,
    ushort vieraMaleScale,
    ushort vieraFemaleScale,
    ushort hyurMidlanderMaleCameraHeight,
    ushort hyurMidlanderFemaleCameraHeight,
    ushort hyurHighlanderMaleCameraHeight,
    byte hyurHighlanderFemaleCameraHeight,
    byte elezenMaleCameraHeight,
    byte elezenFemaleCameraHeight,
    byte lalafellMaleCameraHeight,
    byte lalafellFemaleCameraHeight,
    byte miqoteMaleCameraHeight,
    byte miqoteFemaleCameraHeight,
    byte roegadynMaleCameraHeight,
    byte roegadynFemaleCameraHeight,
    byte auRaMaleCameraHeight,
    byte auRaFemaleCameraHeight,
    byte hrothgarMaleCameraHeight,
    byte hrothgarFemaleCameraHeight,
    byte vieraMaleCameraHeight,
    byte vieraFemaleCameraHeight)
{
    public ushort HyurMidlanderMaleScale = hyurMidlanderMaleScale;
    public ushort HyurMidlanderFemaleScale = hyurMidlanderFemaleScale;
    public ushort HyurHighlanderMaleScale = hyurHighlanderMaleScale;
    public ushort HyurHighlanderFemaleScale = hyurHighlanderFemaleScale;
    public ushort ElezenMaleScale = elezenMaleScale;
    public ushort ElezenFemaleScale = elezenFemaleScale;
    public ushort LalafellMaleScale = lalafellMaleScale;
    public ushort LalafellFemaleScale = lalafellFemaleScale;
    public ushort MiqoteMaleScale = miqoteMaleScale;
    public ushort MiqoteFemaleScale = miqoteFemaleScale;
    public ushort RoegadynMaleScale = roegadynMaleScale;
    public ushort RoegadynFemaleScale = roegadynFemaleScale;
    public ushort AuRaMaleScale = auRaMaleScale;
    public ushort AuRaFemaleScale = auRaFemaleScale;
    public ushort HrothgarMaleScale = hrothgarMaleScale;
    public ushort HrothgarFemaleScale = hrothgarFemaleScale;
    public ushort VieraMaleScale = vieraMaleScale;
    public ushort VieraFemaleScale = vieraFemaleScale;
    public ushort HyurMidlanderMaleCameraHeight = hyurMidlanderMaleCameraHeight;
    public ushort HyurMidlanderFemaleCameraHeight = hyurMidlanderFemaleCameraHeight;
    public ushort HyurHighlanderMaleCameraHeight = hyurHighlanderMaleCameraHeight;
    public byte HyurHighlanderFemaleCameraHeight = hyurHighlanderFemaleCameraHeight;
    public byte ElezenMaleCameraHeight = elezenMaleCameraHeight;
    public byte ElezenFemaleCameraHeight = elezenFemaleCameraHeight;
    public byte LalafellMaleCameraHeight = lalafellMaleCameraHeight;
    public byte LalafellFemaleCameraHeight = lalafellFemaleCameraHeight;
    public byte MiqoteMaleCameraHeight = miqoteMaleCameraHeight;
    public byte MiqoteFemaleCameraHeight = miqoteFemaleCameraHeight;
    public byte RoegadynMaleCameraHeight = roegadynMaleCameraHeight;
    public byte RoegadynFemaleCameraHeight = roegadynFemaleCameraHeight;
    public byte AuRaMaleCameraHeight = auRaMaleCameraHeight;
    public byte AuRaFemaleCameraHeight = auRaFemaleCameraHeight;
    public byte HrothgarMaleCameraHeight = hrothgarMaleCameraHeight;
    public byte HrothgarFemaleCameraHeight = hrothgarFemaleCameraHeight;
    public byte VieraMaleCameraHeight = vieraMaleCameraHeight;
    public byte VieraFemaleCameraHeight = vieraFemaleCameraHeight;
    public unsafe void WriteToPointer(MountCustomizeData* ptr)
    {
        ptr->HyurMidlanderMaleScale = HyurMidlanderMaleScale;
        ptr->HyurMidlanderFemaleScale = HyurMidlanderFemaleScale;
        ptr->HyurHighlanderMaleScale = HyurHighlanderMaleScale;
        ptr->HyurHighlanderFemaleScale = HyurHighlanderFemaleScale;
        ptr->ElezenMaleScale = ElezenMaleScale;
        ptr->ElezenFemaleScale = ElezenFemaleScale;
        ptr->LalafellMaleScale = LalafellMaleScale;
        ptr->LalafellFemaleScale = LalafellFemaleScale;
        ptr->MiqoteMaleScale = MiqoteMaleScale;
        ptr->MiqoteFemaleScale = MiqoteFemaleScale;
        ptr->RoegadynMaleScale = RoegadynMaleScale;
        ptr->RoegadynFemaleScale = RoegadynFemaleScale;
        ptr->AuRaMaleScale = AuRaMaleScale;
        ptr->AuRaFemaleScale = AuRaFemaleScale;
        ptr->HrothgarMaleScale = HrothgarMaleScale;
        ptr->HrothgarFemaleScale = HrothgarFemaleScale;
        ptr->VieraMaleScale = VieraMaleScale;
        ptr->VieraFemaleScale = VieraFemaleScale;
        ptr->HyurMidlanderMaleCameraHeight = HyurMidlanderMaleCameraHeight;
        ptr->HyurMidlanderFemaleCameraHeight = HyurMidlanderFemaleCameraHeight;
        ptr->HyurHighlanderMaleCameraHeight = HyurHighlanderMaleCameraHeight;
        ptr->HyurHighlanderFemaleCameraHeight = HyurHighlanderFemaleCameraHeight;
        ptr->ElezenMaleCameraHeight = ElezenMaleCameraHeight;
        ptr->ElezenFemaleCameraHeight = ElezenFemaleCameraHeight;
        ptr->LalafellMaleCameraHeight = LalafellMaleCameraHeight;
        ptr->LalafellFemaleCameraHeight = LalafellFemaleCameraHeight;
        ptr->MiqoteMaleCameraHeight = MiqoteMaleCameraHeight;
        ptr->MiqoteFemaleCameraHeight = MiqoteFemaleCameraHeight;
        ptr->RoegadynMaleCameraHeight = RoegadynMaleCameraHeight;
        ptr->RoegadynFemaleCameraHeight = RoegadynFemaleCameraHeight;
        ptr->AuRaMaleCameraHeight = AuRaMaleCameraHeight;
        ptr->AuRaFemaleCameraHeight = AuRaFemaleCameraHeight;
        ptr->HrothgarMaleCameraHeight = HrothgarMaleCameraHeight;
        ptr->HrothgarFemaleCameraHeight = HrothgarFemaleCameraHeight;
        ptr->VieraMaleCameraHeight = VieraMaleCameraHeight;
        ptr->VieraFemaleCameraHeight = VieraFemaleCameraHeight;
    }
}