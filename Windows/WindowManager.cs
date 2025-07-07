using System;
using Dalamud.Interface.Windowing;
#pragma warning disable CA1416 // Validate platform compatibility

namespace ActionTimelineReplacement.Windows;

public sealed class WindowManager : IDisposable
{
    private readonly WindowSystem _windowSystem = new("Custom ATR");
    private readonly ConfigWindow _configWindow = new();

    public WindowManager()
    {
        _windowSystem.AddWindow(_configWindow);
        Service.PluginInterface.UiBuilder.Draw += DrawUi;
        Service.PluginInterface.UiBuilder.OpenConfigUi += _configWindow.Toggle;
        Service.PluginInterface.UiBuilder.OpenMainUi += _configWindow.Toggle;
        Service.CommandManager.AddHandler("/atr", new((_, _) => _configWindow.Toggle())
        {
            HelpMessage = "Toggle config window",
            ShowInHelp = true,
        });
    }
    
    public void Dispose()
    {
        Service.PluginInterface.UiBuilder.Draw -= DrawUi;
        Service.PluginInterface.UiBuilder.OpenConfigUi -= _configWindow.Toggle;
        Service.PluginInterface.UiBuilder.OpenMainUi -= _configWindow.Toggle;
        Service.CommandManager.RemoveHandler("/atr");
        _windowSystem.RemoveAllWindows();
    }

    private void DrawUi()
    {
        _windowSystem.Draw();
    }
}