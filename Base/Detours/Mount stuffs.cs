using ActionTimelineReplacement;
using ActionTimelineReplacement.Base;
using ActionTimelineReplacement.Base.Detours;
using ActionTimelineReplacement.Base.Global;
using ActionTimelineReplacement.Base.Structs;
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
using FFXIVClientStructs.FFXIV.Client.Game.Character;
using FFXIVClientStructs.FFXIV.Client.System.Resource.Handle;
using FFXIVClientStructs.FFXIV.Client.UI;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using FFXIVClientStructs.FFXIV.Client.UI.Misc;
using FFXIVClientStructs.FFXIV.Common.Lua;
using FFXIVClientStructs.FFXIV.Component.GUI;
using Lumina.Data.Parsing.Scd;
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


/*notes
 * 
 * Sections where mount data we want to edit is loaded:
 * mountdata - E8 ?? ?? ?? ?? 4C 8B F8 48 85 C0 0F 84 ?? ?? ?? ?? 0F 29 B4 24
 * 
 * 
 * 
 * this is where isons load for dragdrop:
 * Component::GUI::AtkComponentDragDrop.GetIconId
 * 
 * 
 * other thoughts - HideHeadgear?
 * for scale have 
 * Mount - Exit move distance, exit move speed, basemotionspeed, basemoptionwalk, 
 * mountcustomize - scale, cameraheight
 * tiltparam - tiltrate(0x0), RotationOriginOffset(0x04), MaxAngle(0x05), unk3(0x06), unk4(0x07), ReverseRotation (8&01) 
 
  */
public unsafe class Mountstuffs : IDisposable
{
    class lastowner
    {
        string Name;
        uint Homeworld;
        string classjob;
    }
    private readonly Hook<MountContainer.Delegates.CreateAndSetupMount>? _CreateAndSetupMountHook;
    private readonly Hook<MountContainer.Delegates.SetupMount>? _SetupMountHook;
    private readonly Hook<EffectContainer.Delegates.LoadTiltData>? _SetupTiltDataHook;
    private readonly Hook<EffectContainer.Delegates.LoadTiltData>? _SetupOtherTiltDataHook;
    //private readonly Hook<AddonInventoryGrid.Delegates.ReceiveEvent>? _InventoryGridReceiveEvent;

    public Mountstuffs()
    {
        _CreateAndSetupMountHook = Service.GameInteropProvider.HookFromAddress<MountContainer.Delegates.CreateAndSetupMount>(MountContainer.Addresses.CreateAndSetupMount.Value, CreateAndSetupDetour);
        _SetupMountHook = Service.GameInteropProvider.HookFromAddress<MountContainer.Delegates.SetupMount> (MountContainer.Addresses.SetupMount.Value, SetupDetour);
        _SetupTiltDataHook = Service.GameInteropProvider.HookFromAddress<EffectContainer.Delegates.LoadTiltData>(EffectContainer.Addresses.LoadTiltData.Value, SetupTiltDataDetour);
        _SetupOtherTiltDataHook = Service.GameInteropProvider.HookFromSignature<EffectContainer.Delegates.LoadTiltData>(Hooks.OtherTiltParamSetup, SetupOtherTiltDataDetour);
        //_ItemDragDropGetIconIDHook = Service.GameInteropProvider.HookFromAddress<AtkComponentDragDrop.Delegates.GetIconId>(AtkComponentDragDrop.Addresses.GetIconId.Value, DragDropGetIconDetour);
        //_ItemHoveredHook = Service.GameInteropProvider.HookFromSignature<ItemHoveredDelegate>(Hooks.itemhoveredhook, ItemHoveredDetour);
        //_ShowTooltipHook = Service.GameInteropProvider.HookFromAddress<AtkTooltipManager.Delegates.ShowTooltip>(AtkTooltipManager.Addresses.ShowTooltip.Value, AtkTooltipManagerShowTooltipDetour);

        this._CreateAndSetupMountHook.Enable();
        this._SetupMountHook.Enable();
        this._SetupTiltDataHook.Enable();
        this._SetupOtherTiltDataHook.Enable();
        //this._ItemHoveredHook.Enable();
        //this._ShowTooltipHook.Enable();

        //Service.AddonLifecycle.RegisterListener(AddonEvent.PostRequestedUpdate, "Tooltip", OnTooltipRequestedUpdate);
        //Service.AddonLifecycle.RegisterListener(AddonEvent.PreDraw, "Tooltip", OnTooltipPreDraw);
        //Service.AddonLifecycle.RegisterListener(AddonEvent.PostRequestedUpdate, "ActionDetail", OnActionTooltipRequestedUpdate);
        //Service.AddonLifecycle.RegisterListener(AddonEvent.PostShow, "_ActionBar07", OnActionBarShow);

    }
    private void SetupTiltDataDetour(EffectContainer* thisptr)
    {
        _SetupTiltDataHook!.Original(thisptr);
        Service.Log.Error("SetupTilt Triggered");
        //Service.Log.Error("Tiltowner address is " + thisptr-> + " TiltOwner name is " + thisptr->OwnerObject->GetName() + " || Owner homeworld is " + thisptr->OwnerObject->HomeWorld.ToString() + " || Owner classjob is " + JobTransientsManager.GetOriginal(thisptr->OwnerObject->ClassJob).Abbreviation);
        var test = (EffectContainerFAFO*)thisptr;
        Service.Log.Error("STOP");
        //thisptr->MountObject->Scale = .2f;
        //thisptr->OwnerObject->DrawData->head

    }
    private void SetupOtherTiltDataDetour(EffectContainer* thisptr)
    {
        _SetupOtherTiltDataHook!.Original(thisptr);
        Service.Log.Error("SetupTiltOTHER Triggered");
        //Service.Log.Error("Tiltowner address is " + thisptr-> + " TiltOwner name is " + thisptr->OwnerObject->GetName() + " || Owner homeworld is " + thisptr->OwnerObject->HomeWorld.ToString() + " || Owner classjob is " + JobTransientsManager.GetOriginal(thisptr->OwnerObject->ClassJob).Abbreviation);
        var test = (EffectContainerFAFO*)thisptr;
        var testOwner = test->OwnerObject->Effects;
        Service.Log.Error("STOP");
        //thisptr->MountObject->Scale = .2f;
        //thisptr->OwnerObject->DrawData->head

    }

