using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using ActionTimelineReplacement.Configurations;
using ActionTimelineReplacement.Hookers;
using ActionTimelineReplacement.Windows.SubSheets;
using Dalamud.Interface.Colors;
using Dalamud.Interface.GameFonts;
using Dalamud.Interface.ImGuiFileDialog;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace ActionTimelineReplacement.Windows;

public sealed class ConfigWindow : Window
{

    private readonly FileDialogManager _dialogManager = new()
    {
        AddedWindowFlags = ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoDocking,
    };

    private static float Scale => ImGuiHelpers.GlobalScale;

    private Configuration.ReplacementSet? _activeSet;

    public ConfigWindow()
        : base("ActionTimeline Replacement v" + typeof(ConfigWindow).Assembly.GetName().Version)
    {
        SizeCondition = ImGuiCond.FirstUseEver;
        Size = new Vector2(960, 540);
        SizeConstraints = new WindowSizeConstraints()
        {
            MinimumSize = new Vector2(480, 360),
            MaximumSize = new Vector2(5000, 5000),
        };
    }

    public override void Draw()
    {
        DrawHeader();

        using var table = ImRaii.Table("Main Table", 2, ImGuiTableFlags.Resizable);
        if (!table) return;

        ImGui.TableSetupColumn("Rotation Config Side Bar", ImGuiTableColumnFlags.WidthFixed, 100 * Scale);
        ImGui.TableNextColumn();

        try
        {
            using var style = ImRaii.PushStyle(ImGuiStyleVar.SelectableTextAlign, new Vector2(0.5f, 0.5f));
            DrawSideBar();
        }
        catch (Exception ex)
        {
            Service.Log.Warning(ex, "Something wrong with sideBar");
        }

        ImGui.TableNextColumn();

        try
        {
            DrawBodyMain();
        }
        catch (Exception ex)
        {
            Service.Log.Warning(ex, "Something wrong with bodymain");
        }

        try
        {
            DrawBodySheets();
        }
        catch (Exception ex)
        {
            Service.Log.Warning(ex, "Something wrong with bodySheets");
        }

        _dialogManager.Draw();
    }

    private void DrawSideBar()
    {
        using var windowPadding = ImRaii.PushStyle(ImGuiStyleVar.WindowPadding, Vector2.Zero);

        using var child = ImRaii.Child("Side bar", -Vector2.One, true);
        if (!child) return;

        var itemHeight = ImGui.CalcTextSize("C").Y + ImGui.GetStyle().FramePadding.Y * 2 +
                         ImGui.GetStyle().WindowPadding.Y;

        using (ImRaii.Child("Items",
                   new Vector2(-1,
                       ImGui.GetWindowSize().Y - ImGui.GetCursorPosY() - itemHeight - ImGui.GetStyle().WindowPadding.Y),
                   false))
        {
            var span = CollectionsMarshal.AsSpan(Service.Config.ReplacementSets);
            for (var i = 0; i < span.Length; i++)
            {
                var set = span[i];
                // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
                if (set is null) continue;
                using var style = !Service.Config.ReplacementSets[i].Enabled ? ImRaii.PushColor(ImGuiCol.Text, ImGuiColors.DalamudGrey) : null;
                if (ImGui.Selectable($"{Service.Config.ReplacementSets[i].Name}##{i}", _activeSet == Service.Config.ReplacementSets[i]))
                {
                    _activeSet = Service.Config.ReplacementSets[i];
                }

                var popUpId = $"Set{i}PopUp";
                if (ImGui.IsItemClicked(ImGuiMouseButton.Right))
                {
                    ImGui.OpenPopup(popUpId);
                }

                using var popup = ImRaii.Popup(popUpId);
                if (popup)
                {
                    using var popUpChild = ImRaii.Child("PopUp" + popUpId,
                        new Vector2(80 * Scale, 60 * Scale),
                        true);

                    if (ImGui.Selectable("Export"))
                    {
                        _dialogManager.SaveFileDialog("Save", ".json", Service.Config.ReplacementSets[i].Name, ".json", (b, file) =>
                        {
                            if (!b) return;
                            //FIXME
                            //set.Save(file);
                        });
                    }

                    if (ImGui.Selectable("Delete"))
                    {
                        if (_activeSet == Service.Config.ReplacementSets[i]) _activeSet = null;
                        Service.Config.ReplacementSets.Remove(set);
                        Service.Config.Save();
                        Methods.SetupAll();
                        ImGui.CloseCurrentPopup();
                    }
                }
            }
        }

        ImGui.SetCursorPosY(ImGui.GetWindowSize().Y - itemHeight);

        using (ImRaii.PushStyle(ImGuiStyleVar.ItemSpacing, new Vector2(0, 0)))
        {
            using var corner = ImRaii.PushStyle(ImGuiStyleVar.FrameRounding, 0);
            var width = Math.Max(ImGui.GetWindowWidth() / 2, 60 * Scale);
            var buttonSize = new Vector2(width, 0);

            ImGui.PushItemWidth(width);
            if (ImGui.Button("Create", buttonSize))
            {
                var randomset = new Random().Next(1, 100).ToString();
                Service.Config.ReplacementSets.Add(new Configuration.ReplacementSet("New Set" + randomset, true, 0, [], [], [], [], [], []));
                Service.Config.Save();
            }

            ImGui.SameLine();
            if (ImGui.Button("Import", buttonSize))
            {
                _dialogManager.OpenFileDialog("Import", ".json", (b, files) =>
                {
                    if (!b) return;
                    foreach (var file in files)
                    {
                        if (Configuration.ReplacementSet.Load(file) is not { } set) continue;
                        Service.Config.ReplacementSets.Add(set);
                        Methods.SetupAll();
                    }

                    Service.Config.Save();
                }, 10, ".");
            }
            ImGui.PopItemWidth();
        }
    }

    

