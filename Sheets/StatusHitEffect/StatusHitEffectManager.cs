using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Lumina.Excel.Sheets;

namespace ActionTimelineReplacement.Sheets;

public static class StatusHitEffectManager
{
    private static Dictionary<uint, string>? _Names;

    public static readonly Dictionary<uint, StatusHitEffectReplace> old = [];

    public static Dictionary<uint, string> Names => _Names
        ??= Service.DataManager.GetExcelSheet<StatusHitEffect>()
            .Where(i => !string.IsNullOrEmpty(i.RowId.ToString()))
            .ToDictionary(i => i.RowId, i => i.RowId.ToString());

    public static IEnumerable<uint> AllStatusHitEffectIds => Service.Config.ReplacementSets.SelectMany(i => i.StatusHitEffectWriter.Keys);

    public static string GetName(uint id)
    {
        return Names.GetValueOrDefault(id, "Unknown");
    }

    public static StatusHitEffectReplace GetReplacement(uint idx)
        => GetConfig(idx) ?? GetOriginal(idx);

    private static StatusHitEffectReplace? GetConfig(uint idx)
    {
        if (!Service.Config.EnableReplacement) return null;

        List<KeyValuePair<int, StatusHitEffectReplace>> replacements = [];

        foreach (var item in Service.Config.ReplacementSets)
        {
            foreach (var replacement in item.StatusHitEffectWriter
                         .Where(r => item.Enabled))
            {
                replacements.Add(new KeyValuePair<int, StatusHitEffectReplace>(item.Priority, replacement.Value.Replacement));
            }
        }
        foreach (var replacement in replacements
                         .OrderByDescending(r => r.Key))
        {
            return replacement.Value;
        }
        return null;
    }

    public static StatusHitEffectReplace GetOriginal(uint idx)
    {
        ref var replacement = ref CollectionsMarshal.GetValueRefOrAddDefault(old, idx, out var exists);
        if (!exists)
        {
            var act = Service.DataManager.GetExcelSheet<StatusHitEffect>()?.GetRow(idx);
            replacement = new StatusHitEffectReplace(
                (ushort)(act?.Location.RowId ?? 0)
                );
        }
        return replacement!;
    }
}