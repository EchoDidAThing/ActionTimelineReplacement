using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Lumina.Excel.Sheets;

namespace ActionTimelineReplacement.Configurations;

public static class ActionCastVFXReplacementsManager
{
    private static Dictionary<uint, string>? _actionCastVFXNames;

    private static readonly Dictionary<uint, ActionCastVFXReplacement> OldValue = [];

    public static Dictionary<uint, string> ActionCastVFXNames => _actionCastVFXNames
        ??= Service.DataManager.GetExcelSheet<Action>()
            .Where(i => !string.IsNullOrEmpty(i.Name.ToString()))
            .ToDictionary(i => i.RowId, i => i.Name.ToString());

    
    public static IEnumerable<uint> AllCastVFXActionIds =>
        Service.Config.ReplacementSets.SelectMany(i => i.ActionCastVFXReplacements.Keys);

    public static string GetName(uint id)
    {
        return ActionCastVFXNames.GetValueOrDefault(id, "Unknown");
    }

    public static ActionCastVFXReplacement GetReplacement(uint actionId)
        => GetConfigReplacement(actionId) ?? GetOriginalReplacement(actionId);

    private static ActionCastVFXReplacement? GetConfigReplacement(uint actionId)
    {
        if (!Service.Config.EnableReplacement) return null;

        foreach (var item in Service.Config.ReplacementSets)
        {
            foreach (var replacement in item.ActionCastVFXReplacements
                         .Where(replacement => item.Enabled)
                         .OrderByDescending(replacement => item.Priority))
            {
                return replacement.Value.Replacement;
            }
        }

        return null;
    }

    public static ActionCastVFXReplacement GetOriginalReplacement(uint actionCastVFXId)
    {
        ref var replacement = ref CollectionsMarshal.GetValueRefOrAddDefault(OldValue, actionCastVFXId, out var exists);
        if (!exists)
        {
            var act = Service.DataManager.GetExcelSheet<ActionCastVFX>()?.GetRow(actionCastVFXId);
            replacement = new Configurations.ActionCastVFXReplacement(
                (ushort)(act?.VFX.RowId ?? 0));
        }

        return replacement!;
    }
}