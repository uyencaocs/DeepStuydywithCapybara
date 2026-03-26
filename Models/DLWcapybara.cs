using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DeeplearningwithCapybara.Models
{
    // 1. Thay đổi DbContext thành IdentityDbContext<Users>
    public class DLWcapybara : IdentityDbContext<Users>
    {
        public DLWcapybara(DbContextOptions<DLWcapybara> options) : base(options)
        {
        }

        // 2. Bỏ DbSet<Users> vì IdentityDbContext đã có sẵn bảng AspNetUsers rồi
        // Tuy nhiên, bạn vẫn giữ các bảng nghiệp vụ khác bên dưới:
        
        public DbSet<Courses> Courses { get; set; } = null!;
        public DbSet<Schedules> Schedules { get; set; } = null!;
        public DbSet<Exams> Exams { get; set; } = null!;
        public DbSet<StudyPlans> StudyPlans { get; set; } = null!;
        public DbSet<Tasks> Tasks { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Bạn có thể tùy chỉnh tên bảng Identity ở đây nếu muốn (Ví dụ: đặt tên tiếng Việt)
            // builder.Entity<Users>().ToTable("NguoiDung");
        }
    }
}