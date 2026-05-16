using ActionTimelineReplacement.Sheets;
using System;

namespace ActionTimelineReplacement.Base.Setups;

public static unsafe partial class Setup
{
    public static void SetupAll(bool reset = false)
    {
        //TOSETUP: add setup here
        LoadAction(ActionManager.AllActionIds, reset);
        LoadActionCastVFX(ActionCastVFXManager.AllActionCastVFXIds, reset);
        LoadActionTimeline(ActionTimelineManager.AllActionTimelineIds, reset);
        LoadMount(MountManager.AllMountIds, reset);
        LoadMountCustomize(MountCustomizeManager.AllMountCustomizeIds, reset);
        LoadStatus(StatusManager.AllStatusIds, reset);
        LoadStatusLoopVFX(StatusLoopVFXManager.AllStatusLoopVFXIds, reset);
        LoadStatusHitEffect(StatusHitEffectManager.AllStatusHitEffectIds, reset);
        LoadTiltParam(TiltParamManager.AllTiltParamIds, reset);
        //LoadPlaceName(PlaceNameManager.AllPlaceNameIds, reset);
        LoadGlasses(GlassesManager.AllGlassesIds, reset);
        LoadGlassesStyle(GlassesStyleManager.AllGlassesStyleIds, reset);
        LoadOrnament(OrnamentManager.AllOrnamentIds, reset);
        LoadOrnamentCustomize(OrnamentCustomizeManager.AllOrnamentCustomizeIds, reset);
        LoadVfx(VfxManager.AllVfxIds, reset);
        //LoadOrnamentCustomizeGroup(OrnamentCustomizeGroupManager.AllOrnamentCustomizeGroupIds, reset);
        //LoadActionTimeline(ActionTimelineManager.AllActionTimelineIds, reset); //find sigs
        //LoadPointMenuChoice(PointMenuChoiceManager.AllPointMenuChoiceIds, reset); //find sigs
    }

    public static void SetupByType(uint id, string type, bool reset = false)
    {
        switch (type)
        {
            case "Action":
                SetAction(id, reset);
                break;
            case "ActionCastTimeline":
                SetActionCastTimeline(id, reset);
                break;
            case "ActionCastVFX":
                SetActionCastVFX(id, reset);
                break;
            case "ActionTimeline":
                SetActionTimeline(id, reset);
                break;
            case "Glasses":
                SetGlasses(id, reset);
                break;
            case "GlassesStyle":
                SetGlassesStyle(id, reset);
                break;
            case "Mount":
                SetMount(id, reset);
                break;
            case "MountCustomize":
                SetMountCustomize(id, reset);
                break;
            case "Ornament":
                SetOrnament(id, reset);
                break;
            case "OrnamentCustomize":
                SetOrnamentCustomize(id, reset);
                break;
            case "OrnamentCustomizeGroup":
                SetOrnamentCustomizeGroup(id, reset);
                break;
            case "Placename":
                //SetPlaceName(id, reset);
                break;
            case "PointMenuChoice":
                //SetPointMenuChoice(id, reset);
                break;
            case "Status":
                SetStatus(id, reset);
                break;
            case "StatusHitEffect":
                SetStatusHitEffect(id, reset);
                break;
            case "StatusLoopVFX":
                SetStatusLoopVFX(id, reset);
                break;
            case "TiltParam":
                SetTiltParam(id, reset);
                break;
            case "VFX":
                SetVfx(id, reset);
                break;
            default:
                Service.Log.Error("Datasheet type [{type}] is not definedin SetupByType", type);
                break;


        }
    }
}