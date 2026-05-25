using ActionTimelineReplacement.Base.Structs;

namespace ActionTimelineReplacement.Sheets;

public class JobTransientsConfig(JobTransientsReplace replace, bool enabled)
{
    public bool Enabled = enabled;
    public JobTransientsReplace Replacement = replace;

    public static JobTransientsConfig CreateEntry(uint key)
    {
        return new JobTransientsConfig(JobTransientsManager.GetOriginal(key), false);
    }
}

public class JobTransientsReplace(
    string name,
    string abbreviation)
{

    public string Name = name;
    public string Abbreviation = abbreviation;

    public bool CompareValues(JobTransientsReplace second)
    {
        if(this.Name == second.Name && this.Abbreviation == second.Abbreviation) return true;
        return false;
    }

    // this is where we would have raw memory writing
}