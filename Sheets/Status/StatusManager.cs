using FFXIVClientStructs.FFXIV.Common.Lua;
using Lumina.Excel.Sheets;
using Serilog.Formatting.Display;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace ActionTimelineReplacement.Sheets;

public class StatusManager
{
    private static Dictionary<uint, string>? _Names;

    private static Dictionary<uint, StatusReplace> old  = [] ;

    public static StatusReplace OldExport(uint id)
    {
        var output = old[id];
        return output;
    }

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

        List<KeyValuePair<int, StatusReplace>> replacements = [];

        foreach (var item in Service.Config.ReplacementSets)
        {
            if (item.CharacterName != Service.PlayerState.CharacterName) continue;
            if (item.HomeWorld != Service.PlayerState.HomeWorld.RowId) continue;
            if (!item.Jobs.CheckJob(Service.PlayerState.ClassJob.Value.Abbreviation.ToString())) continue;
            if (!item.Enabled) continue;
            foreach (var replacement in item.StatusWriter)
            {
                if (replacement.Key != idx) continue;
                if (!replacement.Value.Enabled) continue;
                replacements.Add(new KeyValuePair<int, StatusReplace>(item.Priority, replacement.Value.Replacement));

            }
        }
        foreach (var replacement in replacements
                         .OrderByDescending(r => r.Key))
        {
            return replacement.Value;
        }
        return null;
    }

    public static StatusReplace GetOriginal(uint idx)
    {
        Service.Log.Error("Initiating GetOriginal for [{ID}]", idx);
        ref var replacement = ref CollectionsMarshal.GetValueRefOrAddDefault(old, idx, out var exists);
        if (!exists)
        {

            Service.Log.Error("No value found in old, pulling fresh");
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
        Service.Log.Error("exporting value [{statusloopvfx}]for id [{ID}]", replacement!.StatusLoopVFX, idx);
        return replacement!;
    }
}