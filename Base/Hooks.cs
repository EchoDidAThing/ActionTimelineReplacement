using System;
using System.Runtime.InteropServices;
using ActionTimelineReplacement.Base.Structs;

namespace ActionTimelineReplacement.Base;

public static unsafe class Hooks
{
    public const string vfxhook = "E8 ?? ?? ?? ?? 48 8B D0 48 8B CB E8 ?? ?? ?? ?? 45 84 F6"; //updated for 7.5
    public const string statusloopvfxhook = "E8 ?? ?? ?? ?? 48 8B D8 48 85 C0 74 ?? 66 39 38";//updated 7.4
    public const string actiontimelinehook = "E8 ?? ?? ?? ?? 48 85 C0 0F 84 ?? ?? ?? ?? 80 78 ?? ?? 0F 85 ?? ?? ?? ?? 32 C0"; //updated for 7.5HF1. this was a pain to find

    #region delegates
    private delegate ActionData* GetActionDataDelegate(uint RowId);
    private delegate ActionCastTimelineData* GetActionCastTimelineDataDelegate(uint RowId);
    private delegate ActionCastVFXData* GetActionCastVFXDataDelegate(uint RowId);
    private delegate StatusData* GetStatusDataDelegate(uint RowId);
    private delegate StatusLoopVFXData* GetStatusLoopVFXDataDelegate(uint RowId);
    private delegate StatusHitEffectData* GetStatusHitEffectDataDelegate(uint RowId);
    private delegate IntPtr GetStatusLoopVFXDataDelegatepublic(uint RowId);
    private delegate MountData* GetMountDataDelegate(uint RowId);
    private delegate MountCustomizeData* GetMountCustomizeDataDelegate(uint RowId);
    private delegate TiltParamData* GetTiltParamDataDelegate(uint RowId);
    private delegate GlassesData* GetGlassesDataDelegate(uint RowId);
    private delegate GlassesStyleData* GetGlassesStyleDataDelegate(uint RowId);
    //private delegate PlaceNameData* GetPlaceNameDataDelegate(uint RowId);
    private delegate ActionTimelineData* GetActionTimelineDataDelegate(uint RowId);
    private delegate IntPtr GetActionTimelineDataDelegatepublic(uint RowId);
    private delegate WeaponTimelineData* GetWeaponTimelineDataDelegate(uint RowId);
    private delegate OrnamentData* GetOrnamentDataDelegate(uint RowId);
    private delegate OrnamentCustomizeData* GetOrnamentCustomizeDataDelegate(uint RowId);
    private delegate OrnamentCustomizeGroupData* GetOrnamentCustomizeGroupDataDelegate(uint RowId);
    private delegate VfxData* GetVFXDataDelegate(uint RowId);
    private delegate IntPtr GetVFXDataDelegatepublic(uint RowId);

    //private delegate MotionTimelineData* GetMotionTimelineDataDelegate(uint RowId);
    //private delegate PointMenuChoiceData* GetPointMenuChoiceDataDelegate(float RowId);
    //private delegate ActionTransientData* GetActionTransientDataDelegate(uint RowId);
    //private delegate StatusHitEffectData* GetStatusHitEffectDataDelegate(uint RowId);
    //private delegate MountTransientData* GetMountTransientDataDelegate(uint RowId);

    #endregion
    #region hooks

    private static GetActionDataDelegate? _getActionDataHook;
    private static GetActionCastTimelineDataDelegate? _getActionCastTimelineDataHook;
    private static GetActionCastVFXDataDelegate? _getActionCastVFXDataHook;
    private static GetActionTimelineDataDelegate? _getActionTimelineDataHook;
    private static GetActionTimelineDataDelegatepublic? _getActionTimelineDataHookpublic;
    private static GetStatusDataDelegate? _getStatusDataHook;
    private static GetStatusLoopVFXDataDelegate? _getStatusLoopVFXDataHook;
    private static GetStatusLoopVFXDataDelegatepublic? _getStatusLoopVFXDataHookpublic;
    private static GetStatusHitEffectDataDelegate? _getStatusHitEffectDataHook;
    private static GetMountDataDelegate? _getMountDataHook; 
    private static GetMountCustomizeDataDelegate? _getMountCustomizeDataHook;
    private static GetTiltParamDataDelegate? _getTiltParamDataHook;
    private static GetGlassesDataDelegate? _getGlassesDataHook;
    private static GetGlassesStyleDataDelegate? _getGlassesStyleDataHook;
    //private static GetPlaceNameDataDelegate? _getPlaceNameDataHook;
    private static GetWeaponTimelineDataDelegate? _getWeaponTimelineDataHook;
    private static GetOrnamentDataDelegate? _getOrnamentDataHook;
    private static GetOrnamentCustomizeDataDelegate? _getOrnamentCustomizeDataHook;
    private static GetOrnamentCustomizeGroupDataDelegate? _getOrnamentCustomizeGroupDataHook;
    private static GetVFXDataDelegate? _getVFXDataHook;
    private static GetVFXDataDelegatepublic? _getVFXDataHookpublic;

