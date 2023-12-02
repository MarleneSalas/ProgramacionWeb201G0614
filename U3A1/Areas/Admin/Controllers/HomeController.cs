using Microsoft.AspNetCore.Mvc;

namespace U3A1.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
