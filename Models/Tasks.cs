using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeeplearningwithCapybara.Models
{
    public class Tasks
    {
        [Key]
        public int TaskId { get; set; }

        public int PlanId { get; set; }

        public string TaskName { get; set; } = null!;

        public DateTime Deadline { get; set; }

        public string Status { get; set; } = null!;

        [ForeignKey("PlanId")]
        public StudyPlans StudyPlans { get; set; } = null!;
    }
}
