using Dalamud.Interface.Windowing;
using ActionTimelineReplacement.Base.Global;
using System;

namespace ActionTimelineReplacement.Windows;

public sealed partial class ConfigWindow : Window
{
    public static int ScoreString(string s1, string search)
    {
        if (s1.Contains(search, StringComparison.OrdinalIgnoreCase))
        {
            return s1.Length - search.Length;
        }
        return CalcGlobals.LevenshteinDistance(s1, search) + 20;
    }
}