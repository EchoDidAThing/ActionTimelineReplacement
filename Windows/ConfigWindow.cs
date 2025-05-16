using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using ActionTimelineReplacement.Configurations;
using ActionTimelineReplacement.Hookers;
using Dalamud.Interface;
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
        : base("ActionTimeline Replacement v" + typeof(ConfigWindow).Assembly.GetName().Version,
            ImGuiWindowFlags.NoScrollbar)
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
            DrawBody();
        }
        catch (Exception ex)
        {
            Service.Log.Warning(ex, "Something wrong with body");
        }

        _dialogManager.Draw();
    }

    private void DrawSideBar()
    {
        using var windowPadding = ImRaii.PushStyle(ImGuiStyleVar.WindowPadding, Vector2.Zero);

        using var child = ImRaii.Child("Side bar", -Vector2.One, true, ImGuiWindowFlags.NoScrollbar);
        if (!child) return;

        var itemHeight = ImGui.CalcTextSize("C").Y + ImGui.GetStyle().FramePadding.Y * 2 +
                         ImGui.GetStyle().WindowPadding.Y;

        using (ImRaii.Child("Items",
                   new Vector2(-1,
                       ImGui.GetWindowSize().Y - ImGui.GetCursorPosY() - itemHeight - ImGui.GetStyle().WindowPadding.Y),
                   false))
        {
            var span = CollectionsMarshal.AsSpan(Service.Config.ReplacementSets.Keys.ToList<string>());
            for (var i = 0; i < span.Length; i++)
            {
                var set = span[i];
                // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
                if (set is null) continue;
                using var style = !Service.Config.ReplacementSets[set].Enabled ? ImRaii.PushColor(ImGuiCol.Text, ImGuiColors.DalamudGrey) : null;
                if (ImGui.Selectable($"{Service.Config.ReplacementSets[set].Name}##{i}", _activeSet == Service.Config.ReplacementSets[set]))
                {
                    _activeSet = Service.Config.ReplacementSets[set];
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
                        true, ImGuiWindowFlags.NoScrollbar);

                    if (ImGui.Selectable("Export"))
                    {
                        _dialogManager.SaveFileDialog("Save", ".json", Service.Config.ReplacementSets[set].Name, ".json", (b, file) =>
                        {
                            if (!b) return;
                            //FIX
                            //set.Save(file);
                        });
                    }

                    if (ImGui.Selectable("Delete"))
                    {
                        if (_activeSet == Service.Config.ReplacementSets[set]) _activeSet = null;
                        Service.Config.ReplacementSets.Remove(set);
                        Service.Config.Save();
                        Methods.SetupActions(Service.Config.ReplacementSets[set].ActionReplacements.Keys);
                        Methods.SetupMounts(Service.Config.ReplacementSets[set].MountReplacements.Keys);
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
                Service.Config.ReplacementSets.Add("new Set" + randomset,new Configuration.ReplacementSet("New Set" + randomset, true, 0, new Dictionary<uint, ActionReplacementConfig>() , new Dictionary<uint, MountReplacementConfig>()));
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
                        Service.Config.ReplacementSets.Add(set.Name, set);
                        Methods.SetupActions(set.ActionReplacements.Keys);
                        Methods.SetupActions(set.MountReplacements.Keys);
                    }

                    Service.Config.Save();
                }, 10, ".");
            }
            ImGui.PopItemWidth();
        }
    }

    private float _itemWidth;

    private void DrawBody()
    {
        using var child = ImRaii.Child("Body",-Vector2.One, false, ImGuiWindowFlags.NoScrollbar);
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
            Methods.SetupActions(_activeSet.ActionReplacements.Keys);
            Methods.SetupMounts(_activeSet.ActionReplacements.Keys);
            Service.Config.Save();
        }

        ImGui.SameLine();
        ImGui.SetNextItemWidth(50 * Scale);
        if (ImGui.DragInt("Priority", ref _activeSet.Priority))
        {
            Methods.SetupActions(_activeSet.ActionReplacements.Keys);
            Methods.SetupMounts(_activeSet.ActionReplacements.Keys);
            Service.Config.Save();
        }

        ImGui.SetCursorPosY(ImGui.GetCursorPosY() + 10 * Scale);


        if (ImGui.CollapsingHeader($"Action"))
        {
            using (ImRaii.PushFont(GetFont(18)))
            {

                if (Service.Config.AdvancedMode)
                {
                    if (_itemWidth == 0)
                    {
                        ImGui.SameLine();
                        ImGui.Text(" Cast Vfx Start timeline End timeline Hit timeline");
                    }
                    else
                    {
                        ImGui.SameLine();
                        ImGui.SetCursorPosX(ImGui.GetWindowWidth() - _itemWidth * 4 / 4);
                        ImGui.Text("Cast Vfx");

                        ImGui.SameLine();
                        ImGui.SetCursorPosX(ImGui.GetWindowWidth() - _itemWidth * 3 / 4);
                        ImGui.Text("Start timeline");

                        ImGui.SameLine();
                        ImGui.SetCursorPosX(ImGui.GetWindowWidth() - _itemWidth * 2 / 4);
                        ImGui.Text("End timeline");

                        ImGui.SameLine();
                        ImGui.SetCursorPosX(ImGui.GetWindowWidth() - _itemWidth * 1 / 4);
                        ImGui.Text("Hit timeline");
                    }
                }
            }

            using (var subList = ImRaii.Child("ActionSubList", -Vector2.One, false))
            {
                if (subList)
                {
                    foreach (var key in _activeSet.ActionReplacements.Keys)
                    {
                        var replacement = _activeSet.ActionReplacements[key].Replacement;

                        if (ImGui.Checkbox("##" + key, ref _activeSet.ActionReplacements[key].Enabled))
                        {
                            Methods.SetupAction(key);
                            Service.Config.Save();
                        }

                        ImGui.SameLine();
                        if (ImGui.Button(" - ##" + key))
                        {
                            _activeSet.ActionReplacements.Remove(key);
                        }

                        ImGui.SameLine();
                        ImGui.Text($"#{key:D5}");

                        ImGui.SameLine();
                        if (_itemWidth != 0 && Service.Config.AdvancedMode)
                        {
                            var widthRest = ImGui.GetWindowWidth() - _itemWidth - ImGui.GetCursorPosX() - 5 * Scale;
                            ImGui.PushTextWrapPos(Math.Max(widthRest, 60 * Scale) + ImGui.GetCursorPosX());
                        }
                        ImGui.TextWrapped(ActionReplacementsManager.GetName(key));
                        if (_itemWidth != 0 && Service.Config.AdvancedMode)
                        {
                            ImGui.PopTextWrapPos();
                        }

                        if (!Service.Config.AdvancedMode)
                        {
                            continue;
                        }

                        ImGui.SameLine();
                        if (_itemWidth != 0)
                        {
                            ImGui.SetCursorPosX(ImGui.GetWindowWidth() - _itemWidth);
                        }

                        var startwidth = ImGui.GetCursorPosX();
                        DrawItem("Cast", ref replacement.CastVfx, i => i.CastVfx);
                        ImGui.SameLine();

                        DrawItem("Start", ref replacement.AnimationStart, i => i.AnimationStart);

                        ImGui.SameLine();
                        DrawItem("End", ref replacement.AnimationEnd, i => i.AnimationEnd);

                        ImGui.SameLine();
                        DrawItem("Hit", ref replacement.ActionTimelineHit, i => i.ActionTimelineHit);

                        ImGui.SameLine();
                        _itemWidth = ImGui.GetCursorPosX() - startwidth;
                        ImGui.NewLine();
                        continue;

                        void DrawItem(string name, ref ushort value,
                            Func<Configurations.ActionReplacement, ushort> getDefault)
                        {
                            ImGui.SetNextItemWidth(60 * Scale);
                            int relay = value;
                            if (ImGui.DragInt("##" + name + key, ref relay))
                            {
                                value = (ushort)relay;
                                Methods.SetupAction(key);
                                Service.Config.Save();
                            }

                            ImGui.SameLine();
                            using (ImRaii.PushFont(UiBuilder.IconFont))
                            {
                                if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{name}{key}"))
                                {
                                    value = getDefault(ActionReplacementsManager.GetOriginalReplacement(key));
                                    Methods.SetupAction(key);
                                    Service.Config.Save();
                                }
                            }
                        }
                    }

                    const string searchActionsPopup = "Search actions";
                    if (ImGui.Button(" + "))
                    {
                        ImGui.OpenPopup(searchActionsPopup);
                    }

                    using var searchPopup = ImRaii.Popup(searchActionsPopup);
                    if (searchPopup)
                    {
                        var width = 200 * Scale;

                        ImGui.SetNextItemWidth(width);
                        ImGui.InputText("##Search Action", ref _searchAction, 256);

                        using var popUpChild = ImRaii.Child(searchActionsPopup, new Vector2(width, 200 * Scale), true);
                        foreach (var pair in ActionReplacementsManager.ActionNames.OrderBy(i =>
                                {
                                    if (string.IsNullOrEmpty(_searchAction)) return 0;
                                    return Math.Min(ScoreString(i.Value, _searchAction),
                                        ScoreString(i.Key.ToString(), _searchAction));
                                }))
                        {
                            if (ImGui.Selectable($"#{pair.Key:D5} {pair.Value}"))
                            {
                                var original = ActionReplacementsManager.GetOriginalReplacement(pair.Key);
                                _activeSet.ActionReplacements[pair.Key] =
                                    new ActionReplacementConfig(new Configurations.ActionReplacement(
                                            original.AnimationStart,
                                            original.AnimationEnd,
                                            original.ActionTimelineHit,
                                            original.CastVfx),
                                        false);
                                Service.Config.Save();
                            }
                        }
                    }
                }
            }
        }

        if (ImGui.CollapsingHeader($"Mount"))
        {
            using (ImRaii.PushFont(GetFont(18)))
            {

                if (Service.Config.AdvancedMode)
                {
                    if (_itemWidth == 0)
                    {
                        ImGui.SameLine();
                        ImGui.Text(" RideBGM TiltParam1 TiltParam2 TiltParam3 TiltParam4 MountCustomize");
                    }
                    else
                    {
                        ImGui.SameLine();
                        ImGui.SetCursorPosX(ImGui.GetWindowWidth() - _itemWidth * 6 / 6);
                        ImGui.Text("RideBGM");

                        ImGui.SameLine();
                        ImGui.SetCursorPosX(ImGui.GetWindowWidth() - _itemWidth * 5 / 6);
                        ImGui.Text("TiltParam1");

                        ImGui.SameLine();
                        ImGui.SetCursorPosX(ImGui.GetWindowWidth() - _itemWidth * 4 / 6);
                        ImGui.Text("TiltParam2");

                        ImGui.SameLine();
                        ImGui.SetCursorPosX(ImGui.GetWindowWidth() - _itemWidth * 3 / 6);
                        ImGui.Text("TiltParam3");

                        ImGui.SameLine();
                        ImGui.SetCursorPosX(ImGui.GetWindowWidth() - _itemWidth * 2 / 6);
                        ImGui.Text("TiltParam3");

                        ImGui.SameLine();
                        ImGui.SetCursorPosX(ImGui.GetWindowWidth() - _itemWidth * 1 / 6);
                        ImGui.Text("TiltParam3");
                    }
                }
            }

            using (var subList = ImRaii.Child("MountSubList", -Vector2.One, false))
            {
                if (subList)
                {
                    foreach (var key in _activeSet.MountReplacements.Keys)
                    {
                        var replacement = _activeSet.MountReplacements[key].Replacement;

                        if (ImGui.Checkbox("##" + key, ref _activeSet.MountReplacements[key].Enabled))
                        {
                            Methods.SetupMount(key);
                            Service.Config.Save();
                        }

                        ImGui.SameLine();
                        if (ImGui.Button(" - ##" + key))
                        {
                            _activeSet.MountReplacements.Remove(key);
                        }

                        ImGui.SameLine();
                        ImGui.Text($"#{key:D5}");

                        ImGui.SameLine();
                        if (_itemWidth != 0 && Service.Config.AdvancedMode)
                        {
                            var widthRest = ImGui.GetWindowWidth() - _itemWidth - ImGui.GetCursorPosX() - 5 * Scale;
                            ImGui.PushTextWrapPos(Math.Max(widthRest, 60 * Scale) + ImGui.GetCursorPosX());
                        }
                        ImGui.TextWrapped(MountReplacementsManager.GetName(key));
                        if (_itemWidth != 0 && Service.Config.AdvancedMode)
                        {
                            ImGui.PopTextWrapPos();
                        }

                        if (!Service.Config.AdvancedMode)
                        {
                            continue;
                        }

                        ImGui.SameLine();
                        if (_itemWidth != 0)
                        {
                            ImGui.SetCursorPosX(ImGui.GetWindowWidth() - _itemWidth);
                        }

                        var startwidth = ImGui.GetCursorPosX();
                        DrawItem("Cast", ref replacement.RideBGM, i => i.RideBGM);
                        ImGui.SameLine();

                        DrawItem("Start", ref replacement.TiltParam1, i => i.TiltParam1);

                        ImGui.SameLine();
                        DrawItem("Start", ref replacement.TiltParam2, i => i.TiltParam2);

                        ImGui.SameLine();
                        DrawItem("Start", ref replacement.TiltParam3, i => i.TiltParam3);

                        ImGui.SameLine();
                        DrawItem("Start", ref replacement.TiltParam3, i => i.TiltParam4);

                        ImGui.SameLine();
                        DrawItem("Start", ref replacement.MountCustomize, i => i.MountCustomize);

                        ImGui.SameLine();
                        _itemWidth = ImGui.GetCursorPosX() - startwidth;
                        ImGui.NewLine();
                        continue;

                        void DrawItem(string name, ref ushort value,
                            Func<Configurations.MountReplacement, ushort> getDefault)
                        {
                            ImGui.SetNextItemWidth(60 * Scale);
                            int relay = value;
                            if (ImGui.DragInt("##" + name + key, ref relay))
                            {
                                value = (ushort)relay;
                                Methods.SetupMount(key);
                                Service.Config.Save();
                            }

                            ImGui.SameLine();
                            using (ImRaii.PushFont(UiBuilder.IconFont))
                            {
                                if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{name}{key}"))
                                {
                                    value = getDefault(MountReplacementsManager.GetOriginalReplacement(key));
                                    Methods.SetupMount(key);
                                    Service.Config.Save();
                                }
                            }
                        }
                    }

                    const string searchMountsPopup = "Search mounts";
                    if (ImGui.Button(" + "))
                    {
                        ImGui.OpenPopup(searchMountsPopup);
                    }

                    using var searchPopup = ImRaii.Popup(searchMountsPopup);
                    if (searchPopup)
                    {
                        var width = 200 * Scale;

                        ImGui.SetNextItemWidth(width);
                        ImGui.InputText("##Search Action", ref _searchMount, 256);

                        using var popUpChild = ImRaii.Child(searchMountsPopup, new Vector2(width, 200 * Scale), true);
                        foreach (var pair in MountReplacementsManager.MountNames.OrderBy(i =>
                        {
                            if (string.IsNullOrEmpty(_searchMount)) return 0;
                            return Math.Min(ScoreString(i.Value, _searchMount),
                                ScoreString(i.Key.ToString(), _searchMount));
                        }))
                        {
                            if (ImGui.Selectable($"#{pair.Key:D5} {pair.Value}"))
                            {
                                var original = MountReplacementsManager.GetOriginalReplacement(pair.Key);
                                _activeSet.MountReplacements[pair.Key] =
                                    new MountReplacementConfig(new Configurations.MountReplacement(
                                            original.RideBGM,
                                            original.TiltParam1,
                                            original.TiltParam2,
                                            original.TiltParam3,
                                            original.TiltParam4,
                                            original.MountCustomize),
                                        false);
                                Service.Config.Save();
                            }
                        }
                    }
                }
            }
        }
    }

    private string _searchAction = string.Empty;
    private string _searchMount = string.Empty;

    private static int ScoreString(string s1, string search)
    {
        if (s1.Contains(search, StringComparison.OrdinalIgnoreCase))
        {
            return s1.Length - search.Length;
        }

        return LevenshteinDistance(s1, search) + 20;
    }

    private static int LevenshteinDistance(string s1, string s2)
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
            Methods.SetupActions(ActionReplacementsManager.AllActionIds);
            Methods.SetupActions(MountReplacementsManager.AllMountIds);
            Service.Config.Save();
        }

        ImGui.SameLine();

        if (ImGui.Button("Redraw"))
        {
            Methods.SetupActions(ActionReplacementsManager.AllActionIds);
            Methods.SetupActions(MountReplacementsManager.AllMountIds);
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