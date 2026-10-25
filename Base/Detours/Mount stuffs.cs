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
using FFXIVClientStructs.FFXIV.Client.Sound;
using FFXIVClientStructs.FFXIV.Client.System.Resource.Handle;
using FFXIVClientStructs.FFXIV.Client.UI;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using FFXIVClientStructs.FFXIV.Client.UI.Misc;
using FFXIVClientStructs.FFXIV.Common.Lua;
using FFXIVClientStructs.FFXIV.Component.Exd;
using FFXIVClientStructs.FFXIV.Component.GUI;
using InteropGenerator.Runtime;
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
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using FFXIVClientStructs.FFXIV.Client.Game.Object;
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
 * other thoughts - HideHeadgear?
 * for scale have 
 * Mount - Exit move distance, exit move speed, basemotionspeed, basemoptionwalk, 
 * mountcustomize - scale, cameraheight
 * tiltparam - tiltrate(0x0), RotationOriginOffset(0x04), MaxAngle(0x05), unk3(0x06), unk4(0x07), ReverseRotation (8&01) 
 
  */
public unsafe class Mountstuffs : IDisposable
{
    public bool stop = false;
    class lastOwner
    {
        string Name;
        uint Homeworld;
        string classjob;
        byte Sex;
        byte Race;
        
    }

    public bool BGMPathUpdate = false;
    public uint MountID = 0;
    public SoundManager* lastSoundManager;
    public string lastbgmpath = "superdefaulttext0w0";
    public bool trykill = false;
    private MountDetourReplace curvalue;

    public const string CheckSig = "E8 ?? ?? ?? ?? 85 C0 74 ?? 48 8B D7 48 8B CE";
    private delegate void MountUpdateDelegate(MountContainer* thisptr);
    private delegate void MountDismountDelegate(MountContainer* a1, char* a2);

    private delegate char CheckSigDelegate(Int64 a1);

    private readonly Hook<MountContainer.Delegates.CreateAndSetupMount>? _CreateAndSetupMountHook;
    private readonly Hook<MountContainer.Delegates.SetupMount>? _SetupMountHook;
    private readonly Hook<EffectContainer.Delegates.LoadMountTiltData>? _SetupMountTiltDataHook;
    private readonly Hook<EffectContainer.Delegates.LoadPlayerTiltData>? _SetupRiderTiltDataHook;
    private readonly Hook<MountUpdateDelegate>? _MountUpdateHook;
    private readonly Hook<MountDismountDelegate>? _MountDismountHook;
    private readonly Hook<SoundManager.Delegates.PlayBGMSound>? _PlayBGMSoundHook;
    private readonly Hook<CheckSigDelegate>? _CheckSigHook;
    //private readonly Hook<MountContainer.Delegates.>? _SetupRiderTiltDataHook;
    //private readonly Hook<AddonInventoryGrid.Delegates.ReceiveEvent>? _InventoryGridReceiveEvent;

