using ActionTimelineReplacement.Sheets;
using System;

namespace ActionTimelineReplacement.Base.Setups;

public static unsafe partial class Setup
{
    public static void SetupAll(bool reset = false)
    {
        //TOSETUP: add setup here
        LoadAction(ActionManager.AllActionIds);
        LoadMount(MountManager.AllMountIds);
        LoadMountCustomize(MountCustomizeManager.AllMountCustomizeIds);
        LoadStatus(StatusManager.AllStatusIds);
        LoadStatusLoopVFX(StatusLoopVFXManager.AllStatusLoopVFXIds);
        LoadStatusHitEffect(StatusHitEffectManager.AllStatusHitEffectIds);
        LoadTiltParam(TiltParamManager.AllTiltParamIds);
        //LoadPlaceName(PlaceNameManager.AllPlaceNameIds);
        LoadGlasses(GlassesManager.AllGlassesIds);
        LoadGlassesStyle(GlassesStyleManager.AllGlassesStyleIds);
        LoadOrnament(OrnamentManager.AllOrnamentIds);
        LoadOrnamentCustomize(OrnamentCustomizeManager.AllOrnamentCustomizeIds);
        LoadVfx(VfxManager.AllVfxIds);
        //LoadOrnamentCustomizeGroup(OrnamentCustomizeGroupManager.AllOrnamentCustomizeGroupIds);
        //LoadActionTimeline(ActionTimelineManager.AllActionTimelineIds); //find sigs
        //LoadPointMenuChoice(PointMenuChoiceManager.AllPointMenuChoiceIds); //find sigs
    }

    public static void SetupByType(uint id, string type, bool reset = false)
    {
        switch (type)
        {
            case "Action":
                SetAction(id, reset);
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
            case "Vfx":
                SetVfx(id, reset);
                break;
            default:
                Service.Log.Error("Datasheet type [{type}] is not defined", type);
                break;


        }
    }
}