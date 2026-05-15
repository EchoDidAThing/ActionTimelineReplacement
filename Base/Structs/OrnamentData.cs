using System.Runtime.InteropServices;

namespace ActionTimelineReplacement.Base.Structs;

[StructLayout(LayoutKind.Explicit)]
public struct OrnamentData
{
    public enum AttachmentPoints
    {
        root_notworking, //0
        right_forearm, //1
        lower_back, //2
        upper_back_smallscale, //3
        upper_back_offset, //4
        head_unk1, //5
        upper_back_extralargescale, //6
        head_unk2, //7
        head_unk3, //8
        upper_back_standardscale //9
    }

    public enum Unknown4Data
    {
        not_working, //0
        parasol_idle, //1
        racial_idle //2
    }

    [FieldOffset(0xB)]
    public sbyte Unknown0;

    [FieldOffset(0x10)]
    public ushort Model;
    //some IDs can cause a crash (e.g. 2941 and 3000)
    //likely need to restrict this to only available ornament IDs

    [FieldOffset(0x12)]
    public ushort Action;

    [FieldOffset(0x16)]
    public ushort Transient;
    //This is the rowid that is references OrnamentTransient, it controls tooltip description

    [FieldOffset(0x1A)]
    public byte AttachmentPoint;

    [FieldOffset(0x1B)]
    public byte Unknown3;
    //hides ornament in certain cases, like weapon drawn
    //0 and 3+ keep it visible always
    //1 and 2 hide when weapon is drawn. might be other things
    //vanilla only uses 0 and 1 at the moment

    [FieldOffset(0x1C)]
    public byte Unknown4;
    //changes cpose animation while using the ornament

    //Echos note: at some point, we should add a quick jump to the action transient data for names/desc/whatevs
}
