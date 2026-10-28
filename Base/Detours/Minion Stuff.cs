using System;
using Dalamud.Hooking;
using FFXIVClientStructs.FFXIV.Client.Game.Character;
using FFXIVClientStructs.FFXIV.Client.Game.Event;
using FFXIVClientStructs.FFXIV.Client.Game.Object;
using FFXIVClientStructs.FFXIV.Client.System.Resource.Handle;

namespace ActionTimelineReplacement.Base.Detours;

public unsafe class Minionstuffs : IDisposable
{
    private readonly Hook<CompanionContainer.Delegates.SetupCompanion>? _SetupCompanionHook;
    public Minionstuffs()
    {
        _SetupCompanionHook = Service.GameInteropProvider.HookFromAddress<CompanionContainer.Delegates.SetupCompanion>(CompanionContainer.Addresses.SetupCompanion.Value, SetupCompanionDetour);
        
        this._SetupCompanionHook.Enable();
    }
    
    
    private void SetupCompanionDetour(CompanionContainer* thisptr, short companionId, uint param)
    {
        _SetupCompanionHook!.Original(thisptr, companionId, param);
        var companionObject = thisptr->CompanionObject;
        if (companionObject == null) return;
        //companionObject->Scale = 20f;
        Service.Log.Error("companion triggered! " + thisptr->CompanionObject->GetName());
    }
    public void Dispose()
    {
        this._SetupCompanionHook.Dispose();
        //FFXIVClientStructs.FFXIV.Client.Game.ActionManager.Delegates.
        //Service.AddonLifecycle.UnregisterListener(OnActionBarRequestedUpdate);
        
    }
}