    //private static GetPointMenuChoiceDataDelegate? _getPointMenuChoiceDataHook;
    //private static GetActionCastVFXDataDelegate? _getActionCastVFXDataHook;
    //private static GetActionTransientDataDelegate? _getActionTransientDataHook;
    //private static GetStatusHitEffectDataDelegate? _getStatusHitEffectDataHook;
    //private static GetMountTransientDataDelegate? _getMountTransientDataHook;

    #endregion
    #region sigs

    public static ActionData* GetActionData(uint RowId)
    {
        _getActionDataHook ??= Marshal.GetDelegateForFunctionPointer<GetActionDataDelegate>(Service.Scanner.ScanText(
            "E8 ?? ?? ?? ?? F6 40 3E 10"));
        //updated for 7.4, Component::Exd::ExdModule.GetActionRow located inside Client::Game::ActionManager.GetActionStatus

        return _getActionDataHook(RowId);
    }

    public static ActionCastVFXData* GetActionCastVFXData(uint RowId)
    {
        _getActionCastVFXDataHook ??= Marshal.GetDelegateForFunctionPointer<GetActionCastVFXDataDelegate>(Service.Scanner.ScanText(
            "E8 ?? ?? ?? ?? 48 8B F8 48 85 C0 74 ?? 48 8B 4B ?? E8"));
        //updated for 7.5, nested in a function below Client::Game::Character::MountContainer.vf4 and Client::Game::Character::TimelineContainer.vf4

        return _getActionCastVFXDataHook(RowId);
    }
    public static ActionCastTimelineData* GetActionCastTimelineData(uint RowId)
    {
        _getActionCastVFXDataHook ??= Marshal.GetDelegateForFunctionPointer<GetActionCastVFXDataDelegate>(Service.Scanner.ScanText(
            "E8 ?? ?? ?? ?? 48 85 C0 0F 84 ?? ?? ?? ?? 0F B7 08 66 85 C9"));
        //updated for 7.5, nested in a function below Client::Game::Character::MountContainer.vf4 and Client::Game::Character::TimelineContainer.vf4

        return _getActionCastTimelineDataHook(RowId);
    }
    public static ActionCastTimelineData* GetBGMData(uint RowId)
    {
        _getActionCastVFXDataHook ??= Marshal.GetDelegateForFunctionPointer<GetActionCastVFXDataDelegate>(Service.Scanner.ScanText(
            "E8 ?? ?? ?? ?? 48 85 C0 0F 84 ?? ?? ?? ?? 0F B7 08 66 85 C9"));
        //updated for 7.5, nested in a function below Client::Game::Character::MountContainer.vf4 and Client::Game::Character::TimelineContainer.vf4

        return _getActionCastTimelineDataHook(RowId);
    }

    public static StatusData* GetStatusData(uint RowId)
    {
        _getStatusDataHook ??= Marshal.GetDelegateForFunctionPointer<GetStatusDataDelegate>(Service.Scanner.ScanText(
            "E8 ?? ?? ?? ?? 4C 8B C0 48 85 C0 0F 84 ?? ?? ?? ?? 0F B7 13"));
        //updated for 7.5, Component::Exd::ExdModule.GetStatusRow located inside Client::Game::ActionManager.GetActionStatus
        //..from Namingway: "E8 ?? ?? ?? ?? 48 85 C0 74 96" Check it if the other one is not gonna work
        return _getStatusDataHook(RowId);
    }

    public static StatusHitEffectData* GetStatusHitEffectData(uint RowId)
    {
        _getStatusHitEffectDataHook ??= Marshal.GetDelegateForFunctionPointer<GetStatusHitEffectDataDelegate>(Service.Scanner.ScanText(
            "E8 ?? ?? ?? ?? 48 85 C0 0F 84 ?? ?? ?? ?? 0F B7 30 E9"));
        //updated for 7.4, under Client::Game::Character::ActionEffectHandler.ApplyOneTargetEffect
        return _getStatusHitEffectDataHook(RowId);
    }

