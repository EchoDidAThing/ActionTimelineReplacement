using System;
using System.Collections.Generic;
using System.IO;
using Dalamud.Configuration;
using Newtonsoft.Json;

namespace ActionTimelineReplacement.Configurations;


[Serializable]
public class Configuration : IPluginConfiguration
{
    //TOSETUP: add new dictionary here
    public class ReplacementSet(string name, bool enabled, int priority, Dictionary<uint, ActionReplacementConfig> actionreplacements, Dictionary<uint, ActionCastVFXReplacementConfig> actioncastvfxreplacements, Dictionary<uint, MountReplacementConfig> mountreplacements, Dictionary<uint, TiltReplacementConfig> tiltreplacements, Dictionary<uint, StatusReplacementConfig> statusreplacements, Dictionary<uint, GlassesReplacementConfig> glassesreplacements, Dictionary<uint, PlaceNameReplacementConfig> placenamereplacements)
    {
        public string Name = name;

        public bool Enabled = enabled;

        public int Priority = priority;

        //TOSETUP: add new dictionary here
        public Dictionary<uint, ActionReplacementConfig> ActionReplacements { get; set; } = actionreplacements;
        public Dictionary<uint, ActionCastVFXReplacementConfig> ActionCastVFXReplacements { get; set; } = actioncastvfxreplacements;
        public Dictionary<uint, MountReplacementConfig> MountReplacements { get; set; } = mountreplacements;
        public Dictionary<uint, TiltReplacementConfig> TiltReplacements { get; set; } = tiltreplacements;
        public Dictionary<uint, StatusReplacementConfig> StatusReplacements { get; set; } = statusreplacements;
        public Dictionary<uint, GlassesReplacementConfig> GlassesReplacements { get; set; } = glassesreplacements;
        public Dictionary<uint, PlaceNameReplacementConfig> PlaceNameReplacements { get; set; } = placenamereplacements;

        public static ReplacementSet? Load(string jsonFile)
        {
            try
            {
                ReplacementSet? replacements =
                    JsonConvert.DeserializeObject<ReplacementSet>(
                        File.ReadAllText(jsonFile));

                if (replacements == null) return null;

                return replacements;
            }
            catch
            {
                return null;
            }
        }

        public static bool Save(string jsonFile, int index)
        {
            try
            {
                File.WriteAllText(jsonFile, JsonConvert.SerializeObject(Service.Config.ReplacementSets[index]));

                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    public int Version { get; set; } = 1;

    public bool EnableReplacement = true;

    public bool AdvancedMode = false;

    public List<ReplacementSet> ReplacementSets { get; set; } = [];

    internal void Save()
    {
        Service.PluginInterface.SavePluginConfig(this);
    }
}