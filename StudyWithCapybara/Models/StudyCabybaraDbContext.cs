using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace StudyWithCapybara.Models
{
    public class StudyCabybaraDbContext : IdentityDbContext
    {
        public StudyCabybaraDbContext(DbContextOptions<StudyCabybaraDbContext> options)
            : base(options)
        {
        }
    }
}
