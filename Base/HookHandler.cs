using ActionTimelineReplace.Base.Detours;
using ActionTimelineReplacement.Base.Detours;
using Dalamud.Plugin.Services;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace ActionTimelineReplacement.Base;

internal class HookHandler : IDisposable
{
    private ScrollDetour _ScrollHook;
    private ShowTooltipDetour _ShowTooltipHook;
    //private ShowActionbarDetour _ShowActionbarHook;
    public HookHandler()
    {
        _Register();
    }

    private void _Register()
    {
        _ShowTooltipHook = new ShowTooltipDetour();
        _ScrollHook = new ScrollDetour();
        //_ShowActionbarHook = new ShowActionbarDetour();
    }

    public void Dispose()
    {
        _ShowTooltipHook?.Dispose();
        _ScrollHook.Dispose();
        //_ShowActionbarHook?.Dispose();
    }

}

