using Dalamud.Memory.Exceptions;
using Lumina.Excel.Sheets;
using Lumina.Text.ReadOnly;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
#pragma warning disable CA1416 // Validate platform compatibility

namespace ActionTimelineReplacement.Sheets;

public static class OrnamentTransientsManager
{
    private static bool usesdetours = true;
    private static Dictionary<uint, string>? _Names;

    public static Dictionary<uint, OrnamentTransientsReplace> old { get; private set; } = [];

    public static Dictionary<uint, string> Names => _Names
        ??= Service.DataManager.GetExcelSheet<Ornament>()
            .Where(i => !string.IsNullOrEmpty(i.Singular.ToString()))
            .ToDictionary(i => i.RowId, i => i.Singular.ToString());

    public static IEnumerable<uint> AllOrnamentIds =>
        Service.Config.ReplacementSets.SelectMany(i => i.OrnamentTransientsWriter.Keys);

    public static string GetName(uint id)
    {
        return Names.GetValueOrDefault(id, "Unknown");
    }

    public static OrnamentTransientsReplace GetReplacement(uint idx)
        => GetConfig(idx) ?? GetOriginal(idx);

    private static OrnamentTransientsReplace? GetConfig(uint idx)
    {
        if (!Service.Config.EnableReplacement) return null;

        List<KeyValuePair<int, OrnamentTransientsReplace>> replacements = [];

        foreach (var item in Service.Config.ReplacementSets)
        {
            if (item.CharacterName != Service.PlayerState.CharacterName) continue;
            if (item.HomeWorld != Service.PlayerState.HomeWorld.RowId) continue;
            if (!item.Jobs.CheckJob(Service.PlayerState.ClassJob.Value.Abbreviation.ToString())) continue;
            if (!item.Enabled) continue;
            foreach (var replacement in item.OrnamentTransientsWriter)
            {
                if (replacement.Key != idx) continue;
                if (!replacement.Value.Enabled) continue;
                replacements.Add(new KeyValuePair<int, OrnamentTransientsReplace>(item.Priority, replacement.Value.Replacement));
                
            }
        }
        foreach (var replacement in replacements
                         .OrderByDescending(r => r.Key))
        {
            return replacement.Value;
        }
        return null;
    }

    public static OrnamentTransientsReplace GetOriginal(uint idx)
    {
        var replacement = new OrnamentTransientsReplace(0,"","", "");
        if (old.ContainsKey(idx) && !usesdetours) 
        {
            replacement = old[idx];
        }
        else
        {
            var act = Service.DataManager.GetExcelSheet<Ornament>()?.GetRow(idx);
            var act2 = Service.DataManager.GetExcelSheet<OrnamentTransient>()?.GetRow(idx);
            replacement = new OrnamentTransientsReplace(
                (ushort)(act?.Icon ?? 0),
                (string)(act?.Singular.ToString() ?? ""),
                (string)(act?.Plural.ToString() ?? ""),
                (string)(act2?.Text.ToMacroString()?? "")
                );
            if (!old.ContainsKey(idx)) { old.Add(idx, replacement); }
            //Service.Log.Error("value does not exist in old or was bypassed, getting new. sample:" + replacement.ActionName);

        }
        //Service.Log.Error("Current in memory default name is:" + Service.DataManager.GetExcelSheet<Action>()?.GetRow(idx).Name.ToString());
        return replacement;
    }
}