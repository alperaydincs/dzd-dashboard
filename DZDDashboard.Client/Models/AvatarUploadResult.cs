namespace DZDDashboard.Client.Models;

public sealed record AvatarUploadResult(string FileName, string ContentType, byte[] Content);

public sealed record AvatarDialogResult(AvatarUploadResult? File, int? ColorIndex, bool ColorChanged);
