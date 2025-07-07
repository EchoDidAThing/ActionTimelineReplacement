using System;
using System.Numerics;
using ImGuiNET;
using Dalamud.Interface.Utility;
#pragma warning disable CA1416 // Validate platform compatibility

namespace ActionTimelineReplacement.Base.Items.Global;

public class CalcGlobals
{
    public static float GlobalScale()
    {
        var globalScale = ImGuiHelpers.GlobalScale;
        return globalScale;
    }

    public static float ItemHeight()
    {
        var itemHeight = ImGui.CalcTextSize("").Y + ImGui.GetStyle().FramePadding.Y * 2 + ImGui.GetStyle().WindowPadding.Y;
        return itemHeight;
    }
    public static Vector2 BodyScale()
    {
        var scale = new Vector2(-1, ImGui.GetWindowSize().Y - ImGui.GetCursorPosY() - ItemHeight() - ImGui.GetStyle().WindowPadding.Y);

        return scale;
    }

    public static Vector2 SearchPopScale()
    {
        var scale = new Vector2(XY(), XY());
        return scale;
    }

    public static float XY()
    {
        var xy = GlobalScale() * 200;
        return xy;
    }

    public static int LevenshteinDistance(string s1, string s2)
    {
        var len1 = s1.Length;
        var len2 = s2.Length;
        var dp = new int[len1 + 1, len2 + 1];

        for (var i = 0; i <= len1; i++)
            dp[i, 0] = i;
        for (var j = 0; j <= len2; j++)
            dp[0, j] = j;

        for (var i = 1; i <= len1; i++)
        {
            for (var j = 1; j <= len2; j++)
            {
                var cost = (s1[i - 1] == s2[j - 1]) ? 0 : 1;
                dp[i, j] = Math.Min(Math.Min(
                        dp[i - 1, j] + 1, // Deletion
                        dp[i, j - 1] + 1), // Insertion
                    dp[i - 1, j - 1] + cost); // Substitution
            }
        }

        return dp[len1, len2];
    }
}