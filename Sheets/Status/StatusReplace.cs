using ActionTimelineReplacement.Base.Structs;

namespace ActionTimelineReplacement.Sheets;

public class StatusConfig(StatusReplace replace, bool enabled)
{
    public bool Enabled = enabled;
    public StatusReplace Replacement => replace;
}

public class StatusReplace(
    int paramModifier,
    ushort statusLoopVFX,
    byte unknown0,
    byte statusCategory,
    byte statusHitEffect,
    byte targetType,
    byte flags,
    byte flag2,
    byte unknown_70_1,
    sbyte unknown2)
{
    public int ParamModifier = paramModifier;
    public ushort StatusLoopVFX = statusLoopVFX;
    public byte Unknown0 = unknown0;
    public byte StatusCategory = statusCategory;
    public byte StatusHitEffect = statusHitEffect;
    public byte TargetType = targetType;
    public byte Flags = flags;
    public byte Flag2 = flag2;
    public byte Unknown_70_1 = unknown_70_1;
    public sbyte Unknown2 = unknown2;
    public unsafe void WriteToPointer(StatusData* ptr)
    {
        ptr->ParamModifier = ParamModifier;
        ptr->StatusLoopVFX = StatusLoopVFX;
        ptr->Unknown0 = Unknown0;
        ptr->StatusCategory = StatusCategory;
        ptr->StatusHitEffect = StatusHitEffect;
        ptr->TargetType = TargetType;
        ptr->Flags = Flags;
        ptr->Flag2 = Flag2;
        ptr->Unknown_70_1 = Unknown_70_1;
        ptr->Unknown2 = Unknown2;
    }
}
