using System.Collections.Generic;
using ActionTimelineReplacement.Sheets;

namespace ActionTimelineReplacement.Base.Setups;

public static unsafe partial class Setup
{
    public static void LoadStatus(IEnumerable<uint> idx, bool reset = false)
    {
        foreach (var key in idx)
        {
            SetStatus(key, reset);
        }
    }
    public static void SetStatus(uint id, bool reset = false)
    {
        var data = Hooks.GetStatusData(id);
        var replacement = reset
            ? StatusManager.GetOriginal(id)
            : StatusManager.GetReplacement(id);

        Service.Log.Info("Setting status data for [{id}].", id);
        replacement.WriteToPointer(data);
    }
}