using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Lumina.Excel.Sheets;

namespace ActionTimelineReplacement.Sheets;
public static class ActionCastVFXReplacementsManager
{
    private static Dictionary<uint, string>? _Names;

    private static readonly Dictionary<uint, ActionCastVFXReplace> old = [];

    public static Dictionary<uint, string> Names => _Names
        ??= Service.DataManager.GetExcelSheet<ActionCastVFX>()
            .Where(i => !string.IsNullOrEmpty(i.RowId.ToString()))
            .ToDictionary(i => i.RowId, i => i.RowId.ToString());


    public static IEnumerable<uint> AllCastVFXActionIds =>
        Service.Config.ReplacementSets.SelectMany(i => i.ActionCastVFXWriter.Keys);

    public static string GetName(uint id)
    {
        return Names.GetValueOrDefault(id, "Unknown");
    }

    public static ActionCastVFXReplace GetReplacement(uint idx)
        => GetConfig(idx) ?? GetOriginal(idx);

    private static ActionCastVFXReplace? GetConfig(uint idx)
    {
        if (!Service.Config.EnableReplacement) return null;

        List<KeyValuePair<int, ActionCastVFXReplace>> replacements = [];

        foreach (var item in Service.Config.ReplacementSets)
        {
            foreach (var replacement in item.ActionCastVFXWriter
                         .Where(r => item.Enabled))
            {
                replacements.Add(new KeyValuePair<int, ActionCastVFXReplace>(item.Priority, replacement.Value.Replacement));
            }
        }
        foreach (var replacement in replacements
                         .OrderByDescending(r => r.Key))
        {
            return replacement.Value;
        }
        return null;
    }

    public static ActionCastVFXReplace GetOriginal(uint idx)
    {
        ref var replacement = ref CollectionsMarshal.GetValueRefOrAddDefault(old, idx, out var exists);
        if (!exists)
        {
            var act = Service.DataManager.GetExcelSheet<ActionCastVFX>()?.GetRow(idx);
            replacement = new ActionCastVFXReplace(
                (ushort)(act?.VFX.RowId ?? 0));
        }
        return replacement!;
    }
}
