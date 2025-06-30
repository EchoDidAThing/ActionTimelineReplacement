using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Lumina.Excel.Sheets;

namespace ActionTimelineReplacement.Configurations;

public static class StatusReplacementsManager
{
    private static Dictionary<uint, string>? _statusNames;

    private static readonly Dictionary<uint, StatusReplacement> OldValue = [];

    public static Dictionary<uint, string> StatusNames => _statusNames
        ??= Service.DataManager.GetExcelSheet<Status>()
            .Where(i => !string.IsNullOrEmpty(i.Name.ToString()))
            .ToDictionary(i => i.RowId, i => i.Name.ToString());

    public static IEnumerable<uint> AllStatusIds => Service.Config.ReplacementSets.SelectMany(i => i.StatusReplacements.Keys);

    public static string GetName(uint id)
    {
        return StatusNames.GetValueOrDefault(id, "Unknown");
    }

    public static StatusReplacement GetReplacement(uint statusId)
        => GetConfigReplacement(statusId) ?? GetOriginalReplacement(statusId);

    private static StatusReplacement? GetConfigReplacement(uint statusId)
    {
        if (!Service.Config.EnableReplacement) return null;

        foreach (var item in Service.Config.ReplacementSets)
        {
            foreach (var replacement in item.StatusReplacements
                         .Where(replacement => item.Enabled)
                         .OrderByDescending(replacement => item.Priority))
            {
                return replacement.Value.Replacement;
            }
        }
        return null;
    }

    public static StatusReplacement GetOriginalReplacement(uint statusId)
    {
        ref var replacement = ref CollectionsMarshal.GetValueRefOrAddDefault(OldValue, statusId, out var exists);
        if (!exists)
        {
            var act = Service.DataManager.GetExcelSheet<Status>()?.GetRow(statusId);
            replacement = new StatusReplacement(
                act?.ParamModifier ?? 0,
                (ushort)(act?.VFX.RowId ?? 0),
                act?.Unknown0 ?? 0,
                act?.StatusCategory ?? 0,
                (byte)(act?.HitEffect.RowId ?? 0),
                act?.TargetType ?? 0,
                act?.Flags ?? 0,
                act?.Flag2 ?? 0,
                act?.Unknown_70_1 ?? 0,
                act?.Unknown2 ?? 0
                );
        }

        return replacement!;
    }
}