    private void DrawBodyMain()
    {
        using var child = ImRaii.Child("BodyMain", new Vector2(-1f,60f), false);
        if (!child) return;
        if (_activeSet is null) return;

        using (ImRaii.PushFont(GetFont(20)))
        {
            var width = ImGui.CalcTextSize(_activeSet.Name).X + ImGui.GetStyle().FramePadding.X * 2;
            var x = ImGui.GetWindowWidth() / 2 - width / 2;
            ImGui.SetCursorPosX(x);

            ImGui.SetNextItemWidth(width);
            if (ImGui.InputText("##Name", ref _activeSet.Name, 256))
            {
                Service.Config.Save();
            }
        }

        if (ImGui.Checkbox("Enable", ref _activeSet.Enabled))
        {
            Methods.SetupAll();
            Service.Config.Save();
        }

        ImGui.SameLine();
        ImGui.SetNextItemWidth(50 * Scale);
        if (ImGui.DragInt("Priority", ref _activeSet.Priority))
        {
            Methods.SetupAll();
            Service.Config.Save();
        }
    }

    private Dictionary<string, List<string>> _AllHeaders = [];
    private Dictionary<string, float> _AllItemWidths = [];

    //TOSETUP: Add new search here
    private string _searchAction = string.Empty;
    private string _searchMount = string.Empty;
    private string _searchTiltParam = string.Empty;
    private string _searchPlaceName = string.Empty;
    private string _searchPetMirage = string.Empty;


