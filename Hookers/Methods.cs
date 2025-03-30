using System.Collections.Generic;
using System.Runtime.InteropServices;
using ActionTimelineReplacement.Configurations;

namespace ActionTimelineReplacement.Hookers;

public static unsafe class Methods
{
    private delegate ActionData* GetActionDataDelegate(uint actionId);

    private static GetActionDataDelegate? _getActionDataHook;

    private static ActionData* GetActionData(uint actionId)
    {
        _getActionDataHook ??=
            Marshal.GetDelegateForFunctionPointer<GetActionDataDelegate>(
                Service.Scanner.ScanText("E8 ?? ?? ?? ?? 80 FB 12"));

        return _getActionDataHook(actionId);
    }

    public static void SetupAction(IEnumerable<uint> actionIds, bool reset = false)
    {
        foreach (var actionId in actionIds)
        {
            SetupAction(actionId, reset);
        }
    }

    public static void SetupAction(uint actionId, bool reset = false)
    {
        var data = GetActionData(actionId);
        var replacement = reset
            ? ReplacementsManager.GetOriginalReplacement(actionId)
            : ReplacementsManager.GetReplacement(actionId);

        replacement.WriteToPointer(data);
    }
}