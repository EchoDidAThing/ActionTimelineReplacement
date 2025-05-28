using ActionTimelineReplacement.Hookers.HookersData;

namespace ActionTimelineReplacement.Configurations;
public class PetMirageReplacementConfig(PetMirageReplacement replacement, bool enabled)
{
    public bool Enabled = enabled;
    public PetMirageReplacement Replacement { get; } = replacement;
}

public class PetMirageReplacement(

    //string name,
    ushort unknown0,
    ushort unknown1,
    ushort unknown2,
    byte unknown3,
    ushort unknown4,
    ushort unknown5,
    ushort unknown6,
    byte unknown7,
    float mirageScale
    //int modelChara
    )
{
    //public string petMirageName = name;
    public ushort petMirageUnk0 = unknown0;
    public ushort petMirageUnk1 = unknown1;
    public ushort petMirageUnk2 = unknown2;
    public byte petMirageUnk3 = unknown3;
    public ushort petMirageUnk4 = unknown4;
    public ushort petMirageUnk5 = unknown5;
    public ushort petMirageUnk6 = unknown6;
    public byte petMirageUnk7 = unknown7;
    public float petMirageScale = mirageScale;
    //public int petMirageModelChara = modelChara;

    public unsafe void WriteToPointer(PetMirageData* pointer)
    {
        //pointer->PetMirage_Name = petMirageName;
        pointer->PetMirageUnk0 = petMirageUnk0;
        pointer->PetMirageUnk1 = petMirageUnk1;
        pointer->PetMirageUnk2 = petMirageUnk2;
        pointer->PetMirageUnk3 = petMirageUnk3;
        pointer->PetMirageUnk4 = petMirageUnk4;
        pointer->PetMirageUnk5 = petMirageUnk5;
        pointer->PetMirageUnk6 = petMirageUnk6;
        pointer->PetMirageUnk7 = petMirageUnk7;
        pointer->PetMirageScale = petMirageScale;
        //pointer->PetMirageModelChara = petMirageModelChara;
    }
}
