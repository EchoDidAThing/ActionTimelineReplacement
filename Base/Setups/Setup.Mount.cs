using System.Collections.Generic;
using ActionTimelineReplacement.Sheets;

namespace ActionTimelineReplacement.Base.Setups;

public static unsafe partial class Setup
{
    public static void LoadMount(IEnumerable<uint> idx, bool reset = false)
    {
        foreach (var key in idx)
        {
            SetMount(key, reset);
        }
    }
    public static void SetMount(uint id, bool reset = false)
    {
        var data = Hooks.GetMountData(id);
        var replacement = reset
            ? MountManager.GetOriginal(id)
            : MountManager.GetReplacement(id);

        Service.Log.Info("Setting mount data for [{id}].", id);
        replacement.WriteToPointer(data);
    }
}