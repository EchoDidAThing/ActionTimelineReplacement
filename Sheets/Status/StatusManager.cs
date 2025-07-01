using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Lumina.Excel.Sheets;

namespace ActionTimelineReplacement.Sheets;

public static class StatusManager
{
    private static Dictionary<uint, string>? _Names;

    private static readonly Dictionary<uint, StatusReplace> old = [];

    public static Dictionary<uint, string> Names => _Names
        ??= Service.DataManager.GetExcelSheet<Status>()
            .Where(i => !string.IsNullOrEmpty(i.Name.ToString()))
            .ToDictionary(i => i.RowId, i => i.Name.ToString());

    public static IEnumerable<uint> AllStatusIds => Service.Config.ReplacementSets.SelectMany(i => i.StatusWriter.Keys);

    public static string GetName(uint id)
    {
        return Names.GetValueOrDefault(id, "Unknown");
    }

    public static StatusReplace GetReplacement(uint idx)
        => GetConfig(idx) ?? GetOriginal(idx);

    private static StatusReplace? GetConfig(uint idx)
    {
        if (!Service.Config.EnableReplacement) return null;

        foreach (var item in Service.Config.ReplacementSets)
        {
            foreach (var replacement in item.StatusWriter
                         .Where(r => item.Enabled)
                         .OrderByDescending(r => item.Priority))
            {
                return replacement.Value.Replacement;
            }
        }
        return null;
    }

    public static StatusReplace GetOriginal(uint idx)
    {
        ref var replacement = ref CollectionsMarshal.GetValueRefOrAddDefault(old, idx, out var exists);
        if (!exists)
        {
            var act = Service.DataManager.GetExcelSheet<Status>()?.GetRow(idx);
            replacement = new StatusReplace(
                act?.ParamModifier ?? 0,
                (ushort)(act?.VFX.RowId ?? 0),
                act?.Unknown0 ?? 0,
                act?.StatusCategory ?? 0,
                (byte)(act?.HitEffect.RowId ?? 0),
                act?.ParamEffect ?? 0,
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