    public Mountstuffs()
    {
        //_CreateAndSetupMountHook = Service.GameInteropProvider.HookFromAddress<MountContainer.Delegates.CreateAndSetupMount>(MountContainer.Addresses.CreateAndSetupMount.Value, CreateAndSetupDetour);
        //_SetupMountHook = Service.GameInteropProvider.HookFromAddress<MountContainer.Delegates.SetupMount> (MountContainer.Addresses.SetupMount.Value, SetupDetour);
        _SetupMountTiltDataHook = Service.GameInteropProvider.HookFromAddress<EffectContainer.Delegates.LoadMountTiltData>(EffectContainer.Addresses.LoadMountTiltData.Value, SetupMountTiltDataDetour);
        _SetupRiderTiltDataHook = Service.GameInteropProvider.HookFromAddress<EffectContainer.Delegates.LoadPlayerTiltData>(EffectContainer.Addresses.LoadPlayerTiltData.Value, SetupRiderTiltDataDetour);
        _MountUpdateHook = Service.GameInteropProvider.HookFromSignature<MountUpdateDelegate>(Hooks.MountUpdate, MountUpdateDetour);
        _PlayBGMSoundHook = Service.GameInteropProvider.HookFromAddress<SoundManager.Delegates.PlayBGMSound>(SoundManager.Addresses.PlayBGMSound.Value, PlayBGMSoundDetour);
        _MountDismountHook = Service.GameInteropProvider.HookFromSignature<MountDismountDelegate>(Hooks.MountDismount, MountDismountDetour);
        //_CheckSigHook = Service.GameInteropProvider.HookFromSignature<CheckSigDelegate>(CheckSig, CheckSigDetour);
        //_ItemDragDropGetIconIDHook = Service.GameInteropProvider.HookFromAddress<AtkComponentDragDrop.Delegates.GetIconId>(AtkComponentDragDrop.Addresses.GetIconId.Value, DragDropGetIconDetour);
        //_ItemHoveredHook = Service.GameInteropProvider.HookFromSignature<ItemHoveredDelegate>(Hooks.itemhoveredhook, ItemHoveredDetour);
        //_ShowTooltipHook = Service.GameInteropProvider.HookFromAddress<AtkTooltipManager.Delegates.ShowTooltip>(AtkTooltipManager.Addresses.ShowTooltip.Value, AtkTooltipManagerShowTooltipDetour);

        //this._CreateAndSetupMountHook.Enable();
        //this._SetupMountHook.Enable();
        this._SetupMountTiltDataHook.Enable();
        this._SetupRiderTiltDataHook.Enable();
        this._MountUpdateHook.Enable();
        this._PlayBGMSoundHook.Enable();
        this._MountDismountHook.Enable();
        //this._CheckSigHook.Enable();
        //this._ShowTooltipHook.Enable();

        //Service.AddonLifecycle.RegisterListener(AddonEvent.PostRequestedUpdate, "Tooltip", OnTooltipRequestedUpdate);
        //Service.AddonLifecycle.RegisterListener(AddonEvent.PreDraw, "Tooltip", OnTooltipPreDraw);
        //Service.AddonLifecycle.RegisterListener(AddonEvent.PostRequestedUpdate, "ActionDetail", OnActionTooltipRequestedUpdate);
        //Service.AddonLifecycle.RegisterListener(AddonEvent.PostShow, "_ActionBar07", OnActionBarShow);

    }
    
    private char CheckSigDetour(Int64 a1)
    {
        var output = _CheckSigHook!.Original(a1);
        Service.Log.Error("Checked Signature has run, result is!" + output.ToString());
        return output;
    }
    
