using Microsoft.AspNetCore.Mvc;

namespace CFRApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
