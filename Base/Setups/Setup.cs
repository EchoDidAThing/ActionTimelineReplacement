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
    }
}