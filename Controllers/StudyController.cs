using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using DeeplearningwithCapybara.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DeeplearningwithCapybara.Controllers
{
    [Authorize]
    public class StudyController : Controller
    {
        private readonly UserManager<Users> _userManager;
        private readonly DLWcapybara _context;

        public StudyController(UserManager<Users> userManager, DLWcapybara context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            
            // Lấy danh sách các môn học mà user này đã lập kế hoạch
            var userCourses = await _context.StudyPlans
                .Where(sp => sp.UserId == userId)
                .Select(sp => sp.Courses)
                .ToListAsync();

            ViewBag.AllCourses = await _context.Courses.ToListAsync();
            return View(userCourses);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Courses course)
        {
            // Loại bỏ kiểm tra validation cho các trường không có trong Form
            ModelState.Remove("Schedules");
            ModelState.Remove("Exams");
            ModelState.Remove("Description");

            if (ModelState.IsValid)
            {
                _context.Courses.Add(course);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Đã thêm môn học mới vào hệ thống! 🐾";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Không thể lưu môn học. Vui lòng kiểm tra lại dữ liệu.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            return View();
        }
    }
}
