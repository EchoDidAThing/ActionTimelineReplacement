using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Utility;
using System;
using System.Numerics;
using static Dalamud.Interface.Utility.Raii.ImRaii;
#pragma warning disable CA1416 // Validate platform compatibility

namespace ActionTimelineReplacement.Base.Global;

public class ProcessingGlobals
{

    public static int ScoreString(string s1, string search)
    {
        if (s1.Contains(search, StringComparison.OrdinalIgnoreCase))
        {
            return s1.Length - search.Length;
        }
        return CalcGlobals.LevenshteinDistance(s1, search) + 20;
    }
    public static byte PackBools(bool bool1, bool bool2 = false, bool bool3 = false, bool bool4 = false, bool bool5 = false, bool bool6 = false, bool bool7 = false, bool bool8 = false)
    {
        byte PackedBool = 0;
        if (bool1) { PackedBool += 1; };//01
        if (bool2) { PackedBool += 2; };//02
        if (bool3) { PackedBool += 4; };//04
        if (bool4) { PackedBool += 8; };//08
        if (bool5) { PackedBool += 16; };//10
        if (bool6) { PackedBool += 32; };//20
        if (bool7) { PackedBool += 64; };//40
        if (bool8) { PackedBool += 128; };//80
        //Service.Log.Info("Setting PackedBool to [{id}].",  PackedBool);

        return PackedBool;
    }
}