using System.Collections.Generic;
using ActionTimelineReplacement.Sheets;

namespace ActionTimelineReplacement.Base.Setups;

public static unsafe partial class Setup
{
    public static void LoadActionCastVFX(IEnumerable<uint> idx, bool reset = false)
    {
        foreach (var key in idx)
        {
            SetActionCastVFX(key, reset);
        }
    }
    public static void SetActionCastVFX(uint id, bool reset = false)
    {
        var data = Hooks.GetActionCastVFXData(id);
        var replacement = reset
            ? ActionCastVFXManager.GetOriginal(id)
            : ActionCastVFXManager.GetReplacement(id);

        Service.Log.Info("Setting action data for [{id}].", id);
        replacement.WriteToPointer(data);
    }
}