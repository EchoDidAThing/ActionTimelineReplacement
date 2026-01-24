using ActionTimelineReplacement.Base;
using ActionTimelineReplacement.Base.Structs;
using System.Data.Common;
using System.Diagnostics.Contracts;
using static Dalamud.Interface.Utility.Raii.ImRaii;
using ActionTimelineReplacement.Base.Global;

namespace ActionTimelineReplacement.Sheets;

public class StatusLoopVFXConfig(StatusLoopVFXReplace replace, bool enabled)
{
    public bool Enabled = enabled;
    public StatusLoopVFXReplace Replacement => replace;
}

public class StatusLoopVFXReplace(
    ushort friendlyVFX,
    ushort stackVFX1,
    ushort stackVFX2,
    ushort enemyVFX,
    byte stackTrigger1,
    byte stackTrigger2,
    byte unknown1,
    byte unknown2,
    bool unknown3,
    bool unknown4,
    bool unknown5)
{
    public ushort FriendlyVFX = friendlyVFX;
    public ushort StackVFX1 = stackVFX1;
    public ushort StackVFX2 = stackVFX2;
    public ushort EnemyVFX = enemyVFX;
    public byte StackTrigger1 = stackTrigger1;
    public byte StackTrigger2 = stackTrigger2;
    public byte Unknown1 = unknown1;
    public byte Unknown2 = unknown2;
    public bool Unknown3 = unknown3;//packedbool1
    public bool Unknown4 = unknown4;//packedbool2
    public bool Unknown5 = unknown5;//packedbool3
    public byte UnknownBitfield1 = ProcessingGlobals.PackBools(unknown3, unknown4, unknown5);
    public unsafe void WriteToPointer(StatusLoopVFXData* ptr)
    {
        ptr->FriendlyVFX = FriendlyVFX;
        ptr->StackVFX1 = StackVFX1;
        ptr->StackVFX2 = StackVFX2;
        ptr->EnemyVFX = EnemyVFX;
        ptr->StackTrigger1 = StackTrigger1;
        ptr->StackTrigger2 = StackTrigger2;
        ptr->Unknown1 = Unknown1;
        ptr->Unknown2 = Unknown2;
        ptr->UnknownBitfield1 = UnknownBitfield1;
    }
}
