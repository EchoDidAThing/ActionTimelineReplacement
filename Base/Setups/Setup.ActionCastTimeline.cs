using System.Collections.Generic;
using ActionTimelineReplacement.Sheets;

namespace ActionTimelineReplacement.Base.Setups;

public static unsafe partial class Setup
{
    public static void LoadActionCastTimelineVFX(IEnumerable<uint> idx, bool reset = false)
    {
        foreach (var key in idx)
        {
            SetActionCastTimeline(key, reset);
        }
    }
    public static void SetActionCastTimeline(uint id, bool reset = false)
    {
        var data = Hooks.GetActionCastTimelineData(id);
        var replacement = reset
            ? ActionCastTimelineManager.GetOriginal(id)
            : ActionCastTimelineManager.GetReplacement(id);

        Service.Log.Info("Setting action data for [{id}].", id);
        replacement.WriteToPointer(data);
    }
}