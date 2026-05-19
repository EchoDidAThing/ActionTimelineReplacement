using ActionTimelineReplacement;
using ActionTimelineReplacement.Base;
using ActionTimelineReplacement.Base.Global;
using ActionTimelineReplacement.Sheets;
using Dalamud.Game;
using Dalamud.Game.Addon.Lifecycle;
using Dalamud.Game.Addon.Lifecycle.AddonArgTypes;
using Dalamud.Game.Agent;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
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
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

public unsafe class ShowTooltipDetour : IDisposable
{
    public class HoveredActionInfo
    {
        public DetailKind ActionKind;
        public uint ActionID;
    }
    private static readonly string[] AllowedPreDrawAddons =
    [
        "AreaMap",
        "_NaviMap",
        "_ActionCross",
        "_ActionDoubleCrossL",
        "_ActionDoubleCrossR",
    ];

    private static readonly string[] AllowPreDrawAddonsStarts =
    [
        "_ActionBar"
    ];

    private static readonly string[] AllowTooltips =
    [
        "Action",
        "Item"
    ];


    private delegate byte ItemHoveredDelegate(IntPtr a1, IntPtr* a2, int* containerId, ushort* slotId, IntPtr a5, uint slotIdInt, IntPtr a7);

    private readonly Hook<ItemHoveredDelegate>? _ItemHoveredHook;
    private readonly Hook<AgentActionDetail.Delegates.HandleActionHover>? _ActionHoveredHook;
    private readonly Hook<AtkTooltipManager.Delegates.ShowTooltip>? _ShowTooltipHook;


    private ushort lastParentId = ushort.MaxValue;
    private bool isAllowedPreDraw = false;

    public ShowTooltipDetour()
    {
        _ActionHoveredHook = Service.GameInteropProvider.HookFromAddress<AgentActionDetail.Delegates.HandleActionHover>(AgentActionDetail.Addresses.HandleActionHover.Value, ActionHoveredDetour);
        _ItemHoveredHook = Service.GameInteropProvider.HookFromSignature<ItemHoveredDelegate>(Hooks.itemhoveredhook, ItemHoveredDetour);
        _ShowTooltipHook = Service.GameInteropProvider.HookFromAddress<AtkTooltipManager.Delegates.ShowTooltip>(AtkTooltipManager.Addresses.ShowTooltip.Value, AtkTooltipManagerShowTooltipDetour);

        this._ActionHoveredHook.Enable();
        this._ItemHoveredHook.Enable();
        this._ShowTooltipHook.Enable();

        Service.AddonLifecycle.RegisterListener(AddonEvent.PostRequestedUpdate, "Tooltip", OnTooltipRequestedUpdate);
        Service.AddonLifecycle.RegisterListener(AddonEvent.PreDraw, "Tooltip", OnTooltipPreDraw);
        Service.AddonLifecycle.RegisterListener(AddonEvent.PostRequestedUpdate, "ActionDetail", OnActionTooltipRequestedUpdate);
        Service.AddonLifecycle.RegisterListener(AddonEvent.PreRequestedUpdate, "ActionDetail", OnActionTooltipRequestedUpdate);
    }

    public static HoveredActionInfo HoveredAction{ get; private set; }
    
    private void ActionHoveredDetour(AgentActionDetail* thisPtr, DetailKind actionKind, uint actionId, int flag, bool isLovmActionDetail, int a5, int a6)
    {
        Service.Log.Info("ActionDetail triggered with type" + actionKind + " and ActionID " + FFXIVClientStructs.FFXIV.Client.Game.ActionManager.Instance()->GetAdjustedActionId(actionId));
        try
        {
            _ActionHoveredHook!.Original(thisPtr, actionKind, actionId, flag, isLovmActionDetail, a5, a6);
            HoveredAction = new HoveredActionInfo();
            HoveredAction.ActionKind = actionKind;
            HoveredAction.ActionID = FFXIVClientStructs.FFXIV.Client.Game.ActionManager.Instance()->GetAdjustedActionId(actionId);
        }
        catch (Exception e)
        {
            Service.Log.Error(e, "Error in AgentActionDetailHoverEvent");
        }

    }
    public static InventoryItem HoveredItem { get; private set; }

