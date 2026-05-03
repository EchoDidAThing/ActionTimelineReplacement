using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Lumina.Excel.Sheets;

namespace ActionTimelineReplacement.Sheets;

public static class GlassesStyleManager
{
    private static Dictionary<uint, string>? _Names;

    private static readonly Dictionary<uint, GlassesStyleReplace> old = [];

    public static Dictionary<uint, string> Names => _Names
        ??= Service.DataManager.GetExcelSheet<GlassesStyle>()
            .Where(i => !string.IsNullOrEmpty(i.Name.ToString()))
            .ToDictionary(i => i.RowId, i => i.Name.ToString());

    public static IEnumerable<uint> AllGlassesStyleIds =>
       Service.Config.ReplacementSets.SelectMany(i => i.GlassesStyleWriter.Keys);

    public static string GetName(uint id)
    {
        return Names.GetValueOrDefault(id, "Unknown");
    }

    public static GlassesStyleReplace GetReplacement(uint idx)
        => GetConfig(idx) ?? GetOriginal(idx);

    private static GlassesStyleReplace? GetConfig(uint idx)
    {
        if (!Service.Config.EnableReplacement) return null;

        List<KeyValuePair<int, GlassesStyleReplace>> replacements = [];

        foreach (var item in Service.Config.ReplacementSets)
        {
            foreach (var replacement in item.GlassesStyleWriter
                         .Where(r => item.Enabled))
            {
                replacements.Add(new KeyValuePair<int, GlassesStyleReplace>(item.Priority, replacement.Value.Replacement));
            }
        }
        foreach (var replacement in replacements
                         .OrderByDescending(r => r.Key))
        {
            return replacement.Value;
        }
        return null;
    }

    public static GlassesStyleReplace GetOriginal(uint idx)
    {
        ref var replacement = ref CollectionsMarshal.GetValueRefOrAddDefault(old, idx, out var exists);
        if (!exists)
        {
            var act = Service.DataManager.GetExcelSheet<GlassesStyle>()?.GetRow(idx);
            replacement = new GlassesStyleReplace(
                act?.Adjective ?? 0,
                act?.PossessivePronoun?? 0,
                act?.StartsWithVowel ?? 0,
                act?.Unknown_70_4 ?? 0,
                act?.Pronoun ?? 0,
                act?.Article ?? 0,
                act?.GlassesSlot ?? 0);
        }
        return replacement!;
    }
}