    private void DrawBodySheets()
    {
        if (!_AllItemWidths.ContainsKey("Action"))
        {
            //TOSETUP: Add new headers here
            _AllHeaders.Add("Action", ["Cast Vfx", "Start timeline", "End timeline", "Hit timeline"]);
            _AllHeaders.Add("Mount", ["RideBGM", "TiltGround", "TiltFlySwim", "TiltParam3", "TiltParam4", "FlyUpDownTilt", "Unk2", "Unk3", "Unk4", "MountCustomize", "Unk5", "SwimAnimSpeed"]);
            _AllHeaders.Add("TiltParam", ["TiltRate", "RotOriginOffset", "MaxAngle", "Unknown3", "Unknown4", "RotReverse"]);
            _AllHeaders.Add("PlaceName", ["Unk1", "Unk2", "Unk3", "Unk4", "Unk5", "Unk6", "Unk7", "Unk8"]);
            _AllHeaders.Add("PetMirage", ["Unk0", "Unk1", "Unk2", "Unk3", "Unk4", "Unk5", "Unk6", "Unk7", "Scale"]);
            foreach (var headerkey in _AllHeaders.Keys)
            {
                _AllItemWidths.Add(headerkey, 0f);
            }

        }
        using var child = ImRaii.Child("BodySheets", new Vector2(-1f, -1f), false);
        if (!child) return;
        if (_activeSet is null) return;

        ImGui.SetCursorPosY(ImGui.GetCursorPosY() + 10 * Scale);
        foreach (var mainkey in _AllHeaders.Keys)
        {
            if (ImGui.CollapsingHeader(mainkey))
            {
                using (ImRaii.PushFont(GetFont(18)))
                {
                    if (Service.Config.AdvancedMode)
                    {
                        if (_AllItemWidths[mainkey] == 0)
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
                        else
                        {
                            for (int i = 0; i < _AllHeaders[mainkey].Count; i++)
                            {
                                ImGui.SameLine();
                                ImGui.SetCursorPosX(ImGui.GetWindowWidth() - _AllItemWidths[mainkey] * (_AllHeaders[mainkey].Count - i) / _AllHeaders[mainkey].Count);
                                ImGui.Text(_AllHeaders[mainkey][i]);
                            }
                        }
                    }
                }
                switch (mainkey)
                {
                    //TOSETUP: Add new case here to call the subsheet
                    case "Action":
                        ActionSubSheet.Draw(mainkey, ref _activeSet, ref _AllItemWidths, ref _searchAction);
                        break;
                    case "Mount":
                        MountSubSheet.Draw(mainkey, ref _activeSet, ref _AllItemWidths, ref _searchMount);
                        break;
                    case "TiltParam":
                        TiltSubSheet.Draw(mainkey, ref _activeSet, ref _AllItemWidths, ref _searchTiltParam);
                        break;
                    case "PlaceName":
                        PlaceNameSubSheet.Draw(mainkey, ref _activeSet, ref _AllItemWidths, ref _searchPlaceName);
                        break;
                    case "PetMirage":
                        PetMirageSubSheet.Draw(mainkey, ref _activeSet, ref _AllItemWidths, ref _searchPetMirage);
                        break;
                }

            }
        }

    }

    public static int ScoreString(string s1, string search)
    {
        if (s1.Contains(search, StringComparison.OrdinalIgnoreCase))
        {
            return s1.Length - search.Length;
        }

        return LevenshteinDistance(s1, search) + 20;
    }

    public static int LevenshteinDistance(string s1, string s2)
    {
        var len1 = s1.Length;
        var len2 = s2.Length;
        var dp = new int[len1 + 1, len2 + 1];

        for (var i = 0; i <= len1; i++)
            dp[i, 0] = i;
        for (var j = 0; j <= len2; j++)
            dp[0, j] = j;

        for (var i = 1; i <= len1; i++)
        {
            for (var j = 1; j <= len2; j++)
            {
                var cost = (s1[i - 1] == s2[j - 1]) ? 0 : 1;
                dp[i, j] = Math.Min(Math.Min(
                        dp[i - 1, j] + 1, // Deletion
                        dp[i, j - 1] + 1), // Insertion
                    dp[i - 1, j - 1] + cost); // Substitution
            }
        }

        return dp[len1, len2];
    }

    private static void DrawHeader()
    {
        if (ImGui.Checkbox("Enable", ref Service.Config.EnableReplacement))
        {
            //SETUP ALL PROCESSING
            Methods.SetupAll();
            Service.Config.Save();
        }

        ImGui.SameLine();

        if (ImGui.Button("Redraw"))
        {
            //SETUP ALL PROCESSING
            Methods.SetupAll();
        }

        ImGui.SameLine();
        if (ImGui.Button(Service.Config.AdvancedMode ? "To Simple Mode" : "To Advanced Mode"))
        {
            Service.Config.AdvancedMode = !Service.Config.AdvancedMode;
            Service.Config.Save();
        }
    }

    private static unsafe ImFontPtr GetFont(float size, GameFontFamily fontFamily = GameFontFamily.Axis)
    {
        var style = new GameFontStyle(GameFontStyle.GetRecommendedFamilyAndSize(fontFamily, size));

        var handle = Service.PluginInterface.UiBuilder.FontAtlas.NewGameFontHandle(style);

        try
        {
            var font = handle.Lock().ImFont;
            if ((IntPtr)font.NativePtr == IntPtr.Zero)
            {
                return ImGui.GetFont();
            }

            font.Scale = size / font.FontSize;
            return font;
        }
        catch
        {
            return ImGui.GetFont();
        }
    }
}