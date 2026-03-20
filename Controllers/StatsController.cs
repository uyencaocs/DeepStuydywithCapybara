using Microsoft.AspNetCore.Mvc;

namespace DeeplearningwithCapybara.Controllers
{
    public class StatsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
