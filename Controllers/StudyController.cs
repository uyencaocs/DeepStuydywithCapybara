using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using DeeplearningwithCapybara.Models;

namespace DeeplearningwithCapybara.Controllers
{
    [Authorize]
    public class StudyController : Controller
    {
        private readonly DLWcapybara _context;

        public StudyController(DLWcapybara context)
        {
            _context = context;
        }

        private async Task<Users> GetOrCreateUserAsync()
        {
            var identityUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var username = User.Identity?.Name ?? "Unknown";

            var user = await _context.Users.FirstOrDefaultAsync(u => u.IdentityUserId == identityUserId);
            if (user == null)
            {
                user = new Users
                {
                    IdentityUserId = identityUserId,
                    Username = username,
                    PasswordHash = "OAUTH",
                    Email = username,
                    CreatedAt = DateTime.Now
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            return user;
        }

        public async Task<IActionResult> Index()
        {
            var user = await GetOrCreateUserAsync();
            var plans = await _context.StudyPlans
                .Include(p => p.Courses)
                .Where(p => p.UserId == user.UserId)
                .ToListAsync();

            var progressDict = new Dictionary<int, int>();
            foreach (var p in plans)
            {
                var tasks = await _context.Tasks.Where(t => t.PlanId == p.ExamId).ToListAsync();
                if (tasks.Any())
                {
                    int completed = tasks.Count(t => t.Status == "Completed");
                    progressDict[p.ExamId] = (int)((completed / (double)tasks.Count) * 100);
                }
                else
                {
                    progressDict[p.ExamId] = 0;
                }
            }
            ViewBag.Progress = progressDict;

            return View(plans);
        }

        public async Task<IActionResult> Details(int id)
        {
            var user = await GetOrCreateUserAsync();
            var plan = await _context.StudyPlans
                .Include(p => p.Courses)
                .FirstOrDefaultAsync(p => p.ExamId == id && p.UserId == user.UserId);

            if (plan == null) return NotFound();

            var tasks = await _context.Tasks
                .Where(t => t.PlanId == id)
                .ToListAsync();

            ViewBag.Tasks = tasks;
            return View(plan);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Courses = await _context.Courses.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlan(int CourseId, DateTime ExamDate, string Room, string Note)
        {
            var user = await GetOrCreateUserAsync();
            var plan = new StudyPlans
            {
                UserId = user.UserId,
                CourseId = CourseId,
                ExamDate = ExamDate,
                Room = Room ?? "",
                Note = Note ?? ""
            };
            _context.StudyPlans.Add(plan);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask(int PlanId, string TaskName, DateTime Deadline)
        {
            var task = new Tasks
            {
                PlanId = PlanId,
                TaskName = TaskName,
                Deadline = Deadline,
                Status = "Pending"
            };
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = PlanId });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTaskStatus(int TaskId, string Status)
        {
            var task = await _context.Tasks.FindAsync(TaskId);
            if (task != null)
            {
                task.Status = Status;
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTask(int TaskId, int PlanId)
        {
            var task = await _context.Tasks.FindAsync(TaskId);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Details), new { id = PlanId });
        }
    }
}
