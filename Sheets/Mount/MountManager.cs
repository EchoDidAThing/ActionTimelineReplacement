using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Lumina.Excel.Sheets;
#pragma warning disable CA1416 // Validate platform compatibility

namespace ActionTimelineReplacement.Sheets;

public static class MountManager
{
    private static Dictionary<uint, string>? _Names;

    private static readonly Dictionary<uint, MountReplace> old = [];

    public static Dictionary<uint, string> Names => _Names
        ??= Service.DataManager.GetExcelSheet<Mount>()
            .Where(i => !string.IsNullOrEmpty(i.Singular.ToString()))
            .ToDictionary(i => i.RowId, i => i.Singular.ToString());

    public static IEnumerable<uint> AllMountIds =>
       Service.Config.ReplacementSets.SelectMany(i => i.MountWriter.Keys);

    public static string GetName(uint id)
    {
        return Names.GetValueOrDefault(id, "Unknown");
    }

    public static MountReplace GetReplacement(uint idx)
        => GetConfig(idx) ?? GetOriginal(idx);

    private static MountReplace? GetConfig(uint idx)
    {
        if (!Service.Config.EnableReplacement) return null;

        List<KeyValuePair<int, MountReplace>> replacements = [];

        foreach (var item in Service.Config.ReplacementSets)
        {
            if (item.CharacterName != Service.PlayerState.CharacterName) continue;
            if (item.HomeWorld != Service.PlayerState.HomeWorld.RowId) continue;
            if (!item.Jobs.CheckJob(Service.PlayerState.ClassJob.Value.Abbreviation.ToString())) continue;
            if (!item.Enabled) continue;
            foreach (var replacement in item.MountWriter)
            {
                if (replacement.Key != idx) continue;
                if (!replacement.Value.Enabled) continue;
                replacements.Add(new KeyValuePair<int, MountReplace>(item.Priority, replacement.Value.Replacement));

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

    public static MountReplace GetOriginal(uint idx)
    {
        ref var replacement = ref CollectionsMarshal.GetValueRefOrAddDefault(old, idx, out var exists);
        if (!exists)
        {
            var act = Service.DataManager.GetExcelSheet<Mount>()?.GetRow(idx);
            replacement = new MountReplace(
                (ushort)(act?.RideBGM.RowId ?? 0),
                act?.Unknown_70_1 ?? 0,
                act?.Unknown_70_2 ?? 0,
                act?.Unknown16 ?? 0,
                act?.Unknown17 ?? 0,
                act?.Unknown5 ?? 0,
                act?.Unknown6 ?? 0,
                act?.Unknown7 ?? 0,
                act?.Unknown8 ?? 0,
                (byte)(act?.MountCustomize.RowId ?? 0),
                act?.Unknown10 ?? 0,
                act?.HideHeadgear ?? false,
                act?.Unknown18 ?? false,
                act?.Unknown19 ?? false);
        }
        return replacement!;
    }
}