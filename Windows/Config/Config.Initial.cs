using System.Numerics;
using ActionTimelineReplacement.Sheets;
using Dalamud.Interface.ImGuiFileDialog;
using Dalamud.Interface.Windowing;
using Dalamud.Bindings.ImGui;
#pragma warning disable CA1416 // Validate platform compatibility

namespace ActionTimelineReplacement.Windows;

public sealed partial class ConfigWindow : Window
{
    private readonly FileDialogManager _dialogManager = new()
    {
        AddedWindowFlags = ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoDocking,
    };

    //private static float Scale => ImGuiHelpers.GlobalScale;
    private Configuration.ReplacementSet? _activeSet;

    public ConfigWindow()
        : base("Replacement" + typeof(ConfigWindow).Assembly.GetName().Version)
    {
        SizeCondition = ImGuiCond.FirstUseEver;
        Size = new Vector2(960, 540);
        SizeConstraints = new WindowSizeConstraints()
        {
            MinimumSize = new Vector2(480, 360),
            MaximumSize = new Vector2(5000, 5000),
        };
    }
}