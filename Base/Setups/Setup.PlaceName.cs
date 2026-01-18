using System.Collections.Generic;
using ActionTimelineReplacement.Sheets;

namespace ActionTimelineReplacement.Base.Setups;

public static unsafe partial class Setup
{
    public static void LoadPlaceName(IEnumerable<uint> idx, bool reset = false)
    {
        foreach (var key in idx)
        {
            SetPlaceName(key, reset);
        }
    }
    public static void SetPlaceName(uint id, bool reset = false)
    {
        var data = Hooks.GetPlaceNameData(id);
        var replacement = reset
            ? PlaceNameManager.GetOriginal(id)
            : PlaceNameManager.GetReplacement(id);

        Service.Log.Info("Setting place name data for [{id}].", id);
        replacement.WriteToPointer(data);
    }
}