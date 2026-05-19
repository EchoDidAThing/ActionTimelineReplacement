using ActionTimelineReplacement;
using Dalamud.Hooking;
using FFXIVClientStructs.FFXIV.Client.UI.Misc;
using FFXIVClientStructs.FFXIV.Component.GUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace ActionTimelineReplacement.Base.Detours;

public unsafe class ScrollDetour : IDisposable
{
    private readonly Hook<AtkComponentScrollBar.Delegates.ReceiveEvent>? _scrollBarReceiveEventHook;

    public ScrollDetour()
    {
        _scrollBarReceiveEventHook = Service.GameInteropProvider.HookFromAddress<AtkComponentScrollBar.Delegates.ReceiveEvent>(AtkComponentScrollBar.StaticVirtualTablePointer->ReceiveEvent, AtkComponentScrollBarReceiveEvent);

        this._scrollBarReceiveEventHook.Enable();
    }

    public void Dispose()
    {
        // When we're done, disable the hook again and clean up. Dispose does this in one go!
        this._scrollBarReceiveEventHook.Dispose();
    }


    private void AtkComponentScrollBarReceiveEvent(AtkComponentScrollBar* thisPtr, AtkEventType type, int param, AtkEvent* eventPointer, AtkEventData* dataPointer)
    {

        Service.Log.Info("AtkComponentScrollBarReceiveEvent triggered");
        try
        {
            dataPointer->MouseData.WheelDirection *= -1;
            _scrollBarReceiveEventHook!.Original(thisPtr, type, param, eventPointer, dataPointer);
        }
        catch (Exception e)
        {
            Service.Log.Error(e, "Error in AtkComponentScrollBarReceiveEvent");
        }
    }
}
