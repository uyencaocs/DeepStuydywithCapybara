using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeeplearningwithCapybara.Models
{
    public class Exams
    {
        [Key]
        public int ExamId { get; set; }

        public string UserId { get; set; } = null!;

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
