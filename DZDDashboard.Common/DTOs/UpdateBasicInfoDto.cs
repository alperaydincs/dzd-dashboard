using System;
using System.Collections.Generic;

namespace DZDDashboard.Common.DTOs
{
    public class UpdateBasicInfoDto
    {
        public int UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? RegistrationNumber { get; set; }
        public DateTime? UserStartDate { get; set; }
        public DateTime? PositionStartDate { get; set; }
        public string? ContractType { get; set; }
        public DateTime? ContractEndDate { get; set; }
        public string? WorkModel { get; set; }
        public int? PayrollLocationId { get; set; }
    }
}
