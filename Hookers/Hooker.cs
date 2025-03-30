using System;
using System.Linq;
using Dalamud.Hooking;
using Dalamud.Utility.Signatures;

namespace ActionTimelineReplacement.Hookers;

public unsafe class Hooker : IDisposable
{
    private delegate ActionData* GetActionDataDelegate(uint actionId);

    [Signature("E8 ?? ?? ?? ?? 80 FB 12", DetourName = nameof(GetActionDataDetour))]
    private Hook<GetActionDataDelegate> GetActionDataHook { get; init; } = null!;

    public Hooker()
    {
        Service.GameInteropProvider.InitializeFromAttributes(this);
        GetActionDataHook.Enable();
    }

    private ActionData* GetActionDataDetour(uint actionId)
    {
        var ret = GetActionDataHook.Original(actionId);
        if (Service.Config.EnableReplacement)
        {
            foreach (var replacement in Service.Config.ActionTimelineReplacements
                         .Where(replacement => replacement.Enabled)
                         .OrderByDescending(replacement => replacement.Priority))
            {
                if (!replacement.Replacements.TryGetValue(actionId, out var replacementValue)) continue;
                if (!replacementValue.Enabled) continue;

                return  replacementValue.Replacement.WriteToPointer(ret);
            }
        }
        
        return  Service.GetOriginalReplacement(actionId).WriteToPointer(ret);
    }

    public void Dispose()
    {
        GetActionDataHook.Dispose();

        GC.SuppressFinalize(this);
    }
}


