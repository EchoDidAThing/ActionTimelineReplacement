using System;
using System.Collections.Generic;
using System.IO;
using Dalamud.Configuration;
using Newtonsoft.Json;

namespace ActionTimelineReplacement.Sheets;

[Serializable]
public class Configuration : IPluginConfiguration
{
    //TOSETUP: add new dictionary here
    public class ReplacementSet(string name, bool enabled, int priority, Dictionary<uint, ActionConfig> actionwriter, Dictionary<uint, ActionCastVFXConfig> actioncastvfxwriter, Dictionary<uint, MountConfig> mountwriter, Dictionary<uint, StatusConfig> statuswriter, Dictionary<uint, TiltParamConfig> tiltparamwriter, Dictionary<uint, GlassesConfig> glasseswriter, Dictionary<uint, PlaceNameConfig> placenamewriter)
    {
        public string Name = name;

        public bool Enabled = enabled;

        public int Priority = priority;

        //TOSETUP: add new dictionary here
        public Dictionary<uint, ActionConfig> ActionWriter { get; set; } = actionwriter;
        public Dictionary<uint, ActionCastVFXConfig> ActionCastVFXWriter { get; set; } = actioncastvfxwriter;
        public Dictionary<uint, MountConfig> MountWriter { get; set; } = mountwriter;
        public Dictionary<uint, TiltParamConfig> TiltParamWriter { get; set; } = tiltparamwriter;
        public Dictionary<uint, StatusConfig> StatusWriter { get; set; } = statuswriter;
        public Dictionary<uint, GlassesConfig> GlassesWriter { get; set; } = glasseswriter;
        public Dictionary<uint, PlaceNameConfig> PlaceNameWriter { get; set; } = placenamewriter;

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

    //public bool AdvancedMode = false;

    public List<ReplacementSet> ReplacementSets { get; set; } = [];

    internal void Save()
    {
        Service.PluginInterface.SavePluginConfig(this);
    }
}