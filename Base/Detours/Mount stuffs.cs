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
    public bool stop = false;
    class lastowner
    {
        string Name;
        uint Homeworld;
        string classjob;
        byte Sex;
        byte Race;
        
    }
    private delegate void MountUpdateDelegate(MountContainer* thisptr);

    private readonly Hook<MountContainer.Delegates.CreateAndSetupMount>? _CreateAndSetupMountHook;
    private readonly Hook<MountContainer.Delegates.SetupMount>? _SetupMountHook;
    private readonly Hook<EffectContainer.Delegates.LoadMountTiltData>? _SetupMountTiltDataHook;
    private readonly Hook<EffectContainer.Delegates.LoadPlayerTiltData>? _SetupRiderTiltDataHook;
    private readonly Hook<MountUpdateDelegate>? _MountUpdateHook;
    //private readonly Hook<MountContainer.Delegates.>? _SetupRiderTiltDataHook;
    //private readonly Hook<AddonInventoryGrid.Delegates.ReceiveEvent>? _InventoryGridReceiveEvent;

    public Mountstuffs()
    {
        //_CreateAndSetupMountHook = Service.GameInteropProvider.HookFromAddress<MountContainer.Delegates.CreateAndSetupMount>(MountContainer.Addresses.CreateAndSetupMount.Value, CreateAndSetupDetour);
       // _SetupMountHook = Service.GameInteropProvider.HookFromAddress<MountContainer.Delegates.SetupMount> (MountContainer.Addresses.SetupMount.Value, SetupDetour);
        //_SetupMountTiltDataHook = Service.GameInteropProvider.HookFromAddress<EffectContainer.Delegates.LoadMountTiltData>(EffectContainer.Addresses.LoadMountTiltData.Value, SetupMountTiltDataDetour);
        //_SetupRiderTiltDataHook = Service.GameInteropProvider.HookFromAddress<EffectContainer.Delegates.LoadPlayerTiltData>(EffectContainer.Addresses.LoadPlayerTiltData.Value, SetupRiderTiltDataDetour);
        _MountUpdateHook = Service.GameInteropProvider.HookFromSignature<MountUpdateDelegate>(Hooks.MountUpdate, MountUpdateDetour);
        //_ItemDragDropGetIconIDHook = Service.GameInteropProvider.HookFromAddress<AtkComponentDragDrop.Delegates.GetIconId>(AtkComponentDragDrop.Addresses.GetIconId.Value, DragDropGetIconDetour);
        //_ItemHoveredHook = Service.GameInteropProvider.HookFromSignature<ItemHoveredDelegate>(Hooks.itemhoveredhook, ItemHoveredDetour);
        //_ShowTooltipHook = Service.GameInteropProvider.HookFromAddress<AtkTooltipManager.Delegates.ShowTooltip>(AtkTooltipManager.Addresses.ShowTooltip.Value, AtkTooltipManagerShowTooltipDetour);

       //this._CreateAndSetupMountHook.Enable();
       //this._SetupMountHook.Enable();
       //this._SetupMountTiltDataHook.Enable();
       //this._SetupRiderTiltDataHook.Enable();
       this._MountUpdateHook.Enable();
       //this._ShowTooltipHook.Enable();

        //Service.AddonLifecycle.RegisterListener(AddonEvent.PostRequestedUpdate, "Tooltip", OnTooltipRequestedUpdate);
        //Service.AddonLifecycle.RegisterListener(AddonEvent.PreDraw, "Tooltip", OnTooltipPreDraw);
        //Service.AddonLifecycle.RegisterListener(AddonEvent.PostRequestedUpdate, "ActionDetail", OnActionTooltipRequestedUpdate);
        //Service.AddonLifecycle.RegisterListener(AddonEvent.PostShow, "_ActionBar07", OnActionBarShow);

    }
    private void SetupMountTiltDataDetour(EffectContainer* thisptr)
    {
        // check if owner is corret, then pull and apply mounted tilt values.
        _SetupMountTiltDataHook!.Original(thisptr);
        //Service.Log.Error("SetupTilt Triggered" + (nint)((thisptr->OwnerObject->CharacterSetup).OwnerObject));
        //Service.Log.Error("This ID is " + thisptr->OwnerObject->Mount.MountObject->OwnerId + " TiltOwner name is " + thisptr->OwnerObject->GetName() + " || Owner homeworld is " + thisptr->OwnerObject->HomeWorld.ToString() + " || Owner classjob is " + JobTransientsManager.GetOriginal(thisptr->OwnerObject->ClassJob).Abbreviation);
        var test = (EffectContainerFAFO*)thisptr;
        var testorig = thisptr;
        Service.Log.Error("STOP");
        //thisptr->MountObject->Scale = .2f;
        //thisptr->OwnerObject->DrawData->head

    }
    private void SetupRiderTiltDataDetour(EffectContainer* thisptr)
    {
        //Check if owner is correct, then pull and apply rider tilt values
        _SetupRiderTiltDataHook!.Original(thisptr);
        Service.Log.Error("SetupTiltOTHER Triggered" + (nint)thisptr->OwnerObject->DrawObject);
        //Service.Log.Error("Tiltowner address is " + thisptr-> + " TiltOwner name is " + thisptr->OwnerObject->GetName() + " || Owner homeworld is " + thisptr->OwnerObject->HomeWorld.ToString() + " || Owner classjob is " + JobTransientsManager.GetOriginal(thisptr->OwnerObject->ClassJob).Abbreviation);
        var test = (EffectContainerFAFO*)thisptr;
        var testOwner = test->OwnerObject->Effects;
        Service.Log.Error("STOP");
        //thisptr->MountObject->Scale = .2f;
        //thisptr->OwnerObject->DrawData->head

    }

    private void CreateAndSetupDetour(MountContainer* thisptr, short mountid, uint buddyModelTop, uint buddyModelBody, uint buddyModelLegs, byte buddystain, byte unk6, byte unk7)
    {
        //do scale and maybe camera scale here. also maybe animation speed?
        //match SEX
        //MODE: Mounted
        // for race: OwnerObject -> DrawData -> CustomizeData
        //for camerascale, checked ownerid       Client::Game::Character::MountContainer.Update(longlong param_1) 0x134(108) 0x184(388), 


        //for HERE: Check if Owner is the PLayercharacter. if it is store owner pointer, then pull current values for the mount, and apply scale and camerascale, updowntilt?[might need another function[
        // store owneraddress for later use.
        //if 
        //Service.Log.Error("Create and Setup Triggered" + (nint)thisptr->MountObject->DrawObject);
        _CreateAndSetupMountHook!.Original(thisptr, mountid, buddyModelTop, buddyModelBody, buddyModelLegs, buddystain, unk6, unk7);
        Service.Log.Error("This ID is " + thisptr->MountObject->OwnerId + "Mount ID is " + thisptr->MountId + "Owner name is " + thisptr->OwnerObject->GetName +  " || Owner homeworld is " + thisptr->OwnerObject->HomeWorld.ToString() + " || Owner classjob is " + JobTransientsManager.GetOriginal(thisptr->OwnerObject->ClassJob).Abbreviation);
        Service.Log.Error("Scale value is currently: " + thisptr->MountObject->Scale);
        var test2 = (CharacterContainerFAFO*)thisptr->OwnerObject;
        Service.Log.Error("camera scale values are probably " + test2->CameraOffset1 + " and " + test2->CameraOffset2 + " testing value 2");
        //test2->CameraOffset1 = 8f;
        //test2->CameraOffset2 = 8f;
        Service.Log.Error("newvalues are probably " + test2->CameraOffset1 + " and " + test2->CameraOffset2 + " testing value 2");
        //thisptr->OwnerObject->DrawData->head
        var test = thisptr;
        Service.Log.Error("STOP");
        var curvalue = MountDetourManager.GetReplacement(thisptr->MountId);
        Service.Log.Error("Ownderobject is " + thisptr->OwnerObject->GetName().ToString() + " gamestate is " + Service.PlayerState.CharacterName);

        if (thisptr->OwnerObject->GetName().ToString() == Service.PlayerState.CharacterName)
        {
            test = thisptr;
            stop = true;
            //thisptr->MountObject->Scale = curvalue.Scale.GetByRaceAndSex();
        }

        //owner id




    }

    private void MountUpdateDetour(MountContainer* thisptr)
    {
        //might not have mount data?
        _MountUpdateHook!.Original(thisptr);
        //thisptr->MountObject->Scale = 1f;
        //camera offset stuff
        var mountObject = (Character*)thisptr->MountObject;
        var ownerObject = (Character*)thisptr->OwnerObject;
        if ((ownerObject->GetName().ToString() == Service.PlayerState.CharacterName) && (ownerObject->HomeWorld == Service.PlayerState.HomeWorld.RowId) && (ownerObject->IsMounted() == true))
        {
            var origvalue = MountDetourManager.GetOriginal(thisptr->MountId);
            var curvalue = MountDetourManager.GetReplacement(thisptr->MountId);
            //if ((mountObject == null) || (ownerObject == null)) { return; }
            if (mountObject != null)
            {
                //Service.Log.Warning("trying apply to mount object");
                //apply scale           
                mountObject->Scale = (curvalue.Scale.GetByRaceAndSex(thisptr->OwnerObject->DrawData.CustomizeData.Race, thisptr->OwnerObject->GetSex())/100f);
                //apply tilts
                mountObject->Effects.MountGroundTiltAngle = curvalue.MountGroundTilt.TiltAngle;
                mountObject->Effects.MountGroundTiltSpeed = curvalue.MountGroundTilt.TiltSpeed;
                mountObject->Effects.MountGroundTiltOrigin = (EffectContainer.TiltOrigin)curvalue.MountGroundTilt.TiltOrigin;

                mountObject->Effects.MountFlightSwimTiltAngle = curvalue.MountFlightTilt.TiltAngle;
                mountObject->Effects.MountFlightSwimTiltSpeed = curvalue.MountFlightTilt.TiltSpeed;
                mountObject->Effects.MountFlightSwimTiltOrigin = (EffectContainer.TiltOrigin)curvalue.MountFlightTilt.TiltOrigin;
                // apply risefalltilt

                // BGM
            }
            if (ownerObject != null)
            {
                //Service.Log.Warning("trying apply to owner object");
                //figure out the math that the game
                //((CharacterContainerFAFO*)ownerObject)->CameraOffset1 = (curvalue.CameraScale.GetByRaceAndSex(thisptr->OwnerObject->DrawData.CustomizeData.Race, thisptr->OwnerObject->GetSex()) / 100);
                //apply owner tilt
                ownerObject->Effects.RiderGroundTiltAngle = curvalue.RiderGroundTilt.TiltAngle;
                ownerObject->Effects.RiderGroundTiltSpeed = curvalue.RiderGroundTilt.TiltSpeed;
                ownerObject->Effects.RiderGroundTiltOrigin = (EffectContainer.TiltOrigin)curvalue.RiderGroundTilt.TiltOrigin;

                ownerObject->Effects.RiderFlightSwimTiltAngle = curvalue.RiderFlightTilt.TiltAngle;
                ownerObject->Effects.RiderFlightSwimTiltSpeed = curvalue.RiderFlightTilt.TiltSpeed;
                ownerObject->Effects.RiderFlightSwimTiltOrigin = (EffectContainer.TiltOrigin)curvalue.RiderFlightTilt.TiltOrigin;
            }

        }
        //Service.Log.Error("camera scale values are probably " + test2->CameraOffset1 + " and " + test2->CameraOffset2 + " testing value 2");
        //test2->CameraOffset1 = 8f;
        //test2->CameraOffset2 = 8f;
        //Service.Log.Error("newvalues are probably " + test2->CameraOffset1 + " and " + test2->CameraOffset2 + " testing value 2");



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
        //this._CreateAndSetupMountHook.Dispose();
        //this._SetupMountHook.Dispose();
        //this._SetupMountTiltDataHook.Dispose();
        //this._SetupRiderTiltDataHook.Dispose();
        this._MountUpdateHook.Dispose();

        //Service.AddonLifecycle.UnregisterListener(OnTooltipPreDraw);
        //Service.AddonLifecycle.UnregisterListener(OnTooltipRequestedUpdate);
        //Service.AddonLifecycle.UnregisterListener(OnActionBarRequestedUpdate);
    }
}
