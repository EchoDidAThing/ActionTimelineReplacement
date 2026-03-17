using ActionTimelineReplacement.Base;
using ActionTimelineReplacement.Base.Structs;
using System.Data.Common;
using System.Diagnostics.Contracts;
using static Dalamud.Interface.Utility.Raii.ImRaii;
using ActionTimelineReplacement.Base.Global;

namespace ActionTimelineReplacement.Sheets;

public class StatusHitEffectConfig(StatusHitEffectReplace replace, bool enabled)
{
    public bool Enabled = enabled;
    public StatusHitEffectReplace Replacement => replace;
}

public class StatusHitEffectReplace(
    ushort vfx)
{
    public ushort VFX = vfx;
    public unsafe void WriteToPointer(StatusHitEffectData* ptr)
    {
        ptr->Vfx = VFX;
    }
}
