using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Lumina.Excel.Sheets;

namespace ActionTimelineReplacement.Sheets;
public static class ActionCastTimelineManager
{
    private static Dictionary<uint, string>? _Names;

    private static readonly Dictionary<uint, ActionCastTimelineReplace> old = [];

    public static Dictionary<uint, string> Names => _Names
        ??= Service.DataManager.GetExcelSheet<Lumina.Excel.Sheets.ActionCastTimeline>()
            .Where(i => !string.IsNullOrEmpty(i.RowId.ToString()))
            .ToDictionary(i => i.RowId, i => i.RowId.ToString());


    public static IEnumerable<uint> AllActionCastTimelineIds =>
        Service.Config.ReplacementSets.SelectMany(i => i.ActionCastTimelineWriter.Keys);

    public static string GetName(uint id)
    {
        return Names.GetValueOrDefault(id, "Unknown");
    }

    public static ActionCastTimelineReplace GetReplacement(uint idx)
        => GetConfig(idx) ?? GetOriginal(idx);

    private static ActionCastTimelineReplace? GetConfig(uint idx)
    {
        if (!Service.Config.EnableReplacement) return null;

        List<KeyValuePair<int, ActionCastTimelineReplace>> replacements = [];

        foreach (var item in Service.Config.ReplacementSets)
        {
            foreach (var replacement in item.ActionCastTimelineWriter
                         .Where(r => item.Enabled))
            {
                replacements.Add(new KeyValuePair<int, ActionCastTimelineReplace>(item.Priority, replacement.Value.Replacement));
            }
        }
        foreach (var replacement in replacements
                         .OrderByDescending(r => r.Key))
        {
            return replacement.Value;
        }
        return null;
    }

    public static ActionCastTimelineReplace GetOriginal(uint idx)
    {
        ref var replacement = ref CollectionsMarshal.GetValueRefOrAddDefault(old, idx, out var exists);
        if (!exists)
        {
            var act = Service.DataManager.GetExcelSheet<Lumina.Excel.Sheets.ActionCastTimeline>()?.GetRow(idx);
            replacement = new ActionCastTimelineReplace(
                (ushort)(act?.Name.RowId ?? 0),
                (ushort)(act?.VFX.RowId ?? 0));
        }
        return replacement!;
    }
}
