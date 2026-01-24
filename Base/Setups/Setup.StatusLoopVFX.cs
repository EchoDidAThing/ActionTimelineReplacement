using ActionTimelineReplacement.Base.Global;
using ActionTimelineReplacement.Sheets;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ActionTimelineReplacement.Base.Setups;

public static unsafe partial class Setup
{
    public static void LoadStatusLoopVFX(IEnumerable<uint> idx, bool reset = false)
    {
        foreach (var key in idx)
        {
            SetStatusLoopVFX(key, reset);
        }
    }
    public static void SetStatusLoopVFX(uint id, bool reset = false)
    {
        var data = Hooks.GetStatusLoopVFXData(id);
        var replacement = reset
            ? StatusLoopVFXManager.GetOriginal(id)
            : StatusLoopVFXManager.GetReplacement(id);

        Service.Log.Info("Setting statusLoopVFX data for [{id}].", id);


        replacement.UnknownBitfield1 = ProcessingGlobals.PackBools(replacement.Unknown3, replacement.Unknown4, replacement.Unknown5);
        replacement.WriteToPointer(data);

        //READ MEMORY FOR TESTS
        /*var data2 = Hooks.GetStatusLoopVFXDatapublic(id);
        for (int i = 0; i < 13; i++)
        {
            Service.Log.Info("Value of byte [{id}] is [{value}]", i, Marshal.ReadByte(data2 + i).ToString());
        }
        Service.Log.Info("Value of PackedBool is [{value}] and individuals is[{1}] [{2}] [{3}]and the expected outcome is", replacement.UnknownBitfield1, replacement.Unknown3, replacement.Unknown4, replacement.Unknown5, ProcessingGlobals.PackBools(replacement.Unknown3, replacement.Unknown4, replacement.Unknown5).ToString());*/
    }
}