using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CFRApp.Controllers
{
    public class HomeController : Controller
    {
        
        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult Search()
        {
            return View();
        }


    }
}
