using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using ActionTimelineReplacement.Configurations;
using Dalamud.Game;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using Lumina.Excel.Sheets;

namespace ActionTimelineReplacement;

internal class Service
{
    internal static Configuration Config { get; set; } = null!;

    private static Dictionary<uint, string>? _actionNames;

    private static Dictionary<uint, Configurations.ActionTimelineReplacement> _oldValue = [];

    public static Dictionary<uint, string> ActionNames => _actionNames
        ??= DataManager.GetExcelSheet<Action>()
            .Where(i => !string.IsNullOrEmpty(i.Name.ToString()))
            .ToDictionary(i => i.RowId, i => i.Name.ToString());

    public static string GetName(uint id)
    {
        var actionName = ActionNames.GetValueOrDefault(id, "Unknown");
        return $"#{id:D5} {actionName}";
    }

    public static Configurations.ActionTimelineReplacement GetOriginalReplacement(uint actionId)
    {
        ref var replacement = ref CollectionsMarshal.GetValueRefOrAddDefault(_oldValue, actionId, out var exists);
        if (!exists)
        {
            var act = DataManager.GetExcelSheet<Action>()?.GetRow(actionId);
            replacement = new Configurations.ActionTimelineReplacement(
                (ushort)(act?.AnimationStart.RowId ?? 0),
                (ushort)(act?.AnimationEnd.RowId ?? 0),
                (ushort)(act?.ActionTimelineHit.RowId ?? 0),
                (ushort)(act?.VFX.RowId ?? 0));
        }

        return replacement;
    }

    [PluginService] internal static IDalamudPluginInterface PluginInterface { get; set; } = null!;
    [PluginService] internal static ISigScanner Scanner { get; set; } = null!;
    [PluginService] internal static ICommandManager CommandManager { get; set; } = null!;
    [PluginService] internal static IClientState ClientState { get; set; } = null!;
    [PluginService] internal static IChatGui ChatGui { get; set; } = null!;
    [PluginService] internal static IDataManager DataManager { get; set; } = null!;
    [PluginService] internal static IGameInteropProvider GameInteropProvider { get; set; } = null!;
    [PluginService] internal static IFramework Framework { get; set; } = null!;
    [PluginService] public static IPluginLog Log { get; set; } = null!;
}