    public static StatusLoopVFXData* GetStatusLoopVFXData(uint RowId)
    {
        _getStatusLoopVFXDataHook ??= Marshal.GetDelegateForFunctionPointer<GetStatusLoopVFXDataDelegate>(Service.Scanner.ScanText(statusloopvfxhook));
        

        return _getStatusLoopVFXDataHook(RowId);
    }
    public static IntPtr GetStatusLoopVFXDatapublic(uint RowId)
    {
        _getStatusLoopVFXDataHookpublic ??= Marshal.GetDelegateForFunctionPointer<GetStatusLoopVFXDataDelegatepublic>(Service.Scanner.ScanText(statusloopvfxhook));

        return _getStatusLoopVFXDataHookpublic(RowId);
    }

    public static MountData* GetMountData(uint RowId)
    {
        _getMountDataHook ??= Marshal.GetDelegateForFunctionPointer<GetMountDataDelegate>(Service.Scanner.ScanText(
            "E8 ?? ?? ?? ?? 4C 8B F8 48 85 C0 0F 84 ?? ?? ?? ?? 0F 29 B4 24"));
        //updated for 7.4, Component::Exd::ExdModule.GetMountRow located inside Client::Game::Character::MountContainer.SetupMountContainer
        return _getMountDataHook(RowId);
    }
    
    public static MountCustomizeData* GetMountCustomizeData(uint RowId)
    {
        _getMountCustomizeDataHook ??= Marshal.GetDelegateForFunctionPointer<GetMountCustomizeDataDelegate>(Service.Scanner.ScanText(
            "E8 ?? ?? ?? ?? F3 0F 10 35 ?? ?? ?? ?? 48 8B F8"));
        //updated for 7.4, Component::Exd::ExdModule.GetMountCustomizeRow located inside Client::Game::Character::MountContainer.SetupMountContainer

        return _getMountCustomizeDataHook(RowId);
    }
    
    public static TiltParamData* GetTiltParamData(uint RowId)
    {
        _getTiltParamDataHook ??= Marshal.GetDelegateForFunctionPointer<GetTiltParamDataDelegate>(Service.Scanner.ScanText(
            "E8 ?? ?? ?? ?? 0F B7 4F ?? 48 8B F0 E8 ?? ?? ?? ?? 33 D2"));
        //updated for 7.4, Component::Exd::ExdModule.GetMountTiltParamRow located inside a function within Client::Game::Character::MountContainer.CreateAndSetupMountContainer
        return _getTiltParamDataHook(RowId);
    }

    public static GlassesData* GetGlassesData(uint RowId)
    {
        _getGlassesDataHook ??= Marshal.GetDelegateForFunctionPointer<GetGlassesDataDelegate>(Service.Scanner.ScanText(
            "E8 ?? ?? ?? ?? 48 85 C0 74 ?? 66 44 39 78 ?? 7C"));
        //Not verified for 7.4 Component::Exd::ExdModule.GetGlassesRow located inside Client::UI::RaptureAtkModule.HandleGlassesDrop
        return _getGlassesDataHook(RowId);
    }

    public static GlassesStyleData* GetGlassesStyleData(uint RowId)
    {
        _getGlassesStyleDataHook ??= Marshal.GetDelegateForFunctionPointer<GetGlassesStyleDataDelegate>(Service.Scanner.ScanText(
            "E8 ?? ?? ?? ?? 48 8B F8 48 85 C0 74 ?? 66 44 39 78"));
        //Updated for 7.4 Component::Exd::ExdModule.GetGlassesStyleRow located inside Client::UI::RaptureAtkModule.HandleGlassesDrop
        return _getGlassesStyleDataHook(RowId);
    }

    /*public static PlaceNameData* GetPlaceNameData(uint RowId)
    {
        _getPlaceNameDataHook ??= Marshal.GetDelegateForFunctionPointer<GetPlaceNameDataDelegate>(Service.Scanner.ScanText(
            "E8 ?? ?? ?? ?? 48 85 C0 74 ?? 0F B6 48 ?? E8 ?? ?? ?? ?? 84 C0 74 ?? 8B CB"));
        return _getPlaceNameDataHook(RowId);
    }
    */
    public static ActionTimelineData* GetActionTimelineData(uint RowId)
    {
        _getActionTimelineDataHook ??= Marshal.GetDelegateForFunctionPointer<GetActionTimelineDataDelegate>(Service.Scanner.ScanText(
            actiontimelinehook)); 


        return _getActionTimelineDataHook(RowId);
    }
    public static IntPtr GetActionTimelineDatapublic(uint RowId)
    {
        _getActionTimelineDataHookpublic ??= Marshal.GetDelegateForFunctionPointer<GetActionTimelineDataDelegatepublic>(Service.Scanner.ScanText(
            actiontimelinehook));


        return _getActionTimelineDataHookpublic(RowId);
    }
    public static WeaponTimelineData* GetWeaponTimelineData(uint RowId)
    {
        _getWeaponTimelineDataHook ??= Marshal.GetDelegateForFunctionPointer<GetWeaponTimelineDataDelegate>(Service.Scanner.ScanText(
            "E8 ?? ?? ?? ?? 48 8B E8 48 85 C0 0F 84 ?? ?? ?? ?? 83 FF ?? 75"));


        return _getWeaponTimelineDataHook(RowId);
    }

