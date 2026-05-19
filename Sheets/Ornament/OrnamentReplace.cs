using ActionTimelineReplacement.Base.Structs;

namespace ActionTimelineReplacement.Sheets;

public class OrnamentConfig(OrnamentReplace replace, bool enabled)
{
    public bool Enabled = enabled;
    public OrnamentReplace Replacement = replace;
    public static OrnamentConfig CreateEntry(uint key)
    {
        OrnamentReplace originalvalues = OrnamentManager.GetOriginal(key);
        return new OrnamentConfig(originalvalues, false);
    }
}

public class OrnamentReplace(
    sbyte unknown0,
    ushort model,
    ushort action,
    ushort transient,
    byte attachmentPoint,
    byte unknown3,
    byte unknown4)
{
    public sbyte Unknown0 = unknown0;
    public ushort Model = model;
    public ushort Action = action;
    public ushort Transient = transient;
    public byte AttachmentPoint = attachmentPoint;
    public byte Unknown3 = unknown3;
    public byte Unknown4 = unknown4;
    public unsafe void WriteToPointer(OrnamentData* ptr)
    {
        ptr->Unknown0 = Unknown0;
        ptr->Model = Model;
        ptr->Action = Action;
        ptr->Transient = Transient;
        ptr->AttachmentPoint = AttachmentPoint;
        ptr->Unknown3 = Unknown3;
        ptr->Unknown4 = Unknown4;
    }
}
