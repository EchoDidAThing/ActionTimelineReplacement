using System.Collections.Generic;
using System.Runtime.InteropServices;
using ActionTimelineReplacement.Configurations;
using ActionTimelineReplacement.Hookers.HookersData;

namespace ActionTimelineReplacement.Hookers;

public static unsafe class Methods
{
    private delegate ActionData* GetActionDataDelegate(uint RowId);
    private delegate ActionTimelineData* GetActionTimelineDataDelegate(uint RowId);
    private delegate WeaponTimelineData* GetWeaponTimelineDataDelegate(uint RowId);
    private delegate ActionCastTimelineData* GetActionCastTimelineDataDelegate(uint RowId);
    private delegate ActionCastVFXData* GetActionCastVFXDataDelegate(uint RowId);
    private delegate ActionTransientData* GetActionTransientDataDelegate(uint RowId);

    private delegate StatusData* GetStatusDataDelegate(uint RowId);
    private delegate StatusHitEffectData* GetStatusHitEffectDataDelegate(uint RowId);
    private delegate StatusLoopVFXData* GetStatusLoopVFXDataDelegate(uint RowId);

    private delegate VFXData* GetVFXDataDelegate(uint RowId);


    private delegate MountData* GetMountDataDelegate(uint RowId);
    private delegate TiltData* GetTiltDataDelegate(uint RowId);
    private delegate MountCustomizeData* GetMountCustomizeDataDelegate(uint RowId);
    private delegate MountTransientData* GetMountTransientDataDelegate(uint RowId);
    private delegate PlaceNameData* GetPlaceNameDataDelegate(uint RowId);
    private delegate PetMirageData* GetPetMirageDataDelegate(uint RowId);




    private static GetActionDataDelegate? _getActionDataHook;
    private static GetActionTimelineDataDelegate? _getActionTimelineDataHook;
    private static GetWeaponTimelineDataDelegate? _getWeaponTimelineDataHook;
    private static GetActionCastTimelineDataDelegate? _getActionCastTimelineDataHook;
    private static GetActionCastVFXDataDelegate? _getActionCastVFXDataHook;
    private static GetActionTransientDataDelegate? _getActionTransientDataHook;

    private static GetStatusDataDelegate? _getStatusDataHook;
    private static GetStatusHitEffectDataDelegate? _getStatusHitEffectDataHook;
    private static GetStatusLoopVFXDataDelegate? _getStatusLoopVFXDataHook;

    private static GetVFXDataDelegate? _getVFXDataHook;


    private static GetMountDataDelegate? _getMountDataHook;
    private static GetTiltDataDelegate? _getMountTiltDataHook;
    private static GetMountCustomizeDataDelegate? _getMountCustomizeDataHook;
    private static GetMountTransientDataDelegate? _getMountTransientDataHook;
    private static GetPlaceNameDataDelegate? _getPlaceNameDataHook;
    private static GetPetMirageDataDelegate? _getPetMirageDataHook;

