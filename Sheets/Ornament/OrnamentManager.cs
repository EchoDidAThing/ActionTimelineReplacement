using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Lumina.Excel.Sheets;

namespace ActionTimelineReplacement.Sheets;

public static class OrnamentManager
{
    private static Dictionary<uint, string>? _Names;

    private static readonly Dictionary<uint, OrnamentReplace> old = [];

    public static Dictionary<uint, string> Names => _Names
        ??= Service.DataManager.GetExcelSheet<Ornament>()
            .Where(i => !string.IsNullOrEmpty(i.Singular.ToString()))
            .ToDictionary(i => i.RowId, i => i.Singular.ToString());

    public static IEnumerable<uint> AllOrnamentIds =>
       Service.Config.ReplacementSets.SelectMany(i => i.OrnamentWriter.Keys);

    public static string GetName(uint id)
    {
        return Names.GetValueOrDefault(id, "Unknown");
    }

    public static OrnamentReplace GetReplacement(uint idx)
        => GetConfig(idx) ?? GetOriginal(idx);

    private static OrnamentReplace? GetConfig(uint idx)
    {
        if (!Service.Config.EnableReplacement) return null;

        List<KeyValuePair<int, OrnamentReplace>> replacements = [];

        foreach (var item in Service.Config.ReplacementSets)
        {
            if (item.CharacterName != Service.PlayerState.CharacterName) continue;
            if (item.HomeWorld != Service.PlayerState.HomeWorld.RowId) continue;
            if (!item.Jobs.CheckJob(Service.PlayerState.ClassJob.Value.Abbreviation.ToString())) continue;
            if (!item.Enabled) continue;
            foreach (var replacement in item.OrnamentWriter)
            {
                if (replacement.Key != idx) continue;
                if (!replacement.Value.Enabled) continue;
                replacements.Add(new KeyValuePair<int, OrnamentReplace>(item.Priority, replacement.Value.Replacement));

            }
        }
        if (replacements.Count == 0) { return null; }
        foreach (var replacement in replacements
                         .OrderByDescending(r => r.Key))
        {
            return replacement.Value;
        }
        return null;
    }

    public static OrnamentReplace GetOriginal(uint idx)
    {
        ref var replacement = ref CollectionsMarshal.GetValueRefOrAddDefault(old, idx, out var exists);
        if (!exists)
        {
            var act = Service.DataManager.GetExcelSheet<Ornament>()?.GetRow(idx);
            replacement = new OrnamentReplace(
                act?.Unknown0 ?? 0,
                act?.Model ?? 0,
                (ushort)(act?.Action.RowId ?? 0),
                act?.Transient ?? 0,
                act?.AttachmentPoint ?? 0,
                act?.Unknown3 ?? 0,
                act?.Unknown4 ?? 0);
        }
        return replacement!;
    }
}