using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using U2A1.Models.Entities;
using U2A1.Models.ViewModels;

namespace U2A1.Controllers
{
    public class HomeController : Controller
    {
        
        AnimalesContext contendor = new();
        public IActionResult Index()
        {
            
            var clases = contendor.Clase.OrderBy(x => x.Nombre).
                Select(x => new IndexClasesViewModel
                {
                    Id = x.Id,
                    Nombre = x.Nombre??"",
                    Descripcion = x.Descripcion??""
                }) ;

            return View(clases);
        }

        //[Route("/clases/{c}")]
        public IActionResult Especies(string id)
        {
            id = id.Replace("-"," ");
			var especiesClase = contendor.Clase.Include(x => x.Especies).
                FirstOrDefault(x=>x.Nombre.ToLower()==id.ToLower());

            if(especiesClase == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                EspecieViewModel vm = new()
                {
                    IdClase = especiesClase.Id,
                    NombreClase= especiesClase.Nombre??"",
                    Especies = especiesClase.Especies.Select(x=> new EspecieModel
                    {
                        IdEspecie = x.Id,
                        NombreEspecie = x.Especie
                    })
                };
                return View(vm);
            }

            
        }

        public IActionResult Especie(string id)
        {
            id = id.Replace("-"," ");

            var esp = contendor.Especies.Include(x => x.IdClaseNavigation).
                FirstOrDefault(x=>x.Especie.ToLower()==id.ToLower());

            if (esp == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                EspeciesDetallesViewModel vm = new()
                {
                    Id = esp.Id,
                    IdClase =esp.IdClase,
                    Especie = esp.Especie,
                    Habitat = esp.Habitat??"",
                    Peso = esp.Peso??0,
                    Tamaño = esp.Tamaño??0,
                    Observaciones = esp.Observaciones??"",
                    Clase = esp.IdClaseNavigation?.Nombre??""
                };
                return View(vm);
            }

            
        }
    }
}
