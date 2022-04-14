namespace PsdParser
{
    [Flags]
    public enum LayerFlags : byte
    {
        TransparencyProtected = 1,
        Visible = 2,
        Obsolete = 4,
        Photoshop5OrLater = 8,
        PixelData = 16,

    }
}