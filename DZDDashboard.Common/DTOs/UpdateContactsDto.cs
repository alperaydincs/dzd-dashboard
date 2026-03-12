using System;

namespace DZDDashboard.Common.DTOs
{
    public class UpdateContactsDto
    {
        public int UserId { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PersonalEmail { get; set; }
        public string? PersonalPhoneNumber { get; set; }
    }
}
