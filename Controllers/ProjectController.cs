using Microsoft.AspNetCore.Mvc;

namespace votek.Controllers
{
    public class ProjectController : Controller
    {
        // Project1.cshtml için action method
        public IActionResult Project1()
        {
            return View();
        }

        // Project2.cshtml için action method
        public IActionResult Project2()
        {
            return View();
        }

        // Project3.cshtml için action method
        public IActionResult Project3()
        {
            return View();
        }
    }
}
