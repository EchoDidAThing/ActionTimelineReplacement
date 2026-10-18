using System;
using FFXIVClientStructs.FFXIV.Client.Game.Event;
using FFXIVClientStructs.FFXIV.Client.Game.Object;

namespace ActionTimelineReplacement.Base.Detours;

public unsafe class Minionstuffs : IDisposable
{
    
    public void Dispose()
    {
        
        //E8 ?? ?? ?? ?? 48 8B E8 48 85 C0 0F 84 ?? ?? ?? ?? 48 8B 13 48 8B CB 48 89 BB 
        
        //This is where the  Data is read off of the sheet
        // When we're done, disable the hook again and clean up. Dispose does this in one go!
        //this._CreateAndSetupMountHook.Dispose();
        //FFXIVClientStructs.FFXIV.Client.Game.ActionManager.Delegates.
        //Service.AddonLifecycle.UnregisterListener(OnActionBarRequestedUpdate);
        
    }
}