    private static ActionData* GetActionData(uint RowId)
    {
        _getActionDataHook ??= Marshal.GetDelegateForFunctionPointer<GetActionDataDelegate>(
                Service.Scanner.ScanText("E8 ?? ?? ?? ?? 80 FB 12"));

        return _getActionDataHook(RowId);
    }
    private static ActionTimelineData* GetActionTimelineData(uint RowId)
    {
        _getActionTimelineDataHook ??= Marshal.GetDelegateForFunctionPointer<GetActionTimelineDataDelegate>(
                Service.Scanner.ScanText("E8 ?? ?? ?? ?? 48 85 C0 74 ?? 80 78 ?? ?? 75 ?? 0F B6 4E"));
        //ANOTHER BIG MAYBE

        return _getActionTimelineDataHook(RowId);
    }
    private static WeaponTimelineData* GetWeaponTimelineData(uint RowId)
    {
        _getWeaponTimelineDataHook ??= Marshal.GetDelegateForFunctionPointer<GetWeaponTimelineDataDelegate>(
                Service.Scanner.ScanText("E8 ?? ?? ?? ?? 48 85 C0 74 ?? F6 40 ?? ?? 74 ?? 83 FE"));

        return _getWeaponTimelineDataHook(RowId);
    }
    private static ActionCastTimelineData* GetActionCastTimelineData(uint RowId)
    {
        _getActionCastTimelineDataHook ??= Marshal.GetDelegateForFunctionPointer<GetActionCastTimelineDataDelegate>(
                Service.Scanner.ScanText("E8 ?? ?? ?? ?? 48 85 C0 0F 84 ?? ?? ?? ?? 0F B7 08 66 85 C9"));
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


    private static StatusData* GetStatusData(uint RowId)
    {
        _getStatusDataHook ??= Marshal.GetDelegateForFunctionPointer<GetStatusDataDelegate>(
                Service.Scanner.ScanText("E8 ?? ?? ?? ?? 48 85 C0 0F 84 ?? ?? ?? ?? 44 0F B7 03"));
        //inside "E8 ?? ?? ?? ?? 48 85 C0 74 ?? F6 40 ?? ?? 75 ?? 0F B6 48 ?? 84 C9" sttausmanager:playgainvfx
        //or "E8 ?? ?? ?? ?? 48 89 45 ?? 48 8B D8 48 85 C0 0F 84 ?? ?? ?? ?? F6 40" from ongainstatus? may be needed.
        // or "E8 ?? ?? ?? ?? 48 85 C0 0F 84 ?? ?? ?? ?? 44 0F B7 03" under actionmanager

        return _getStatusDataHook(RowId);
    }
    private static StatusHitEffectData* GetStatusHitEffectData(uint RowId)
    {
        _getStatusHitEffectDataHook ??= Marshal.GetDelegateForFunctionPointer<GetStatusHitEffectDataDelegate>(
                Service.Scanner.ScanText("E8 ?? ?? ?? ?? 48 85 C0 74 ?? 48 8B 1F 0F B7 38"));
        //inside sttausmanager:playgainvfx

        return _getStatusHitEffectDataHook(RowId);
    }
    private static StatusLoopVFXData* GetStatusLoopVFXData(uint RowId)
    {
        _getStatusLoopVFXDataHook ??= Marshal.GetDelegateForFunctionPointer<GetStatusLoopVFXDataDelegate>(
                Service.Scanner.ScanText("E8 ?? ?? ?? ?? 48 8B D8 48 85 C0 74 ?? 66 44 39 30 74 "));
        //tHIS IS A BIG MAYBE. NOT SURE IF IT ACTUALLY IS.

        return _getStatusLoopVFXDataHook(RowId);
    }

    private static VFXData* GetVFXData(uint RowId)
    {
        _getVFXDataHook ??= Marshal.GetDelegateForFunctionPointer<GetVFXDataDelegate>(
                Service.Scanner.ScanText("E8 ?? ?? ?? ?? 48 8B D0 48 8B CB E8 ?? ?? ?? ?? 8B 83 ?? ?? ?? ?? 0F 28 B4 24"));

        return _getVFXDataHook(RowId);
    }



    private static MountData* GetMountData(uint RowId)
    {
        _getMountDataHook ??= Marshal.GetDelegateForFunctionPointer<GetMountDataDelegate>(
                Service.Scanner.ScanText("E8 ?? ?? ?? ?? 48 8B F8 48 85 C0 0F 84 ?? ?? ?? ?? 0F B7 40 ?? 66 85 C0 75 ?? 66 39 6F"));

        return _getMountDataHook(RowId);
    }
    private static TiltData* GetTiltData(uint RowId)
    {
        _getMountTiltDataHook ??= Marshal.GetDelegateForFunctionPointer<GetTiltDataDelegate>(
                Service.Scanner.ScanText("E8 ?? ?? ?? ?? 0F B7 4F ?? 48 8B F0 E8 ?? ?? ?? ?? C6 43"));

        return _getMountTiltDataHook(RowId);
    }
    private static MountTransientData* GetMountTransientData(uint RowId)
    {
        _getMountTransientDataHook ??= Marshal.GetDelegateForFunctionPointer<GetMountTransientDataDelegate>(
                Service.Scanner.ScanText("E8 ?? ?? ?? ?? 48 8B F8 48 85 F6 74 ?? 48 85 C0 74 ?? 41 B9 ?? ?? ?? ?? 8B D3 45 8D 41"));
        // OR "E8 ?? ?? ?? ?? 48 8B F8 4D 85 F6 0F 84 ?? ?? ?? ?? 48 85 C0 0F 84 ?? ?? ?? ?? 41 B9 ?? ?? ?? ?? 8B D3 45 8D 41"

        return _getMountTransientDataHook(RowId);
    }
    private static MountCustomizeData* GetMountCustomizeData(uint RowId)
    {
        _getMountCustomizeDataHook ??= Marshal.GetDelegateForFunctionPointer<GetMountCustomizeDataDelegate>(
                Service.Scanner.ScanText("E8 ?? ?? ?? ?? F3 0F 10 35 ?? ?? ?? ?? 48 8B F8"));

        return _getMountCustomizeDataHook(RowId);
    }

    private static PlaceNameData* GetPlaceNameData(uint RowId)
    {
        _getPlaceNameDataHook ??= Marshal.GetDelegateForFunctionPointer<GetPlaceNameDataDelegate>(
                Service.Scanner.ScanText("E8 ?? ?? ?? ?? 48 85 C0 74 ?? 0F B6 48 ?? E8 ?? ?? ?? ?? 84 C0 74 ?? 8B CB"));
        //E8 ?? ?? ?? ?? 48 85 C0 74 ?? 48 8B C8 48 83 C4 ?? 5B E9 ?? ?? ?? ?? 48 8B 0D
        //there are, like, 30 calls, so just picking one to test

        return _getPlaceNameDataHook(RowId);
    }

    private static PetMirageData* GetPetMirageData(uint RowId)
    {
        _getPetMirageDataHook ??= Marshal.GetDelegateForFunctionPointer<GetPetMirageDataDelegate>(
                Service.Scanner.ScanText("E8 ?? ?? ?? ?? 48 85 C0 0F 84 ?? ?? ?? ?? 48 8B C8 E8 ?? ?? ?? ?? 49 8B CC"));
        //should be right. double check against 7.21

        return _getPetMirageDataHook(RowId);
    }



    public static void SetupAll(bool reset = false)
    {
        //add setup here
        SetupActions(ActionReplacementsManager.AllActionIds);
        SetupMounts(MountReplacementsManager.AllMountIds);
        SetupTilts(TiltReplacementsManager.AllTiltIds);
        SetupPlaceNames(PlaceNameReplacementsManager.AllPlaceNameIds);
        SetupPetMirages(PetMirageReplacementsManager.AllPetMirageIds);
    }




    public static void SetupActions(IEnumerable<uint> actionIds, bool reset = false)
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
            ? ActionReplacementsManager.GetOriginalReplacement(actionId)
            : ActionReplacementsManager.GetReplacement(actionId);

        Service.Log.Info("Set the Action[{ActionID}] with Start[{Start}] End[{End}] Hit[{Hit}] Vfc[{Vfx}]",
            actionId, replacement.AnimationStart, replacement.AnimationEnd, replacement.ActionTimelineHit, replacement.CastVfx);
        replacement.WriteToPointer(data);
    }



