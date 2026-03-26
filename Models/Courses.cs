using System.ComponentModel.DataAnnotations;

namespace DeeplearningwithCapybara.Models
{
    public class Courses
    {
        [Key]
        public int CourseId { get; set; }

        [Required]
        public string CourseName { get; set; } = null!;

        public int Credits { get; set; }

        public string? Description { get; set; }

        public ICollection<Schedules> Schedules { get; set; } = null!;

        public ICollection<Exams> Exams { get; set; } = null!;
    }
}
