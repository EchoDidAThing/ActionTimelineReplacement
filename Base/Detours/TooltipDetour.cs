using ActionTimelineReplacement;
using ActionTimelineReplacement.Base;
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
        public FFXIVClientStructs.FFXIV.Client.Enums.DetailKind ActionKind;
        public uint ActionID;
    }
    public class ReplEntry(uint icon, string name, string description, string descenhanced, string tooltip, string jobname, string jobabbreviation)
    {
        public uint Icon = icon;
        public string Name = name;
        public string Description = description;
        public string DescriptionEnhanced = descenhanced;
        public string Tooltip = tooltip;
        public string JobName = jobname;
        public string JobAbbreviation = jobabbreviation;
    }
    public class OrigAndReplace(ReplEntry original, ReplEntry replacement)
    {
        public ReplEntry Original = original;
        public ReplEntry Replacement = replacement;

        public bool Matches()
        {
            if (this.Original.Icon == this.Replacement.Icon && this.Original.Name == this.Replacement.Name && this.Original.Description == this.Replacement.Description 
                && this.Original.DescriptionEnhanced == this.Replacement.DescriptionEnhanced && this.Original.Tooltip == this.Replacement.Tooltip 
                && this.Original.JobName == this.Replacement.JobName && this.Original.JobAbbreviation == this.Replacement.JobAbbreviation) return true;
            return false;
        }
    }

    private static OrigAndReplace GetOrigAndConfByType(string type, uint idx, uint idy)
    {
        uint origicon = 0;
        string origname = "";
        string origdesc = "";
        string origdescenhanced = "";
        string origtooltip = "";
        string origjobname = "";
        string origjobabbreviation = "";

        uint replicon = 0;
        string replname = "";
        string repldesc = "";
        string repldescenhanced = "";
        string repltooltip = "";
        string repljobname = "";
        string repljobabbreviation = "";

        switch (type)
        {
            case ("Action"):
                origicon = ActionTransientsManager.GetOriginal(idx).Icon;
                origname = ActionTransientsManager.GetOriginal(idx).ActionName;
                origdesc = ActionTransientsManager.GetOriginal(idx).ActionDesc;
                origjobname = JobTransientsManager.GetOriginal(idy).Name;
                origjobabbreviation = JobTransientsManager.GetOriginal(idy).Abbreviation;
                replicon = ActionTransientsManager.GetReplacement(idx).Icon;
                replname = ActionTransientsManager.GetReplacement(idx).ActionName;
                repldesc = ActionTransientsManager.GetReplacement(idx).ActionDesc;
                repljobname = JobTransientsManager.GetReplacement(idy).Name;
                repljobabbreviation = JobTransientsManager.GetReplacement(idy).Abbreviation;
                break;
            case ("Mount"):
                origicon = MountTransientsManager.GetOriginal(idx).Icon;
                origname = MountTransientsManager.GetOriginal(idx).Singular;
                origdesc = MountTransientsManager.GetOriginal(idx).Description;
                origdescenhanced = MountTransientsManager.GetOriginal(idx).DescriptionEnhanced;
                origtooltip = MountTransientsManager.GetOriginal(idx).Tooltip;
                replicon = MountTransientsManager.GetReplacement(idx).Icon;
                replname = MountTransientsManager.GetReplacement(idx).Singular;
                repldesc = MountTransientsManager.GetReplacement(idx).Description;
                repldescenhanced = MountTransientsManager.GetReplacement(idx).DescriptionEnhanced;
                repltooltip = MountTransientsManager.GetReplacement(idx).Tooltip;
                break;
            case ("Companion"):
                origicon = CompanionTransientsManager.GetOriginal(idx).Icon;
                origname = CompanionTransientsManager.GetOriginal(idx).Singular;
                origdesc = CompanionTransientsManager.GetOriginal(idx).Description;
                origdescenhanced = CompanionTransientsManager.GetOriginal(idx).DescriptionEnhanced;
                origtooltip = CompanionTransientsManager.GetOriginal(idx).Tooltip;
                replicon = CompanionTransientsManager.GetReplacement(idx).Icon;
                replname = CompanionTransientsManager.GetReplacement(idx).Singular;
                repldesc = CompanionTransientsManager.GetReplacement(idx).Description;
                repldescenhanced = CompanionTransientsManager.GetReplacement(idx).DescriptionEnhanced;
                repltooltip = CompanionTransientsManager.GetReplacement(idx).Tooltip;
                break;
            case ("Ornament"):
                origicon = OrnamentTransientsManager.GetOriginal(idx).Icon;
                origname = OrnamentTransientsManager.GetOriginal(idx).Singular;
                origdesc = OrnamentTransientsManager.GetOriginal(idx).Description;
                replicon = OrnamentTransientsManager.GetReplacement(idx).Icon;
                replname = OrnamentTransientsManager.GetReplacement(idx).Singular;
                repldesc = OrnamentTransientsManager.GetReplacement(idx).Description;
                break;
            case ("Trait"):
                origicon = TraitTransientsManager.GetOriginal(idx).Icon;
                origname = TraitTransientsManager.GetOriginal(idx).Name;
                origdesc = TraitTransientsManager.GetOriginal(idx).Description;
                replicon = TraitTransientsManager.GetReplacement(idx).Icon;
                replname = TraitTransientsManager.GetReplacement(idx).Name;
                repldesc = TraitTransientsManager.GetReplacement(idx).Description;
                break;
            default:
                Service.Log.Error("|" + HoveredAction.ActionKind + "| does not exist in GetOrigAndConfByType");
                return null;
        }

        OrigAndReplace Output = new OrigAndReplace(new ReplEntry(origicon, origname, origdesc, origdescenhanced, origtooltip, origjobname, origjobabbreviation), new ReplEntry(replicon, replname, repldesc, repldescenhanced, repltooltip, repljobname, repljobabbreviation));
        return Output;
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
        Service.AddonLifecycle.RegisterListener(AddonEvent.PostRequestedUpdate, "ActionDetail", OnActionTooltipPostRequestedUpdate);
        Service.AddonLifecycle.RegisterListener(AddonEvent.PreRequestedUpdate, "ActionDetail", OnActionTooltipPreRequestedUpdate);
        
    }

    public static HoveredActionInfo HoveredAction{ get; private set; }
    public static HoveredActionInfo LastHoveredAction { get; private set; }


    private void ActionHoveredDetour(AgentActionDetail* thisPtr, FFXIVClientStructs.FFXIV.Client.Enums.DetailKind actionKind, uint actionId, int flag, bool isLovmActionDetail, int a5, int a6)
    {
        if (LastHoveredAction == null)
        {
            LastHoveredAction = new HoveredActionInfo();
            LastHoveredAction.ActionID = 0;
        }
        Service.Log.Error("LastHoveredAction" + LastHoveredAction.ActionID);
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
    public static InventoryItem LastHoveredItem { get; private set; }

    private byte ItemHoveredDetour(IntPtr a1, IntPtr* a2, int* containerid, ushort* slotid, IntPtr a5, uint slotidint, IntPtr a7)
    {
        var returnValue = _ItemHoveredHook!.Original(a1, a2, containerid, slotid, a5, slotidint, a7);
        HoveredItem = *(InventoryItem*)(a7);
        Service.Log.Info("Hovered item is " + HoveredItem.ItemId);
        return returnValue;
    }


    public const char forbiddenCharacter = '^';

    public unsafe void ReplaceATKString(AtkTextNode* atkNode, string basestring, string replacestring)
    {
        //Service.Log.Info("HERE replaceatkText input text is " + atkNode->NodeText.ToString());
        if (!MakeSeString(atkNode, out ReadOnlySeString? seString))
        {
            return;
        }

        //Service.Log.Info("HERE SEString is " + seString);
        ReplaceSeString(ref seString, basestring, replacestring);
        //Service.Log.Info("HERE ReplaceSEString is " + seString);

        var text = seString;
        using var rssb = new RentedSeStringBuilder();
        atkNode->SetText(
           rssb.Builder
              .Append(text)
              .GetViewAsSpan());
        //Service.Log.Info("HERE replaceatkText output text is " + atkNode->NodeText.ToString());
    }

    public unsafe void ConvertATKString(AtkTextNode* atkNode, string replacestring)
    {
        //Service.Log.Info("HERE replaceatkText input text is " + atkNode->NodeText.ToString());
        //FIXHERE
        //Service.Log.Info("HERE SEString is " + seString);
        //ReadOnlySeString CreatePayloadsFromMacrostring(replacestring);
        //Service.Log.Info("HERE ReplaceSEString is " + seString);

        //atkNode->SetText(CreatePayloadsFromMacrostring(replacestring));

        var text = ReadOnlySeString.FromMacroString(replacestring);
        using var rssb = new RentedSeStringBuilder();
        atkNode->SetText(
           rssb.Builder
              .Append(text)
              .GetViewAsSpan());
        //Service.Log.Info("HERE replaceatkText output text is " + atkNode->NodeText.ToString());
    }
    private ReadOnlySeString CreatePayloadsFromReplace(string baseString, string toReplace, string replaceWith)
    {
        Lumina.Text.SeStringBuilder builder = new Lumina.Text.SeStringBuilder();

        if (baseString.IsNullOrWhitespace())
        {
            return builder.ToReadOnlySeString();
        }

        //Service.Log.Info($"Trying to replace: ['{toReplace}'] with ['{replaceWith}'] in ['{baseString}'].");

        string nodeText = baseString;

        string regString = toReplace.Replace("[", @"^\[").Replace("]", @"^\]\");

        nodeText = Regex.Replace(nodeText, regString, forbiddenCharacter.ToString(), RegexOptions.IgnoreCase);

        //Service.Log.Info($"After REGEX Replace of + " + regString + "  is." + nodeText);
        string[] splitStrings = Regex.Split(nodeText, @$"(\{forbiddenCharacter}+)");

        foreach (string splitString in splitStrings)
        {
            if (splitString.IsNullOrWhitespace())
            {
                continue;
            }

            if (splitString != forbiddenCharacter.ToString())
            {
                builder.Append(splitString);
            }
            else
            {
                builder.Append(replaceWith);
            }
        }

        return builder.ToReadOnlySeString();
    }
    private ReadOnlySeString CreatePayloadsFromMacrostring(string macroString)
    {
        Lumina.Text.SeStringBuilder builder = new Lumina.Text.SeStringBuilder();
        string[] splitstring = macroString.Split('<', '>');

        foreach (string substring in splitstring)
        {

            Service.Log.Error("substring is" + substring);
            if (substring.IsNullOrWhitespace()) {  continue; }
            else if (substring == "br")
            {
                builder.AppendNewLine();
                continue;
            }
            else if (substring.Substring(0,5) == "color")
            {
                builder.PushColorType(Convert.ToUInt32(substring.Split('(')[1].Split(')')[0]));
                continue;
            }
            else if (substring.Substring(0, 8) == "edgecolor")
            {
                builder.PushEdgeColorType(Convert.ToUInt32(substring.Split('(')[1].Split(')')[0]));
                continue;
            }
            else if (substring.Substring(substring.Length-1) != ">")
            {
                builder.Append(substring);
                continue;
            }
            else
            {
                Service.Log.Error("unknown substring type in createpayloadsfrommacrosting of " + substring);
                continue;
            }
        }
        return builder.ToReadOnlySeString();
    }

    private void ReplaceSeString(ref ReadOnlySeString? seString, string baseString, string replaceString)
    {
        Lumina.Text.SeStringBuilder builder = new Lumina.Text.SeStringBuilder();
        //Service.Log.Info("INSIDE ReplaceSEString is " + seString.Payloads.Count + "payloads");
        foreach (var payload in seString)
        {
            Service.Log.Info("INSIDE ReplaceSEString checkpayload is " + payload.ToString() + payload.Type);
            if (payload.Type != ReadOnlySePayloadType.Text)
            {
                Service.Log.Info("notatext");
                builder.Append(payload.ToString());
                continue;
            }
            else if (payload.ToString().Contains(baseString, StringComparison.OrdinalIgnoreCase))
            {
                Service.Log.Info(" payload is " + payload.ToString().Replace(baseString, replaceString, StringComparison.OrdinalIgnoreCase));
                builder.Append(payload.ToString().Replace(baseString, replaceString, StringComparison.OrdinalIgnoreCase));
                continue;
            }
            //Service.Log.Info("INSIDE ReplaceSEString textpayload is " + textPayload.Text);
            //Service.Log.Info("INSIDE ReplaceSEString base and replace  is " + baseString + " + " + replaceString);
            //builder.Append(CreatePayloadsFromReplace(payload.ToString(), baseString, replaceString));
            
        }
        seString = builder.ToReadOnlySeString();
    }
    private unsafe bool MakeSeString(AtkTextNode* atkNode, [NotNullWhen(true)] out ReadOnlySeString? seString)
    {
        seString = null;

        if (atkNode == null)
        {
            return false;
        }

        seString = atkNode->NodeText.AsReadOnlySeString();

        return true;
    }
    private void SetTextNoSwap(AtkTextNode* textNode, string replacestring)
    {
        //if (Service.HoverService.CurrentlyHoveredPet == null)
        //{
        //    Service.Log.Verbose("hoveredpetnull");
        //    return;
        //}
        var ReplaceString = replacestring;

        ConvertATKString(textNode, ReplaceString);
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

        AddonTooltip* addonTooltip = (AddonTooltip*)addonArgs.Addon.Address;
        if (addonTooltip == null){ return;}


        AtkTextNode* nametextNode = addonTooltip->GetTextNodeById(2);
        AtkNineGridNode* backgroundNode = (AtkNineGridNode*)addonTooltip->GetNodeById(3);
        if (nametextNode == null) { return; }

        OrigAndReplace origandreplace = GetOrigAndConfByType(HoveredAction.ActionKind.ToString(), HoveredAction.ActionID, Service.PlayerState.ClassJob.RowId);
        if (origandreplace == null){ return; }
        if (origandreplace.Matches()) { return; }

        if (origandreplace.Original.Name == origandreplace.Replacement.Name)
        {
            //Service.Log.Error("ActionID: " + HoveredAction.ActionID + " does not have a changed entry to push");
            return;
        }
        //Service.Log.Error("Tooltipreqeustupdate triggered");
        if (origandreplace.Original.Name.ToLower() != origandreplace.Replacement.Name.ToLower() && nametextNode->NodeText.ToString().Split(" [")[0].ToLower() == origandreplace.Original.Name.ToLower() && origandreplace.Replacement.Name.ToLower() != "")
        {
            //Service.Log.Error("OnTooltiprequestupdate NAMEtriggered");
            //Service.Log.Error("Name Values at current location: " + origandreplace.Original.Name + " + " + origandreplace.Replacement.Name);
            SetText(nametextNode, backgroundNode, origandreplace.Original.Name, origandreplace.Replacement.Name);
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
    private void OnActionTooltipPreRequestedUpdate(AddonEvent addonEvent, AddonArgs addonArgs)
    {
        Service.Log.Error("Actionrequestupdate triggered");
        //HandlePronounChange(addonEvent);

        AtkUnitBase* addonActionTooltip = (AtkUnitBase*)addonArgs.Addon.Address;
        if (addonActionTooltip == null) { return; }

        OrigAndReplace origandreplace = GetOrigAndConfByType(HoveredAction.ActionKind.ToString(), HoveredAction.ActionID, Service.PlayerState.ClassJob.RowId);
        if (origandreplace == null) { return; }
        if (origandreplace.Matches()) { return; }
        if (origandreplace.Original.JobAbbreviation != origandreplace.Replacement.JobAbbreviation && origandreplace.Replacement.JobAbbreviation != "")
        {
            AtkTextNode* jobtextNode = addonActionTooltip->GetTextNodeById(29);
            Service.Log.Error("Actionrequestupdate ABBREVIATION triggered");
            //Service.Log.Error("desc Values at current location: " + origandreplace.Original.Description + " + " + origandreplace.Replacement.Description);
            SetText(jobtextNode, null, origandreplace.Original.JobAbbreviation, origandreplace.Replacement.JobAbbreviation);
        }
    }
    private void OnActionTooltipPostRequestedUpdate(AddonEvent addonEvent, AddonArgs addonArgs)
    {
        Service.Log.Error("Actionrequestupdate triggered");
        //HandlePronounChange(addonEvent);

        AtkUnitBase* addonActionTooltip = (AtkUnitBase*)addonArgs.Addon.Address;
        if (addonActionTooltip == null) { return; }

        OrigAndReplace origandreplace = GetOrigAndConfByType(HoveredAction.ActionKind.ToString(), HoveredAction.ActionID, Service.PlayerState.ClassJob.RowId);
        if (origandreplace == null) { return; }
        if (origandreplace.Matches()) { return; }

        //if (LastHoveredAction.ActionID == HoveredAction.ActionID ) { return; }
        LastHoveredAction.ActionID = HoveredAction.ActionID;
        Service.Log.Error("Actionrequestupdate triggered");

        //AtkImageNode* ImageNode = ImageNodeComponent->GetImageNodeById(20);
        //AtkImageNode* ImageNode = (AtkImageNode*)(((AtkComponentIcon*)addonActionTooltip->GetNodeById(4))->GetNodeById(20));

        //Service.Log.Error("Name Values at current location: " + origandreplace.Original.Name + " + " + origandreplace.Replacement.Name);
        //Service.Log.Error("Icon Values at current location: " + origandreplace.Original.Icon + " + " + origandreplace.Replacement.Icon);
        //Service.Log.Error("desc Values at current location: " + origandreplace.Original.Description + " + " + origandreplace.Replacement.Description);
        //Service.Log.Error("nodetext: " + nametextNode->NodeText.ToString().Split(" [")[0]);

        if (origandreplace.Original.Name.ToLower() != origandreplace.Replacement.Name.ToLower() && origandreplace.Replacement.Name.ToLower() != "")
        {
            AtkTextNode* nametextNode = addonActionTooltip->GetTextNodeById(5);
            Service.Log.Error("Actionrequestupdate NAMEtriggered");
            if ((nametextNode == null) || (nametextNode->NodeText.ToString().Split(" [")[0].ToLower() != origandreplace.Original.Name.ToLower())) { goto IconReplace; }
            //Service.Log.Error("Name Values at current location: " + origandreplace.Original.Name + " + " + origandreplace.Replacement.Name);
            SetText(nametextNode, null, origandreplace.Original.Name, origandreplace.Replacement.Name);
        }
    IconReplace:
        if (origandreplace.Original.Icon != origandreplace.Replacement.Icon && origandreplace.Replacement.Icon != 0)
        {
            AtkImageNode* ImageNode = addonActionTooltip->GetNodeById(4)->GetAsAtkComponentIcon()->GetImageNodeById(20);
            if (ImageNode == null) { goto DescReplace; }
            //Service.Log.Error("Actionrequestupdate ICONtriggered");
            //Service.Log.Error("Icon Values at current location: " + origandreplace.Original.Icon + " + " + origandreplace.Replacement.Icon);
            //why isn';'t this working. what can I do to fix it?
            //Service.Log.Error("Icon Values at current location: " + );
            //string iconstring = "ui/icon/" + origandreplace.Replacement.Icon.ToString().PadLeft(6, '0').Substring(0, 3).PadRight(6, '0') + "/" + origandreplace.Replacement.Icon.ToString().PadLeft(6, '0') + ".tex";
            //string iconstring = "ui/icon/" + origandreplace.Replacement.Icon.ToString().Substring(0, 3).PadRight(6, '0') + "/" + origandreplace.Replacement.Icon.ToString().PadLeft(6, '0') + ".tex";
            //Service.Log.Error(iconstring);
            ImageNode->LoadIconTexture(origandreplace.Replacement.Icon, 0);
        }
    DescReplace:
        if (origandreplace.Original.Description != origandreplace.Replacement.Description && origandreplace.Replacement.Description != "")
        {
            AtkResNode* backgroundNode = (AtkResNode*)addonActionTooltip->GetNodeById(17);
            AtkResNode* backgroundNode2 = (AtkResNode*)addonActionTooltip->GetNodeById(30)->GetAsAtkComponentWindow()->GetNodeById(2);
            AtkResNode* backgroundNode3 = (AtkResNode*)addonActionTooltip->GetNodeById(21);
            AtkResNode* backgroundNode4 = (AtkResNode*)addonActionTooltip->GetNodeById(27);
            AtkTextNode* desctextNode = addonActionTooltip->GetTextNodeById(19);
            if (desctextNode == null && backgroundNode == null && backgroundNode2 == null && backgroundNode3 == null && backgroundNode4 == null) { return; }
            //Service.Log.Error("Actionrequestupdate DESC triggered");
            //Service.Log.Error("desc Values at current location: " + origandreplace.Original.Description + " + " + origandreplace.Replacement.Description);
            SetTextNoSwap(desctextNode, origandreplace.Replacement.Description);
            desctextNode->ResizeNodeForCurrentText();
            desctextNode->SetWidth(342);
            if (backgroundNode == null) { return; }
            ushort tempheight = (ushort)(backgroundNode->GetHeight() - 6);
            backgroundNode->SetHeight((ushort)(desctextNode->GetHeight() + 6));
            if (backgroundNode2 == null) { return; }
            backgroundNode2->SetHeight((ushort)((backgroundNode->GetHeight() - tempheight) + backgroundNode2->GetHeight()));
            if (backgroundNode3 == null) { return; }
            backgroundNode3->SetYShort((short)((backgroundNode->GetHeight() - tempheight) + backgroundNode3->GetYShort()));
            if (backgroundNode4 == null) { return; }
            backgroundNode4->SetYShort((short)((backgroundNode->GetHeight() - tempheight) + backgroundNode4->GetYShort()));
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
        Service.AddonLifecycle.UnregisterListener(OnActionTooltipPreRequestedUpdate);
        Service.AddonLifecycle.UnregisterListener(OnActionTooltipPostRequestedUpdate);
    }
}
