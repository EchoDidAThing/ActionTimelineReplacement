using System;
using Dalamud.Interface.Windowing;

namespace ActionTimelineReplacement.Windows;

public sealed class WindowManager : IDisposable
{
    private readonly WindowSystem _windowSystem = new("ActionTimeline");
    private readonly ConfigWindow _configWindow = new();

    public WindowManager()
    {
        _windowSystem.AddWindow(_configWindow);
        Service.PluginInterface.UiBuilder.Draw += DrawUi;
        Service.PluginInterface.UiBuilder.OpenConfigUi += _configWindow.Toggle;
        Service.PluginInterface.UiBuilder.OpenMainUi += _configWindow.Toggle;
    }
    public void Dispose()
    {
        Service.PluginInterface.UiBuilder.Draw -= DrawUi;
        Service.PluginInterface.UiBuilder.OpenConfigUi -= _configWindow.Toggle;
        Service.PluginInterface.UiBuilder.OpenMainUi -= _configWindow.Toggle;
        _windowSystem.RemoveAllWindows();
    }

    private void DrawUi()
    {
        _windowSystem.Draw();
    }
}