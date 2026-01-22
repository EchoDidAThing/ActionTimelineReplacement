using ActionTimelineReplacement.Base.Structs;
using System.Runtime.InteropServices;
using System.Text;
using ActionTimelineReplacement.Base;
using System;


namespace ActionTimelineReplacement.Sheets;

public class VfxConfig(VfxReplace replace, bool enabled)
{
    public bool Enabled = enabled;
    public VfxReplace Replacement => replace;
}

public class VfxReplace(uint rowId,
     string string1)
{
    public uint RowId = rowId;
    public string String1 = string1;

    public unsafe void WriteSEString(IntPtr ptr)
    {
        var data = ptr;
        var stringbytes = Encoding.UTF8.GetBytes(String1);
        var offset = Marshal.ReadByte(data);
        Marshal.Copy(stringbytes, 0, data + offset, stringbytes.Length);
        Marshal.WriteByte(data + offset +stringbytes.Length, 0);
    }
}
