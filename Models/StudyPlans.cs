using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeeplearningwithCapybara.Models
{
    public class StudyPlans
    {
        [Key]
        public int ExamId { get; set; }

        public int UserId { get; set; }

        public int CourseId { get; set; }

        public DateTime ExamDate { get; set; }

        public string Room { get; set; } = null!;

        public string Note { get; set; } = null!;

        [ForeignKey("UserId")]
        public Users Users { get; set; } = null!;

        [ForeignKey("CourseId")]
        public Courses Courses { get; set; } = null!;
    }
}
