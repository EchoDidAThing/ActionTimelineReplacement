using ActionTimelineReplacement.Base.Structs;

namespace ActionTimelineReplacement.Sheets;

public class TraitTransientsConfig(TraitTransientsReplace replace, bool enabled)
{
    public bool Enabled = enabled;
    public TraitTransientsReplace Replacement = replace;

    public static TraitTransientsConfig CreateEntry(uint key)
    {
        return new TraitTransientsConfig(TraitTransientsManager.GetOriginal(key), false);
    }
}

public class TraitTransientsReplace(
    ushort icon,
    string singular,
    string description
    )
{

    public ushort Icon = icon;
    public string Name = singular;
    public string Description = description;

    public bool CompareValues(TraitTransientsReplace second)
    {
        if(this.Icon == second.Icon && this.Name == second.Name && this.Description == second.Description) return true;
        return false;
    }

    // this is where we would have raw memory writing
}
