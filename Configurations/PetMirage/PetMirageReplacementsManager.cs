using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Lumina.Excel.Sheets;

namespace ActionTimelineReplacement.Configurations;

public static class PetMirageReplacementsManager
{
    private static Dictionary<uint, string>? _petMirageNames;

    private static readonly Dictionary<uint, PetMirageReplacement> OldValue = [];

    public static Dictionary<uint, string> PetMirageNames => _petMirageNames
        ??= Service.DataManager.GetExcelSheet<PetMirage>()
            .Where(i => !string.IsNullOrEmpty(i.RowId.ToString()))
            .ToDictionary(i => i.RowId, i => i.RowId.ToString());

    public static IEnumerable<uint> AllPetMirageIds =>
       Service.Config.ReplacementSets.SelectMany(i => i.PetMirageReplacements.Keys);

    public static string GetName(uint id)
    {
        return PetMirageNames.GetValueOrDefault(id, "Unknown");
    }

    public static PetMirageReplacement GetReplacement(uint petMirageId)
        => GetConfigReplacement(petMirageId) ?? GetOriginalReplacement(petMirageId);

    private static PetMirageReplacement? GetConfigReplacement(uint petMirageId)
    {
        if (!Service.Config.EnableReplacement) return null;

        foreach (var item in Service.Config.ReplacementSets)
        {
            foreach (var replacement in item.PetMirageReplacements
                         .Where(replacement => replacement.Value.Enabled)
                         .OrderByDescending(replacement => replacement.Key))
            {
                return replacement.Value.Replacement;
            }
        }
        return null;
    }

    public static PetMirageReplacement GetOriginalReplacement(uint petMirageId)
    {
        ref var replacement = ref CollectionsMarshal.GetValueRefOrAddDefault(OldValue, petMirageId, out var exists);
        if (!exists)
        {
            var act = Service.DataManager.GetExcelSheet<PetMirage>()?.GetRow(petMirageId);
            replacement = new PetMirageReplacement(
                //(string)(act?.Name.ToString() ?? ""),
                act?.Unknown0 ?? 0,
                act?.Unknown1 ?? 0,
                act?.Unknown2 ?? 0,
                act?.Unknown3 ?? 0,
                act?.Unknown4 ?? 0,
                act?.Unknown5 ?? 0,
                act?.Unknown6 ?? 0,
                act?.Unknown7 ?? 0,
                act?.Scale ?? 0.0f);
        }
        return replacement!;
    }
}
