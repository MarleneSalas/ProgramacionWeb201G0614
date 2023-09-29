using Microsoft.AspNetCore.Mvc;
using U1A2.Models.ViewModels;

namespace U1A2.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index(ConversionViewModel c)
        {
            if (c.Moneda == "USD")
            {
                c.Total = c.Importe / 18;
                c.Tipo = "USD";
            }
            else
            {
                c.Total = c.Importe * 18;
                c.Tipo = "MXN";
            }
            return View(c);
        }
    }
}
