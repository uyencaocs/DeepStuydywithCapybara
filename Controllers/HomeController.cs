using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using DeeplearningwithCapybara.Models;
using Microsoft.AspNetCore.Mvc;

namespace DeeplearningwithCapybara.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            var hour = DateTime.Now.Hour;
            string greeting;
            string subGreeting;

            if (hour >= 5 && hour < 12)
            {
                greeting = "Chào buổi sáng! ☀️";
                subGreeting = "Chúc bạn một ngày học tập đầy năng lượng.";
            }
            else if (hour >= 12 && hour < 18)
            {
                greeting = "Chào buổi chiều! ☕";
                subGreeting = "Đừng quên nghỉ ngơi một chút giữa các task nhé.";
            }
            else
            {
                greeting = "Chào buổi tối! ✨";
                subGreeting = "Sắp xếp xong việc rồi nghỉ ngơi cùng Capy thôi.";
            }

            ViewBag.Greeting = greeting;
            ViewBag.SubGreeting = subGreeting;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Profile()
        {
            // Populate view data from the current user (claims when available)
            var displayName = User?.Identity != null && User.Identity.IsAuthenticated
                ? User.Identity.Name
                : "User";

            var email = User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ?? "";
            var joinDate = User?.Claims?.FirstOrDefault(c => c.Type == "JoinDate")?.Value ?? "";
            var major = User?.Claims?.FirstOrDefault(c => c.Type == "Major")?.Value ?? "";

            ViewBag.DisplayName = displayName;
            ViewBag.Email = email;
            ViewBag.JoinDate = joinDate;
            ViewBag.Major = major;

            // MVC will look for Views/Home/Profile.cshtml then Views/Shared/Profile.cshtml
            return View("Profile");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



    }
}
