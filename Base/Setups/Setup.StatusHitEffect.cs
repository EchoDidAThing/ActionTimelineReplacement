using System.Collections.Generic;
using ActionTimelineReplacement.Sheets;

namespace ActionTimelineReplacement.Base.Setups;

public static unsafe partial class Setup
{
    public static void LoadStatusHitEffect(IEnumerable<uint> idx, bool reset = false)
    {
        foreach (var key in idx)
        {
            SetStatusHitEffect(key, reset);
        }
    }
    public static void SetStatusHitEffect(uint id, bool reset = false)
    {
        var data = Hooks.GetStatusHitEffectData(id);
        var replacement = reset
            ? StatusHitEffectManager.GetOriginal(id)
            : StatusHitEffectManager.GetReplacement(id);

        Service.Log.Info("Setting StatusHitEffect data for [{id}].", id);
        replacement.WriteToPointer(data);
    }
}