using System.Collections.Generic;
using ActionTimelineReplacement.Sheets;

namespace ActionTimelineReplacement.Base.Setups;

public static unsafe partial class Setup
{
    public static void LoadGlassesStyle(IEnumerable<uint> idx, bool reset = false)
    {
        foreach (var key in idx)
        {
            SetGlassesStyle(key, reset);
        }
    }
    public static void SetGlassesStyle(uint id, bool reset = false)
    {
        var data = Hooks.GetGlassesStyleData(id);
        var replacement = reset
            ? GlassesStyleManager.GetOriginal(id)
            : GlassesStyleManager.GetReplacement(id);

        Service.Log.Info("Setting glasses style data for [{id}].", id);
        replacement.WriteToPointer(data);
    }
}