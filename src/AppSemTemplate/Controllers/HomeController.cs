using Microsoft.AspNetCore.Mvc;

namespace AppSemTemplate.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
