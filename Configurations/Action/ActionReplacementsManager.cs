using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Lumina.Excel.Sheets;

namespace ActionTimelineReplacement.Configurations;

public static class ActionReplacementsManager
{
    private static Dictionary<uint, string>? _actionNames;

    private static readonly Dictionary<uint, ActionReplacement> OldValue = [];

    public static Dictionary<uint, string> ActionNames => _actionNames
        ??= Service.DataManager.GetExcelSheet<Action>()
            .Where(i => !string.IsNullOrEmpty(i.Name.ToString()))
            .ToDictionary(i => i.RowId, i => i.Name.ToString());
    
    public static IEnumerable<uint> AllActionIds =>
        Service.Config.ReplacementSets.SelectMany(i => i.ActionReplacements.Keys);

    public static string GetName(uint id)
    {
        return ActionNames.GetValueOrDefault(id, "Unknown");
    }

    public static ActionReplacement GetReplacement(uint actionId)
        => GetConfigReplacement(actionId) ?? GetOriginalReplacement(actionId);

    private static ActionReplacement? GetConfigReplacement(uint actionId)
    {
        if (!Service.Config.EnableReplacement) return null;

        foreach (var item in Service.Config.ReplacementSets)
        {
            foreach (var replacement in item.ActionReplacements
                         .Where(replacement => item.Enabled)
                         .OrderByDescending(replacement => item.Priority))
            {
                return replacement.Value.Replacement;
            }
        }
        return null;
    }

    public static ActionReplacement GetOriginalReplacement(uint actionId)
    {
        ref var replacement = ref CollectionsMarshal.GetValueRefOrAddDefault(OldValue, actionId, out var exists);
        if (!exists)
        {
            var act = Service.DataManager.GetExcelSheet<Action>()?.GetRow(actionId);
            replacement = new ActionReplacement(
                (ushort)(act?.AnimationStart.RowId ?? 0),
                (ushort)(act?.AnimationEnd.RowId ?? 0),
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