    public static void SetupMounts(IEnumerable<uint> mountIds, bool reset = false)
    {
        foreach (var mountId in mountIds)
        {
            SetupMount(mountId, reset);
        }
    }

    public static void SetupMount(uint mountId, bool reset = false)
    {
        var data = GetMountData(mountId);
        var replacement = reset
            ? MountReplacementsManager.GetOriginalReplacement(mountId)
            : MountReplacementsManager.GetReplacement(mountId);

        Service.Log.Info("Set the Mount[{MountID}] with RideBGM[{RideBGM}] TiltParam1[{TiltParam1}] TiltParam2[{TiltParam2}] TiltParam3[{TiltParam3}] TiltParam4[{TiltParam4}] MountCustomize[{MountCustomize}]",
            mountId, replacement.RideBGM, replacement.TiltParam1, replacement.TiltParam2, replacement.TiltParam3, replacement.TiltParam4, replacement.MountCustomize);
        replacement.WriteToPointer(data);
    }


    public static void SetupTilts(IEnumerable<uint> tiltIds, bool reset = false)
    {
        foreach (var tiltId in tiltIds)
        {
            SetupTilt(tiltId, reset);
        }
    }

    public static void SetupTilt(uint tiltId, bool reset = false)
    {
        var data = GetTiltData(tiltId);
        var replacement = reset
            ? TiltReplacementsManager.GetOriginalReplacement(tiltId)
            : TiltReplacementsManager.GetReplacement(tiltId);

        Service.Log.Info("Set the Tilt[{TiltID}] with Unknown0[{Unknown0}] Unknown1[{Unknown1}] Unknown2[{Unknown2}] Unknown3[{Unknown3}] Unknown4[{Unknown4}]",
            tiltId, replacement.Unknown0, replacement.Unknown1, replacement.Unknown2, replacement.Unknown3, replacement.Unknown4);
        replacement.WriteToPointer(data);
    }

