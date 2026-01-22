using System.Collections.Generic;
using ActionTimelineReplacement.Sheets;

namespace ActionTimelineReplacement.Base.Setups;

public static unsafe partial class Setup
{
    public static void LoadVfx(IEnumerable<uint> idx, bool reset = false)
    {
        foreach (var key in idx)
        {
            SetVfx(key, reset);
        }
    }


    public static void SetVfx(uint id, bool reset = false)
    {
        var data = Hooks.GetVFXData(id);
        var data2 = Hooks.GetVFXDatapublic(id);
        var replacement = reset
            ? VfxManager.GetOriginal(id)
            : VfxManager.GetReplacement(id);

        Service.Log.Info("Setting vfx data for [{id}].", id);
        replacement.WriteSEString(data2);
    }
}