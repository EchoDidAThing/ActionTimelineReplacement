using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Lumina.Excel.Sheets;

namespace ActionTimelineReplacement.Sheets;

public static class OrnamentCustomizeGroupManager
{
    /*
    private static Dictionary<uint, string>? _Names;

    private static readonly Dictionary<uint, OrnamentCustomizeGroupReplace> old = [];

    public static Dictionary<uint, string> Names => _Names
        ??= Service.DataManager.GetExcelSheet<OrnamentCustomizeGroup>()
            .Where(i => !string.IsNullOrEmpty(i.RowId.ToString()))
            .ToDictionary(i => i.RowId, i => i.RowId.ToString());

    public static IEnumerable<uint> AllOrnamentCustomizeGroupIds =>
       Service.Config.ReplacementSets.SelectMany(i => i.OrnamentCustomizeGroupWriter.Keys);

    public static string GetName(uint id)
    {
        return Names.GetValueOrDefault(id, "Unknown");
    }

    public static OrnamentCustomizeGroupReplace GetReplacement(uint idx)
        => GetConfig(idx) ?? GetOriginal(idx);

    private static OrnamentCustomizeGroupReplace? GetConfig(uint idx)
    {
        if (!Service.Config.EnableReplacement) return null;

        foreach (var item in Service.Config.ReplacementSets)
        {
            foreach (var replacement in item.OrnamentCustomizeGroupWriter
                         .Where(r => item.Enabled)
                         .OrderByDescending(r => r.Key))
            {
                return replacement.Value.Replacement;
            }
        }
        return null;
    }

    public static OrnamentCustomizeGroupReplace GetOriginal(uint idx)
    {
        ref var replacement = ref CollectionsMarshal.GetValueRefOrAddDefault(old, idx, out var exists);
        if (!exists)
        {
            var act = Service.DataManager.GetExcelSheet<OrnamentCustomizeGroup>()?.GetRow(idx);
            replacement = new OrnamentCustomizeGroupReplace(
                act?.Unknown0 ?? 0,
                act?.Unknown1 ?? 0,
                act?.Unknown2 ?? 0,
                act?.Unknown3 ?? 0,
                act?.Unknown4 ?? 0,
                act?.Unknown5 ?? 0,
                act?.Unknown6 ?? 0);
        }
        return replacement!;
    }
    */
}