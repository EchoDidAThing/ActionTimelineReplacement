using System;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using ActionTimelineReplacement.Configurations;
using ActionTimelineReplacement.Hookers;
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

    private ActionTimelineReplacementSet? _activeSet;

    public ConfigWindow()
        : base("ActionTimeline Replacement v" + typeof(ConfigWindow).Assembly.GetName().Version,
            ImGuiWindowFlags.NoScrollbar)
    {
        SizeCondition = ImGuiCond.FirstUseEver;
        Size = new Vector2(500, 200);
        SizeConstraints = new WindowSizeConstraints()
        {
            MinimumSize = new Vector2(250, 300),
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
        using var child = ImRaii.Child("Side bar", -Vector2.One, true, ImGuiWindowFlags.NoScrollbar);
        if (!child) return;

        var itemHeight = ImGui.CalcTextSize("C").Y + ImGui.GetStyle().CellPadding.Y * 2 +
                         ImGui.GetStyle().WindowPadding.Y;

        using (ImRaii.Child("Items",
                   new Vector2(-1,
                       ImGui.GetWindowSize().Y - ImGui.GetCursorPosY() - itemHeight - ImGui.GetStyle().WindowPadding.Y),
                   false))
        {
            var span = CollectionsMarshal.AsSpan(Service.Config.ActionTimelineReplacements);
            for (var i = 0; i < span.Length; i++)
            {
                var set = span[i];
                // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
                if (set is null) continue;
                using var style = !set.Enabled ? ImRaii.PushColor(ImGuiCol.Text, ImGuiColors.DalamudGrey) : null;
                if (ImGui.Selectable($"{set.Name}##{i}", _activeSet == set))
                {
                    _activeSet = set;
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
                        _dialogManager.SaveFileDialog("Save", ".json", set.Name, ".json", (b, file) =>
                        {
                            if (!b) return;
                            set.Save(file);
                        });
                    }

                    if (ImGui.Selectable("Delete"))
                    {
                        Service.Config.ActionTimelineReplacements.Remove(set);
                        Service.Config.Save();
                        Methods.SetupAction(set.Replacements.Keys);
                        ImGui.CloseCurrentPopup();
                    }
                }
            }
        }

        ImGui.SetCursorPosY(ImGui.GetWindowSize().Y - itemHeight);
        if (ImGui.Button("Create"))
        {
            Service.Config.ActionTimelineReplacements.Add(new ActionTimelineReplacementSet("New Item", [], true, 0));
            Service.Config.Save();
        }

        ImGui.SameLine();
        if (ImGui.Button("Import"))
        {
            _dialogManager.OpenFileDialog("Import", ".json", (b, files) =>
            {
                if (!b) return;
                foreach (var file in files)
                {
                    if (ActionTimelineReplacementSet.Load(file) is not { } set) continue;
                    Service.Config.ActionTimelineReplacements.Add(set);
                    Methods.SetupAction(set.Replacements.Keys);
                }

                Service.Config.Save();
            }, 10, ".");
        }
    }

    private void DrawBody()
    {
        using var child = ImRaii.Child("Body", -Vector2.One, false, ImGuiWindowFlags.NoScrollbar);
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
            Methods.SetupAction(_activeSet.Replacements.Keys);
            Service.Config.Save();
        }

        ImGui.SameLine();
        ImGui.SetNextItemWidth(50 * Scale);
        if (ImGui.DragInt("Priority", ref _activeSet.Priority))
        {
            Methods.SetupAction(_activeSet.Replacements.Keys);
            Service.Config.Save();
        }

        using (ImRaii.Table("SetSkills", 5, ImGuiTableFlags.Resizable))
        {
            ImGui.TableSetupColumn("Skill", ImGuiTableColumnFlags.WidthStretch);
            ImGui.TableSetupColumn("Start timeline", ImGuiTableColumnFlags.WidthFixed);
            ImGui.TableSetupColumn("End timeline", ImGuiTableColumnFlags.WidthFixed);
            ImGui.TableSetupColumn("Hit timeline", ImGuiTableColumnFlags.WidthFixed);
            ImGui.TableSetupColumn("Cast Vfx", ImGuiTableColumnFlags.WidthFixed);
            ImGui.TableHeadersRow();

            foreach (var key in _activeSet.Replacements.Keys)
            {
                var replacement = _activeSet.Replacements[key];
                ImGui.TableNextRow();

                ImGui.TableNextColumn();
                if (ImGui.Checkbox("##" + key, ref replacement.Enabled))
                {
                    Methods.SetupAction(key);
                    Service.Config.Save();
                }

                ImGui.SameLine();
                if (ImGui.Button(" - ##" + key))
                {
                    _activeSet.Replacements.Remove(key);
                }

                ImGui.SameLine();
                ImGui.Text(ReplacementsManager.GetName(key));

                ImGui.TableNextColumn();
                DrawItem("Start", ref replacement.Replacement.AnimationStart);

                ImGui.TableNextColumn();
                DrawItem("End", ref replacement.Replacement.AnimationEnd);

                ImGui.TableNextColumn();
                DrawItem("Hit", ref replacement.Replacement.ActionTimelineHit);

                ImGui.TableNextColumn();
                DrawItem("Cast", ref replacement.Replacement.CastVfx);

                continue;

                void DrawItem(string name, ref ushort value)
                {
                    ImGui.SetNextItemWidth(80 * Scale);
                    int relay = value;
                    if (ImGui.DragInt("##" + name + key, ref relay))
                    {
                        value = (ushort)relay;
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
            foreach (var pair in ReplacementsManager.ActionNames.OrderBy(i =>
                     {
                         if (string.IsNullOrEmpty(_searchAction)) return 0;
                         return Math.Min(ScoreString(i.Value, _searchAction),
                             ScoreString(i.Key.ToString(), _searchAction));
                     }))
            {
                if (ImGui.Selectable($"#{pair.Key:D5} {pair.Value}"))
                {
                    _activeSet.Replacements[pair.Key] =
                        new ActionTimelineReplacementConfig(new Configurations.ActionTimelineReplacement(0, 0, 0, 0),
                            false);
                    Service.Config.Save();
                }
            }
        }
    }

    private string _searchAction = string.Empty;

    private static int ScoreString(string s1, string search)
    {
        if (s1.Contains(search, StringComparison.OrdinalIgnoreCase))
        {
            return s1.Length - search.Length;
        }
        return LevenshteinDistance(s1, search)  + 20;
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
            Methods.SetupAction(ReplacementsManager.AllActionIds);
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