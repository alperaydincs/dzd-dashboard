namespace DZDDashboard.Common.DTOs;

public record UpdateEmergencyContactsDto
{
    public List<EmergencyContactDto> EmergencyContacts { get; init; } = [];
}
