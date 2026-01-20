using System.Numerics;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Windowing;
using ActionTimelineReplacement.Base.Global;
using Dalamud.Bindings.ImGui;
using System.Collections.Generic;
using ActionTimelineReplacement.Sheets;
#pragma warning disable CA1416 // Validate platform compatibility

namespace ActionTimelineReplacement.Windows;

public sealed partial class ConfigWindow : Window
{
    private Dictionary<string, List<string>> _AllHeaders = [];
    private Dictionary<string, float> _AllItemWidths = [];

    //TOSETUP: Add new search here
    private string _searchAction = string.Empty;
    private string _searchMount = string.Empty;
    private string _searchMountCustomize = string.Empty;
    private string _searchTiltParam = string.Empty;
    private string _searchStatus = string.Empty;
    private string _searchGlasses = string.Empty;
    private string _searchGlassesStyle = string.Empty;
    //private string _searchPlaceName = string.Empty;
    private string _searchActionTimeline = string.Empty;
    private string _searchOrnament = string.Empty;
    private string _searchOrnamentCustomize = string.Empty;
    //private string _searchPointMenuChoice = string.Empty;

    private void DrawSheets()
    {
        if (!_AllItemWidths.ContainsKey("Action"))
        {
            //TOSETUP: Add new headers here
            _AllHeaders.Add("Action", []);
            _AllHeaders.Add("Mount", []);
            _AllHeaders.Add("MountCustomize", []);
            _AllHeaders.Add("Status", []);
            _AllHeaders.Add("TiltParam", []);
            _AllHeaders.Add("Glasses", []);
            _AllHeaders.Add("GlassesStyle", []);
            _AllHeaders.Add("PlaceName", []);
            _AllHeaders.Add("ActionTimeline", []);
            _AllHeaders.Add("Ornament", []);
            _AllHeaders.Add("OrnamentCustomize", []);
            //_AllHeaders.Add("PointMenuChoice", []);
            foreach (var headerkey in _AllHeaders.Keys)
            {
                _AllItemWidths.Add(headerkey, 0f);
            }

        }

        using var child = ImRaii.Child("Sheets", new Vector2(-1f, -1f), false);
        if (!child) return;
        if (_activeSet is null) return;

        ImGui.SetCursorPosY(ImGui.GetCursorPosY() + 10 * CalcGlobals.GlobalScale());

        foreach (var mainkey in _AllHeaders.Keys)
        {
            if (ImGui.CollapsingHeader(mainkey))
            {
                using (ImRaii.PushFont(GetFont(18)))
                {
                    if (_AllItemWidths[mainkey] != 0)
                    {
                        for (int i = 0; i < _AllHeaders[mainkey].Count; i++)
                        {
                            ImGui.SameLine();
                            ImGui.SetCursorPosX(ImGui.GetWindowWidth() - _AllItemWidths[mainkey] * (_AllHeaders[mainkey].Count - i) / _AllHeaders[mainkey].Count);
                            ImGui.Text(_AllHeaders[mainkey][i]);
                        }
                    }
                    else
                    {
                        ImGui.SameLine();
                        var headerstring = " ";
                        for (int i = 0; i < _AllHeaders[mainkey].Count; i++)
                        {
                            if (i > 0) headerstring += " ";
                            headerstring += _AllHeaders[mainkey][i];
                        }
                        ImGui.Text(headerstring);
                    }
                }
                switch (mainkey)
                {
                    //TOSETUP: Add new case here to call the subsheet
                    case "Action":
                        ActionMain.Draw(mainkey, ref _activeSet, ref _searchAction);
                        break;
                    case "Mount":
                        MountMain.Draw(mainkey, ref _activeSet, ref _searchMount);
                        break;
                    case "MountCustomize":
                        MountCustomizeMain.Draw(mainkey, ref _activeSet, ref _searchMountCustomize);
                        break;
                    case "Status":
                        StatusMain.Draw(mainkey, ref _activeSet, ref _searchStatus);
                        break;
                    case "TiltParam":
                        TiltParamMain.Draw(mainkey, ref _activeSet, ref _searchTiltParam);
                        break;
                    case "Glasses":
                        GlassesMain.Draw(mainkey, ref _activeSet, ref _searchGlasses);
                        break;
                    case "GlassesStyle":
                        GlassesStyleMain.Draw(mainkey, ref _activeSet, ref _searchGlassesStyle);
                        break;
                    /*case "PlaceName":
                        PlaceNameMain.Draw(mainkey, ref _activeSet, ref _searchPlaceName);
                        break;*/
                    case "ActionTimeline":
                        ActionTimelineMain.Draw(mainkey, ref _activeSet, ref _searchActionTimeline);
                        break;
                    case "Ornament":
                        OrnamentMain.Draw(mainkey, ref _activeSet, ref _searchOrnament);
                        break;
                    case "OrnamentCustomize":
                        OrnamentCustomizeMain.Draw(mainkey, ref _activeSet, ref _searchOrnamentCustomize);
                        break;
                        //case "OrnamentCustomizeGroup":
                        //    OrnamentCustomizeGroupMain.Draw(mainkey, ref _activeSet, ref _searchOrnamentCustomizeGroup);
                        //    break;
                        //case "PointMenuChoice":
                        //    PointMenuChoiceMain.Draw(mainkey, ref _activeSet, ref _searchPointMenuChoice);
                        //    break;
                }

            }
        }
    }
}