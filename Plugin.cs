using ActionTimelineReplacement.Base;
using ActionTimelineReplacement.Base.Setups;
using ActionTimelineReplacement.Interface;
using ActionTimelineReplacement.Sheets;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using System;
using System.Collections.Immutable;

namespace ActionTimelineReplacement;

public sealed class Plugin : IDalamudPlugin
{


    private GlassesDropDetour _VFXHook;

    private readonly ImmutableArray<IDisposable> _disposables;
    public Plugin(IDalamudPluginInterface pluginInterface)
    {

        _VFXHook = new GlassesDropDetour(Service.GameInteropProvider);
        pluginInterface.Create<Service>();

        //用于读取Config
        Service.Config = pluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
        
        _disposables = [new WindowManager()];

        Setup.SetupAll();

    }

    public void Dispose()
    {
        _VFXHook.Dispose();
        Setup.SetupAll(true);
        Service.Config.Save();
        foreach (var disposable in _disposables)
        {
            disposable.Dispose();
        }
    }
}