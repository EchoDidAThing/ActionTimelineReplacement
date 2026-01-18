/*using System.Collections.Generic;
using ActionTimelineReplacement.Sheets;

namespace ActionTimelineReplacement.Base.Setups;

public static unsafe partial class Setup
{
    public static void LoadPointMenuChoice(IEnumerable<float> idx, bool reset = false)
    {
        foreach (var key in idx)
        {
            SetPointMenuChoice(key, reset);
        }
    }
    public static void SetPointMenuChoice(float id, bool reset = false)
    {
        var data = Hooks.GetPointMenuChoiceData(id);
        var replacement = reset
            ? PointMenuChoiceManager.GetOriginal(id)
            : PointMenuChoiceManager.GetReplacement(id);

        Service.Log.Info("Setting point menu choice data for [{id}].", id);
        replacement.WriteToPointer(data);
    }
}
*/