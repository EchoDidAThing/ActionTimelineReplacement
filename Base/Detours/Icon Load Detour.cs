using ActionTimelineReplacement;
using ActionTimelineReplacement.Base;
using ActionTimelineReplacement.Base.Detours;
using ActionTimelineReplacement.Base.Global;
using ActionTimelineReplacement.Sheets;
using Dalamud.Game;
using Dalamud.Game.Addon.Lifecycle;
using Dalamud.Game.Addon.Lifecycle.AddonArgTypes;
using Dalamud.Game.Agent;
using Dalamud.Game.Gui;
using Dalamud.Hooking;
using Dalamud.Plugin.Services;
using Dalamud.Utility;
using FFXIVClientStructs.FFXIV.Client.Enums;
using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Client.UI;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using FFXIVClientStructs.FFXIV.Client.UI.Misc;
using FFXIVClientStructs.FFXIV.Common.Lua;
using FFXIVClientStructs.FFXIV.Component.GUI;
using Lumina.Text;
using Lumina.Text.Payloads;
using Lumina.Text.ReadOnly;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using static FFXIVClientStructs.FFXIV.Client.UI.AddonRelicNoteBook;
using static FFXIVClientStructs.FFXIV.Client.UI.Agent.AgentActionDetail.Delegates;
using static FFXIVClientStructs.FFXIV.Client.UI.ListPanel.Delegates;
using static Lumina.Data.Parsing.Uld.NodeData;
using static System.Net.Mime.MediaTypeNames;

namespace ActionTimelineReplace.Base.Detours;

public unsafe class IconLoadDetour : IDisposable
{

    private readonly Hook<AtkComponentDragDrop.Delegates.LoadIcon>? _ItemDragDropLoadHook;
    private readonly Hook<AtkComponentDragDrop.Delegates.GetIconId>? _ItemDragDropGetIconIDHook;
    //private readonly Hook<AddonInventoryGrid.Delegates.ReceiveEvent>? _InventoryGridReceiveEvent;

    public IconLoadDetour()
    {
         IntPtr InvGridRecEvntPtR = Service.Scanner.ScanText("E8 ?? ?? ?? FF 48 8B 7C 24 48 C7 46 0C 01 00 00 00");
        //_InventoryGridReceiveEvent = Service.GameInteropProvider.HookFromAddress<AddonInventoryGrid.Delegates.ReceiveEvent>(InvGridRecEvntPtR, InventoryGridReceiveDetour);
        _ItemDragDropLoadHook = Service.GameInteropProvider.HookFromAddress<AtkComponentDragDrop.Delegates.LoadIcon>(AtkComponentDragDrop.Addresses.LoadIcon.Value, DragDropIconLoadedDetour);
        //_ItemDragDropGetIconIDHook = Service.GameInteropProvider.HookFromAddress<AtkComponentDragDrop.Delegates.GetIconId>(AtkComponentDragDrop.Addresses.GetIconId.Value, DragDropGetIconDetour);
        //_ItemHoveredHook = Service.GameInteropProvider.HookFromSignature<ItemHoveredDelegate>(Hooks.itemhoveredhook, ItemHoveredDetour);
        //_ShowTooltipHook = Service.GameInteropProvider.HookFromAddress<AtkTooltipManager.Delegates.ShowTooltip>(AtkTooltipManager.Addresses.ShowTooltip.Value, AtkTooltipManagerShowTooltipDetour);

        //this._InventoryGridReceiveEvent.Enable();
        this._ItemDragDropLoadHook.Enable();
        //this._ItemHoveredHook.Enable();
        //this._ShowTooltipHook.Enable();

        //Service.AddonLifecycle.RegisterListener(AddonEvent.PostRequestedUpdate, "Tooltip", OnTooltipRequestedUpdate);
        //Service.AddonLifecycle.RegisterListener(AddonEvent.PreDraw, "Tooltip", OnTooltipPreDraw);
        //Service.AddonLifecycle.RegisterListener(AddonEvent.PostRequestedUpdate, "ActionDetail", OnActionTooltipRequestedUpdate);
        //Service.AddonLifecycle.RegisterListener(AddonEvent.PostShow, "_ActionBar07", OnActionBarShow);
        
    }

    private bool DragDropIconLoadedDetour(AtkComponentDragDrop* thisPtr, uint iconid)
    {
        //this works for most things, but items are a problem.need to work around that
        uint feedid = iconid;
        var tocheck = thisPtr;
        if (JobChangeDetour.CurrentJobIcons.ContainsKey(iconid)) { feedid = JobChangeDetour.CurrentJobIcons[iconid]; }
        if (feedid != iconid)
        {
            Service.Log.Error("original value was " + iconid + "decided value was " + feedid);
        }
        //maybe inventory stuff here
        //Service.GameInventory.GetInventoryItems(Dalamud.Game.Inventory.GameInventoryType.Inventory1)
        //Service.Log.Error("count of changed is " + JobChangeDetour.CurrentJobIcons.Count());
        var returnValue = _ItemDragDropLoadHook.Original(thisPtr, feedid);
        return returnValue;

    }

    //private int DragDropGetIconDetour(AtkComponentDragDrop* thisPtr)
    //{
        /*uint feedid = iconid;
        var tocheck = thisPtr;
        if (JobChangeDetour.CurrentJobIcons.ContainsKey(iconid)) { feedid = JobChangeDetour.CurrentJobIcons[iconid]; }
        if (feedid != iconid)
        {
            Service.Log.Error("original value was " + iconid + "decided value was " + feedid);
        }
        //Service.Log.Error("count of changed is " + JobChangeDetour.CurrentJobIcons.Count());*/
        //var returnValue = _ItemDragDropGetIconIDHook.Original(thisPtr);
        //return returnValue;

    //}
    /*private void InventoryGridReceiveDetour(AddonInventoryGrid* thisPtr,AtkEventType eventType, int eventParam, AtkEvent* atkEvent, AtkEventData* atkEventData)
    {
        
        _InventoryGridReceiveEvent.Original(thisPtr, eventType, eventParam, atkEvent, atkEventData);
        Service.Log.Info("event type is " + eventType);

    }*/


    public void Dispose()
    {
        // When we're done, disable the hook again and clean up. Dispose does this in one go!
        this._ItemDragDropLoadHook.Dispose();
        //this._ItemDragDropGetIconIDHook.Dispose();
        //this._InventoryGridReceiveEvent.Dispose();
        //this._ActionHoveredHook.Dispose();

        //Service.AddonLifecycle.UnregisterListener(OnTooltipPreDraw);
        //Service.AddonLifecycle.UnregisterListener(OnTooltipRequestedUpdate);
        //Service.AddonLifecycle.UnregisterListener(OnActionBarRequestedUpdate);
    }
}
