using ActionTimelineReplacement.Base;
using Lumina.Excel.Sheets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using static Dalamud.Interface.Utility.Raii.ImRaii;

namespace ActionTimelineReplacement.Sheets;

public static class VfxManager
{
    private static Dictionary<uint, string>? _Names;

    private static readonly Dictionary<uint, VfxReplace> old = [];

    public static Dictionary<uint, string> Names => _Names
        ??= Service.DataManager.GetExcelSheet<VFX>()
            .Where(i => !string.IsNullOrEmpty(i.Location.ToString()))
            .ToDictionary(i => i.RowId, i => i.Location.ToString());

    public static IEnumerable<uint> AllVfxIds => Service.Config.ReplacementSets.SelectMany(i => i.VfxWriter.Keys);

    public static string GetName(uint id)
    {
        return Names.GetValueOrDefault(id, "Unknown");
    }

    public static VfxReplace GetReplacement(uint idx)
        => GetConfig(idx) ?? GetOriginal(idx);


    public static VfxReplace GetConfig(uint idx)
    {
        if (!Service.Config.EnableReplacement) return null;

        List<KeyValuePair<int, VfxReplace>> replacements = [];

        foreach (var item in Service.Config.ReplacementSets)
        {
            if (item.CharacterName != Service.PlayerState.CharacterName) continue;
            if (item.HomeWorld != Service.PlayerState.HomeWorld.RowId) continue;
            if (!item.Jobs.CheckJob(Service.PlayerState.ClassJob.Value.Abbreviation.ToString())) continue;
            if (!item.Enabled) continue;
            foreach (var replacement in item.VfxWriter)
            {
                if (replacement.Key != idx) continue;
                if (!replacement.Value.Enabled) continue;
                replacements.Add(new KeyValuePair<int, VfxReplace>(item.Priority, replacement.Value.Replacement));

            }
        }
        foreach (var replacement in replacements
                         .OrderByDescending(r => r.Key))
        {
            return replacement.Value;
        }
        return null;
    }

    public static VfxReplace GetOriginal(uint idx)
    {
        ref var replacement = ref CollectionsMarshal.GetValueRefOrAddDefault(old, idx, out var exists);
        if (!exists)
        {
            var act = Service.DataManager.GetExcelSheet<VFX>()?.GetRow(idx);
            replacement = new VfxReplace(
                 act?.RowId ?? 0,
                 act?.Location.ToString() ?? ""
                );
        }
        return replacement!;
    }
}