    private byte ItemHoveredDetour(IntPtr a1, IntPtr* a2, int* containerid, ushort* slotid, IntPtr a5, uint slotidint, IntPtr a7)
    {
        var returnValue = _ItemHoveredHook.Original(a1, a2, containerid, slotid, a5, slotidint, a7);
        HoveredItem = *(InventoryItem*)(a7);
        Service.Log.Info("Hovered item is " + HoveredItem.ItemId);
        return returnValue;
    }


    public const char forbiddenCharacter = '^';

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private TextPayload CreateNewTextPayload(string text)
        => new TextPayload(text);

    public unsafe void ReplaceATKString(AtkTextNode* atkNode, string basestring, string replacestring)
    {
        Service.Log.Info("HERE replaceatkText input text is " + atkNode->NodeText.ToString());
        if (!MakeSeString(atkNode, out Dalamud.Game.Text.SeStringHandling.SeString? seString))
        {
            return;
        }

        Service.Log.Info("HERE SEString is " + seString);
        ReplaceSeString(ref seString, basestring, replacestring);
        Service.Log.Info("HERE ReplaceSEString is " + seString);

        atkNode->SetText(seString.EncodeWithNullTerminator());
        Service.Log.Info("HERE replaceatkText output text is " + atkNode->NodeText.ToString());
    }
    private List<Payload> CreatePayloadsFromReplace(string baseString, string toReplace, string replaceWith)
    {
        List<Payload> newPayloads = [];

        if (baseString.IsNullOrWhitespace())
        {
            return newPayloads;
        }

        Service.Log.Info($"Trying to replace: ['{toReplace}'] with ['{replaceWith}'] in ['{baseString}'].");

        string nodeText = baseString;

        string regString = toReplace.Replace("[", @"^\[").Replace("]", @"^\]\");

        nodeText = Regex.Replace(nodeText, regString, forbiddenCharacter.ToString(), RegexOptions.IgnoreCase);

        Service.Log.Info($"After REGEX Replace of + " + regString + "  is." + nodeText);
        string[] splitStrings = Regex.Split(nodeText, @$"(\{forbiddenCharacter}+)");

        foreach (string splitString in splitStrings)
        {
            if (splitString.IsNullOrWhitespace())
            {
                continue;
            }

            if (splitString != forbiddenCharacter.ToString())
            {
                newPayloads.Add(CreateNewTextPayload(splitString));
            }
            else
            {
                Lumina.Text.SeStringBuilder builder = new Lumina.Text.SeStringBuilder();
                builder.Append(replaceWith);
                newPayloads.AddRange(builder.ToReadOnlySeString().ToDalamudString().Payloads);
            }
        }

        return newPayloads;
    }

    private void ReplaceSeString(ref Dalamud.Game.Text.SeStringHandling.SeString seString, string baseString, string replaceString)
    {
        Service.Log.Info("INSIDE ReplaceSEString is " + seString.Payloads.Count + "payloads");
        for (int i = 0; i < seString.Payloads.Count; i++)
        {
            Payload payload = seString.Payloads[i];

            if (payload.Type != PayloadType.RawText)
            {
                continue;
            }

            if (payload is not TextPayload textPayload)
            {
                continue;
            }

            Service.Log.Info("INSIDE ReplaceSEString checkpayload is " + payload.ToString());
            Service.Log.Info("INSIDE ReplaceSEString textpayload is " + textPayload.Text);
            Service.Log.Info("INSIDE ReplaceSEString base and replace  is " + baseString + " + " + replaceString);
            List<Payload> newPayloads = CreatePayloadsFromReplace(textPayload.Text, baseString, replaceString);

            seString.Payloads.RemoveAt(i);

            seString.Payloads.InsertRange(i, newPayloads);
        }
    }
    private unsafe bool MakeSeString(AtkTextNode* atkNode, [NotNullWhen(true)] out Dalamud.Game.Text.SeStringHandling.SeString? seString)
    {
        seString = null;

        if (atkNode == null)
        {
            return false;
        }

        seString = atkNode->NodeText.AsDalamudSeString();

        return true;
    }
    private void SetText(AtkTextNode* textNode, AtkNineGridNode* backgroundNode, string basestring, string replacestring)
    {
        //if (Service.HoverService.CurrentlyHoveredPet == null)
        //{
        //    Service.Log.Verbose("hoveredpetnull");
        //    return;
        //}
        var BaseString = basestring;
        var ReplaceString = replacestring;

        ReplaceATKString(textNode, BaseString, ReplaceString);
        if (backgroundNode == null)
        {
            return;
        }

        textNode->ResizeNodeForCurrentText();

        backgroundNode->AtkResNode.SetWidth((ushort)(textNode->Width + 18));
    }

    private void OnTooltipPreDraw(AddonEvent addonEvent, AddonArgs addonArgs)
    {
        if (!isAllowedPreDraw)
        {
            return;
        }

        OnTooltipRequestedUpdate(addonEvent, addonArgs);
    }

    private void OnTooltipRequestedUpdate(AddonEvent addonEvent, AddonArgs addonArgs)
    {
        var baseentry = ActionTransientsManager.GetOriginal(HoveredAction.ActionID);
        var replaceentry = ActionTransientsManager.GetReplacement(HoveredAction.ActionID);
        if (baseentry.CompareValues(replaceentry)) { return; }

        AddonTooltip* addonTooltip = (AddonTooltip*)addonArgs.Addon.Address;
        if (addonTooltip == null){ return;}

        AtkTextNode* nametextNode = addonTooltip->GetTextNodeById(2);
        Service.Log.Info("Node Text OnTooltipRequestedUpdateis " + nametextNode->NodeText.ToString());

        if (nametextNode == null) { return; }

        AtkNineGridNode* backgroundNode = (AtkNineGridNode*)addonTooltip->GetNodeById(3);


        if ((ActionTransientsManager.GetReplacement(HoveredAction.ActionID) == ActionTransientsManager.GetOriginal(HoveredAction.ActionID)))
        {
            Service.Log.Error("ActionID: " + HoveredAction.ActionID + " does not have a changed entry to push");
            return;
        }
        Service.Log.Error("Tooltipreqeustupdate triggered");
        if (baseentry.ActionName != replaceentry.ActionName && nametextNode->NodeText.ToString().Split(" [")[0] == baseentry.ActionName)
        {
            Service.Log.Error("TooltipreqeustupdateSETTEXT triggered");
            Service.Log.Error("Name Values at current location: " + baseentry.ActionName + " + " + replaceentry.ActionName);
            SetText(nametextNode, backgroundNode, baseentry.ActionName, replaceentry.ActionName);
        }
    }
    private void HandlePreDraw(string addonName)
    {
        Service.Log.Verbose("addon name is " + addonName);
        isAllowedPreDraw = AllowedPreDrawAddons.Contains(addonName);

        if (isAllowedPreDraw)
        {
            return;
        }

        for (int i = 0; i < AllowPreDrawAddonsStarts.Length; i++)
        {
            if (!addonName.Contains(AllowPreDrawAddonsStarts[i]))
            {
                continue;
            }

            isAllowedPreDraw = true;

            break;
        }
    }
    private void OnActionTooltipRequestedUpdate(AddonEvent addonEvent, AddonArgs addonArgs)
    {
        //HandlePronounChange(addonEvent);
        
        AtkUnitBase* addonActionTooltip =  (AtkUnitBase*)addonArgs.Addon.Address;

        ActionTransientsReplace baseentry = ActionTransientsManager.GetOriginal(HoveredAction.ActionID);
        ActionTransientsReplace replaceentry = ActionTransientsManager.GetReplacement(HoveredAction.ActionID);
        if (baseentry.CompareValues(replaceentry)) { return; }

        if (addonActionTooltip == null) { return; }

        AtkTextNode* nametextNode = addonActionTooltip->GetTextNodeById(5);
        AtkImageNode* ImageNode = addonActionTooltip->GetImageNodeById(20);

        if (nametextNode == null) { return; }

        Service.Log.Error("Actionrequestupdate triggered");
        //find a better way to handl;e
        if (baseentry.ActionName != replaceentry.ActionName && nametextNode->NodeText.ToString().Split(" [")[0] == baseentry.ActionName)
        {
            Service.Log.Error("Actionrequestupdate SETTEXTtriggered");
            Service.Log.Error("Name Values at current location: " + baseentry.ActionName + " + " + replaceentry.ActionName);
            SetText(nametextNode, null, baseentry.ActionName, replaceentry.ActionName);
        }
        if (baseentry.Icon != replaceentry.Icon)
        {
            Service.Log.Error("Actionrequestupdate ICONtriggered");
            Service.Log.Error("Icon Values at current location: " + baseentry.Icon + " + " + replaceentry.Icon);
            //why isn;t this working. what can I do to fix it?
            string iconstring = replaceentry.Icon.ToString().PadLeft(6, '0');
            ImageNode->LoadIconTexture(replaceentry.Icon, 0);
        }
    }
    
    private void RelayShowTooltip(ushort parentId)
    {
        if (lastParentId == parentId)
        {
            return;
        }

        lastParentId = parentId;

        //PetServices.HoverService.SetHoveredPet(null);

        //MapHook.Refresh();

        AtkUnitBase* hoveredOverAddon = AtkStage.Instance()->RaptureAtkUnitManager->GetAddonById(parentId);

        if (hoveredOverAddon == null)
        {
            return;
        }

        if (!hoveredOverAddon->IsFullyLoaded())
        {
            return;
        }

        string addonName = hoveredOverAddon->NameString;

        HandlePreDraw(addonName);

        Service.Log.Verbose($"Showing tooltips: {addonName} {isAllowedPreDraw}");
    }

    

    private void AtkTooltipManagerShowTooltipDetour(AtkTooltipManager* thisPtr, AtkTooltipType type, ushort parentId, AtkResNode* targetNode, AtkTooltipManager.AtkTooltipArgs* tooltipArgs, delegate* unmanaged[Stdcall]<float*, float*, AtkResNode*, void> unkDelegate, bool unk7, bool unk8)
    {
        Service.Log.Info("ShowTooltip triggered with parentId" + parentId + "and type" + type.ToString());
        try
        {
            if (CheckifAllowedType(type.ToString())) { RelayShowTooltip(parentId); }
            _ShowTooltipHook!.Original(thisPtr, type, parentId, targetNode, tooltipArgs, unkDelegate, unk7, unk8);
        }
        catch (Exception e)
        {
            Service.Log.Error(e, "Error in AtkComponentScrollBarReceiveEvent");
        }
    }
    private bool CheckifAllowedType(string types)
    {
        for (int i = 0; i < AllowTooltips.Length; i++)
        {
            if (!types.Contains(AllowTooltips[i]))
            {
                continue;
            }

            return true;

            break;
        }

        return false;
    }

    public void Dispose()
    {
        // When we're done, disable the hook again and clean up. Dispose does this in one go!
        this._ShowTooltipHook.Dispose();
        this._ItemHoveredHook.Dispose();
        this._ActionHoveredHook.Dispose();

        Service.AddonLifecycle.UnregisterListener(OnTooltipPreDraw);
        Service.AddonLifecycle.UnregisterListener(OnTooltipRequestedUpdate);
        Service.AddonLifecycle.UnregisterListener(OnActionTooltipRequestedUpdate);
    }
}
