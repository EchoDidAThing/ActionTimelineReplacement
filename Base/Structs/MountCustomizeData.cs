using System.Runtime.InteropServices;

namespace ActionTimelineReplacement.Base.Structs;

[StructLayout(LayoutKind.Explicit)]
public struct MountCustomizeData
{
    [FieldOffset(0x0)]
    public ushort HyurMidlanderMaleScale;

    [FieldOffset(0x2)]
    public ushort HyurMidlanderFemaleScale;

    [FieldOffset(0x4)]
    public ushort HyurHighlanderMaleScale;

    [FieldOffset(0x6)]
    public ushort HyurHighlanderFemaleScale;

    [FieldOffset(0x8)]
    public ushort ElezenMaleScale;

    [FieldOffset(0xA)]
    public ushort ElezenFemaleScale;

    [FieldOffset(0xC)]
    public ushort LalafellMaleScale;

    [FieldOffset(0xE)]
    public ushort LalafellFemaleScale;

    [FieldOffset(0x10)]
    public ushort MiqoteMaleScale;

    [FieldOffset(0x12)]
    public ushort MiqoteFemaleScale;

    [FieldOffset(0x14)]
    public ushort RoegadynMaleScale;

    [FieldOffset(0x16)]
    public ushort RoegadynFemaleScale;

    [FieldOffset(0x18)]
    public ushort AuRaMaleScale;

    [FieldOffset(0x1A)]
    public ushort AuRaFemaleScale;

    [FieldOffset(0x1C)]
    public ushort HrothgarMaleScale;

    [FieldOffset(0x1E)]
    public ushort HrothgarFemaleScale;

    [FieldOffset(0x20)]
    public ushort VieraMaleScale;

    [FieldOffset(0x22)]
    public ushort VieraFemaleScale;

    [FieldOffset(0x24)]
    public ushort HyurMidlanderMaleCameraHeight;

    [FieldOffset(0x26)]
    public ushort HyurMidlanderFemaleCameraHeight;

    [FieldOffset(0x28)]
    public ushort HyurHighlanderMaleCameraHeight;

    [FieldOffset(0x2A)]
    public byte HyurHighlanderFemaleCameraHeight;

    [FieldOffset(0x2B)]
    public byte ElezenMaleCameraHeight;

    [FieldOffset(0x2C)]
    public byte ElezenFemaleCameraHeight;

    [FieldOffset(0x2D)]
    public byte LalafellMaleCameraHeight;

    [FieldOffset(0x2E)]
    public byte LalafellFemaleCameraHeight;

    [FieldOffset(0x2F)]
    public byte MiqoteMaleCameraHeight;

    [FieldOffset(0x30)]
    public byte MiqoteFemaleCameraHeight;

    [FieldOffset(0x31)]
    public byte RoegadynMaleCameraHeight;

    [FieldOffset(0x32)]
    public byte RoegadynFemaleCameraHeight;

    [FieldOffset(0x33)]
    public byte AuRaMaleCameraHeight;

    [FieldOffset(0x34)]
    public byte AuRaFemaleCameraHeight;

    [FieldOffset(0x35)]
    public byte HrothgarMaleCameraHeight;

    [FieldOffset(0x36)]
    public byte HrothgarFemaleCameraHeight;

    [FieldOffset(0x37)]
    public byte VieraMaleCameraHeight;

    [FieldOffset(0x38)]
    public byte VieraFemaleCameraHeight;

    [FieldOffset(0x39)]
    public byte Unknown0;

    [FieldOffset(0x3A)]
    public byte Unknown1;

    [FieldOffset(0x3B)]
    public byte Unknown2;

    [FieldOffset(0x3C)]
    public byte Unknown3;

    [FieldOffset(0x3D)]
    public byte Unknown4;

    [FieldOffset(0x3E)]
    public byte Unknown5;
}
