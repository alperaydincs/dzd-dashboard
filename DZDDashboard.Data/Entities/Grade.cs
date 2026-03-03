using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DZDDashboard.Data.Entities
{
    public class Grade
    {
        public int Id { get; set; }

        [Required]
        public string Level { get; set; } = string.Empty; 

        [Column(TypeName = "decimal(18,2)")]
        public decimal MinSalary { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal MaxSalary { get; set; }

        [Required]
        public string Currency { get; set; } = "TRY";

        public int? NextStepId { get; set; }
        
        [ForeignKey("NextStepId")]
        public Grade? NextStep { get; set; }
    }
}
