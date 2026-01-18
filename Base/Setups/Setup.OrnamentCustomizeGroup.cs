using System.Collections.Generic;
using ActionTimelineReplacement.Sheets;

namespace ActionTimelineReplacement.Base.Setups;

public static unsafe partial class Setup
{
    public static void LoadOrnamentCustomizeGroup(IEnumerable<uint> idx, bool reset = false)
    {
        /*
        foreach (var key in idx)
        {
            SetOrnamentCustomizeGroup(key, reset);
        }
        */
    }
    public static void SetOrnamentCustomizeGroup(uint id, bool reset = false)
    {
        /*
        var data = Hooks.GetOrnamentCustomizeGroupData(id);
        var replacement = reset
            ? OrnamentCustomizeGroupManager.GetOriginal(id)
            : OrnamentCustomizeGroupManager.GetReplacement(id);

        Service.Log.Info("Setting ornament customize group data for [{id}].", id);
        replacement.WriteToPointer(data);
        */
    }
}