using Microsoft.EntityFrameworkCore;

namespace DeeplearningwithCapybara.Models
{
    public class DLWcapybara(DbContextOptions<DLWcapybara> options) : DbContext(options)
    {
        public DbSet<Users> Users { get; set; } = null!;
        public DbSet<Courses> Courses { get; set; } = null!;
        public DbSet<Schedules> Schedules { get; set; } = null!;
        public DbSet<Exams> Exams { get; set; } = null!;
        public DbSet<StudyPlans> StudyPlans { get; set; } = null!;
        public DbSet<Tasks> Tasks { get; set; } = null!;
    }
}
