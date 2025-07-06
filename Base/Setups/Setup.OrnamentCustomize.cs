using System.Collections.Generic;
using ActionTimelineReplacement.Sheets;

namespace ActionTimelineReplacement.Base.Setups;

public static unsafe partial class Setup
{
    public static void LoadOrnamentCustomize(IEnumerable<uint> idx, bool reset = false)
    {
        foreach (var key in idx)
        {
            SetOrnamentCustomize(key, reset);
        }
    }
    public static void SetOrnamentCustomize(uint id, bool reset = false)
    {
        var data = Hooks.GetOrnamentCustomizeData(id);
        var replacement = reset
            ? OrnamentCustomizeManager.GetOriginal(id)
            : OrnamentCustomizeManager.GetReplacement(id);

        Service.Log.Info("Setting ornament customize data for [{id}].", id);
        replacement.WriteToPointer(data);
    }
}