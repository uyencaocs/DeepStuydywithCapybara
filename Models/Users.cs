using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DeeplearningwithCapybara.Models
{
    // Kế thừa từ IdentityUser để sử dụng hệ thống Auth của Microsoft
    // Lưu ý: IdentityUser mặc định dùng kiểu Id là string (Guid). 
    // Nếu bạn muốn dùng int làm khóa chính, hãy dùng: IdentityUser<int>
    public class Users : IdentityUser
    {
        // Các thuộc tính sau ĐÃ CÓ SẴN trong IdentityUser, bạn KHÔNG CẦN viết lại:
        // - Id (Thay thế cho UserId)
        // - UserName (Thay thế cho Username)
        // - Email
        // - PasswordHash

        // Bạn chỉ thêm các thuộc tính mở rộng (Custom Fields) mà Identity không có:
        
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Các mối quan hệ (Navigation Properties) giữ nguyên
        public ICollection<Schedules> Schedules { get; set; } = new List<Schedules>();

        public ICollection<Exams> Exams { get; set; } = new List<Exams>();

        public ICollection<StudyPlans> StudyPlans { get; set; } = new List<StudyPlans>();
    }
}