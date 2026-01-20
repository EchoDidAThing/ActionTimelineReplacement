/*using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Lumina.Excel.Sheets;

namespace ActionTimelineReplacement.Sheets;

public static class PlaceNameManager
{
    private static Dictionary<uint, string>? _Names;

    private static readonly Dictionary<uint, PlaceNameReplace> old = [];

    public static Dictionary<uint, string> Names => _Names
        ??= Service.DataManager.GetExcelSheet<PlaceName>()
            .Where(i => !string.IsNullOrEmpty(i.Name.ToString()))
            .ToDictionary(i => i.RowId, i => i.Name.ToString());

    public static IEnumerable<uint> AllPlaceNameIds =>
       Service.Config.ReplacementSets.SelectMany(i => i.PlaceNameWriter.Keys);

    public static string GetName(uint id)
    {
        return Names.GetValueOrDefault(id, "Unknown");
    }

    public static PlaceNameReplace GetReplacement(uint idx)
        => GetConfig(idx) ?? GetOriginal(idx);

    private static PlaceNameReplace? GetConfig(uint idx)
    {
        if (!Service.Config.EnableReplacement) return null;

        foreach (var item in Service.Config.ReplacementSets)
        {
            foreach (var replacement in item.PlaceNameWriter
                         .Where(r => item.Enabled)
                         .OrderByDescending(r => r.Key))
            {
                return replacement.Value.Replacement;
            }
        }
        return null;
    }

    public static PlaceNameReplace GetOriginal(uint idx)
    {
        ref var replacement = ref CollectionsMarshal.GetValueRefOrAddDefault(old, idx, out var exists);
        if (!exists)
        {
            var act = Service.DataManager.GetExcelSheet<PlaceName>()?.GetRow(idx);
            replacement = new PlaceNameReplace(
                //act?.Name.ToString() ?? "",
                //act?.NameNoArticle.ToString() ?? "",
                //act?.Unknown0.ToString() ?? "",
                act?.Unknown1 ?? 0,
                act?.Unknown2 ?? 0,
                act?.Unknown3 ?? 0,
                act?.Unknown4 ?? 0,
                act?.Unknown5 ?? 0,
                act?.Unknown6 ?? 0,
                act?.Unknown7 ?? 0,
                act?.Unknown8 ?? 0);
        }
        return replacement!;
    }
}*/
