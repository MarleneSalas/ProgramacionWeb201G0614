using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using U2A2.Models.Entities;
using U2A2.Models.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace U2A2.Controllers
{
	public class HomeController : Controller
	{
		PerrosContext context = new();
		[Route("~/")]
		[Route("~/Razas/{letra}")]
		public IActionResult Index(string letra)
		{
			IndexViewModel ivm;

			if (letra == null)
			{
				ivm = new IndexViewModel()
				{
					RazasPerro = context.Razas.Select(p => new RazaModel
					{
						IdRaza = (int)p.Id,
						Nombre = p.Nombre
					}).OrderBy(x => x.Nombre),
					Letras = context.Razas.Select(x => x.Nombre).
					Select(x => x.FirstOrDefault()).Distinct().OrderBy(x => x)
				};
			}
			else
			{
				ivm = new IndexViewModel()
				{
					RazasPerro = context.Razas.Select(p => new RazaModel
					{
						IdRaza = (int)p.Id,
						Nombre = p.Nombre
					}).Where(x => x.Nombre.StartsWith(letra)).OrderBy(x => x.Nombre),
					Letras = context.Razas.Select(x => x.Nombre).
					Select(x => x.FirstOrDefault()).Distinct().OrderBy(x => x)
				};
			}

			//var perros = context.Razas.OrderBy(r => r.Nombre).
			//    Select(x=> new IndexViewModel
			//    {
			//        IdRaza= (int)x.Id,
			//        Nombre=x.Nombre
			//    });

			return View(ivm);
		}

		[Route("~/Pais")]
		public IActionResult Pais()
		{
			var paises = context.Paises.Include(x => x.Razas).
				Select(x => new RazaPaisViewModel
				{
					Nombre = x.Nombre ?? "",
					Razas = x.Razas.Select(r => new RazaModel
					{
						IdRaza = (int)r.Id,
						Nombre = r.Nombre ?? ""
					})
				}).OrderBy(x => x.Nombre);

			if (paises == null)
			{
				return RedirectToAction("index");
			}

			return View(paises);
		}

		[Route("/Detalles-Raza/{id}")]
		public IActionResult Raza(string id)
		{
			id = id.Replace("-", " ");

			Random r = new();
			//int n = context.Razas.Count();
			var tRazas = context.Razas.ToList();
			var aleatorios = tRazas.OrderBy(x=>r.Next()).Take(4);

			var x = context.Razas.Include(x => x.IdPaisNavigation).
				Include(x => x.Caracteristicasfisicas).
				Include(x => x.Estadisticasraza).Where(x => x.Nombre.ToLower() == id.ToLower()).
				FirstOrDefault();

			var razas = new RazaDetallesViewModel()
			{
				Id = (int)x.Id,
				Nombre = x.Nombre ?? "",
				OtrosNombres = x.OtrosNombres ?? "No tiene",
				Pais = x.IdPaisNavigation.Nombre ?? "",
				Descripcion = x.Descripcion ?? "",
				PesoMin = x.PesoMin,
				PesoMax = x.PesoMax,
				AlturaMin = x.AlturaMin,
				AlturaMax = x.AlturaMax,
				EsperanzaVida = x.EsperanzaVida,
				NivelEnergia = x.Estadisticasraza.NivelEnergia,
				FacilidadEntrenamiento = x.Estadisticasraza.FacilidadEntrenamiento,
				EjercicioObligatorio = x.Estadisticasraza.EjercicioObligatorio,
				AmistadDesconocidos = x.Estadisticasraza.AmistadDesconocidos,
				AmistadPerros = x.Estadisticasraza.AmistadPerros,
				NecesidadCepillado = x.Estadisticasraza.NecesidadCepillado,
				Patas = x.Caracteristicasfisicas.Patas ?? "",
				Cola = x.Caracteristicasfisicas.Cola ?? "",
				Hocico = x.Caracteristicasfisicas.Hocico ?? "",
				Pelo = x.Caracteristicasfisicas.Pelo ?? "",
				Color = x.Caracteristicasfisicas.Color ?? "",
				Razas = aleatorios.Select(x => new RazaModel
				{
					IdRaza = (int)x.Id,
					Nombre = x.Nombre
				})
			};

			if (razas == null)
			{
				return RedirectToAction("Index");
			}



			return View(razas);
		}
	}
}
