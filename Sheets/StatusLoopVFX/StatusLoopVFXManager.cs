using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Lumina.Excel.Sheets;

namespace ActionTimelineReplacement.Sheets;

public static class StatusLoopVFXManager
{
    private static Dictionary<uint, string>? _Names;

    private static readonly Dictionary<uint, StatusLoopVFXReplace> old = [];

    public static Dictionary<uint, string> Names => _Names
        ??= Service.DataManager.GetExcelSheet<StatusLoopVFX>()
            .Where(i => !string.IsNullOrEmpty(i.RowId.ToString()))
            .ToDictionary(i => i.RowId, i => i.RowId.ToString());

    public static IEnumerable<uint> AllStatusLoopVFXIds => Service.Config.ReplacementSets.SelectMany(i => i.StatusLoopVFXWriter.Keys);

    public static string GetName(uint id)
    {
        return Names.GetValueOrDefault(id, "Unknown");
    }

    public static StatusLoopVFXReplace GetReplacement(uint idx)
        => GetConfig(idx) ?? GetOriginal(idx);

    private static StatusLoopVFXReplace? GetConfig(uint idx)
    {
        if (!Service.Config.EnableReplacement) return null;

        List<KeyValuePair<int, StatusLoopVFXReplace>> replacements = [];

        foreach (var item in Service.Config.ReplacementSets)
        {
            foreach (var replacement in item.StatusLoopVFXWriter
                         .Where(r => item.Enabled))
            {
                replacements.Add(new KeyValuePair<int, StatusLoopVFXReplace>(item.Priority, replacement.Value.Replacement));
            }
        }
        foreach (var replacement in replacements
                         .OrderByDescending(r => r.Key))
        {
            return replacement.Value;
        }
        return null;
    }

    public static StatusLoopVFXReplace GetOriginal(uint idx)
    {
        ref var replacement = ref CollectionsMarshal.GetValueRefOrAddDefault(old, idx, out var exists);
        if (!exists)
        {
            var act = Service.DataManager.GetExcelSheet<StatusLoopVFX>()?.GetRow(idx);
            replacement = new StatusLoopVFXReplace(
                act?.ExcelPage.ReadUInt16(act?.RowOffset + 0 ?? 0) ?? 0,
                act?.ExcelPage.ReadUInt16(act?.RowOffset + 2 ?? 0) ?? 0,
                act?.ExcelPage.ReadUInt16(act?.RowOffset + 4 ?? 0) ?? 0,
                act?.ExcelPage.ReadUInt16(act?.RowOffset + 6 ?? 0) ?? 0,
                act?.ExcelPage.ReadUInt8(act?.RowOffset + 8 ?? 0) ?? 0,
                act?.ExcelPage.ReadUInt8(act?.RowOffset + 9 ?? 0) ?? 0,
                act?.ExcelPage.ReadUInt8(act?.RowOffset + 10 ?? 0) ?? 0,
                act?.ExcelPage.ReadUInt8(act?.RowOffset + 11 ?? 0) ?? 0,
                act?.ExcelPage.ReadPackedBool(act?.RowOffset + 12 ?? 0, 0) ?? false,
                act?.ExcelPage.ReadPackedBool(act?.RowOffset + 12 ?? 0, 1) ?? false,
                act?.ExcelPage.ReadPackedBool(act?.RowOffset + 12 ?? 0, 2) ?? false
                );
        }
        return replacement!;
    }
}