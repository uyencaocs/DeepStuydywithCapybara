using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DeeplearningwithCapybara.Models
{
    public class DLWcapybara : IdentityDbContext<Users>
    {
        public DLWcapybara(DbContextOptions<DLWcapybara> options) : base(options)
        {
        }

     
        public DbSet<Courses> Courses { get; set; } = null!;
        public DbSet<Schedules> Schedules { get; set; } = null!;
        public DbSet<Exams> Exams { get; set; } = null!;
        public DbSet<StudyPlans> StudyPlans { get; set; } = null!;
        public DbSet<Tasks> Tasks { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
         
        }
    }
}