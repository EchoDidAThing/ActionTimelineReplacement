using System;
using System.Collections.Generic;
using System.IO;
using Dalamud.Configuration;
using FFXIVClientStructs;
using FFXIVClientStructs.FFXIV.Client.Graphics;
using Newtonsoft.Json;
#pragma warning disable CA1416 // Validate platform compatibility

namespace ActionTimelineReplacement.Sheets;

[Serializable]
public class Configuration : IPluginConfiguration
{
    public class JobArray (bool gla = false, bool mrd = false, bool cnj = false, bool pgl = false, bool lnc = false,
        bool rog = false, bool arc = false, bool thm = false, bool acn = false, bool pld = false, bool war = false,
        bool drk = false, bool gnb = false, bool whm = false, bool sch = false, bool ast = false, bool sge = false,
        bool mnk = false, bool drg = false, bool nin = false, bool sam = false, bool rpr = false, bool vpr = false,
        bool bst = false, bool brd = false, bool mch = false, bool dnc = false, bool blm = false, bool smn = false,
        bool rdm = false, bool pct = false, bool blu = false, bool crp = false, bool bsm = false, bool arm = false,
        bool gsm = false, bool ltw = false, bool wvr = false, bool alc = false, bool cul = false, bool min = false,
        bool btn = false, bool fsh = false)
    {
        public bool GLA = gla;
        public bool MRD = mrd;

        public bool CNJ = cnj;

        public bool PGL = pgl;
        public bool LNC = lnc;
        public bool ROG = rog;

        public bool ARC = arc;

        public bool THM = thm;
        public bool ACN = acn;


        public bool PLD = pld;
        public bool WAR = war;
        public bool DRK = drk;
        public bool GNB = gnb;

        public bool WHM = whm;
        public bool SCH = sch;
        public bool AST = ast;
        public bool SGE = sge;


        public bool MNK = mnk;
        public bool DRG = drg;
        public bool NIN = nin;
        public bool SAM = sam;
        public bool RPR = rpr;
        public bool VPR = vpr;
        public bool BST = bst;

        public bool BRD = brd;
        public bool MCH = mch;
        public bool DNC = dnc;

        public bool BLM = blm;
        public bool SMN = smn;
        public bool RDM = rdm;
        public bool PCT = pct;
        public bool BLU = blu;


        public bool CRP = crp;
        public bool BSM = bsm;
        public bool ARM = arm;
        public bool GSM = gsm;
        public bool LTW = ltw;
        public bool WVR = wvr;
        public bool ALC = alc;
        public bool CUL = cul;

        public bool MIN = min;
        public bool BTN = btn;
        public bool FSH = fsh;

        public bool CheckJob(string job)
        {
            switch (job)
            {
                case "GLA": return this.GLA;
                case "MRD": return this.MRD;
                case "CNJ": return this.CNJ;
                case "PGL": return this.PGL;
                case "LNC": return this.LNC;
                case "ROG": return this.ROG;
                case "ARC": return this.ARC;
                case "THM": return this.THM;
                case "ACN": return this.ACN;
                case "PLD": return this.PLD;
                case "WAR": return this.WAR;
                case "DRK": return this.DRK;
                case "GNB": return this.GNB;
                case "WHM": return this.WHM;
                case "SCH": return this.SCH;
                case "AST": return this.AST;
                case "SGE": return this.SGE;
                case "MNK": return this.MNK;
                case "DRG": return this.DRG;
                case "NIN": return this.NIN;
                case "SAM": return this.SAM;
                case "RPR": return this.RPR;
                case "VPR": return this.VPR;
                case "BST": return this.BST;
                case "BRD": return this.BRD;
                case "MCH": return this.MCH;
                case "DNC": return this.DNC;
                case "BLM": return this.BLM;
                case "SMN": return this.SMN;
                case "RDM": return this.RDM;
                case "PCT": return this.PCT;
                case "BLU": return this.BLU;
                case "CRP": return this.CRP;
                case "BSM": return this.BSM;
                case "ARM": return this.ARM;
                case "GSM": return this.GSM;
                case "LTW": return this.LTW;
                case "WVR": return this.WVR;
                case "ALC": return this.ALC;
                case "CUL": return this.CUL;
                case "MIN": return this.MIN;
                case "BTN": return this.BTN;
                case "FSH": return this.FSH;
                default:
                    {
                        Service.Log.Info(job + " does not exist in checkjob switch");
                        return false;
                    }
            }
        }
    }
    //TOSETUP: add new dictionary here
    public class ReplacementSet(string name, bool enabled, int priority,
    JobArray jobarray,
    string charactername,
    uint homeworld,
    Dictionary<uint, ActionConfig> actionwriter,
    Dictionary<uint, ActionCastVFXConfig> actioncastvfxwriter,
    Dictionary<uint, ActionTransientsConfig> actiontransientswriter,
    Dictionary<uint, ActionTimelineConfig> actiontimelinewriter,
    Dictionary<uint, CompanionTransientsConfig> companiontransientswriter,
    Dictionary<uint, JobTransientsConfig> jobtransientswriter,
    Dictionary<uint, MountConfig> mountwriter,
    Dictionary<uint, MountCustomizeConfig> mountcustomizewriter,
    Dictionary<uint, MountDetourConfig> mountdetourwriter,
    Dictionary<uint, MountTransientsConfig> mounttransientswriter,
    Dictionary<uint, OrnamentTransientsConfig> ornamenttransientswriter,
    Dictionary<uint, StatusConfig> statuswriter,
    Dictionary<uint, StatusLoopVFXConfig> statusloopvfxwriter,
    Dictionary<uint, StatusHitEffectConfig> statushiteffectwriter,
    Dictionary<uint, StatusTransientsConfig> statustransientswriter,
    Dictionary<uint, TiltParamConfig> tiltparamwriter,
    Dictionary<uint, TraitTransientsConfig> traittransientswriter,
    Dictionary<uint, GlassesConfig> glasseswriter,
    Dictionary<uint, GlassesStyleConfig> glassesstylewriter,
    //Dictionary<uint, PlaceNameConfig> placenamewriter,
    Dictionary<uint, OrnamentConfig> ornamentwriter,
    Dictionary<uint, OrnamentCustomizeConfig> ornamentcustomizewriter,
    Dictionary<uint, VfxConfig> vfxwriter)
    //Dictionary<uint, OrnamentCustomizeGroupConfig> ornamentcustomizegroupwriter)
    {
        public string Name = name;

        public JobArray Jobs = jobarray;

        public bool Enabled = enabled;

        public int Priority = priority;

        public string CharacterName = charactername;

        public uint HomeWorld = homeworld;

        //TOSETUP: add new dictionary here
        public Dictionary<uint, ActionConfig> ActionWriter { get; set; } = actionwriter;
        public Dictionary<uint, ActionCastVFXConfig> ActionCastVFXWriter { get; set; } = actioncastvfxwriter;
        public Dictionary<uint, ActionTimelineConfig> ActionTimelineWriter { get; set; } = actiontimelinewriter;
        public Dictionary<uint, ActionTransientsConfig> ActionTransientsWriter { get; set; } = actiontransientswriter;
        public Dictionary<uint, CompanionTransientsConfig> CompanionTransientsWriter { get; set; } = companiontransientswriter;
        public Dictionary<uint, JobTransientsConfig> JobTransientsWriter { get; set; } = jobtransientswriter;
        public Dictionary<uint, MountConfig> MountWriter { get; set; } = mountwriter;
        public Dictionary<uint, MountCustomizeConfig> MountCustomizeWriter { get; set; } = mountcustomizewriter;
        public Dictionary<uint, MountDetourConfig> MountDetourWriter { get; set; } = mountdetourwriter;
        public Dictionary<uint, MountTransientsConfig> MountTransientsWriter { get; set; } = mounttransientswriter;
        public Dictionary<uint, OrnamentTransientsConfig> OrnamentTransientsWriter { get; set; } = ornamenttransientswriter;
        public Dictionary<uint, TiltParamConfig> TiltParamWriter { get; set; } = tiltparamwriter;
        public Dictionary<uint, TraitTransientsConfig> TraitTransientsWriter { get; set; } = traittransientswriter;
        public Dictionary<uint, StatusConfig> StatusWriter { get; set; } = statuswriter;
        public Dictionary<uint, StatusLoopVFXConfig> StatusLoopVFXWriter { get; set; } = statusloopvfxwriter;
        public Dictionary<uint, StatusHitEffectConfig> StatusHitEffectWriter { get; set; } = statushiteffectwriter;
        public Dictionary<uint, StatusTransientsConfig> StatusTransientsWriter { get; set; } = statustransientswriter;
        public Dictionary<uint, GlassesConfig> GlassesWriter { get; set; } = glasseswriter;
        public Dictionary<uint, GlassesStyleConfig> GlassesStyleWriter { get; set; } = glassesstylewriter;
        //public Dictionary<uint, PlaceNameConfig> PlaceNameWriter { get; set; } = placenamewriter;
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

                replacements.CharacterName = Service.PlayerState.CharacterName;
                replacements.HomeWorld = Service.PlayerState.HomeWorld.RowId;

                return replacements;
            }
            catch
            {
                return null;
            }
        }

        public static bool Save(string jsonFile, int index)
        {
            //scrub out player name and homeworld to prevent leakage
            ReplacementSet replacements = Service.Config.ReplacementSets[index];
            replacements.CharacterName = "";
            replacements.HomeWorld = 0;
            try
            {
                File.WriteAllText(jsonFile, JsonConvert.SerializeObject(replacements));
                return true;
            }
            catch
            {
                Service.Log.Error("savefailed for set" + index);
                return false;
            }
        }
    }

    public int Version { get; set; } = 1;

    public bool EnableReplacement = true;

    //public bool AdvancedMode = false;

    public List<ReplacementSet> ReplacementSets { get; set; } = [];

    /*internal void Load()
    {
        Service.Config =
                    JsonConvert.DeserializeObject<Configuration>(
                        File.ReadAllText(Service.PluginInterface.ConfigFile.FullName));
    }*/
    internal void Save()
    {
        //File.WriteAllText(Service.PluginInterface.ConfigFile.FullName, JsonConvert.SerializeObject(Service.Config));

        Service.PluginInterface.SavePluginConfig(this);
    }
}