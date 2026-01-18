using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Lumina.Excel.Sheets;

namespace ActionTimelineReplacement.Sheets;

public static class GlassesManager
{
    private static Dictionary<uint, string>? _Names;

    private static readonly Dictionary<uint, GlassesReplace> old = [];

    public static Dictionary<uint, string> Names => _Names
        ??= Service.DataManager.GetExcelSheet<Glasses>()
            .Where(i => !string.IsNullOrEmpty(i.Name.ToString()))
            .ToDictionary(i => i.RowId, i => i.Name.ToString());

    public static IEnumerable<uint> AllGlassesIds =>
       Service.Config.ReplacementSets.SelectMany(i => i.GlassesWriter.Keys);

    public static string GetName(uint id)
    {
        return Names.GetValueOrDefault(id, "Unknown");
    }

    public static GlassesReplace GetReplacement(uint idx)
        => GetConfig(idx) ?? GetOriginal(idx);

    private static GlassesReplace? GetConfig(uint idx)
    {
        if (!Service.Config.EnableReplacement) return null;

        foreach (var item in Service.Config.ReplacementSets)
        {
            foreach (var replacement in item.GlassesWriter
                         .Where(r => item.Enabled)
                         .OrderByDescending(r => r.Key))
            {
                return replacement.Value.Replacement;
            }
        }
        return null;
    }

    public static GlassesReplace GetOriginal(uint idx)
    {
        ref var replacement = ref CollectionsMarshal.GetValueRefOrAddDefault(old, idx, out var exists);
        if (!exists)
        {
            var act = Service.DataManager.GetExcelSheet<Glasses>()?.GetRow(idx);
            replacement = new GlassesReplace(
                act?.Unknown_70_1 ?? 0,
                act?.Unknown_70_2 ?? 0,
                act?.Unknown_70_3 ?? 0,
                act?.Unknown_70_4 ?? 0,
                act?.Unknown_70_5 ?? 0,
                act?.Unknown_70_6 ?? 0,
                act?.Unknown_70_7 ?? 0,
                act?.Unknown_70_8 ?? 0
                );
        }
        return replacement!;
    }
}