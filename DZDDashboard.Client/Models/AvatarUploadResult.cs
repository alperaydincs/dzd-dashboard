namespace DZDDashboard.Client.Models;

/// <summary>Result returned by <c>AvatarUploadDialog</c> on successful file selection.</summary>
public sealed record AvatarUploadResult(string FileName, string ContentType, byte[] Content);
