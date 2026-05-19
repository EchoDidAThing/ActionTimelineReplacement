using ActionTimelineReplacement.Base.Structs;

namespace ActionTimelineReplacement.Sheets;

public class ActionTransientsConfig(ActionTransientsReplace replace, bool enabled)
{
    public bool Enabled = enabled;
    public ActionTransientsReplace Replacement = replace;

    public static ActionTransientsConfig CreateEntry(uint key)
    {
        return new ActionTransientsConfig(ActionTransientsManager.GetOriginal(key), false);
    }
}

public class ActionTransientsReplace(
    ushort icon,
    string actionName,
    string actionDesc)
{

    public ushort Icon = icon;
    public string ActionName = actionName;
    public string ActionDesc = actionDesc;

    public bool CompareValues(ActionTransientsReplace second)
    {
        if(this.Icon == second.Icon && this.ActionName == second.ActionName && this.ActionDesc == second.ActionDesc) return true;
        return false;
    }

    // this is where we would have raw memory writing
}