    private void CreateAndSetupDetour(MountContainer* thisptr, short mountid, uint buddyModelTop, uint buddyModelBody, uint buddyModelLegs, byte buddystain, byte unk6, byte unk7)
    {
        Service.Log.Error("Create and Setup Triggered");
        _CreateAndSetupMountHook!.Original(thisptr, mountid, buddyModelTop, buddyModelBody, buddyModelLegs, buddystain, unk6, unk7);
        Service.Log.Error("This ID is " + thisptr->MountObject->OwnerId + "Owner ID is " + thisptr->OwnerObject->OwnerId + "Owner name is " + thisptr->OwnerObject->GetName() +  " || Owner homeworld is " + thisptr->OwnerObject->HomeWorld.ToString() + " || Owner classjob is " + JobTransientsManager.GetOriginal(thisptr->OwnerObject->ClassJob).Abbreviation);
        //thisptr->MountObject->Scale = .2f;
        //thisptr->OwnerObject->DrawData->head
        var test = thisptr;
        Service.Log.Error("STOP");

        //owner id




    }
    private void SetupDetour(MountContainer* thisptr, short mountid, uint buddyModelTop, uint buddyModelBody, uint buddyModelLegs, byte buddystain)
    {
        Service.Log.Error("Setup Triggered");
        _SetupMountHook!.Original(thisptr, mountid, buddyModelTop, buddyModelBody, buddyModelLegs, buddystain);
        //thisptr->MountObject->Effects.GroundTiltAngle = 90f;
        //thisptr->MountObject->Effects.GroundTiltSpeed = 50f;
        //test = thisptr->MountObject->Effects;
        //Service.Log.Error("TiltSpeed is is ");
    }


    public void Dispose()
    {
        // When we're done, disable the hook again and clean up. Dispose does this in one go!
        this._CreateAndSetupMountHook.Dispose();
        this._SetupMountHook.Dispose();
        this._SetupTiltDataHook.Dispose();
        this._SetupOtherTiltDataHook.Dispose();

        //Service.AddonLifecycle.UnregisterListener(OnTooltipPreDraw);
        //Service.AddonLifecycle.UnregisterListener(OnTooltipRequestedUpdate);
        //Service.AddonLifecycle.UnregisterListener(OnActionBarRequestedUpdate);
    }
}
