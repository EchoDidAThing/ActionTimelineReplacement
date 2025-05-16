using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Lumina.Excel.Sheets;

namespace ActionTimelineReplacement.Configurations;

public static class MountReplacementsManager
{
    private static Dictionary<uint, string>? _mountNames;

    private static readonly Dictionary<uint, MountReplacement> OldValue = [];

    public static Dictionary<uint, string> MountNames => _mountNames
        ??= Service.DataManager.GetExcelSheet<Mount>()
            .Where(i => !string.IsNullOrEmpty(i.Singular.ToString()))
            .ToDictionary(i => i.RowId, i => i.Singular.ToString());

    public static IEnumerable<uint> AllMountIds =>
       Service.Config.ReplacementSets.SelectMany(i => i.Value.MountReplacements.Keys);

    public static string GetName(uint id)
    {
        return MountNames.GetValueOrDefault(id, "Unknown");
    }

    public static MountReplacement GetReplacement(uint mountId)
        => GetConfigReplacement(mountId) ?? GetOriginalReplacement(mountId);

    private static MountReplacement? GetConfigReplacement(uint mountId)
    {
        if (!Service.Config.EnableReplacement) return null;

        foreach (var item in Service.Config.ReplacementSets)
        {
            foreach (var replacement in item.Value.MountReplacements
                         .Where(replacement => replacement.Value.Enabled)
                         .OrderByDescending(replacement => replacement.Key))
            {
                return replacement.Value.Replacement;
            }
        }

        return null;
    }

    public static MountReplacement GetOriginalReplacement(uint mountId)
    {
        ref var replacement = ref CollectionsMarshal.GetValueRefOrAddDefault(OldValue, mountId, out var exists);
        if (!exists)
        {
            var act = Service.DataManager.GetExcelSheet<Mount>()?.GetRow(mountId);
            replacement = new Configurations.MountReplacement(
                (ushort)(act?.RideBGM.RowId ?? 0),
                (ushort)(act?.Unknown_70_1 ?? 0),
                (ushort)(act?.Unknown_70_2 ?? 0),
                (ushort)(act?.Unknown16 ?? 0),
                (ushort)(act?.Unknown17 ?? 0),
                (ushort)(act?.MountCustomize.RowId ?? 0));
        }

        return replacement!;
    }
}