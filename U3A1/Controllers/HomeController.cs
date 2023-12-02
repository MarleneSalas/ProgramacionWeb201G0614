using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using System.Runtime.InteropServices;
using U3A1.Models.Entities;
using U3A1.Models.ViewModels;
using U3A1.Repositories;

namespace U3A1.Controllers
{
    public class HomeController : Controller
    {

        public HomeController(HamburguesasRepository reposH, Repository<Clasificacion> reposC)
        {
            ReposH = reposH;
            ReposC = reposC;
        }

        public HamburguesasRepository ReposH { get; }
        public Repository<Clasificacion> ReposC { get; }

        public IActionResult Index()
        {
            return View();
        }

        [Route("~/Home/Promociones/{id}")]
        [Route("~/Home/Promociones")]
        public IActionResult Promociones(string id)
        {
            var promociones = ReposH.GetMenuPromos().Select(x => x.Nombre).ToArray();

            if (promociones == null || !promociones.Any())
            {
                return RedirectToAction("Index");
            }

            id = id?.Replace("-", " ") ?? promociones[0];

            var datos = ReposH.GetById(id);
            int currentIndex = Array.FindIndex(promociones, x => x == id);

            PromocionesViewModel vm = new()
            {
                Nombre = id,
                Id = datos.Id,
                Precio = (decimal)datos.Precio,
                PrecioPromocion = datos.PrecioPromocion,
                Descripcion = datos.Descripción,
                Anterior = promociones.ElementAt((currentIndex - 1 + promociones.Length) % promociones.Length),
                Siguiente = promociones.ElementAt((currentIndex + 1) % promociones.Length)
            };

            return View("Promociones",vm);
        }

        public IActionResult Menu(string id)
        {
            MenuViewModel vm;

            vm = new()
            {
                Clasificaciones = ReposC.GetAll().OrderBy(c => c.Nombre).
                 Select(x => new ClasificacionModel
                 {
                     Nombre = x.Nombre,
                     Menus = ReposH.GetMenuByCategoria(x.Nombre).OrderBy(x => x.Nombre).Select(m => new MenuModel
                     {
                         IdMenu = m.Id,
                         Nombre = m.Nombre,
                         Precio = (decimal)m.Precio
                     })
                 })
            };

            if (id == null)
            {
                vm.MenuInfo = ReposH.GetById(vm.Clasificaciones.First().Menus.First().Nombre);
            }
            else
            {
                id = id.Replace("-", " ");
                vm.MenuInfo = ReposH.GetById(id);
            }

            return View(vm);
        }  
    }
}
