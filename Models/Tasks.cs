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

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool Status { get; set; }

        public int Priority { get; set; }

        public int? CourseId { get; set; }

        [ForeignKey("PlanId")]
        public StudyPlans StudyPlans { get; set; } = null!;
    }
}
