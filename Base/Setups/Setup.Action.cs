using System.Collections.Generic;
using ActionTimelineReplacement.Sheets;

namespace ActionTimelineReplacement.Base.Setups;

public static unsafe partial class Setup
{
    public static void LoadAction(IEnumerable<uint> idx, bool reset = false)
    {
        foreach (var key in idx)
        {
            SetAction(key, reset);
        }
    }
    public static void SetAction(uint id, bool reset = false)
    {
        var data = Hooks.GetActionData(id);
        var replacement = reset
            ? ActionManager.GetOriginal(id)
            : ActionManager.GetReplacement(id);

        Service.Log.Info("Setting action data for [{id}].", id);
        replacement.WriteToPointer(data);
    }
}