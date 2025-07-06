using ActionTimelineReplacement.Sheets;

namespace ActionTimelineReplacement.Base.Setups;

public static unsafe partial class Setup
{
    public static void SetupAll(bool reset = false)
    {
        //TOSETUP: add setup here
        LoadAction(ActionManager.AllActionIds);
        LoadMount(MountManager.AllMountIds);
        LoadStatus(StatusManager.AllStatusIds);
        LoadTiltParam(TiltParamManager.AllTiltParamIds);
        LoadPlaceName(PlaceNameManager.AllPlaceNameIds);
        LoadGlasses(GlassesManager.AllGlassesIds);
        LoadGlassesStyle(GlassesStyleManager.AllGlassesStyleIds);
        LoadOrnament(OrnamentManager.AllOrnamentIds);
        LoadOrnamentCustomize(OrnamentCustomizeManager.AllOrnamentCustomizeIds);
        //LoadOrnamentCustomizeGroup(OrnamentCustomizeGroupManager.AllOrnamentCustomizeGroupIds);
        //LoadActionTimeline(ActionTimelineManager.AllActionTimelineIds); //find sigs
        //LoadPointMenuChoice(PointMenuChoiceManager.AllPointMenuChoiceIds); //find sigs
    }
}