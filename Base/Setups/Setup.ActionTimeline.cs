using System.Collections.Generic;
using ActionTimelineReplacement.Sheets;

namespace ActionTimelineReplacement.Base.Setups;

public static unsafe partial class Setup
{
    public static void LoadActionTimeline(IEnumerable<uint> idx, bool reset = false)
    {
        foreach (var key in idx)
        {
            SetActionTimeline(key, reset);
        }
    }
    public static void SetActionTimeline(uint id, bool reset = false)
    {
        var data = Hooks.GetActionTimelineData(id);
        var data2 = Hooks.GetActionTimelineDatapublic(id);
        var replacement = reset
            ? ActionTimelineManager.GetOriginal(id)
            : ActionTimelineManager.GetReplacement(id);

        Service.Log.Info("Setting action timeline data for [{id}].", id);
        replacement.WriteToPointer(data);
        replacement.WriteSEString(data2);
    }
}