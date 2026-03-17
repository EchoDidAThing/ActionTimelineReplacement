using ActionTimelineReplacement.Sheets;

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
}