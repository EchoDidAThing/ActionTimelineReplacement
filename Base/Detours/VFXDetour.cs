using ActionTimelineReplacement.Base.Structs;
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
public unsafe class VFXDetour : IDisposable
{    // This method is in CS, but we're having this as an example of how to declare our own delegate.
    private delegate Lumina.Excel.Sheets.VFX * SetVFXDelegate(ushort a2);
    //"45 85 C0 75 04 88 51 3D"
    
    [Signature("E8 ?? ?? ?? ?? 48 8B D0 48 8B CB E8 ?? ?? ?? ?? 45 84 F6 ", DetourName = nameof(SetVFXDetour))]
    private Hook<SetVFXDelegate>? _macroVFXHook;

    public void Init()
    {
        this._macroVFXHook?.Enable();
    }


    public void Dispose()
    {
        this._macroVFXHook?.Dispose();
    }

    private Lumina.Excel.Sheets.VFX * SetVFXDetour(ushort RowId)
    {
        Lumina.Excel.Sheets.VFX * returner = this._macroVFXHook!.Original(RowId);
        Service.Log.Information("A VFX has happened(outside try)");
        try
        {
            Service.Log.Information("A VFX happened for row!" + RowId);
            // Your plugin logic goes here.
        }
        catch (Exception ex)
        {
            Service.Log.Error(ex, "An error occured when handling a VFX event.");
        }

        // We're intentionally suppressing nullability checks. You can only get to this code if the hook exists.
        // There's no way this can ever be null.
        return returner;
    }
}
