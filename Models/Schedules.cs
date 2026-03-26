using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeeplearningwithCapybara.Models
{
    public class Schedules
    {
        [Key]
        public int ScheduleId { get; set; }

        public string UserId { get; set; } = null!;

        public int CourseId { get; set; }

        public string DayOfWeek { get; set; } = null!;

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public string Room { get; set; } = null!;

        [ForeignKey("UserId")]
        public Users Users { get; set; } = null!;

        [ForeignKey("CourseId")]
        public Courses Courses { get; set; } = null!;
    }
}
