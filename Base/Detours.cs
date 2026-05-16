using ActionTimelineReplacement.Base.Structs;
using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Hooking;
using Dalamud.Plugin.Services;
using Lumina.Excel.Sheets;
using Lumina.Excel.Sheets.Experimental;
using System;
using System.Collections.Generic;
using System.Text;

namespace ActionTimelineReplacement.Base;
public class GlassesDropDetour : IDisposable
{

    public Action<uint> OnVFX;

    public delegate void OnGlassesDropFuncDelegate(uint rowID);
    private readonly Hook<OnGlassesDropFuncDelegate> hookGlassesDrop;


    public bool IsValid = false;
    public GlassesDropDetour(IGameInteropProvider interopprovider)
    {
        try
        {
            var GlassesDropFuncPtr = "4C 8D 05 ?? ?? ?? ?? 48 8B CF 8D 53 ?? E8 ?? ?? ?? ?? 89 74 24";
            hookGlassesDrop = interopprovider.HookFromSignature<OnGlassesDropFuncDelegate>(GlassesDropFuncPtr, OnGlassesDropDetour);

            hookGlassesDrop.Enable();

            IsValid = true;
        }
        catch (Exception ex)
        {
            //Service.Log.Error("VFXHook");

        }
    }


    public void Dispose()
    {
        hookGlassesDrop?.Dispose();
        IsValid = false;
    }

    void OnGlassesDropDetour(uint rowID)
    {

        hookGlassesDrop.Original(rowID);
    }
}
