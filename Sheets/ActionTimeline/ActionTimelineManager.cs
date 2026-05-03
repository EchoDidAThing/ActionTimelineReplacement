using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Lumina.Excel;
using Lumina.Excel.Sheets;

namespace ActionTimelineReplacement.Sheets;

public static class ActionTimelineManager
{
    private static Dictionary<uint, string>? _Names;

    private static readonly Dictionary<uint, ActionTimelineReplace> old = [];

    public static Dictionary<uint, string> Names => _Names
        ??= Service.DataManager.GetExcelSheet<ActionTimeline>()
            .Where(i => !string.IsNullOrEmpty(i.Key.ToString()))
            .ToDictionary(i => i.RowId, i => i.Key.ToString());

    public static IEnumerable<uint> AllActionTimelineIds => Service.Config.ReplacementSets.SelectMany(i => i.ActionTimelineWriter.Keys);

    public static string GetName(uint id)
    {
        return Names.GetValueOrDefault(id, "Unknown");
    }

    public static ActionTimelineReplace GetReplacement(uint idx)
        => GetConfig(idx) ?? GetOriginal(idx);

    private static ActionTimelineReplace? GetConfig(uint idx)
    {
        if (!Service.Config.EnableReplacement) return null;

        List<KeyValuePair<int, ActionTimelineReplace>> replacements = [];

        foreach (var item in Service.Config.ReplacementSets)
        {
            foreach (var replacement in item.ActionTimelineWriter
                         .Where(r => item.Enabled))
            {
                replacements.Add(new KeyValuePair<int, ActionTimelineReplace>(item.Priority, replacement.Value.Replacement));
            }
        }
        foreach (var replacement in replacements
                         .OrderByDescending(r => r.Key))
        {
            return replacement.Value;
        }
        return null;
    }

    public static ActionTimelineReplace GetOriginal(uint idx)
    {
        ref var replacement = ref CollectionsMarshal.GetValueRefOrAddDefault(old, idx, out var exists);
        if (!exists)
        {
            var act = Service.DataManager.GetExcelSheet<ActionTimeline>()?.GetRow(idx);
            replacement = new ActionTimelineReplace(
                act?.Type ?? 0,
                act?.Priority ?? 0,
                act?.Stance ?? 0,
                act?.Slot ?? 0,
                act?.LookAtMode ?? 0,
                act?.ActionTimelineIDMode ?? 0,
                act?.LoadType ?? 0,
                act?.StartAttach ?? 0,
                act?.ResidentPap ?? 0,
                act?.Unknown6 ?? 0,
                act?.Unknown1 ?? 0,
                act?.Unknown1 ?? 0 //dummy value for blade state
                );
        }
        return replacement!;
    }
}