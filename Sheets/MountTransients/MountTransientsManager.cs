using Dalamud.Memory.Exceptions;
using Lumina.Excel.Sheets;
using Lumina.Text.ReadOnly;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
#pragma warning disable CA1416 // Validate platform compatibility

namespace ActionTimelineReplacement.Sheets;

public static class MountTransientsManager
{
    private static bool usesdetours = true;
    private static Dictionary<uint, string>? _Names;

    public static Dictionary<uint, MountTransientsReplace> old { get; private set; } = [];

    public static Dictionary<uint, string> Names => _Names
        ??= Service.DataManager.GetExcelSheet<Mount>()
            .Where(i => !string.IsNullOrEmpty(i.Singular.ToString()))
            .ToDictionary(i => i.RowId, i => i.Singular.ToString());

    public static IEnumerable<uint> AllMountTransientsIds =>
        Service.Config.ReplacementSets.SelectMany(i => i.MountTransientsWriter.Keys);

    public static string GetName(uint id)
    {
        return Names.GetValueOrDefault(id, "Unknown");
    }

    public static MountTransientsReplace GetReplacement(uint idx)
        => GetConfig(idx) ?? GetOriginal(idx);

    private static MountTransientsReplace? GetConfig(uint idx)
    {
        if (!Service.Config.EnableReplacement) return null;

        List<KeyValuePair<int, MountTransientsReplace>> replacements = [];

        foreach (var item in Service.Config.ReplacementSets)
        {
            if (item.CharacterName != Service.PlayerState.CharacterName) continue;
            if (item.HomeWorld != Service.PlayerState.HomeWorld.RowId) continue;
            if (!item.Jobs.CheckJob(Service.PlayerState.ClassJob.Value.Abbreviation.ToString())) continue;
            if (!item.Enabled) continue;
            foreach (var replacement in item.MountTransientsWriter)
            {
                if (replacement.Key != idx) continue;
                if (!replacement.Value.Enabled) continue;
                replacements.Add(new KeyValuePair<int, MountTransientsReplace>(item.Priority, replacement.Value.Replacement));
                
            }
        }
        if (replacements.Count == 0) { return null; }
        foreach (var replacement in replacements
                         .OrderByDescending(r => r.Key))
        {
            return replacement.Value;
        }
        return null;
    }

    public static MountTransientsReplace GetOriginal(uint idx)
    {
        var replacement = new MountTransientsReplace(0,"","", "", "", "");
        if (old.ContainsKey(idx) && !usesdetours) 
        {
            replacement = old[idx];
            //Service.Log.Error("value did exist in old, keeping existing. sample:" + replacement.ActionName);
        }
        else
        {
            var act = Service.DataManager.GetExcelSheet<Mount>()?.GetRow(idx);
            var act2 = Service.DataManager.GetExcelSheet<MountTransient>()?.GetRow(idx);
            replacement = new MountTransientsReplace(
                (ushort)(act?.Icon ?? 0),
                (string)(act?.Singular.ToString() ?? ""),
                (string)(act?.Plural.ToString() ?? ""),
                (string)(act2?.Description.ToMacroString()?? ""),
                (string)(act2?.DescriptionEnhanced.ToMacroString() ?? ""),
                (string)(act2?.Tooltip.ToMacroString() ?? "")
                );
            if (!old.ContainsKey(idx)) { old.Add(idx, replacement); }
            //Service.Log.Error("value does not exist in old or was bypassed, getting new. sample:" + replacement.ActionName);

        }
        //Service.Log.Error("Current in memory default name is:" + Service.DataManager.GetExcelSheet<Action>()?.GetRow(idx).Name.ToString());
        return replacement;
    }
}