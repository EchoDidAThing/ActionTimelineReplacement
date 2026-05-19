using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Lumina.Excel.Sheets;
#pragma warning disable CA1416 // Validate platform compatibility

namespace ActionTimelineReplacement.Sheets;

public static class MountCustomizeManager
{
    private static Dictionary<uint, string>? _Names;

    private static readonly Dictionary<uint, MountCustomizeReplace> old = [];

    public static Dictionary<uint, string> Names => _Names
        ??= Service.DataManager.GetExcelSheet<MountCustomize>()
            .Where(i => !string.IsNullOrEmpty(i.RowId.ToString()))
            .ToDictionary(i => i.RowId, i => i.RowId.ToString());

    public static IEnumerable<uint> AllMountCustomizeIds =>
       Service.Config.ReplacementSets.SelectMany(i => i.MountCustomizeWriter.Keys);

    public static string GetName(uint id)
    {
        return Names.GetValueOrDefault(id, "Unknown");
    }

    public static MountCustomizeReplace GetReplacement(uint idx)
        => GetConfig(idx) ?? GetOriginal(idx);

    private static MountCustomizeReplace? GetConfig(uint idx)
    {
        if (!Service.Config.EnableReplacement) return null;

        List<KeyValuePair<int, MountCustomizeReplace>> replacements = [];

        foreach (var item in Service.Config.ReplacementSets)
        {
            if (item.CharacterName != Service.PlayerState.CharacterName) continue;
            if (item.HomeWorld != Service.PlayerState.HomeWorld.RowId) continue;
            if (!item.Jobs.CheckJob(Service.PlayerState.ClassJob.Value.Abbreviation.ToString())) continue;
            if (!item.Enabled) continue;
            foreach (var replacement in item.MountCustomizeWriter)
            {
                if (replacement.Key != idx) continue;
                if (!replacement.Value.Enabled) continue;
                replacements.Add(new KeyValuePair<int, MountCustomizeReplace>(item.Priority, replacement.Value.Replacement));

            }
        }
        foreach (var replacement in replacements
                         .OrderByDescending(r => r.Key))
        {
            return replacement.Value;
        }
        return null;
    }

    public static MountCustomizeReplace GetOriginal(uint idx)
    {
        ref var replacement = ref CollectionsMarshal.GetValueRefOrAddDefault(old, idx, out var exists);
        if (!exists)
        {
            var act = Service.DataManager.GetExcelSheet<MountCustomize>()?.GetRow(idx);
            replacement = new MountCustomizeReplace(
                act?.HyurMidlanderMaleScale ?? 0,
                act?.HyurMidlanderFemaleScale ?? 0,
                act?.HyurHighlanderMaleScale ?? 0,
                act?.HyurHighlanderFemaleScale ?? 0,
                act?.ElezenMaleScale ?? 0,
                act?.ElezenFemaleScale ?? 0,
                act?.LalaMaleScale ?? 0,
                act?.LalaFemaleScale ?? 0,
                act?.MiqoMaleScale ?? 0,
                act?.MiqoFemaleScale ?? 0,
                act?.RoeMaleScale ?? 0,
                act?.RoeFemaleScale ?? 0,
                act?.AuRaMaleScale ?? 0,
                act?.AuRaFemaleScale ?? 0,
                act?.HrothgarMaleScale ?? 0,
                act?.HrothgarFemaleScale ?? 0,
                act?.VieraMaleScale ?? 0,
                act?.VieraFemaleScale ?? 0,
                act?.HyurHighlanderMaleCameraHeight ?? 0,
                act?.HyurMidlanderFemaleCameraHeight ?? 0,
                act?.HyurHighlanderMaleCameraHeight ?? 0,
                act?.HyurHighlanderFemaleCameraHeight ?? 0,
                act?.ElezenMaleCameraHeight ?? 0,
                act?.ElezenFemaleCameraHeight ?? 0,
                act?.LalaMaleCameraHeight ?? 0,
                act?.LalaFemaleCameraHeight ?? 0,
                act?.MiqoMaleCameraHeight ?? 0,
                act?.MiqoFemaleCameraHeight ?? 0,
                act?.RoeMaleCameraHeight ?? 0,
                act?.RoeFemaleCameraHeight ?? 0,
                act?.AuRaMaleCameraHeight ?? 0,
                act?.AuRaFemaleCameraHeight ?? 0,
                act?.HrothgarMaleCameraHeight ?? 0,
                act?.HrothgarFemaleCameraHeight ?? 0,
                act?.VieraMaleCameraHeight ?? 0,
                act?.VieraFemaleCameraHeight ?? 0);
        }
        return replacement!;
    }
}