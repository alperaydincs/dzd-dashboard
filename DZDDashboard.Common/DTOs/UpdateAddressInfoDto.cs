using System;

namespace DZDDashboard.Common.DTOs
{
    public class UpdateAddressInfoDto
    {
        public int UserId { get; set; }
        public string? LegalAddress { get; set; }
        public string? CurrentAddress { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
    }
}
