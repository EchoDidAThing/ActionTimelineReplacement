using ActionTimelineReplacement.Base.Global;
using ActionTimelineReplacement.Base.Setups;
using ActionTimelineReplacement.Sheets;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using Dalamud.Utility;
using Lumina.Excel.Sheets;
using Lumina.Excel.Sheets.Experimental;
using System;
using System.Numerics;
using System.Threading.Channels;
#pragma warning disable CA1416 // Validate platform compatibility

namespace ActionTimelineReplacement.Interface;

public sealed partial class ConfigWindow : Window
{
    private void DrawBodyMain()
    {
        using var child = ImRaii.Child("Main body", new Vector2(-1f, 335f), false);
        if (!child) return;
        if (_activeSet is null) return;

        using (ImRaii.PushFont(GetFont(20)))
        {
            var width = ImGui.CalcTextSize(_activeSet.Name).X + ImGui.GetStyle().FramePadding.X * 2;
            var x = ImGui.GetWindowWidth() / 2 - width / 2;

            ImGui.SetCursorPosX(x);
            ImGui.SetNextItemWidth(width);

            if (ImGui.InputText("##Name", ref _activeSet.Name, 256))
            {
                Service.Config.Save();
            }
        }

        if (ImGui.Checkbox("Enable", ref _activeSet.Enabled))
        {
            Setup.SetupAll();
            Service.Config.Save();
        }
        ImGui.SameLine();
        ImGui.SetNextItemWidth(50 * CalcGlobals.GlobalScale());

        if (ImGui.DragInt("Priority", ref _activeSet.Priority))
        {
            Setup.SetupAll();
            Service.Config.Save();
        }

        if (ImGui.InputText("name", ref _activeSet.CharacterName, 512, ImGuiInputTextFlags.EnterReturnsTrue))
        {
            Service.Config.Save();
        }
        ImGui.SameLine();
        if (ImGui.Button("Current##name"))
        {
            string playername = Service.PlayerState.CharacterName;
            if (playername.IsNullOrEmpty()) { return; }
            _activeSet.CharacterName = playername;
        }

        if (ImGui.InputUInt("HomeWorld", ref _activeSet.HomeWorld, 0, 0, default, ImGuiInputTextFlags.EnterReturnsTrue))
        {
            Service.Config.Save();
        }

        ImGui.SameLine();
        if (ImGui.Button("Current##world"))
        {
            uint worldname = Service.PlayerState.HomeWorld.RowId;
            _activeSet.HomeWorld = worldname;
        }

        if (Service.DataManager.GetExcelSheet<Lumina.Excel.Sheets.World>().TryGetRow(_activeSet.HomeWorld, out var worldRow))
        {
            using (ImRaii.PushFont(UiBuilder.IconFont))
            ImGui.Text($"{FontAwesomeIcon.ArrowTurnUp.ToIconString()}");
            ImGui.SameLine();
            ImGui.Text(worldRow.Name.ToString());
        }
        else
        {
            ImGui.SameLine();
            ImGui.Text("Invalid world.");
        }

        ImGui.NewLine();

        DrawJobSelectors(ref _activeSet);
    }

    private void DrawJobSelectors(ref Configuration.ReplacementSet activeset)
    {
        ImGui.Columns(Math.Max(3, (int)(ImGui.GetWindowWidth() / (80 * ImGui.GetIO().FontGlobalScale))), "###equipAsClassList", false);
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.GLA, "GLA", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.MRD, "MRD", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.CNJ, "CNJ", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.PGL, "PGL", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.LNC, "LNC", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.ROG, "ROG", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.ARC, "ARC", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.THM, "THM", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.ACN, "ACN", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.PLD, "PLD", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.WAR, "WAR", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.DRK, "DRK", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.GNB, "GNB", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.WHM, "WHM", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.SCH, "SCH", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.AST, "AST", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.SGE, "SGE", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.MNK, "MNK", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.DRG, "DRG", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.NIN, "NIN", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.SAM, "SAM", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.RPR, "RPR", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.VPR, "VPR", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.BST, "BST", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.BRD, "BRD", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.MCH, "MCH", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.DNC, "DNC", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.BLM, "BLM", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.SMN, "SMN", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.RDM, "RDM", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.PCT, "PCT", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.BLU, "BLU", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.CRP, "CRP", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.BSM, "BSM", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.ARM, "ARM", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.GSM, "GSM", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.LTW, "LTW", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.WVR, "WVR", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.ALC, "ALC", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.CUL, "CUL", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.MIN, "MIN", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.BTN, "BTN", activeset.Name);
        ImGui.NextColumn();
        UiGlobals.DrawClassJobBool(ref activeset.Jobs.FSH, "FSH", activeset.Name);
    }
        
}