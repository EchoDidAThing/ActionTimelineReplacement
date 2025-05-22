using Dalamud.Plugin;
using System;
using System.Collections.Immutable;
using ActionTimelineReplacement.Hookers;
using ActionTimelineReplacement.Windows;
using ActionTimelineReplacement.Configurations;

namespace ActionTimelineReplacement;

public sealed class Plugin : IDalamudPlugin
{
    private readonly ImmutableArray<IDisposable> _disposables;
    public Plugin(IDalamudPluginInterface pluginInterface)
    {
        //初始化服务，主要用于ID
        pluginInterface.Create<Service>();

        //用于读取Config
        Service.Config = pluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
        
        //添加各种小模组
        _disposables = [new WindowManager()];

        Methods.SetupAll();

    }

    public void Dispose()
    {
        Methods.SetupAll(true);
        Service.Config.Save();
        foreach (var disposable in _disposables)
        {
            disposable.Dispose();
        }
    }
}