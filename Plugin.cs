using Dalamud.Plugin;
using System;
using System.Collections.Immutable;
using ActionTimelineReplacement.Sheets;
using ActionTimelineReplacement.Windows;
using ActionTimelineReplacement.Base.Setups;

namespace ActionTimelineReplacement;

public sealed class Plugin : IDalamudPlugin
{
    private readonly ImmutableArray<IDisposable> _disposables;
    public Plugin(IDalamudPluginInterface pluginInterface)
    {
        pluginInterface.Create<Service>();

        //用于读取Config
        Service.Config = pluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
        
        _disposables = [new WindowManager()];

        Setup.SetupAll();

    }

    public void Dispose()
    {
        Setup.SetupAll(true);
        Service.Config.Save();
        foreach (var disposable in _disposables)
        {
            disposable.Dispose();
        }
    }
}