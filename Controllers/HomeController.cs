using System.Diagnostics;
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
