using System.Collections.Generic;
using System.Runtime.InteropServices;
using ActionTimelineReplacement.Configurations;

namespace ActionTimelineReplacement.Hookers;

public static unsafe class Methods
{
    private delegate ActionData* GetActionDataDelegate(uint actionId);
    private delegate MountData* GetMountDataDelegate(uint mountId);
    private delegate TiltData* GetTiltDataDelegate(uint tiltID);
    private delegate TiltData* GetMountTilt2DataDelegate(uint tiltID);
    private delegate TiltData* GetMountTilt3DataDelegate(uint tiltID);
    private delegate TiltData* GetMountTilt4DataDelegate(uint tiltID);

    private static GetActionDataDelegate? _getActionDataHook;
    private static GetMountDataDelegate? _getMountDataHook;
    private static GetTiltDataDelegate? _getMountTilt1DataHook;
    private static GetMountTilt2DataDelegate? _getMountTilt2DataHook;
    private static GetMountTilt3DataDelegate? _getMountTilt3DataHook;
    private static GetMountTilt4DataDelegate? _getMountTilt4DataHook;

    private static ActionData* GetActionData(uint actionId)
    {
        _getActionDataHook ??= Marshal.GetDelegateForFunctionPointer<GetActionDataDelegate>(
                Service.Scanner.ScanText("E8 ?? ?? ?? ?? 80 FB 12"));

        return _getActionDataHook(actionId);
    }
    private static MountData* GetMountData(uint mountId)
    {
        _getMountDataHook ??= Marshal.GetDelegateForFunctionPointer<GetMountDataDelegate>(
                Service.Scanner.ScanText("E8 ?? ?? ?? ?? 48 8B F8 48 85 C0 0F 84 ?? ?? ?? ?? 0F B7 40 ?? 66 85 C0 75 ?? 66 39 6F"));

        return _getMountDataHook(mountId);
    }
    private static TiltData* GetTiltData(uint tiltId)
    {
        _getMountTilt1DataHook ??= Marshal.GetDelegateForFunctionPointer<GetTiltDataDelegate>(
                Service.Scanner.ScanText("E8 ?? ?? ?? ?? 0F B7 4F ?? 48 8B F0 E8 ?? ?? ?? ?? C6 43 "));

        return _getMountTilt1DataHook(tiltId);
    }
    private static TiltData* GetMountTilt2Data(uint tiltId)
    {
        _getMountTilt2DataHook ??= Marshal.GetDelegateForFunctionPointer<GetMountTilt2DataDelegate>(
                Service.Scanner.ScanText("E8 ?? ?? ?? ?? C6 43 ?? ?? 4C 8B C0"));

        return _getMountTilt2DataHook(tiltId);
    }
    private static TiltData* GetMountTilt3Data(uint tiltId)
    {
        _getMountTilt3DataHook ??= Marshal.GetDelegateForFunctionPointer<GetMountTilt3DataDelegate>(
                Service.Scanner.ScanText("E8 ?? ?? ?? ?? 0F B7 4F ?? 48 8B F0 E8 ?? ?? ?? ?? 33 D2"));

        return _getMountTilt3DataHook(tiltId);
    }
    private static TiltData* GetMountTilt4Data(uint tiltId)
    {
        _getMountTilt4DataHook ??= Marshal.GetDelegateForFunctionPointer<GetMountTilt4DataDelegate>(
                Service.Scanner.ScanText("E8 ?? ?? ?? ?? 33 D2 4C 8B C0 48 85 F6"));

        return _getMountTilt4DataHook(tiltId);
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
        var data = GetMountTilt3Data(tiltId);
        var replacement = reset
            ? TiltReplacementsManager.GetOriginalReplacement(tiltId)
            : TiltReplacementsManager.GetReplacement(tiltId);

        Service.Log.Info("Set the Tilt[{TiltID}] with Unknown0[{Unknown0}] Unknown1[{Unknown1}] Unknown2[{Unknown2}] Unknown3[{Unknown3}] Unknown4[{Unknown4}]",
            tiltId, replacement.Unknown0, replacement.Unknown1, replacement.Unknown2, replacement.Unknown3, replacement.Unknown4);
        replacement.WriteToPointer(data);
    }
}