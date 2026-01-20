using System.Collections.Generic;
using ActionTimelineReplacement.Sheets;

namespace ActionTimelineReplacement.Base.Setups;

public static unsafe partial class Setup
{
    public static void LoadMountCustomize(IEnumerable<uint> idx, bool reset = false)
    {
        foreach (var key in idx)
        {
            SetMountCustomize(key, reset);
        }
    }
    public static void SetMountCustomize(uint id, bool reset = false)
    {
        var data = Hooks.GetMountCustomizeData(id);
        var replacement = reset
            ? MountCustomizeManager.GetOriginal(id)
            : MountCustomizeManager.GetReplacement(id);

        Service.Log.Info("Setting mountcustomize data for [{id}].", id);
        replacement.WriteToPointer(data);
    }
}