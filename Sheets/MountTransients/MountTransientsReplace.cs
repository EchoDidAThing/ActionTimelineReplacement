using ActionTimelineReplacement.Base.Structs;

namespace ActionTimelineReplacement.Sheets;

public class MountTransientsConfig(MountTransientsReplace replace, bool enabled)
{
    public bool Enabled = enabled;
    public MountTransientsReplace Replacement = replace;

    public static MountTransientsConfig CreateEntry(uint key)
    {
        return new MountTransientsConfig(MountTransientsManager.GetOriginal(key), false);
    }
}

public class MountTransientsReplace(
    ushort icon,
    string singular,
    string plural,
    string description,
    string descriptionenhanced,
    string tooltip
    )
{

    public ushort Icon = icon;
    public string Singular = singular;
    public string Plural = plural;
    public string Description = description;
    public string DescriptionEnhanced = descriptionenhanced;
    public string Tooltip = tooltip;

    public bool CompareValues(MountTransientsReplace second)
    {
        if(this.Icon == second.Icon && this.Singular == second.Singular && this.Plural == second.Plural && this.Description == second.Description && this.DescriptionEnhanced == second.DescriptionEnhanced && this.Tooltip == second.Tooltip) return true;
        return false;
    }

    // this is where we would have raw memory writing
}
