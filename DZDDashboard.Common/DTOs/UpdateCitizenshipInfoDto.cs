using System;

namespace DZDDashboard.Common.DTOs
{
    public class UpdateCitizenshipInfoDto
    {
        public int UserId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Nationality { get; set; }
        public string? CitizenshipNumber { get; set; }
        public bool DisabilityStatus { get; set; }
        public string? DisabilityDegree { get; set; }
    }
}
