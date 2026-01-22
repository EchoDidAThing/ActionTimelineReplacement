using System;
using System.Collections.Generic;
using System.IO;
using Dalamud.Configuration;
using Newtonsoft.Json;
#pragma warning disable CA1416 // Validate platform compatibility

namespace ActionTimelineReplacement.Sheets;

[Serializable]
public class Configuration : IPluginConfiguration
{
    //TOSETUP: add new dictionary here
    public class ReplacementSet(string name, bool enabled, int priority,
    Dictionary<uint, ActionConfig> actionwriter,
    Dictionary<uint, ActionCastVFXConfig> actioncastvfxwriter,
    Dictionary<uint, MountConfig> mountwriter,
    Dictionary<uint, MountCustomizeConfig> mountcustomizewriter,
    Dictionary<uint, StatusConfig> statuswriter,
    Dictionary<uint, TiltParamConfig> tiltparamwriter,
    Dictionary<uint, GlassesConfig> glasseswriter,
    Dictionary<uint, GlassesStyleConfig> glassesstylewriter,
    //Dictionary<uint, PlaceNameConfig> placenamewriter,
    Dictionary<uint, ActionTimelineConfig> actiontimelinewriter,
    Dictionary<uint, OrnamentConfig> ornamentwriter,
    Dictionary<uint, OrnamentCustomizeConfig> ornamentcustomizewriter,
    Dictionary<uint, VfxConfig> vfxwriter)
    //Dictionary<uint, OrnamentCustomizeGroupConfig> ornamentcustomizegroupwriter)
    {
        public string Name = name;

        public bool Enabled = enabled;

        public int Priority = priority;

        //TOSETUP: add new dictionary here
        public Dictionary<uint, ActionConfig> ActionWriter { get; set; } = actionwriter;
        public Dictionary<uint, ActionCastVFXConfig> ActionCastVFXWriter { get; set; } = actioncastvfxwriter;
        public Dictionary<uint, MountConfig> MountWriter { get; set; } = mountwriter;
        public Dictionary<uint, MountCustomizeConfig> MountCustomizeWriter { get; set; } = mountcustomizewriter;
        public Dictionary<uint, TiltParamConfig> TiltParamWriter { get; set; } = tiltparamwriter;
        public Dictionary<uint, StatusConfig> StatusWriter { get; set; } = statuswriter;
        public Dictionary<uint, GlassesConfig> GlassesWriter { get; set; } = glasseswriter;
        public Dictionary<uint, GlassesStyleConfig> GlassesStyleWriter { get; set; } = glassesstylewriter;
        //public Dictionary<uint, PlaceNameConfig> PlaceNameWriter { get; set; } = placenamewriter;
        public Dictionary<uint, ActionTimelineConfig> ActionTimelineWriter { get; set; } = actiontimelinewriter;
        public Dictionary<uint, OrnamentConfig> OrnamentWriter { get; set; } = ornamentwriter;
        public Dictionary<uint, OrnamentCustomizeConfig> OrnamentCustomizeWriter { get; set; } = ornamentcustomizewriter;
        public Dictionary<uint, VfxConfig> VfxWriter { get; set; } = vfxwriter;
        //public Dictionary<uint, OrnamentCustomizeGroupConfig> OrnamentCustomizeGroupWriter { get; set; } = ornamentcustomizegroupwriter;
        //public Dictionary<float, PointMenuChoiceConfig> PointMenuChoiceWriter { get; set; } = pointmenuchoicewriter; //not really float, but it kind of is?

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