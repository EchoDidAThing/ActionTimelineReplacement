using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Lumina.Excel.Sheets;
#pragma warning disable CA1416 // Validate platform compatibility

namespace ActionTimelineReplacement.Sheets;

public static class ActionManager
{
    private static Dictionary<uint, string>? _Names;

    private static readonly Dictionary<uint, ActionReplace> old = [];

    public static Dictionary<uint, string> Names => _Names
        ??= Service.DataManager.GetExcelSheet<Action>()
            .Where(i => !string.IsNullOrEmpty(i.Name.ToString()))
            .ToDictionary(i => i.RowId, i => i.Name.ToString());

    public static IEnumerable<uint> AllActionIds =>
        Service.Config.ReplacementSets.SelectMany(i => i.ActionWriter.Keys);

    public static string GetName(uint id)
    {
        return Names.GetValueOrDefault(id, "Unknown");
    }

    public static ActionReplace GetReplacement(uint idx)
        => GetConfig(idx) ?? GetOriginal(idx);

    private static ActionReplace? GetConfig(uint idx)
    {
        if (!Service.Config.EnableReplacement) return null;

        List<KeyValuePair<int, ActionReplace>> replacements = [];

        foreach (var item in Service.Config.ReplacementSets)
        {
            foreach (var replacement in item.ActionWriter
                         .Where(r => item.Enabled))
            {
                replacements.Add(new KeyValuePair<int, ActionReplace>(item.Priority, replacement.Value.Replacement));
            }
        }
        foreach (var replacement in replacements
                         .OrderByDescending(r => r.Key))
        {
            return replacement.Value;
        }
        return null;
    }

    public static ActionReplace GetOriginal(uint idx)
    {
        ref var replacement = ref CollectionsMarshal.GetValueRefOrAddDefault(old, idx, out var exists);
        if (!exists)
        {
            var act = Service.DataManager.GetExcelSheet<Action>()?.GetRow(idx);
            replacement = new ActionReplace(
                (ushort)(act?.AnimationStart.RowId ?? 0),
                (short)(act?.AnimationEnd.RowId ?? 0),
                (ushort)(act?.ActionTimelineHit.RowId ?? 0),
                (ushort)(act?.VFX.RowId ?? 0),
                act?.Unknown1 ?? 0,
                act?.Unknown2 ?? 0,
                act?.Unknown4 ?? 0,
                act?.Unknown_70 ?? 0
                );
        }
        return replacement!;
    }
}