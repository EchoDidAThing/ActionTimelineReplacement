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
public unsafe class PlayTimelineDetour : IDisposable
{    // This method is in CS, but we're having this as an example of how to declare our own delegate.
    private delegate void SetPlayTimelineDelegate(ulong a1, ushort a2, ulong a3);
    //"45 85 C0 75 04 88 51 3D"
    
    [Signature("E8 ?? ?? ?? ?? 4C 8B BC 24 ?? ?? ?? ?? 4C 8D 9C 24 ?? ?? ?? ?? 49 8B 5B ?? 49 8B 73 ?? 41 0F 28 73", DetourName = nameof(SetPlayTimelineDetour))]
    private Hook<SetPlayTimelineDelegate>? _PlayTimelineHook;

    public void Init()
    {
        this._PlayTimelineHook?.Enable();
    }

    public void Dispose()
    {
        this._PlayTimelineHook?.Dispose();
    }

    private void SetPlayTimelineDetour(ulong unk1, ushort RowId, ulong unk2)
    {
        Service.Log.Information("A PLaytimeline has happened(outside try)");
        try
        {
            Service.Log.Information("A PLaytimeline happened for row!" + RowId);
            // Your plugin logic goes here.
        }
        catch (Exception ex)
        {
            Service.Log.Error(ex, "An error occured when handling a PLaytimeline event.");
        }

        // We're intentionally suppressing nullability checks. You can only get to this code if the hook exists.
        // There's no way this can ever be null.
        this._PlayTimelineHook!.Original(unk1, RowId, unk2);
    }
}
