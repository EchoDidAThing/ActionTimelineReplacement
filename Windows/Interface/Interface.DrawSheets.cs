using ActionTimelineReplacement.Base.Global;
using ActionTimelineReplacement.Sheets;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Windowing;
using FFXIVClientStructs.FFXIV.Client.LayoutEngine.Node;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Xml.Linq;
#pragma warning disable CA1416 // Validate platform compatibility

namespace ActionTimelineReplacement.Interface;

public sealed partial class ConfigWindow : Window
{
    private Dictionary<string, List<string>> _allHeaders = [];

    //TOSETUP: Add new search here
    private string _searchAction = string.Empty;
    private string _searchActionCastVfx = string.Empty;
    private string _searchActionTransients = string.Empty;
    private string _searchCompanionTransients = string.Empty;
    private string _searchJobTransients = string.Empty;
    private string _searchMount = string.Empty;
    private string _searchMountCustomize = string.Empty;
    private string _searchMountDetour = string.Empty;
    private string _searchMountTransients = string.Empty;
    private string _searchOrnamentTransients = string.Empty;
    private string _searchTiltParam = string.Empty;
    private string _searchStatus = string.Empty;
    private string _searchStatusLoopVfx = string.Empty;
    private string _searchStatusHitEffect = string.Empty;
    private string _searchStatusTransients = string.Empty;
    private string _searchTraitTransients = string.Empty;
    private string _searchGlasses = string.Empty;
    private string _searchGlassesStyle = string.Empty;
    //private string _searchPlaceName = string.Empty;
    private string _searchActionTimeline = string.Empty;
    private string _searchOrnament = string.Empty;
    private string _searchOrnamentCustomize = string.Empty;
    private string _searchVfx = string.Empty;
    //private string _searchPointMenuChoice = string.Empty;
    private string activesheet = "NONE";

    private void DrawSheets()
    {
        if (!_allHeaders.ContainsKey("Action"))
        {
            //TOSETUP: Add new headers here
            _allHeaders.Add("Action", []);
            _allHeaders.Add("ActionCastVFX", []);
            _allHeaders.Add("ActionTimeline", []);
            _allHeaders.Add("ActionTransients", []);
            _allHeaders.Add("CompanionTransients", []);
            _allHeaders.Add("JobTransients", []);
            _allHeaders.Add("Mount", []);
            _allHeaders.Add("MountCustomize", []);
            _allHeaders.Add("MountDetour", []);
            _allHeaders.Add("MountTransients", []);
            _allHeaders.Add("OrnamentTransients", []);
            _allHeaders.Add("Status", []);
            _allHeaders.Add("StatusLoopVFX", []);
            _allHeaders.Add("StatusHitEffect", []);
            _allHeaders.Add("StatusTransients", []);
            _allHeaders.Add("TiltParam", []);
            _allHeaders.Add("TraitTransients", []);
            _allHeaders.Add("Glasses", []);
            _allHeaders.Add("GlassesStyle", []);
            _allHeaders.Add("Ornament", []);
            _allHeaders.Add("OrnamentCustomize", []);
            _allHeaders.Add("VFX", []);
            //_AllHeaders.Add("PointMenuChoice", []);

        }

        using var child = ImRaii.Child("Sheets", new Vector2(-1f, -1f), false);
        if (!child) return;
        if (_activeSet is null) return;
        

        switch (activesheet)
        {
            //TOSETUP: Add new case here to call the subsheet
            case "Action":
                ActionMain.Draw(activesheet, ref _activeSet, ref _searchAction);
                break;
            case "ActionCastVFX":
                ActionCastVFXMain.Draw(activesheet, ref _activeSet, ref _searchActionCastVfx);
                break;
            case "ActionTimeline":
                ActionTimelineMain.Draw(activesheet, ref _activeSet, ref _searchActionTimeline);
                break;
            case "ActionTransients":
                ActionTransientsMain.Draw(activesheet, ref _activeSet, ref _searchActionTransients);
                break;
            case "CompanionTransients":
                CompanionTransientsMain.Draw(activesheet, ref _activeSet, ref _searchCompanionTransients);
                break;
            case "JobTransients":
               JobTransientsMain.Draw(activesheet, ref _activeSet, ref _searchJobTransients);
                break;
            case "Mount":
                MountMain.Draw(activesheet, ref _activeSet, ref _searchMount);
                break;
            case "MountCustomize":
                MountCustomizeMain.Draw(activesheet, ref _activeSet, ref _searchMountCustomize);
                break;
            case "MountDetour":
                MountDetourMain.Draw(activesheet, ref _activeSet, ref _searchMountDetour);
                break;
            case "MountTransients":
                MountTransientsMain.Draw(activesheet, ref _activeSet, ref _searchMountTransients);
                break;
            case "OrnamentTransients":
                OrnamentTransientsMain.Draw(activesheet, ref _activeSet, ref _searchOrnamentTransients);
                break;
            case "Status":
                StatusMain.Draw(activesheet, ref _activeSet, ref _searchStatus);
                break;
            case "StatusLoopVFX":
                StatusLoopVFXMain.Draw(activesheet, ref _activeSet, ref _searchStatusLoopVfx);
                break;
            case "StatusHitEffect":
                StatusHitEffectMain.Draw(activesheet, ref _activeSet, ref _searchStatusHitEffect);
                break;
            case "StatusTransients":
                StatusTransientsMain.Draw(activesheet, ref _activeSet, ref _searchStatusTransients);
                break;
            case "TiltParam":
                TiltParamMain.Draw(activesheet, ref _activeSet, ref _searchTiltParam);
                break;
            case "TraitTransients":
                TraitTransientsMain.Draw(activesheet, ref _activeSet, ref _searchTraitTransients);
                break;
            case "Glasses":
                GlassesMain.Draw(activesheet, ref _activeSet, ref _searchGlasses);
                break;
            case "GlassesStyle":
                GlassesStyleMain.Draw(activesheet, ref _activeSet, ref _searchGlassesStyle);
                break;
            /*case "PlaceName":
                PlaceNameMain.Draw(activesheet, ref _activeSet, ref _searchPlaceName);
                break;*/
            case "Ornament":
                OrnamentMain.Draw(activesheet, ref _activeSet, ref _searchOrnament);
                break;
            case "OrnamentCustomize":
                OrnamentCustomizeMain.Draw(activesheet, ref _activeSet, ref _searchOrnamentCustomize);
                break;
            case "VFX":
                VfxMain.Draw(activesheet, ref _activeSet, ref _searchVfx);
                break;
            case "NONE":
                break;
            default:
                Service.Log.Error(activesheet + " is not defined in DrawSheets");
                break;
                //case "OrnamentCustomizeGroup":
                //    OrnamentCustomizeGroupMain.Draw(activesheet, ref _activeSet, ref _searchOrnamentCustomizeGroup);
                //    break;
                //case "PointMenuChoice":
                //    PointMenuChoiceMain.Draw(activesheet, ref _activeSet, ref _searchPointMenuChoice);
                //    break;
        }
    }
}