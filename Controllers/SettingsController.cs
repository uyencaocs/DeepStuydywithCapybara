using Microsoft.AspNetCore.Mvc;

namespace DeeplearningwithCapybara.Controllers
{
    public class SettingsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
