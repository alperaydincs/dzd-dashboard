using System.ComponentModel.DataAnnotations;
namespace DZDDashboard.Common.DTOs.Users;

public record CreateUserDto
{
    [Required, EmailAddress, StringLength(256)]
    public string Email { get; init; } = null!;

    [Required, MinLength(6)]
    public string Password { get; init; } = null!;

    [Required, StringLength(64)]
    public string FirstName { get; init; } = null!;

    [Required, StringLength(64)]
    public string LastName { get; init; } = null!;

    [Required]
    public string RegistrationNumber { get; init; } = null!;

    [Required]
    public DateTime? UserStartDate { get; init; }
}