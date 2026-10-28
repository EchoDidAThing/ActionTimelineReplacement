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

    private delegate void MountUpdateDelegate(MountContainer* thisptr);
    private delegate void MountDismountDelegate(MountContainer* a1, char* a2);


    private readonly Hook<MountContainer.Delegates.CreateAndSetupMount>? _CreateAndSetupMountHook;
    private readonly Hook<MountContainer.Delegates.SetupMount>? _SetupMountHook;
    private readonly Hook<EffectContainer.Delegates.LoadMountTiltData>? _SetupMountTiltDataHook;
    private readonly Hook<EffectContainer.Delegates.LoadPlayerTiltData>? _SetupRiderTiltDataHook;
    private readonly Hook<MountUpdateDelegate>? _MountUpdateHook;
    private readonly Hook<MountDismountDelegate>? _MountDismountHook;
    private readonly Hook<SoundManager.Delegates.PlayBGMSound>? _PlayBGMSoundHook;

    public Mountstuffs()
    {
        _SetupMountTiltDataHook = Service.GameInteropProvider.HookFromAddress<EffectContainer.Delegates.LoadMountTiltData>(EffectContainer.Addresses.LoadMountTiltData.Value, SetupMountTiltDataDetour);
        _SetupRiderTiltDataHook = Service.GameInteropProvider.HookFromAddress<EffectContainer.Delegates.LoadPlayerTiltData>(EffectContainer.Addresses.LoadPlayerTiltData.Value, SetupRiderTiltDataDetour);
        _MountUpdateHook = Service.GameInteropProvider.HookFromSignature<MountUpdateDelegate>(Hooks.MountUpdate, MountUpdateDetour);
        _PlayBGMSoundHook = Service.GameInteropProvider.HookFromAddress<SoundManager.Delegates.PlayBGMSound>(SoundManager.Addresses.PlayBGMSound.Value, PlayBGMSoundDetour);
        _MountDismountHook = Service.GameInteropProvider.HookFromSignature<MountDismountDelegate>(Hooks.MountDismount, MountDismountDetour);
        
        this._SetupMountTiltDataHook.Enable();
        this._SetupRiderTiltDataHook.Enable();
        this._MountUpdateHook.Enable();
        this._PlayBGMSoundHook.Enable();
        this._MountDismountHook.Enable();

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
        Service.Log.Warning("Mount Id is " + MountID);
        var origvalue = MountDetourManager.GetOriginal(MountID);
        curvalue = MountDetourManager.GetReplacement(MountID);
        var outpath = path;
        lastSoundManager = thisptr;
        lastbgmpath = origvalue.BGMPath;
        trykill = true;
        if ((path.ToString() != curvalue.BGMPath) && (path.ToString() == origvalue.BGMPath) && (MountID!= 0) && (origvalue != curvalue))
        {
            outpath = ProcessingGlobals.PathCString(curvalue.BGMPath);
            lastbgmpath = curvalue.BGMPath;
            Service.Log.Info("replaced mountBGMpath with" + curvalue.BGMPath);
        }
        var retvalue  = _PlayBGMSoundHook!.Original(thisptr, outpath);
        return retvalue;
    }
    private void SetupRiderTiltDataDetour(EffectContainer* thisptr)
    {
        _SetupRiderTiltDataHook!.Original(thisptr);
        var ownerObject = thisptr->OwnerObject->GetAsCharacter();
        if (ownerObject == null) return;
        if (ownerObject->Mount.MountObject == null) return;
        if ((ownerObject->GetName().ToString() == Service.PlayerState.CharacterName) &&
            (ownerObject->HomeWorld == Service.PlayerState.HomeWorld.RowId) && (ownerObject->IsMounted() == true) &&
            (ownerObject->Mount.MountId != 0))
        {
            var curvalue = MountDetourManager.GetReplacement(ownerObject->Mount.MountId);
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

    private void MountUpdateDetour(MountContainer* thisptr)
    {
        _MountUpdateHook!.Original(thisptr);
        var mountObject = thisptr->MountObject;
        var ownerObject = thisptr->OwnerObject;
        if ((ownerObject->GetName().ToString() == Service.PlayerState.CharacterName) 
            && (ownerObject->HomeWorld == Service.PlayerState.HomeWorld.RowId) 
            && (ownerObject->IsMounted() == true) && (thisptr->MountId != 0))
        {
            var origvalue = MountDetourManager.GetOriginal(thisptr->MountId);
            var curvalue = MountDetourManager.GetReplacement(thisptr->MountId);
            if (mountObject != null)
            {
                mountObject->Scale = (curvalue.Scale.GetByRaceAndSex(ownerObject->DrawData.CustomizeData.Race, ownerObject->GetSex())/100f);
                
                MountID = thisptr->MountId;
            }
            if (ownerObject != null)
            {
                ownerObject->GameObject.CameraOffset.Y = (ownerObject->GameObject.CameraOffset.Y / (origvalue.CameraScale.GetByRaceAndSex(ownerObject->DrawData.CustomizeData.Race, ownerObject->GetSex()) / 100))* (curvalue.CameraScale.GetByRaceAndSex(ownerObject->DrawData.CustomizeData.Race, ownerObject->GetSex()) / 100);
                ownerObject->GameObject.CameraOffsetTarget.Y = (ownerObject->GameObject.CameraOffsetTarget.Y / (origvalue.CameraScale.GetByRaceAndSex(ownerObject->DrawData.CustomizeData.Race, ownerObject->GetSex()) / 100)) * (curvalue.CameraScale.GetByRaceAndSex(ownerObject->DrawData.CustomizeData.Race, ownerObject->GetSex()) / 100);
            }

        }
    }
    private void MountDismountDetour(MountContainer* a1, char* a2)
    {
        _MountDismountHook!.Original(a1, a2);
        if (lastSoundManager == null) return;
        if (a1->OwnerObject->GetName().ToString() != Service.PlayerState.CharacterName) return;
        foreach (var soundData in lastSoundManager->SoundDataPool->Entries)
        {
            if (soundData.GetFileName().ExtractText() == lastbgmpath)
            {
                //TODO:fix this adhering to dfade out time, when I tried to do so it would only work on second dismount;
                soundData.Stop();
                soundData.TryReset();
                break;
            }
        }
    }


    public void Dispose()
    {
        // When we're done, disable the hook again and clean up. Dispose does this in one go!
        this._SetupMountTiltDataHook.Dispose();
        this._SetupRiderTiltDataHook.Dispose();
        this._MountUpdateHook.Dispose();
        this._MountDismountHook.Dispose();
        this._PlayBGMSoundHook.Dispose();
    }
}
