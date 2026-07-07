namespace DZDDashboard.Api.Utils;

public static class FileMagicBytes
{
    private static readonly byte[] Pdf  = [0x25, 0x50, 0x44, 0x46];  
    private static readonly byte[] Png  = [0x89, 0x50, 0x4E, 0x47];
    private static readonly byte[] Jpeg = [0xFF, 0xD8, 0xFF];
    private static readonly byte[] Zip  = [0x50, 0x4B, 0x03, 0x04];   
    private static readonly byte[] Ole  = [0xD0, 0xCF, 0x11, 0xE0];
    public static bool IsValid(byte[] bytes, string mimeType)
    {
        if (bytes is null || bytes.Length < 4) return false;

        return mimeType.ToLowerInvariant() switch
        {
            "application/pdf"  => StartsWith(bytes, Pdf),
            "image/png"        => StartsWith(bytes, Png),
            "image/jpeg"       => StartsWith(bytes, Jpeg),
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document" => StartsWith(bytes, Zip),
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"       => StartsWith(bytes, Zip),
            "application/msword"     => StartsWith(bytes, Ole),
            "application/vnd.ms-excel" => StartsWith(bytes, Ole),
            _ => false
        };
    }

    private static bool StartsWith(byte[] bytes, byte[] signature)
        => bytes.Length >= signature.Length && bytes[..signature.Length].SequenceEqual(signature);
}
