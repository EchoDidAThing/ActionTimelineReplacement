using System.Collections.Generic;
using ActionTimelineReplacement.Sheets;

namespace ActionTimelineReplacement.Base.Setups;

public static unsafe partial class Setup
{
    public static void LoadTiltParam(IEnumerable<uint> idx, bool reset = false)
    {
        foreach (var key in idx)
        {
            SetTiltParam(key, reset);
        }
    }
    public static void SetTiltParam(uint id, bool reset = false)
    {
        var data = Hooks.GetTiltParamData(id);
        var replacement = reset
            ? TiltParamManager.GetOriginal(id)
            : TiltParamManager.GetReplacement(id);

        Service.Log.Info("Setting tilt data for [{id}].", id);
        replacement.WriteToPointer(data);
    }
}