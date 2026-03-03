namespace DZDDashboard.Common.DTOs.Auth;

public record LoginDto(string AccessToken, DateTime ExpiresAtUtc, int Id, string Username, string Email, IEnumerable<string> Roles);