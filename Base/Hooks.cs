using System.Runtime.InteropServices;
using ActionTimelineReplacement.Base.Structs;

namespace ActionTimelineReplacement.Base;

public static unsafe class Hooks
{
    #region delegates
    private delegate ActionData* GetActionDataDelegate(uint RowId);
    private delegate StatusData* GetStatusDataDelegate(uint RowId);
    private delegate MountData* GetMountDataDelegate(uint RowId);
    private delegate TiltParamData* GetTiltParamDataDelegate(uint RowId);
    private delegate GlassesData* GetGlassesDataDelegate(uint RowId);
    private delegate GlassesStyleData* GetGlassesStyleDataDelegate(uint RowId);
    private delegate PlaceNameData* GetPlaceNameDataDelegate(uint RowId);

    //private delegate ActionTimelineData* GetActionTimelineDataDelegate(uint RowId);
    //private delegate WeaponTimelineData* GetWeaponTimelineDataDelegate(uint RowId);
    //private delegate ActionCastTimelineData* GetActionCastTimelineDataDelegate(uint RowId);
    //private delegate ActionCastVFXData* GetActionCastVFXDataDelegate(uint RowId);
    //private delegate ActionTransientData* GetActionTransientDataDelegate(uint RowId);
    //private delegate StatusHitEffectData* GetStatusHitEffectDataDelegate(uint RowId);
    //private delegate StatusLoopVFXData* GetStatusLoopVFXDataDelegate(uint RowId);
    //private delegate VFXData* GetVFXDataDelegate(uint RowId);
    //private delegate MountCustomizeData* GetMountCustomizeDataDelegate(uint RowId);
    //private delegate MountTransientData* GetMountTransientDataDelegate(uint RowId);

    #endregion
    #region hooks

    private static GetActionDataDelegate? _getActionDataHook;
    private static GetStatusDataDelegate? _getStatusDataHook;
    private static GetMountDataDelegate? _getMountDataHook;
    private static GetTiltParamDataDelegate? _getTiltParamDataHook;
    private static GetGlassesDataDelegate? _getGlassesDataHook;
    private static GetGlassesStyleDataDelegate? _getGlassesStyleDataHook;
    private static GetPlaceNameDataDelegate? _getPlaceNameDataHook;

    //private static GetActionTimelineDataDelegate? _getActionTimelineDataHook;
    //private static GetWeaponTimelineDataDelegate? _getWeaponTimelineDataHook;
    //private static GetActionCastTimelineDataDelegate? _getActionCastTimelineDataHook;
    //private static GetActionCastVFXDataDelegate? _getActionCastVFXDataHook;
    //private static GetActionTransientDataDelegate? _getActionTransientDataHook;
    //private static GetStatusHitEffectDataDelegate? _getStatusHitEffectDataHook;
    //private static GetStatusLoopVFXDataDelegate? _getStatusLoopVFXDataHook;
    //private static GetVFXDataDelegate? _getVFXDataHook;
    //private static GetMountCustomizeDataDelegate? _getMountCustomizeDataHook;
    //private static GetMountTransientDataDelegate? _getMountTransientDataHook;

    #endregion
    #region sigs
    
    public static ActionData* GetActionData(uint RowId)
    {
        _getActionDataHook ??= Marshal.GetDelegateForFunctionPointer<GetActionDataDelegate>
        (Service.Scanner.ScanText(
            "E8 ?? ?? ?? ?? 80 FB 12"));

        return _getActionDataHook(RowId);
    }

    public static StatusData* GetStatusData(uint RowId)
    {
        _getStatusDataHook ??= Marshal.GetDelegateForFunctionPointer<GetStatusDataDelegate>(Service.Scanner.ScanText(
            "E8 ?? ?? ?? ?? 48 85 C0 0F 84 ?? ?? ?? ?? 44 0F B7 03"));
        //inside "E8 ?? ?? ?? ?? 48 85 C0 74 ?? F6 40 ?? ?? 75 ?? 0F B6 48 ?? 84 C9" sttausmanager:playgainvfx
        //or "E8 ?? ?? ?? ?? 48 89 45 ?? 48 8B D8 48 85 C0 0F 84 ?? ?? ?? ?? F6 40" from ongainstatus? may be needed.
        // or "E8 ?? ?? ?? ?? 48 85 C0 0F 84 ?? ?? ?? ?? 44 0F B7 03" under actionmanager
        return _getStatusDataHook(RowId);
    }

    public static MountData* GetMountData(uint RowId)
    {
        _getMountDataHook ??= Marshal.GetDelegateForFunctionPointer<GetMountDataDelegate>(Service.Scanner.ScanText(
            "E8 ?? ?? ?? ?? 48 8B F8 48 85 C0 0F 84 ?? ?? ?? ?? 0F B7 40 ?? 66 85 C0 75 ?? 66 39 6F"));
        return _getMountDataHook(RowId);
    }
    public static TiltParamData* GetTiltParamData(uint RowId)
    {
        _getTiltParamDataHook ??= Marshal.GetDelegateForFunctionPointer<GetTiltParamDataDelegate>(Service.Scanner.ScanText(
            "E8 ?? ?? ?? ?? 0F B7 4F ?? 48 8B F0 E8 ?? ?? ?? ?? C6 43"));
        return _getTiltParamDataHook(RowId);
    }

    public static GlassesData* GetGlassesData(uint RowId)
    {
        _getGlassesDataHook ??= Marshal.GetDelegateForFunctionPointer<GetGlassesDataDelegate>(Service.Scanner.ScanText(
            "E8 ?? ?? ?? ?? 48 85 C0 74 ?? 66 44 39 78 ?? 7C"));
        return _getGlassesDataHook(RowId);
    }

