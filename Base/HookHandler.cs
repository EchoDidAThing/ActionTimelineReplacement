using ActionTimelineReplacement.Base.Detours;
using Dalamud.Plugin.Services;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace ActionTimelineReplacement.Base;

internal class HookHandler : IDisposable
{

    private HoverDetour _HoverHook;
    private PlayTimelineDetour _PlayTimelineHook;
    private VFXDetour _VFXHook;
    public HookHandler()
    {
        _Register();
    }

    private void _Register()
    {
       // _PlayTimelineHook = new PlayTimelineDetour();
        //_PlayTimelineHook.Init();


        //_HoverHook = new HoverDetour();
        //_HoverHook.Init();
        _VFXHook = new VFXDetour();
        _VFXHook.Init();
    }

    public void Dispose()
    {
        //_PlayTimelineHook?.Dispose();
        //_HoverHook?.Dispose();
        _VFXHook?.Dispose();
    }

}

