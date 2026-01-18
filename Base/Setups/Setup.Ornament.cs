using System.Collections.Generic;
using ActionTimelineReplacement.Sheets;

namespace ActionTimelineReplacement.Base.Setups;

public static unsafe partial class Setup
{
    public static void LoadOrnament(IEnumerable<uint> idx, bool reset = false)
    {
        foreach (var key in idx)
        {
            SetOrnament(key, reset);
        }
    }
    public static void SetOrnament(uint id, bool reset = false)
    {
        var data = Hooks.GetOrnamentData(id);
        var replacement = reset
            ? OrnamentManager.GetOriginal(id)
            : OrnamentManager.GetReplacement(id);

        Service.Log.Info("Setting ornament data for [{id}].", id);
        replacement.WriteToPointer(data);
    }
}