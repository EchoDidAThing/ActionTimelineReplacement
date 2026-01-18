/*using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Lumina.Excel.Sheets;

namespace ActionTimelineReplacement.Sheets;

public static class PointMenuChoiceManager
{
    private static Dictionary<float, string>? _Names;

    private static readonly Dictionary<float, PointMenuChoiceReplace> old = [];

    public static Dictionary<float, string> Names => _Names
        ??= Service.DataManager.GetSubrowExcelSheet<PointMenuChoice>()
            .Where(i => !string.IsNullOrEmpty(i.RowId.ToString()))
            .ToDictionary(i => i.RowId, i => i.RowId.ToString());
    //really doesn't like this, even though the row is decimal
    //have to do decimal, since the game crashes if trying to access int

    public static IEnumerable<float> AllPointMenuChoiceIds => Service.Config.ReplacementSets.SelectMany(i => i.PointMenuChoiceWriter.Keys);

    public static string GetName(float id)
    {
        return Names.GetValueOrDefault(id, "Unknown");
    }

    public static PointMenuChoiceReplace GetReplacement(float idx)
        => GetConfig(idx) ?? GetOriginal(idx);

    private static PointMenuChoiceReplace? GetConfig(float idx)
    {
        if (!Service.Config.EnableReplacement) return null;

        foreach (var item in Service.Config.ReplacementSets)
        {
            foreach (var replacement in item.PointMenuChoiceWriter
                         .Where(r => item.Enabled)
                         .OrderByDescending(r => item.Priority))
            {
                return replacement.Value.Replacement;
            }
        }
        return null;
    }

    public static PointMenuChoiceReplace GetOriginal(float idx)
    {
        ref var replacement = ref CollectionsMarshal.GetValueRefOrAddDefault(old, idx, out var exists);
        if (!exists)
        {
            var act = Service.DataManager.GetSubrowExcelSheet<PointMenuChoice>()?.GetRow(idx);
            replacement = new PointMenuChoiceReplace(
                act?.Scale ?? 0f,
                act?.Scale ?? 0f,
                act?.Scale ?? 0f,
                act?.Scale ?? 0f,
                act?.Scale ?? 0f,
                act?.Scale ?? 0f,
                act?.Special ?? 0,
                act?.Rank ?? 0,
                act?.Rank ?? 0,
                act?.Rank ?? 0,
                act?.Rank ?? 0,
                act?.Rank ?? 0,
                act?.Rank ?? 0
                );
        }
        return replacement!;
    }
}
*/