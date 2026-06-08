namespace DZDDashboard.Api.Utils;

/// <summary>
/// Validates image file magic bytes against declared MIME type.
/// Prevents content-type spoofing (e.g. uploading a script with image/jpeg header).
/// </summary>
public static class AvatarMagicBytes
{
    // https://en.wikipedia.org/wiki/List_of_file_signatures
    private static readonly byte[] Jpeg = [0xFF, 0xD8, 0xFF];
    private static readonly byte[] Png  = [0x89, 0x50, 0x4E, 0x47];
    private static readonly byte[] Gif  = [0x47, 0x49, 0x46, 0x38];  // GIF8
    private static readonly byte[] WebP      = [0x52, 0x49, 0x46, 0x46];        // RIFF container
    private static readonly byte[] WebPFourCC = [0x57, 0x45, 0x42, 0x50];     // "WEBP" at bytes 8–11

    public static bool IsValid(byte[] bytes, string mimeType)
    {
        if (bytes is null || bytes.Length < 8) return false;

        return mimeType.ToLowerInvariant() switch
        {
            "image/jpeg" => StartsWith(bytes, Jpeg),
            "image/png"  => StartsWith(bytes, Png),
            "image/gif"  => StartsWith(bytes, Gif),
            // Validate both the RIFF container header AND the WebP FourCC at bytes 8-11.
            // A plain RIFF file (e.g. .avi) would pass RIFF-only check; the FourCC makes it unambiguous.
            "image/webp" => bytes.Length >= 12
                            && StartsWith(bytes, WebP)
                            && bytes[8..12].SequenceEqual(WebPFourCC),
            _            => false
        };
    }

    private static bool StartsWith(byte[] bytes, byte[] signature)
        => bytes.Length >= signature.Length &&
           bytes[..signature.Length].SequenceEqual(signature);
}
