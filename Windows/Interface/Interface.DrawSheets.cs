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
    private Dictionary<string, List<string>> _AllHeaders = [];
    private Dictionary<string, float> _AllItemWidths = [];

    //TOSETUP: Add new search here
    private string _searchAction = string.Empty;
    private string _searchActionCastVFX = string.Empty;
    private string _searchActionTransients = string.Empty;
    private string _searchMount = string.Empty;
    private string _searchMountCustomize = string.Empty;
    private string _searchTiltParam = string.Empty;
    private string _searchStatus = string.Empty;
    private string _searchStatusLoopVFX = string.Empty;
    private string _searchStatusHitEffect = string.Empty;
    private string _searchGlasses = string.Empty;
    private string _searchGlassesStyle = string.Empty;
    //private string _searchPlaceName = string.Empty;
    private string _searchActionTimeline = string.Empty;
    private string _searchOrnament = string.Empty;
    private string _searchOrnamentCustomize = string.Empty;
    private string _searchVfx = string.Empty;
    //private string _searchPointMenuChoice = string.Empty;
    private string activesheet;

    private void DrawSheets()
    {
        if (!_AllItemWidths.ContainsKey("Action"))
        {
            //TOSETUP: Add new headers here
            _AllHeaders.Add("Action", []);
            _AllHeaders.Add("ActionCastVFX", []);
            _AllHeaders.Add("ActionTimeline", []);
            _AllHeaders.Add("ActionTransients", []);
            _AllHeaders.Add("Mount", []);
            _AllHeaders.Add("MountCustomize", []);
            _AllHeaders.Add("Status", []);
            _AllHeaders.Add("StatusLoopVFX", []);
            _AllHeaders.Add("StatusHitEffect", []);
            _AllHeaders.Add("TiltParam", []);
            _AllHeaders.Add("Glasses", []);
            _AllHeaders.Add("GlassesStyle", []);
            _AllHeaders.Add("Ornament", []);
            _AllHeaders.Add("OrnamentCustomize", []);
            _AllHeaders.Add("VFX", []);
            //_AllHeaders.Add("PointMenuChoice", []);
            foreach (var headerkey in _AllHeaders.Keys)
            {
                _AllItemWidths.Add(headerkey, 0f);
            }

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
                ActionCastVFXMain.Draw(activesheet, ref _activeSet, ref _searchActionCastVFX);
                break;
            case "ActionTimeline":
                ActionTimelineMain.Draw(activesheet, ref _activeSet, ref _searchActionTimeline);
                break;
            case "ActionTransients":
                ActionTransientsMain.Draw(activesheet, ref _activeSet, ref _searchActionTransients);
                break;
            case "Mount":
                MountMain.Draw(activesheet, ref _activeSet, ref _searchMount);
                break;
            case "MountCustomize":
                MountCustomizeMain.Draw(activesheet, ref _activeSet, ref _searchMountCustomize);
                break;
            case "Status":
                StatusMain.Draw(activesheet, ref _activeSet, ref _searchStatus);
                break;
            case "StatusLoopVFX":
                StatusLoopVFXMain.Draw(activesheet, ref _activeSet, ref _searchStatusLoopVFX);
                break;
            case "StatusHitEffect":
                StatusHitEffectMain.Draw(activesheet, ref _activeSet, ref _searchStatusHitEffect);
                break;
            case "TiltParam":
                TiltParamMain.Draw(activesheet, ref _activeSet, ref _searchTiltParam);
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
                //case "OrnamentCustomizeGroup":
                //    OrnamentCustomizeGroupMain.Draw(activesheet, ref _activeSet, ref _searchOrnamentCustomizeGroup);
                //    break;
                //case "PointMenuChoice":
                //    PointMenuChoiceMain.Draw(activesheet, ref _activeSet, ref _searchPointMenuChoice);
                //    break;
        }
    }
}