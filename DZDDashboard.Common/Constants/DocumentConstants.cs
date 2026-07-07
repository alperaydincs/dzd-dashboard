namespace DZDDashboard.Common.Constants;

public static class DocumentConstants
{
    public const long MaxFileSizeBytes = 20 * 1024 * 1024;
    public static readonly IReadOnlyList<string> AllowedMimeTypes =
    [
        "application/pdf",
        "image/png",
        "image/jpeg",
        "application/msword",       
        "application/vnd.openxmlformats-officedocument.wordprocessingml.document",        
        "application/vnd.ms-excel",        
        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"];
}
