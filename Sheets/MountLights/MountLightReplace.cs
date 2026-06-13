using ActionTimelineReplacement.Base.Global;
using ActionTimelineReplacement.Base.Structs;
using ActionTimelineReplacement.Sheets;
using FFXIVClientStructs.FFXIV.Client.Graphics.Render;
using System.Collections;
using System.Numerics;
using System.Runtime.InteropServices;
using static ActionTimelineReplacement.Base.Structs.EffectContainerFAFO;

namespace ActionTimelineReplacement.Sheets;


public class MountLightConfig(MountLightReplace replace, bool enabled)
{
    public bool Enabled = enabled;
    public MountLightReplace Replacement = replace;
    public static MountLightConfig CreateEntry(uint key)
    {
        MountLightReplace originalvalues = MountLightManager.GetOriginal(key);
        return new MountLightConfig(originalvalues, false);
    }
}

public class MountLightReplace(
    LightShape shape,
    bool visible,
    Vector3 colour,
    float intensity,
    float range,
    float charShadowRange,
    float shadowPlaneNear,
    float shadowPlaneFar,
    LightFalloffType falloffType,
    float falloffFactor,
    Vector2 flatLightSkewAngleDegrees,
    float spotLightAngleDegrees)
{
    public LightShape LightShape = shape;
    //int FloatHeight = floatHeight;
    public bool Visible = visible;
    public Vector3 Colour = colour;
    public float Intensity = intensity;
    public float Range = range;
    public float CharShadowRange = charShadowRange;
    public float ShadowPlaneNear = shadowPlaneNear;
    public float ShadowPlaneFar = shadowPlaneFar;
    public LightFalloffType FalloffType = falloffType;
    public float FalloffFactor= falloffFactor;
    public Vector2 FlatLightSkewAngleDegrees = flatLightSkewAngleDegrees;
    public float SpotLightAngleDegrees = spotLightAngleDegrees;
}