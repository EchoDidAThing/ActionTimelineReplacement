using System.Collections.Generic;
using ActionTimelineReplacement.Sheets;

namespace ActionTimelineReplacement.Base.Setups;

public static unsafe partial class Setup
{
    public static void LoadGlasses(IEnumerable<uint> idx, bool reset = false)
    {
        foreach (var key in idx)
        {
            SetGlasses(key, reset);
        }
    }
    public static void SetGlasses(uint id, bool reset = false)
    {
        var data = Hooks.GetGlassesData(id);
        var replacement = reset
            ? GlassesManager.GetOriginal(id)
            : GlassesManager.GetReplacement(id);

        Service.Log.Info("Setting glasses data for [{id}].", id);
        replacement.WriteToPointer(data);
    }
}