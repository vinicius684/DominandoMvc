using Microsoft.AspNetCore.Mvc;

namespace AppSemTemplate.Controllers
{
    public class TesteController : Controller
    {
        public IActionResult Index()
        {
            return PartialView();
        }
    }
}
