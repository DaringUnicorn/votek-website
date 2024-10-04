using Microsoft.AspNetCore.Mvc;

namespace votek.Controllers
{
    public class WelcomeController : Controller
    {
        // Project1.cshtml için action method
        public IActionResult Index()
        {
            return View();
        }


    }
}
