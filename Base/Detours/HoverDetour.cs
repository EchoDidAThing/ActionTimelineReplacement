using ActionTimelineReplacement.Base.Structs;
using Dalamud.Game.Addon.Lifecycle;
using Dalamud.Game.Addon.Lifecycle.AddonArgTypes;
using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Hooking;
using Dalamud.Plugin.Services;
using Dalamud.Utility.Signatures;
using FFXIVClientStructs.FFXIV.Client.UI.Misc;
using Lumina.Excel.Sheets;
using Lumina.Excel.Sheets.Experimental;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ActionTimelineReplacement.Base.Detours;
public unsafe class HoverDetour : IDisposable
{    
    public void Init()
    {
        Service.AddonLifecycle.RegisterListener(AddonEvent.PreReceiveEvent, OnEvent);
    }


    public void Dispose()
    {
        Service.AddonLifecycle.UnregisterListener(AddonEvent.PreReceiveEvent, OnEvent);
    }

    private void OnEvent(AddonEvent type, AddonArgs args)
    {
        Service.Log.Information("A hover has happened(outside try)");
        try
        {
            Service.Log.Information("A hover has happened");
            // Your plugin logic goes here.
        }
        catch (Exception ex)
        {
            Service.Log.Error(ex, "An error occured when handling A hover.");
        }

        // We're intentionally suppressing nullability checks. You can only get to this code if the hook exists.
        // There's no way this can ever be null.
        //this._macroPlayTimelineHook!.Original(unk1, RowId, unk2);
    }
}
