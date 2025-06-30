using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Lumina.Excel.Sheets;

namespace ActionTimelineReplacement.Configurations;

public static class PlaceNameReplacementsManager
{
    private static Dictionary<uint, string>? _placeNameNames;

    private static readonly Dictionary<uint, PlaceNameReplacement> OldValue = [];

    public static Dictionary<uint, string> PlaceNameNames => _placeNameNames
        ??= Service.DataManager.GetExcelSheet<PlaceName>()
            .Where(i => !string.IsNullOrEmpty(i.Name.ToString()))
            .ToDictionary(i => i.RowId, i => i.Name.ToString());

    public static IEnumerable<uint> AllPlaceNameIds =>
       Service.Config.ReplacementSets.SelectMany(i => i.PlaceNameReplacements.Keys);

    public static string GetName(uint id)
    {
        return PlaceNameNames.GetValueOrDefault(id, "Unknown");
    }

    public static PlaceNameReplacement GetReplacement(uint placeNameId)
        => GetConfigReplacement(placeNameId) ?? GetOriginalReplacement(placeNameId);

    private static PlaceNameReplacement? GetConfigReplacement(uint placeNameId)
    {
        if (!Service.Config.EnableReplacement) return null;

        foreach (var item in Service.Config.ReplacementSets)
        {
            foreach (var replacement in item.PlaceNameReplacements
                         .Where(replacement => replacement.Value.Enabled)
                         .OrderByDescending(replacement => replacement.Key))
            {
                return replacement.Value.Replacement;
            }
        }

        return null;
    }

    public static PlaceNameReplacement GetOriginalReplacement(uint placeNameId)
    {
        ref var replacement = ref CollectionsMarshal.GetValueRefOrAddDefault(OldValue, placeNameId, out var exists);
        if (!exists)
        {
            var act = Service.DataManager.GetExcelSheet<PlaceName>()?.GetRow(placeNameId);
            replacement = new PlaceNameReplacement(
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
}
