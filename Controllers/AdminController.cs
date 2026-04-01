using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DeeplearningwithCapybara.Models;

namespace DeeplearningwithCapybara.Controllers
{
    [Authorize(Roles = SD.Role_Admin)]
    public class AdminController : Controller
    {
        private readonly DLWcapybara _context;
        private readonly UserManager<Users> _userManager;

        public AdminController(DLWcapybara context, UserManager<Users> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TotalUsers = (await _userManager.GetUsersInRoleAsync(SD.Role_Customer)).Count;
            ViewBag.TotalCourses = await _context.Courses.CountAsync();
            ViewBag.TotalPlans = await _context.StudyPlans.CountAsync();
            
            return View();
        }

        // ─── QL HỌC VIÊN (CRUD) ────────────────────────────────────────────────
        public async Task<IActionResult> Users()
        {
            var users = await _userManager.GetUsersInRoleAsync(SD.Role_Customer);
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
                TempData["SuccessMessage"] = "Đã xóa học viên thành công.";
            }
            return RedirectToAction(nameof(Users));
        }

        // ─── QL MÔN HỌC (CRUD) ────────────────────────────────────────────────
        public async Task<IActionResult> Courses()
        {
            var courses = await _context.Courses.ToListAsync();
            return View(courses);
        }

        [HttpGet]
        public IActionResult CreateCourse()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse(Courses course)
        {
            ModelState.Remove("StudyPlans");
            if (ModelState.IsValid)
            {
                _context.Courses.Add(course);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Thêm môn học thành công.";
                return RedirectToAction(nameof(Courses));
            }
            return View(course);
        }

        [HttpGet]
        public async Task<IActionResult> EditCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();
            return View(course);
        }

        [HttpPost]
        public async Task<IActionResult> EditCourse(Courses course)
        {
            ModelState.Remove("StudyPlans");
            if (ModelState.IsValid)
            {
                _context.Courses.Update(course);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Cập nhật môn học thành công.";
                return RedirectToAction(nameof(Courses));
            }
            return View(course);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Đã xóa môn học.";
            }
            return RedirectToAction(nameof(Courses));
        }
    }
}
