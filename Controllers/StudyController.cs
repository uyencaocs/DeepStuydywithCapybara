using Microsoft.AspNetCore.Mvc;

namespace DeeplearningwithCapybara.Controllers
{
    // [Authorize] - Temporarily removed for UI testing
    public class StudyController : Controller
    {
        // Removed DB Context injection to prevent startup crash

        [HttpGet]
        public IActionResult Index()
        {
            // Return the static mock Capy UI
            return View();
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            return View();
        }
    }
}