    public static void SetupPlaceNames(IEnumerable<uint> placeNameIds, bool reset = false)
    {
        foreach (var placeNameId in placeNameIds)
        {
            SetupPlaceName(placeNameId, reset);
        }
    }

    public static void SetupPlaceName(uint placeNameId, bool reset = false)
    {
        var data = GetPlaceNameData(placeNameId);
        var replacement = reset
            ? PlaceNameReplacementsManager.GetOriginalReplacement(placeNameId)
            : PlaceNameReplacementsManager.GetReplacement(placeNameId);

        Service.Log.Info("Set the place name[{placeNameId}] with Unk1[{placeNameUnk1}] Unk2[{placeNameUnk2}] Unk3[{placeNameUnk3}] Unk4[{placeNameUnk4}] Unk5[{placeNameUnk5}] Unk6[{placeNameUnk6}] Unk7[{placeNameUnk7}] Unk8[{placeNameUnk8}]",
            placeNameId, replacement.placeNameUnk1, replacement.placeNameUnk2, replacement.placeNameUnk3, replacement.placeNameUnk4, replacement.placeNameUnk5, replacement.placeNameUnk6, replacement.placeNameUnk7, replacement.placeNameUnk8);
        replacement.WriteToPointer(data);
    }
    
    public static void SetupPetMirages(IEnumerable<uint> petMirageIds, bool reset = false)
    {
        foreach (var petMirageId in petMirageIds)
        {
            SetupPetMirage(petMirageId, reset);
        }
    }

    public static void SetupPetMirage(uint petMirageId, bool reset = false)
    {
        var data = GetPetMirageData(petMirageId);
        var replacement = reset
            ? PetMirageReplacementsManager.GetOriginalReplacement(petMirageId)
            : PetMirageReplacementsManager.GetReplacement(petMirageId);

        Service.Log.Info("Set the pet mirage name[{petMirageNameId}] with Unk0[{petMirageUnk0}] Unk1[{petMirageUnk1}] Unk2[{petMirageUnk2}] Unk3[{petMirageUnk3}] Unk4[{petMirageUnk4}] Unk5[{petMirageUnk5}] Unk6[{petMirageUnk6}] Unk7[{petMirageUnk7}] Scale[{petMirageScale}]",
            petMirageId, replacement.petMirageUnk0, replacement.petMirageUnk1, replacement.petMirageUnk2, replacement.petMirageUnk3, replacement.petMirageUnk4, replacement.petMirageUnk5, replacement.petMirageUnk6, replacement.petMirageUnk7, replacement.petMirageScale);
        replacement.WriteToPointer(data);
    }
}