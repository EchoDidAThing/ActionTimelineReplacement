using ActionTimelineReplacement.Base.Structs;

namespace ActionTimelineReplacement.Sheets;

public class StatusTransientsConfig(StatusTransientsReplace replace, bool enabled)
{
    public bool Enabled = enabled;
    public StatusTransientsReplace Replacement = replace;

    public static StatusTransientsConfig CreateEntry(uint key)
    {
        return new StatusTransientsConfig(StatusTransientsManager.GetOriginal(key), false);
    }
}

public class StatusTransientsReplace(
    ushort icon,
    string singular,
    string description
    )
{

    public ushort Icon = icon;
    public string Name = singular;
    public string Description = description;

    public bool CompareValues(StatusTransientsReplace second)
    {
        if(this.Icon == second.Icon && this.Name == second.Name && this.Description == second.Description) return true;
        return false;
    }

    // this is where we would have raw memory writing
}
