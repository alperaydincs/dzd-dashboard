namespace DZDDashboard.Common.DTOs;

public class UpdateEmergencyContactsDto
{
    public int UserId { get; set; }
    public List<EmergencyContactDto> EmergencyContacts { get; set; } = new();
}
