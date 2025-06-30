using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Lumina.Excel.Sheets;

namespace ActionTimelineReplacement.Configurations;

public static class GlassesReplacementsManager
{
    private static Dictionary<uint, string>? _glassesNames;

    private static readonly Dictionary<uint, GlassesReplacement> OldValue = [];

    public static Dictionary<uint, string> GlassesNames => _glassesNames
        ??= Service.DataManager.GetExcelSheet<Glasses>()
            .Where(i => !string.IsNullOrEmpty(i.Name.ToString()))
            .ToDictionary(i => i.RowId, i => i.Name.ToString());

    public static IEnumerable<uint> AllGlassesIds =>
       Service.Config.ReplacementSets.SelectMany(i => i.GlassesReplacements.Keys);

    public static string GetName(uint id)
    {
        return GlassesNames.GetValueOrDefault(id, "Unknown");
    }

    public static GlassesReplacement GetReplacement(uint glassesId)
        => GetConfigReplacement(glassesId) ?? GetOriginalReplacement(glassesId);

    private static GlassesReplacement? GetConfigReplacement(uint glassesId)
    {
        if (!Service.Config.EnableReplacement) return null;

        foreach (var item in Service.Config.ReplacementSets)
        {
            foreach (var replacement in item.GlassesReplacements
                         .Where(replacement => replacement.Value.Enabled)
                         .OrderByDescending(replacement => replacement.Key))
            {
                return replacement.Value.Replacement;
            }
        }
        return null;
    }

    public static GlassesReplacement GetOriginalReplacement(uint glassesId)
    {
        ref var replacement = ref CollectionsMarshal.GetValueRefOrAddDefault(OldValue, glassesId, out var exists);
        if (!exists)
        {
            var act = Service.DataManager.GetExcelSheet<Glasses>()?.GetRow(glassesId);
            replacement = new GlassesReplacement(
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