using ActionTimelineReplacement.Base.Structs;

namespace ActionTimelineReplacement.Sheets;

public class OrnamentTransientsConfig(OrnamentTransientsReplace replace, bool enabled)
{
    public bool Enabled = enabled;
    public OrnamentTransientsReplace Replacement = replace;

    public static OrnamentTransientsConfig CreateEntry(uint key)
    {
        return new OrnamentTransientsConfig(OrnamentTransientsManager.GetOriginal(key), false);
    }
}

public class OrnamentTransientsReplace(
    ushort icon,
    string singular,
    string plural,
    string description
    )
{

    public ushort Icon = icon;
    public string Singular = singular;
    public string Plural = plural;
    public string Description = description;

    public bool CompareValues(OrnamentTransientsReplace second)
    {
        if(this.Icon == second.Icon && this.Singular == second.Singular && this.Plural == second.Plural && this.Description == second.Description) return true;
        return false;
    }

    // this is where we would have raw memory writing
}
