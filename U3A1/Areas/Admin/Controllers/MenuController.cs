using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using NuGet.Packaging.Signing;
using U3A1.Areas.Admin.Models;
using U3A1.Models.Entities;
using U3A1.Repositories;

namespace U3A1.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class MenuController : Controller
    {
        public HamburguesasRepository ReposM { get; }
        public Repository<Clasificacion> ReposC { get; }
        public MenuController(HamburguesasRepository reposM, Repository<Clasificacion> reposC) 
        {
            ReposM = reposM;
            ReposC = reposC;
        }

        //[HttpGet]
        //[HttpPost]
        public IActionResult Index()
        {
            var data = ReposM.GetAll().GroupBy(x => x.IdClasificacionNavigation.Nombre)
                .Select(x => new AdminMenuViewModel
                {
                    Nombre = x.Key,
                    Menus = x.Select(y => new MenuModel
                    {
                        Nombre = y.Nombre,
                        IdMenu = y.Id,
                        Descripcion = y.Descripción,
                        Precio = (decimal)y.Precio,
                        PrecioPromocion = y.PrecioPromocion ?? 0
                    })
                });
         
            return View(data);
        }

        public IActionResult Agregar()
        {
            AdminAgregarViewModel vm = new();

            vm.Clasificaciones = ReposC.GetAll().Select(x=> new ClasificacionModel
            {
                Id =x.Id,
                Nombre=x.Nombre
            });

            return View(vm);
        }

        [HttpPost]
        public IActionResult Agregar(AdminAgregarViewModel vm)
        {
            ModelState.Clear();
            if (string.IsNullOrEmpty(vm.MenuN.Nombre))
            {
                ModelState.AddModelError("","Campo de nombre está vacío.");
            }
            else if(vm.MenuN.Nombre.Length > 100)
            {
                ModelState.AddModelError("", "Nombre ha superado los carácteres permitidos.");
            }

            if (string.IsNullOrEmpty(vm.MenuN.Descripción))
            {
                ModelState.AddModelError("", "Campo de descripción está vacío.");
            }

            if(vm.MenuN.Precio<=0)
            {
                ModelState.AddModelError("", "El precio no puede ser menor o igual a 0.");
            }

            if (vm.MenuN.IdClasificacion == 0)
            {
                ModelState.AddModelError("", "Seleccione una clasificación.");
            }


            if (vm.Archivo != null) //si selecciono un archivo
            {
                //Validar tipo de archivo:  MIMETYPE
                if (vm.Archivo.ContentType != "image/png")
                {
                    ModelState.AddModelError("", "Solo se permiten archivos PNG");
                }

                if (vm.Archivo.Length > 500 * 1024)//500kb
                {
                    ModelState.AddModelError("", "Solo se permiten archivos no mayores a 500Kb.");
                }
            }

            if(ModelState.IsValid)
            {
                ReposM.Insert(vm.MenuN);

                if (vm.Archivo == null)// no eligio archivo
                {
                    // obtener id del producto
                    // copiar el archivo llamado nodisponible.jpg y cambiar el nombre
                    System.IO.File.Copy("wwwroot/images/burger.png", $"wwwroot/hamburguesas/{vm.MenuN.Id}.png");
                }
                else
                {
                    System.IO.FileStream fs = System.IO.File.Create($"wwwroot/hamburguesas/{vm.MenuN.Id}.png");
                    vm.Archivo.CopyTo(fs);
                    fs.Close();
                }
                return RedirectToAction("Index");
            }

            vm.Clasificaciones = ReposC.GetAll().Select(x => new ClasificacionModel
            {
                Id = x.Id,
                Nombre = x.Nombre
            });
            return View(vm);
        }

        public IActionResult Editar(int id)
        {
            var menu = ReposM.Get(id);
            if(menu== null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                AdminAgregarViewModel vm = new();

                vm.Clasificaciones = ReposC.GetAll().Select(x => new ClasificacionModel
                {
                    Id = x.Id,
                    Nombre = x.Nombre
                });
                vm.MenuN = menu;
                return View(vm);
            }
            
        }

        [HttpPost]
        public IActionResult Editar(AdminAgregarViewModel vm)
        {
            ModelState.Clear();
            if (string.IsNullOrEmpty(vm.MenuN.Nombre))
            {
                ModelState.AddModelError("", "Campo de nombre está vacío.");
            }
            else if (vm.MenuN.Nombre.Length > 100)
            {
                ModelState.AddModelError("", "Nombre ha superado los carácteres permitidos.");
            }

            if (string.IsNullOrEmpty(vm.MenuN.Descripción))
            {
                ModelState.AddModelError("", "Campo de descripción está vacío.");
            }

            if (vm.MenuN.Precio <= 0)
            {
                ModelState.AddModelError("", "El precio no puede ser menor o igual a 0.");
            }

            if (vm.MenuN.IdClasificacion == 0)
            {
                ModelState.AddModelError("", "Seleccione una clasificación.");
            }

            if (vm.Archivo != null) 
            {
                if (vm.Archivo.ContentType != "image/png")
                {
                    ModelState.AddModelError("", "Solo se permiten archivos PNG");
                }

                if (vm.Archivo.Length > 500 * 1024)//500kb
                {
                    ModelState.AddModelError("", "Solo se permiten archivos no mayores a 500Kb.");
                }
            }

            if (ModelState.IsValid)
            {
                var menu = ReposM.Get(vm.MenuN.Id);
                if (menu == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    menu.Nombre = vm.MenuN.Nombre;
                    menu.Precio = vm.MenuN.Precio;
                    menu.Descripción = vm.MenuN.Nombre;
                    menu.IdClasificacion = vm.MenuN.IdClasificacion;

                    ReposM.Update(menu);

                    if (vm.Archivo != null)
                    {
                        System.IO.FileStream fs = System.IO.File.Create($"wwwroot/hamburguesas/{vm.MenuN.Id}.png");
                        vm.Archivo.CopyTo(fs);
                        fs.Close();
                    }
                    return RedirectToAction("Index");
                }
            }
            vm.Clasificaciones = ReposC.GetAll().Select(x => new ClasificacionModel
            {
                Id = x.Id,
                Nombre = x.Nombre
            });
            return View(vm);
        }

        public IActionResult Eliminar(int id)
        {
            var menu = ReposM.Get(id);
            if (menu==null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(menu);
            }
        }

        [HttpPost]
        public IActionResult Eliminar(Menu m) 
        {
            var menu = ReposM.Get(m.Id);
            if (menu == null)
            {
                return RedirectToAction("Index");
            }

            ReposM.Delete(menu);

            var ruta = $"wwwroot/hamburguesas/{m.Id}.png";

            if (System.IO.File.Exists(ruta))
            {
                System.IO.File.Delete(ruta);
            }

            return RedirectToAction("Index");

        }

        public IActionResult AgregarPromocion(int id)
        {
            var menu = ReposM.Get(id);
            if (menu == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                AdminPromocionViewModel vm = new()
                {
                    Id = menu.Id,
                    Nombre = menu.Nombre,
                    Precio = (decimal)menu.Precio,
                    PrecioPromocion = menu.PrecioPromocion
                };
                return View(vm);
            }
        }

        [HttpPost]
         public IActionResult AgregarPromocion(AdminPromocionViewModel vm)
        {
            ModelState.Clear();
            if(vm.PrecioPromocion==0)
            {
                ModelState.AddModelError("", "La promoción no puede ser menor o igual a 0.");
            }

            if (vm.PrecioPromocion >= (double)vm.Precio)
            {
                ModelState.AddModelError("", "Verifique la promoción, esta no puede ser mayor al precio normal.");
            }

            if(ModelState.IsValid)
            {
                var promo = ReposM.Get(vm.Id);

                if(promo== null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    promo.PrecioPromocion = vm.PrecioPromocion;

                    ReposM.Update(promo);
                    return RedirectToAction("Index");
                }
            }

            return View(vm);
        }

        public IActionResult QuitarPromocion(int id)
        {
            var menu = ReposM.Get(id);
            if (menu == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                AdminPromocionViewModel vm = new()
                {
                    Id = menu.Id,
                    Nombre = menu.Nombre,
                    Precio = (decimal)menu.Precio,
                    PrecioPromocion = menu.PrecioPromocion
                };
                return View(vm);
            }
        }

        [HttpPost]
        public IActionResult QuitarPromocion(AdminPromocionViewModel vm)
        {
            var adiospromo = ReposM.Get(vm.Id);
            if (adiospromo == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                adiospromo.PrecioPromocion = 0;

                ReposM.Update(adiospromo);
                return RedirectToAction("Index");
            }
        }

    }
}
