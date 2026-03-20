using System.ComponentModel.DataAnnotations;

namespace DeeplearningwithCapybara.Models
{
    public class Users
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string Username { get; set; } = null!;

        public string Email { get; set; } = null!;

        [Required]
        public string PasswordHash { get; set; } = null!;

        public string? IdentityUserId { get; set; }

        public DateTime CreatedAt { get; set; }

        public ICollection<Schedules> Schedules { get; set; } = null!;

        public ICollection<Exams> Exams { get; set; } = null!;

        public ICollection<StudyPlans> StudyPlans { get; set; } = null!;
    }
}
