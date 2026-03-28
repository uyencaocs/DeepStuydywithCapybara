using DeeplearningwithCapybara.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace DeeplearningwithCapybara.Models
{
    public class SearchController : Controller
    {
        private readonly DLWcapybara _context;

        public SearchController(DLWcapybara context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GlobalSearch(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                // Trả về mảng rỗng nếu người dùng chưa gõ gì
                return Json(new { plans = new object[] { }, tasks = new object[] { } });
            }

            // Chuyển từ khóa về chữ thường để tìm kiếm không phân biệt hoa/thường
            query = query.ToLower();

            // ==========================================
            // 1. TÌM TRONG BẢNG LỘ TRÌNH (StudyPlans)
            // ==========================================
            var plans = await _context.StudyPlans
                .Include(p => p.Courses) // Bắt buộc Include để lấy được thông tin từ bảng Courses
                                         // LƯU Ý 1: Thay 'CourseName' thành tên cột lưu tên môn học trong file Courses.cs của bạn (VD: Name, Title...)
                .Where(p => p.Courses.CourseName.ToLower().Contains(query) || p.Note.ToLower().Contains(query))
                .Select(p => new
                {
                    id = p.PlanId, // Dùng đúng PlanId theo model của bạn
                    name = "Lộ trình: " + p.Courses.CourseName, // Lấy tên từ bảng Courses
                    time = "Ngày thi: " + p.ExamDate.ToString("dd/MM/yyyy"), // Lấy ngày thi hiển thị cực đẹp
                    icon = "solar:map-arrow-up-bold-duotone",
                    color = "text-[#F4831F]",
                    bg = "bg-amber-100"
                })
                .Take(3) // Giới hạn lấy 3 kết quả để menu không bị quá dài
                .ToListAsync();

            // ==========================================
            // 2. TÌM TRONG BẢNG CÔNG VIỆC (Tasks)
            // ==========================================
            var tasks = await _context.Tasks
                // LƯU Ý 2: Thay 'TaskName' thành tên cột lưu tên công việc trong file Tasks.cs của bạn
                .Where(t => t.TaskName.ToLower().Contains(query))
                .Select(t => new
                {
                    // LƯU Ý 3: Thay 'TaskId' thành tên khóa chính trong file Tasks.cs (Có thể là Id hoặc TaskId)
                    id = t.TaskId,
                    name = t.TaskName,
                    deadline = "Deadline sắp tới", // Nếu bảng Tasks có cột Deadline, bạn đổi thành: t.Deadline.ToString("dd/MM")
                    icon = "solar:check-square-bold-duotone",
                    color = "text-teal-600",
                    bg = "bg-teal-100"
                })
                .Take(3)
                .ToListAsync();

            // Đóng gói và gửi về cho Javascript dưới dạng JSON
            return Json(new { plans = plans, tasks = tasks });
        }
    }
}
