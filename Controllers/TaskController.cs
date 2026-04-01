using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DeeplearningwithCapybara.Models;

namespace DeeplearningwithCapybara.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        private readonly UserManager<Users> _userManager;
        private readonly DLWcapybara _context;

        public TaskController(UserManager<Users> userManager, DLWcapybara context)
        {
            _userManager = userManager;
            _context = context;
        }

        // Hiện thị danh sách công việc của User hiện tại qua các StudyPlan
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            // Lấy tất cả Tasks thuộc về các Plan của User này.
            var tasks = await _context.Tasks
                .Include(t => t.StudyPlans)
                .ThenInclude(p => p.Courses)
                .Where(t => t.StudyPlans.UserId == user.Id)
                .OrderBy(t => t.StartDate)
                .ToListAsync();

            return View(tasks);
        }

        // API: Hoàn thành task -> Status = true
        [HttpPost("task/complete/{id}")]
        public async Task<IActionResult> Complete(int id)
        {
            var task = await GetUserTask(id);
            if (task == null) return NotFound("Không tìm thấy công việc hoặc không có quyền.");

            task.Status = true;
            await _context.SaveChangesAsync();
            return Ok(new { success = true });
        }

        [HttpPost("task/postpone/{id}")]
        public async Task<IActionResult> Postpone(int id)
        {
            var task = await GetUserTask(id);
            if (task == null) return NotFound("Không tìm thấy công việc hoặc không có quyền.");

            task.Status = false;
            task.StartDate = task.StartDate.AddDays(1);
            task.EndDate = task.EndDate.AddDays(1);
            await _context.SaveChangesAsync();
            return Ok(new { success = true, newDate = task.StartDate.ToString("dd/MM/yyyy") });
        }

        // API: Đổi ngày cho 1 task cụ thể
        [HttpPost("task/reschedule/{id}")]
        public async Task<IActionResult> Reschedule(int id, [FromBody] RescheduleRequest req)
        {
            var task = await GetUserTask(id);
            if (task == null) return NotFound("Không tìm thấy công việc.");

            if (DateTime.TryParse(req.NewDate, out DateTime newDate))
            {
                task.Status = false;
                var duration = task.EndDate - task.StartDate;
                task.StartDate = newDate;
                task.EndDate = newDate.Add(duration);
                await _context.SaveChangesAsync();
                return Ok(new { success = true, newDate = task.StartDate.ToString("dd/MM/yyyy") });
            }
            return BadRequest("Ngày không hợp lệ.");
        }

        [HttpPost("task/reschedule-all/{planId}")]
        public async Task<IActionResult> RescheduleAll(int planId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var plan = await _context.StudyPlans.FirstOrDefaultAsync(p => p.PlanId == planId && p.UserId == user.Id);
            if (plan == null) return NotFound("Không tìm thấy kế hoạch.");

            // Lấy các task chưa làm của Plan này
            var pendingTasks = await _context.Tasks
                .Where(t => t.PlanId == planId && t.Status == false)
                .OrderBy(t => t.StartDate)
                .ToListAsync();

            if (!pendingTasks.Any())
                return Ok(new { success = true, message = "Không có task nào cần sắp xếp lại." });

            // Phân bổ lại từ hôm nay đến ExamDate
            var today = DateTime.Today;
            var examDate = plan.ExamDate;
            var totalDays = Math.Max(1, (int)(examDate - today).TotalDays);
            var count = pendingTasks.Count;

            for (int i = 0; i < count; i++)
            {
                var t = pendingTasks[i];
                var daysOffset = (int)Math.Round((double)i / (count - 1 == 0 ? 1 : count - 1) * totalDays);
                var newStart = today.AddDays(daysOffset);
                var dur = t.EndDate - t.StartDate;
                t.StartDate = newStart;
                t.EndDate = newStart.Add(dur);
            }

            await _context.SaveChangesAsync();
            return Ok(new { success = true, rescheduled = count });
        }

        private async Task<DeeplearningwithCapybara.Models.Tasks?> GetUserTask(int taskId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return null;

            return await _context.Tasks
                .Include(t => t.StudyPlans)
                .FirstOrDefaultAsync(t => t.TaskId == taskId && t.StudyPlans.UserId == user.Id);
        }
    }

    public class RescheduleRequest
    {
        public string NewDate { get; set; } = string.Empty;
    }
}