    public static GlassesStyleData* GetGlassesStyleData(uint RowId)
    {
        _getGlassesStyleDataHook ??= Marshal.GetDelegateForFunctionPointer<GetGlassesStyleDataDelegate>(Service.Scanner.ScanText(
            "E8 ?? ?? ?? ?? 48 85 C0 74 ?? 0F B7 CE"));
        return _getGlassesStyleDataHook(RowId);
    }

    public static PlaceNameData* GetPlaceNameData(uint RowId)
    {
        _getPlaceNameDataHook ??= Marshal.GetDelegateForFunctionPointer<GetPlaceNameDataDelegate>(Service.Scanner.ScanText(
            "E8 ?? ?? ?? ?? 48 85 C0 74 ?? 0F B6 48 ?? E8 ?? ?? ?? ?? 84 C0 74 ?? 8B CB"));
        return _getPlaceNameDataHook(RowId);
    }

    /*
    private static ActionTimelineData* GetActionTimelineData(uint RowId)
    {
        _getActionTimelineDataHook ??= Marshal.GetDelegateForFunctionPointer<GetActionTimelineDataDelegate>(Service.Scanner.ScanText(
            "E8 ?? ?? ?? ?? 48 85 C0 74 ?? 80 78 ?? ?? 75 ?? 0F B6 4E" ));
        //ANOTHER BIG MAYBE

        return _getActionTimelineDataHook(RowId);
    }

    private static WeaponTimelineData* GetWeaponTimelineData(uint RowId)
    {
        _getWeaponTimelineDataHook ??= Marshal.GetDelegateForFunctionPointer<GetWeaponTimelineDataDelegate>(Service.Scanner.ScanText(
            "E8 ?? ?? ?? ?? 48 85 C0 74 ?? F6 40 ?? ?? 74 ?? 83 FE" ));

        return _getWeaponTimelineDataHook(RowId);
    }

    private static ActionCastTimelineData* GetActionCastTimelineData(uint RowId)
    {
        _getActionCastTimelineDataHook ??= Marshal.GetDelegateForFunctionPointer<GetActionCastTimelineDataDelegate>(Service.Scanner.ScanText(
            "E8 ?? ?? ?? ?? 48 85 C0 0F 84 ?? ?? ?? ?? 0F B7 08 66 85 C9" ));
        //inside castaction

        return _getActionCastTimelineDataHook(RowId);
    }

    private static ActionCastVFXData* GetActionCastVFXData(uint RowId)
    {
        _getActionCastVFXDataHook ??= Marshal.GetDelegateForFunctionPointer<GetActionCastVFXDataDelegate>(
                Service.Scanner.ScanText(""));
        //inside castaction

        return _getActionCastVFXDataHook(RowId);
    }

    private static ActionTransientData* GetActionTransientData(uint RowId)
    {
        _getActionTransientDataHook ??= Marshal.GetDelegateForFunctionPointer<GetActionTransientDataDelegate>(
                Service.Scanner.ScanText(""));
        //inside castaction

        return _getActionTransientDataHook(RowId);
    }

    private static StatusHitEffectData* GetStatusHitEffectData(uint RowId)
    {
        _getStatusHitEffectDataHook ??= Marshal.GetDelegateForFunctionPointer<GetStatusHitEffectDataDelegate>(Service.Scanner.ScanText(
            "E8 ?? ?? ?? ?? 48 85 C0 74 ?? 48 8B 1F 0F B7 38" ));
        //inside sttausmanager:playgainvfx

        return _getStatusHitEffectDataHook(RowId);
    }

    private static StatusLoopVFXData* GetStatusLoopVFXData(uint RowId)
    {
        _getStatusLoopVFXDataHook ??= Marshal.GetDelegateForFunctionPointer<GetStatusLoopVFXDataDelegate>(Service.Scanner.ScanText(
            "E8 ?? ?? ?? ?? 48 8B D8 48 85 C0 74 ?? 66 44 39 30 74" ));
        //THIS IS A BIG MAYBE. NOT SURE IF IT ACTUALLY IS.

        return _getStatusLoopVFXDataHook(RowId);
    }

    private static VFXData* GetVFXData(uint RowId)
    {
        _getVFXDataHook ??= Marshal.GetDelegateForFunctionPointer<GetVFXDataDelegate>(Service.Scanner.ScanText(
            "E8 ?? ?? ?? ?? 48 8B D0 48 8B CB E8 ?? ?? ?? ?? 8B 83 ?? ?? ?? ?? 0F 28 B4 24" ));

        return _getVFXDataHook(RowId);
    }

    private static MountTransientData* GetMountTransientData(uint RowId)
    {
        _getMountTransientDataHook ??= Marshal.GetDelegateForFunctionPointer<GetMountTransientDataDelegate>(Service.Scanner.ScanText(
            "E8 ?? ?? ?? ?? 48 8B F8 48 85 F6 74 ?? 48 85 C0 74 ?? 41 B9 ?? ?? ?? ?? 8B D3 45 8D 41" ));
        // OR "E8 ?? ?? ?? ?? 48 8B F8 4D 85 F6 0F 84 ?? ?? ?? ?? 48 85 C0 0F 84 ?? ?? ?? ?? 41 B9 ?? ?? ?? ?? 8B D3 45 8D 41"

        return _getMountTransientDataHook(RowId);
    }

    private static MountCustomizeData* GetMountCustomizeData(uint RowId)
    {
        _getMountCustomizeDataHook ??= Marshal.GetDelegateForFunctionPointer<GetMountCustomizeDataDelegate>(Service.Scanner.ScanText(
            "E8 ?? ?? ?? ?? F3 0F 10 35 ?? ?? ?? ?? 48 8B F8" ));

        return _getMountCustomizeDataHook(RowId);
    }
    */
    #endregion
}