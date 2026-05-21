using ActionTimelineReplacement.Base.Structs;

namespace ActionTimelineReplacement.Sheets;

public class CompanionTransientsConfig(CompanionTransientsReplace replace, bool enabled)
{
    public bool Enabled = enabled;
    public CompanionTransientsReplace Replacement = replace;

    public static CompanionTransientsConfig CreateEntry(uint key)
    {
        return new CompanionTransientsConfig(CompanionTransientsManager.GetOriginal(key), false);
    }
}

public class CompanionTransientsReplace(
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

    public bool CompareValues(CompanionTransientsReplace second)
    {
        if(this.Icon == second.Icon && this.Singular == second.Singular && this.Plural == second.Plural && this.Description == second.Description && this.DescriptionEnhanced == second.DescriptionEnhanced && this.Tooltip == second.Tooltip) return true;
        return false;
    }

    // this is where we would have raw memory writing
}
