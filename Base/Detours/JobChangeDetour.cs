using ActionTimelineReplacement.Sheets;
using FFXIVClientStructs.FFXIV.Client.UI.Misc;
using FFXIVClientStructs.FFXIV.Component.GUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace ActionTimelineReplacement.Base.Detours;

internal class JobChangeDetour
{

    public static Dictionary<RaptureHotbarModule.HotbarSlotType, Dictionary<uint, uint>>  CurrentJobIcons = [];
    public static void UpdateIconList(Dictionary<RaptureHotbarModule.HotbarSlotType, Dictionary<uint, uint>> currentjobicons)
    {
        currentjobicons.Clear();
        var tempcurrenticons = new Dictionary<uint, uint>();
        var ids = ActionTransientsManager.AllActionTransientIds;

        if (ids != null) 
        {
            foreach (var id in ids)
            {
                if (tempcurrenticons.ContainsKey(id)) {
                    Service.Log.Error("key" + id + " already exists");
                    continue;
                }
                var replacement = ActionTransientsManager.GetReplacement(id).Icon;
                var original = ActionTransientsManager.GetOriginal(id).Icon;
                Service.Log.Error("adding" + original + " with value of " + replacement);
                if (original == replacement) { continue; }
                Service.Log.Error("adding" + original + " with value of " + replacement);
                tempcurrenticons.Add(id, replacement);
            }
        }
        currentjobicons.Add(RaptureHotbarModule.HotbarSlotType.Action , tempcurrenticons);

        ids = MountTransientsManager.AllMountTransientsIds;
        if (ids != null)
        {
            foreach (var id in ids)
            {
                if (tempcurrenticons.ContainsKey(id))
                {
                    Service.Log.Error("key" + id + " already exists");
                    continue;
                }
                var replacement = MountTransientsManager.GetReplacement(id).Icon;
                var original = MountTransientsManager.GetOriginal(id).Icon;
                Service.Log.Error("adding" + original + " with value of " + replacement);
                if (original == replacement) { continue; }
                Service.Log.Error("adding" + original + " with value of " + replacement);
                tempcurrenticons.Add(original, replacement);
            }
        }
        ids = CompanionTransientsManager.AllCompanionTransientsIds;
        if (ids != null)
        {
            foreach (var id in ids)
            {
                if (tempcurrenticons.ContainsKey(id))
                {
                    Service.Log.Error("key" + id + " already exists");
                    continue;
                }
                var replacement = CompanionTransientsManager.GetReplacement(id).Icon;
                var original = CompanionTransientsManager.GetOriginal(id).Icon;
                Service.Log.Error("adding" + original + " with value of " + replacement);
                if (original == replacement) { continue; }
                Service.Log.Error("adding" + original + " with value of " + replacement);
                tempcurrenticons.Add(original, replacement);
            }
        }

        Service.Log.Error("after update count of changed ids is " + tempcurrenticons.Count);
    }
}

