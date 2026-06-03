using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Lumina.Excel.Sheets;
#pragma warning disable CA1416 // Validate platform compatibility

namespace ActionTimelineReplacement.Sheets;

public static class MountDetourManager
{
    private static Dictionary<uint, string>? _Names;

    private static readonly Dictionary<uint, MountDetourReplace> old = [];

    public static Dictionary<uint, string> Names => _Names
        ??= Service.DataManager.GetExcelSheet<Mount>()
            .Where(i => !string.IsNullOrEmpty(i.Singular.ToString()))
            .ToDictionary(i => i.RowId, i => i.Singular.ToString());

    public static IEnumerable<uint> AllMountDetourIds =>
       Service.Config.ReplacementSets.SelectMany(i => i.MountDetourWriter.Keys);

    public static string GetName(uint id)
    {
        return Names.GetValueOrDefault(id, "Unknown");
    }

    public static MountDetourReplace GetReplacement(uint idx)
        => GetConfig(idx) ?? GetOriginal(idx);

    private static MountDetourReplace? GetConfig(uint idx)
    {
        if (!Service.Config.EnableReplacement) return null;

        List<KeyValuePair<int, MountDetourReplace>> replacements = [];

        foreach (var item in Service.Config.ReplacementSets)
        {
            if (item.CharacterName != Service.PlayerState.CharacterName) continue;
            if (item.HomeWorld != Service.PlayerState.HomeWorld.RowId) continue;
            if (!item.Jobs.CheckJob(Service.PlayerState.ClassJob.Value.Abbreviation.ToString())) continue;
            if (!item.Enabled) continue;
            foreach (var replacement in item.MountDetourWriter)
            {
                if (replacement.Key != idx) continue;
                if (!replacement.Value.Enabled) continue;
                replacements.Add(new KeyValuePair<int, MountDetourReplace>(item.Priority, replacement.Value.Replacement));

            }
        }
        foreach (var replacement in replacements
                         .OrderByDescending(r => r.Key))
        {
            return replacement.Value;
        }
        return null;
    }

    public static MountDetourReplace GetOriginal(uint idx)
    {
        ref var replacement = ref CollectionsMarshal.GetValueRefOrAddDefault(old, idx, out var exists);
        if (!exists)
        {
            var act = Service.DataManager.GetExcelSheet<Mount>()?.GetRow(idx);
            var act2 = Service.DataManager.GetExcelSheet<BGM>()?.GetRow(act?.RideBGM.RowId ?? 0);
            var act3 = Service.DataManager.GetExcelSheet<MountCustomize>()?.GetRow(act?.MountCustomize.RowId ?? 0);
            var act4 = Service.DataManager.GetExcelSheet<TiltParam>()?.GetRow(act?.Unknown_70_1 ?? 0);
            var act5 = Service.DataManager.GetExcelSheet<TiltParam>()?.GetRow(act?.Unknown_70_2 ?? 0);
            var act6 = Service.DataManager.GetExcelSheet<TiltParam>()?.GetRow(act?.Unknown16 ?? 0);
            var act7 = Service.DataManager.GetExcelSheet<TiltParam>()?.GetRow(act?.Unknown17 ?? 0);
            replacement = new MountDetourReplace(
                act2?.File.ToString() ?? "",
                act?.Unknown5 ?? 0,
                act?.BaseMotionSpeed_Walk ?? 0,
                act?.BaseMotionSpeed_Run ?? 0,
                act?.Unknown9 ?? 0,
                new RacialScalingSet (
                    act3?.HyurMidlanderMaleScale ?? 0,
                    act3?.HyurMidlanderFemaleScale ?? 0,
                    act3?.HyurHighlanderMaleScale ?? 0,
                    act3?.HyurHighlanderFemaleScale ?? 0,
                    act3?.ElezenMaleScale ?? 0,
                    act3?.ElezenFemaleScale ?? 0,
                    act3?.LalaMaleScale ?? 0,
                    act3?.LalaFemaleScale ?? 0,
                    act3?.MiqoMaleScale ?? 0,
                    act3?.MiqoFemaleScale ?? 0,
                    act3?.RoeMaleScale ?? 0,
                    act3?.RoeFemaleScale ?? 0,
                    act3?.AuRaMaleScale ?? 0,
                    act3?.AuRaFemaleScale ?? 0,
                    act3?.HrothgarMaleScale ?? 0,
                    act3?.HrothgarFemaleScale ?? 0,
                    act3?.VieraMaleScale ?? 0,
                    act3?.VieraFemaleScale ?? 0
                    ),
                new RacialScalingSet(
                    act3?.HyurMidlanderMaleCameraHeight ?? 0,
                    act3?.HyurMidlanderFemaleCameraHeight ?? 0,
                    act3?.HyurHighlanderMaleCameraHeight ?? 0,
                    act3?.HyurHighlanderFemaleCameraHeight ?? 0,
                    act3?.ElezenMaleCameraHeight ?? 0,
                    act3?.ElezenFemaleCameraHeight ?? 0,
                    act3?.LalaMaleCameraHeight ?? 0,
                    act3?.LalaFemaleCameraHeight ?? 0,
                    act3?.MiqoMaleCameraHeight ?? 0,
                    act3?.MiqoFemaleCameraHeight ?? 0,
                    act3?.RoeMaleCameraHeight ?? 0,
                    act3?.RoeFemaleCameraHeight ?? 0,
                    act3?.AuRaMaleCameraHeight ?? 0,
                    act3?.AuRaFemaleCameraHeight ?? 0,
                    act3?.HrothgarMaleCameraHeight ?? 0,
                    act3?.HrothgarFemaleCameraHeight ?? 0,
                    act3?.VieraMaleCameraHeight ?? 0,
                    act3?.VieraFemaleCameraHeight ?? 0
                    ),
                new TiltSet (
                    act4?.Unknown1 ?? 0,
                    act4?.Unknown3 ?? 0,
                    act4?.Unknown4 ?? 0,
                    act4?.Unknown2 ?? 0,
                    act4?.Unknown0 ?? 0,
                    act4?.Unknown5 ?? false
                    ),
                new TiltSet(
                    act5?.Unknown1 ?? 0,
                    act5?.Unknown3 ?? 0,
                    act5?.Unknown4 ?? 0,
                    act5?.Unknown2 ?? 0,
                    act5?.Unknown0 ?? 0,
                    act5?.Unknown5 ?? false
                    ),
                new TiltSet(
                    act6?.Unknown1 ?? 0,
                    act6?.Unknown3 ?? 0,
                    act6?.Unknown4 ?? 0,
                    act6?.Unknown2 ?? 0,
                    act6?.Unknown0 ?? 0,
                    act6?.Unknown5 ?? false
                    ),
                new TiltSet(
                    act7?.Unknown1 ?? 0,
                    act7?.Unknown3 ?? 0,
                    act7?.Unknown4 ?? 0,
                    act7?.Unknown2 ?? 0,
                    act7?.Unknown0 ?? 0,
                    act7?.Unknown5 ?? false
                    )
                );
        }
        return replacement!;
    }
}