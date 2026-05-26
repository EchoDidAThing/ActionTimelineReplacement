using FFXIVClientStructs.FFXIV.Client.Game.Character;
using FFXIVClientStructs.FFXIV.Client.Game.Object;
using InteropGenerator.Runtime.Attributes;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ActionTimelineReplacement.Base.Structs;

[StructLayout(LayoutKind.Explicit, Size = 9072)]
public unsafe partial struct CharacterContainerFAFO
{
    [FieldOffset(6800)]
    public EffectContainerFAFO Effects;
}

