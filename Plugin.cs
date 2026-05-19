using ActionTimelineReplacement.Base;
using ActionTimelineReplacement.Base.Setups;
using ActionTimelineReplacement.Interface;
using ActionTimelineReplacement.Sheets;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using System;
using System.Collections.Immutable;
using System.Collections.Specialized;

namespace ActionTimelineReplacement;

public sealed class Plugin : IDalamudPlugin
{

    private readonly HookHandler HookHandler;


    private readonly ImmutableArray<IDisposable> _disposables;
    public Plugin(IDalamudPluginInterface pluginInterface)
    {

        pluginInterface.Create<Service>();
        HookHandler = new HookHandler();

        //用于读取Config
        Service.Config = pluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
        
        _disposables = [new WindowManager()];

        Setup.SetupAll();

    }

    public void Dispose()
    {
        HookHandler?.Dispose();
        Setup.SetupAll(true);
        Service.Config.Save();
        foreach (var disposable in _disposables)
        {
            disposable.Dispose();
        }
    }
}