    public static OrnamentData* GetOrnamentData(uint RowId)
    {
        _getOrnamentDataHook ??= Marshal.GetDelegateForFunctionPointer<GetOrnamentDataDelegate>(Service.Scanner.ScanText(
            "E8 ?? ?? ?? ?? 48 8B D8 48 85 C0 0F 84 ?? ?? ?? ?? F3 0F 10 05 ?? ?? ?? ?? 48 8D 8E"));
        //Updated for 7.4 Component::Exd::ExdModule.GetOrnamentRow located inside Client::Game::Character::OrnamentContainer.SetupOrnament
        return _getOrnamentDataHook(RowId);
    }

    public static OrnamentCustomizeData* GetOrnamentCustomizeData(uint RowId)
    {
        _getOrnamentCustomizeDataHook ??= Marshal.GetDelegateForFunctionPointer<GetOrnamentCustomizeDataDelegate>(Service.Scanner.ScanText(
            "E8 ?? ?? ?? ?? 48 8B D8 48 85 C0 0F 84 ?? ?? ?? ?? 0F B7 08"));
        //Updated for 7.5HF1 Component::Exd::ExdModule.GetOrnamentCustomizeRow located inside A nested function under Client::Game::Character::OrnamentContainer.Update
        return _getOrnamentCustomizeDataHook(RowId);
    }
    
    public static OrnamentCustomizeGroupData* GetOrnamentCustomizeGroupData(uint RowId)
    {
        _getOrnamentCustomizeGroupDataHook ??= Marshal.GetDelegateForFunctionPointer<GetOrnamentCustomizeGroupDataDelegate>(Service.Scanner.ScanText(
            "E8 ?? ?? ?? ?? 48 8B F0 48 85 C0 0F 84 ?? ?? ?? ?? 48 8B 17 48 8B CF 0F B6 9F"));
        //Updated for 7.4 Component::Exd::ExdModule.GetOrnamentCustomizeGroupRow located inside A nested function under Client::Game::Character::OrnamentContainer.Update
        return _getOrnamentCustomizeGroupDataHook(RowId);
    }

    public static VfxData* GetVFXData(uint RowId)
    {
        _getVFXDataHook ??= Marshal.GetDelegateForFunctionPointer<GetVFXDataDelegate>(Service.Scanner.ScanText(vfxhook));

        return _getVFXDataHook(RowId);
    }
    public static IntPtr GetVFXDatapublic(uint RowId)
    {
        _getVFXDataHookpublic ??= Marshal.GetDelegateForFunctionPointer<GetVFXDataDelegatepublic>(Service.Scanner.ScanText(vfxhook));

        return _getVFXDataHookpublic(RowId);
    }

    /*
    public static PointMenuChoiceData* GetPointMenuChoiceData(float RowId)
    {
        _getPointMenuChoiceDataHook ??= Marshal.GetDelegateForFunctionPointer<GetPointMenuChoiceDataDelegate>(Service.Scanner.ScanText(
            "E8 ?? ?? ?? ?? 4D 89 66 ?? 48 89 5C 24"));
            //this is a massive guess since the actual struct isn't defined yet, only its agent helper
        return _getPointMenuChoiceDataHook(RowId);
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

    private static MountTransientData* GetMountTransientData(uint RowId)
    {
        _getMountTransientDataHook ??= Marshal.GetDelegateForFunctionPointer<GetMountTransientDataDelegate>(Service.Scanner.ScanText(
            "E8 ?? ?? ?? ?? 48 8B F8 48 85 F6 74 ?? 48 85 C0 74 ?? 41 B9 ?? ?? ?? ?? 8B D3 45 8D 41" ));
        // OR "E8 ?? ?? ?? ?? 48 8B F8 4D 85 F6 0F 84 ?? ?? ?? ?? 48 85 C0 0F 84 ?? ?? ?? ?? 41 B9 ?? ?? ?? ?? 8B D3 45 8D 41"

        return _getMountTransientDataHook(RowId);
    }
    */
    #endregion
}