using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Lumina.Excel.Sheets;

namespace ActionTimelineReplacement.Sheets;

public static class TiltParamManager
{
    private static Dictionary<uint, string>? _Names;

    private static readonly Dictionary<uint, TiltParamReplace> old = [];

    public static Dictionary<uint, string> Names => _Names
        ??= Service.DataManager.GetExcelSheet<TiltParam>()
            .Where(i => !string.IsNullOrEmpty(i.RowId.ToString()))
            .ToDictionary(i => i.RowId, i => i.RowId.ToString());

    public static IEnumerable<uint> AllTiltParamIds => Service.Config.ReplacementSets.SelectMany(i => i.TiltParamWriter.Keys);

    public static string GetName(uint id)
    {
        return Names.GetValueOrDefault(id, "Unknown");
    }

    public static TiltParamReplace GetReplacement(uint idx)
        => GetConfig(idx) ?? GetOriginal(idx);

    private static TiltParamReplace? GetConfig(uint idx)
    {
        if (!Service.Config.EnableReplacement) return null;

        List<KeyValuePair<int, TiltParamReplace>> replacements = [];

        foreach (var item in Service.Config.ReplacementSets)
        {
            if (item.CharacterName != Service.PlayerState.CharacterName) continue;
            if (item.HomeWorld != Service.PlayerState.HomeWorld.RowId) continue;
            if (!item.Jobs.CheckJob(Service.PlayerState.ClassJob.Value.Abbreviation.ToString())) continue;
            if (!item.Enabled) continue;
            foreach (var replacement in item.TiltParamWriter)
            {
                if (replacement.Key != idx) continue;
                if (!replacement.Value.Enabled) continue;
                replacements.Add(new KeyValuePair<int, TiltParamReplace>(item.Priority, replacement.Value.Replacement));

            }
        }
        foreach (var replacement in replacements
                         .OrderByDescending(r => r.Key))
        {
            return replacement.Value;
        }
        return null;
    }

    public static TiltParamReplace GetOriginal(uint idx)
    {
        ref var replacement = ref CollectionsMarshal.GetValueRefOrAddDefault(old, idx, out var exists);
        if (!exists)
        {
            var act = Service.DataManager.GetExcelSheet<TiltParam>()?.GetRow(idx);
            replacement = new TiltParamReplace(
                (ushort)(act?.Unknown0 ?? 0),
                act?.Unknown1 ?? 0,
                act?.Unknown2 ?? 0,
                act?.Unknown3 ?? 0,
                act?.Unknown4 ?? 0,
                act?.Unknown5 ?? false
                );
        }
        return replacement!;
    }
}