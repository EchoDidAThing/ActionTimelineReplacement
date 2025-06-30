using ActionTimelineReplacement.Hookers.HookersData;

namespace ActionTimelineReplacement.Configurations;

public class StatusReplacementConfig(StatusReplacement replacement, bool enabled)
{
    public bool Enabled = enabled;
    public StatusReplacement Replacement { get; } = replacement;
}

public class StatusReplacement(
    int paramModifier,
    ushort StatusLoopVFX,
    byte unknown0,
    byte statusCategory,
    byte StatusHitEffect,
    byte targetType,
    byte flags,
    byte flag2,
    byte unknown_70_1,
    sbyte unknown2)
{
    public int ParamModifier = paramModifier;
    public ushort StatusLoopVFX = StatusLoopVFX;
    public byte Unknown0 = unknown0;
    public byte StatusCategory = statusCategory;
    public byte StatusHitEffect = StatusHitEffect;
    public byte TargetType = targetType;
    public byte Flags = flags;
    public byte Flag2 = flag2;
    public byte Unknown_70_1 = unknown_70_1;
    public sbyte Unknown2 = unknown2;
    public unsafe void WriteToPointer(StatusData* pointer)
    {
        pointer->ParamModifier = ParamModifier;
        pointer->StatusLoopVFX = StatusLoopVFX;
        pointer->Unknown0 = Unknown0;
        pointer->StatusCategory = StatusCategory;
        pointer->StatusHitEffect = StatusHitEffect;
        pointer->TargetType = TargetType;
        pointer->Flags = Flags;
        pointer->Flag2 = Flag2;
        pointer->Unknown_70_1 = Unknown_70_1;
        pointer->Unknown2 = Unknown2;
    }
}