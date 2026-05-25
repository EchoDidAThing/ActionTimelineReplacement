using ActionTimelineReplacement.Base.Global;
using ActionTimelineReplacement.Sheets;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Windowing;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Xml.Linq;
#pragma warning disable CA1416 // Validate platform compatibility

namespace ActionTimelineReplacement.Interface;

public sealed partial class ConfigWindow : Window
{
    private void DrawSheetButtons()
    {
        using var child = ImRaii.Child("SheetButtons body", new Vector2(-1f, 161f), false);
        if (!child) return;
        if (_activeSet is null) return;
        ImGui.Columns(5, "###SheetButtonsSelector", false);
        bool firstrun = true;
        foreach (var mainkey in _AllHeaders.Keys)
        {
            if (firstrun) { firstrun = false; }
            else ImGui.NextColumn();
            UiGlobals.DrawSheetButton(mainkey, ref activesheet, _activeSet);
        }

    }
}