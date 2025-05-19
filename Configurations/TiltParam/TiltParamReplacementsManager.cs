using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Lumina.Excel.Sheets;

namespace ActionTimelineReplacement.Configurations;

public static class TiltReplacementsManager
{
    private static Dictionary<uint, string>? _tiltNames;

    private static readonly Dictionary<uint, TiltReplacement> OldValue = [];

    public static Dictionary<uint, string> TiltNames => _tiltNames
        ??= Service.DataManager.GetExcelSheet<TiltParam>()
            .Where(i => !string.IsNullOrEmpty(i.RowId.ToString()))
            .ToDictionary(i => i.RowId, i => i.RowId.ToString());

    public static IEnumerable<uint> AllTiltIds =>
       Service.Config.ReplacementSets.SelectMany(i => i.TiltReplacements.Keys);

    public static string GetName(uint id)
    {
        return TiltNames.GetValueOrDefault(id, "Unknown");
    }

    public static TiltReplacement GetReplacement(uint tiltId)
        => GetConfigReplacement(tiltId) ?? GetOriginalReplacement(tiltId);

    private static TiltReplacement? GetConfigReplacement(uint tiltId)
    {
        if (!Service.Config.EnableReplacement) return null;

        foreach (var item in Service.Config.ReplacementSets)
        {
            foreach (var replacement in item.TiltReplacements
                         .Where(replacement => replacement.Value.Enabled)
                         .OrderByDescending(replacement => replacement.Key))
            {
                return replacement.Value.Replacement;
            }
        }

        return null;
    }

    public static TiltReplacement GetOriginalReplacement(uint tiltId)
    {
        ref var replacement = ref CollectionsMarshal.GetValueRefOrAddDefault(OldValue, tiltId, out var exists);
        if (!exists)
        {
            var act = Service.DataManager.GetExcelSheet<TiltParam>()?.GetRow(tiltId);
            replacement = new Configurations.TiltReplacement(
                (ushort)(act?.Unknown0 ?? 0),
                (byte)(act?.Unknown1 ?? 0),
                (byte)(act?.Unknown2 ?? 0),
                (byte)(act?.Unknown3 ?? 0),
                (byte)(act?.Unknown4 ?? 0));
        }

        return replacement!;
    }
}