    private void SetupMountTiltDataDetour(EffectContainer* thisptr)
    {
        _SetupMountTiltDataHook!.Original(thisptr);
        var ownerObject = thisptr->OwnerObject->GetParentCharacter();
        if (ownerObject == null) return;
        if ((ownerObject->GetName().ToString() == Service.PlayerState.CharacterName) &&
            (ownerObject->HomeWorld == Service.PlayerState.HomeWorld.RowId) && (ownerObject->IsMounted() == true) &&
            (ownerObject->Mount.MountId != 0))
        {
            var curvalue = MountDetourManager.GetReplacement(ownerObject->Mount.MountId);
            Service.Log.Warning("TILTmountID is" + ownerObject->Mount.MountId);
            thisptr->MountGroundTiltAngle = curvalue.MountGroundTilt.TiltAngle;
            thisptr->MountGroundTiltSpeed = curvalue.MountGroundTilt.TiltSpeed;
            thisptr->MountGroundTiltOrigin = (EffectContainer.TiltOrigin)curvalue.MountGroundTilt.TiltOrigin;
            switch (curvalue.MountGroundTilt.ReverseRotation)
            {
                case true:
                    thisptr->MountGroundTiltFlags = EffectContainer.TiltFlags.ReverseRotation;
                    break;
                case false:
                    thisptr->MountGroundTiltFlags = EffectContainer.TiltFlags.None;
                    break;
            }

            thisptr->MountFlightSwimTiltAngle = curvalue.MountFlightTilt.TiltAngle;
            thisptr->MountFlightSwimTiltSpeed = curvalue.MountFlightTilt.TiltSpeed;
            thisptr->MountFlightSwimTiltOrigin = (EffectContainer.TiltOrigin)curvalue.MountFlightTilt.TiltOrigin;
            switch (curvalue.MountFlightTilt.ReverseRotation)
            {
                case true:
                    thisptr->MountFlightSwimTiltFlags = EffectContainer.TiltFlags.ReverseRotation;
                    break;
                case false:
                    thisptr->MountFlightSwimTiltFlags = EffectContainer.TiltFlags.None;
                    break;
            }
        }
    }
    private SoundData* PlayBGMSoundDetour(SoundManager* thisptr, CStringPointer path)
    {
        //Just do it normally for now, this is a test.
        Service.Log.Warning("Mount Id is " + MountID);
        var origvalue = MountDetourManager.GetOriginal(MountID);
        curvalue = MountDetourManager.GetReplacement(MountID);
        var outpath = path;
        lastSoundManager = thisptr;
        //if (BGMPathUpdate)
        //{
            lastbgmpath = origvalue.BGMPath;
            trykill = true;
            if ((path.ToString() != curvalue.BGMPath) && (path.ToString() == origvalue.BGMPath) && (MountID!= 0) && (origvalue != curvalue))
            {
                outpath = ProcessingGlobals.PathCString(curvalue.BGMPath);
                lastbgmpath = curvalue.BGMPath;
                Service.Log.Error("replace path with" + curvalue.BGMPath);
            }
            //BGMPathUpdate = false;
            //MountID = 0;
        //}
        var retvalue  = _PlayBGMSoundHook!.Original(thisptr, outpath);
        return retvalue;
    }
    private void SetupRiderTiltDataDetour(EffectContainer* thisptr)
    {
        // check if owner is corret, then pull and apply mounted tilt values.
        _SetupRiderTiltDataHook!.Original(thisptr);
        var ownerObject = thisptr->OwnerObject->GetAsCharacter();
        Service.Log.Warning("RIDERTILT called");
        if (ownerObject == null) return;
        if (ownerObject->Mount.MountObject == null) return;
        Service.Log.Warning("OWNERmountID is" + ownerObject->Mount.MountId);
        if ((ownerObject->GetName().ToString() == Service.PlayerState.CharacterName) &&
            (ownerObject->HomeWorld == Service.PlayerState.HomeWorld.RowId) && (ownerObject->IsMounted() == true) &&
            (ownerObject->Mount.MountId != 0))
        {
            var curvalue = MountDetourManager.GetReplacement(ownerObject->Mount.MountId);
            Service.Log.Warning("SECONDOWNERmountID is" + ownerObject->Mount.MountId);
            thisptr->RiderGroundTiltAngle = curvalue.RiderGroundTilt.TiltAngle;
            thisptr->RiderGroundTiltSpeed = curvalue.RiderGroundTilt.TiltSpeed;
            thisptr->RiderGroundTiltOrigin = (EffectContainer.TiltOrigin)curvalue.RiderGroundTilt.TiltOrigin;
            switch (curvalue.RiderGroundTilt.ReverseRotation)
            {
                case true:
                    thisptr->RiderGroundTiltFlags = EffectContainer.TiltFlags.ReverseRotation;
                    break;
                case false:
                    thisptr->RiderGroundTiltFlags = EffectContainer.TiltFlags.None;
                    break;
            }

            thisptr->RiderFlightSwimTiltAngle = curvalue.RiderFlightTilt.TiltAngle;
            thisptr->RiderFlightSwimTiltSpeed = curvalue.RiderFlightTilt.TiltSpeed;
            thisptr->RiderFlightSwimTiltOrigin = (EffectContainer.TiltOrigin)curvalue.RiderFlightTilt.TiltOrigin;
            switch (curvalue.RiderFlightTilt.ReverseRotation)
            {
                case true:
                    thisptr->RiderFlightSwimReverseTilt = EffectContainer.TiltFlags.ReverseRotation;
                    break;
                case false:
                    thisptr->RiderFlightSwimReverseTilt = EffectContainer.TiltFlags.None;
                    break;
            }
        }
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
        //var test2 = (CharacterContainerFAFO*)thisptr->OwnerObject;
        //Service.Log.Error("camera scale values are probably " + test2->CameraOffset1 + " and " + test2->CameraOffset2 + " testing value 2");
        //test2->CameraOffset1 = 8f;
        //test2->CameraOffset2 = 8f;
        //Service.Log.Error("newvalues are probably " + test2->CameraOffset1 + " and " + test2->CameraOffset2 + " testing value 2");
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
        var mountObject = thisptr->MountObject;
        var ownerObject = thisptr->OwnerObject;
        if ((ownerObject->GetName().ToString() == Service.PlayerState.CharacterName) && (ownerObject->HomeWorld == Service.PlayerState.HomeWorld.RowId) && (ownerObject->IsMounted() == true) && (thisptr->MountId != 0))
        {
            var origvalue = MountDetourManager.GetOriginal(thisptr->MountId);
            var curvalue = MountDetourManager.GetReplacement(thisptr->MountId);
            //if ((mountObject == null) || (ownerObject == null)) { return; }
            if (mountObject != null)
            {
                //CharacterFAFO* mountObjectFAFO = (CharacterFAFO*)thisptr->MountObject;
                //EffectContainer* mountTiltFAFO = (EffectContainer*)mountObjectFAFO->EffectContainerMem;
                //Service.Log.Warning("trying apply to mount object");
                //apply scale[WORKS]
                mountObject->Scale = (curvalue.Scale.GetByRaceAndSex(ownerObject->DrawData.CustomizeData.Race, ownerObject->GetSex())/100f);
             
                // apply risefalltilt
                MountID = thisptr->MountId;
                
                

                //if ((!BGMPathUpdate) && (lastSoundManager->PlayBGMSound() != curvalue.BGMPath))
                //{ 
                //    BGMSystem.SetBGM((ushort)curvalue.BGMIndex, 6);
                //}
            }
            if (ownerObject != null)
            {
                //Service.Log.Warning("trying apply to owner object");
                //apply Camerascale[WORKS]
                ownerObject->GameObject.CameraOffset.Y = (ownerObject->GameObject.CameraOffset.Y / (origvalue.CameraScale.GetByRaceAndSex(ownerObject->DrawData.CustomizeData.Race, ownerObject->GetSex()) / 100))* (curvalue.CameraScale.GetByRaceAndSex(ownerObject->DrawData.CustomizeData.Race, ownerObject->GetSex()) / 100);
                ownerObject->GameObject.CameraOffsetTarget.Y = (ownerObject->GameObject.CameraOffsetTarget.Y / (origvalue.CameraScale.GetByRaceAndSex(ownerObject->DrawData.CustomizeData.Race, ownerObject->GetSex()) / 100)) * (curvalue.CameraScale.GetByRaceAndSex(ownerObject->DrawData.CustomizeData.Race, ownerObject->GetSex()) / 100);
                //apply owner tilt[FIX]
                ownerObject->Effects.RiderGroundTiltAngle = curvalue.RiderGroundTilt.TiltAngle;
                ownerObject->Effects.RiderGroundTiltSpeed = curvalue.RiderGroundTilt.TiltSpeed;
                ownerObject->Effects.RiderGroundTiltOrigin = (EffectContainer.TiltOrigin)curvalue.RiderGroundTilt.TiltOrigin;

                ownerObject->Effects.RiderFlightSwimTiltAngle = curvalue.RiderFlightTilt.TiltAngle;
                ownerObject->Effects.RiderFlightSwimTiltSpeed = curvalue.RiderFlightTilt.TiltSpeed;
                ownerObject->Effects.RiderFlightSwimTiltOrigin = (EffectContainer.TiltOrigin)curvalue.RiderFlightTilt.TiltOrigin;
                //ownerObject->Update();
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
    private void MountDismountDetour(MountContainer* a1, char* a2)
    {
        //Service.Log.Error("Dismount Triggered");
        _MountDismountHook!.Original(a1, a2);
        if (lastSoundManager == null) return;
        if (a1->OwnerObject->GetName().ToString() != Service.PlayerState.CharacterName) return;
        foreach (var soundData in lastSoundManager->SoundDataPool->Entries)
        {
            //Service.Log.Error("Current sounddata is  " + soundData.GetFileName().ExtractText());
            if (soundData.GetFileName().ExtractText() == lastbgmpath)
            {
                //Service.Log.Error("Attempting to kill sounddata for " + soundData.GetFileName().ExtractText());
                //TODO:fix this adhering to dfade out time, when I tried to do so it would only work on second dismount;
                soundData.Stop();
                soundData.TryReset();
                break;
            }
        }
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
        this._SetupMountTiltDataHook.Dispose();
        this._SetupRiderTiltDataHook.Dispose();
        this._MountUpdateHook.Dispose();
        this._MountDismountHook.Dispose();
        this._PlayBGMSoundHook.Dispose();
        //this._CheckSigHook.Dispose();

        //Service.AddonLifecycle.UnregisterListener(OnTooltipPreDraw);
        //Service.AddonLifecycle.UnregisterListener(OnTooltipRequestedUpdate);
        //Service.AddonLifecycle.UnregisterListener(OnActionBarRequestedUpdate);
    }
}
