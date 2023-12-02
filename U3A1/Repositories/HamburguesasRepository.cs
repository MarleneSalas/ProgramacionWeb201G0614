using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using U3A1.Models.Entities;

namespace U3A1.Repositories
{
    public class HamburguesasRepository : Repository<Menu>
    {
        public HamburguesasRepository(NeatContext context) : base(context)
        {
        }

        public override IEnumerable<Menu> GetAll()
        {
            return Context.Menu.Include(c => c.IdClasificacionNavigation).OrderBy(h=>h.Nombre);
        }

        public IEnumerable<Menu> GetMenuByCategoria(string id)
        {
            return Context.Menu.Include(x => x.IdClasificacionNavigation).
                Where(x => x.IdClasificacionNavigation != null &&
                x.IdClasificacionNavigation.Nombre == id).OrderBy(x => x.Nombre);
        }

        public Menu? GetById(string id)
        {
            return Context.Menu.Include(c=>c.IdClasificacionNavigation).FirstOrDefault(x=>x.Nombre ==id);
        }

        public IEnumerable<Menu> GetMenuPromos()
        {
            return Context.Menu.Where(x => x.PrecioPromocion > 0).OrderBy(x=>x.Nombre);
        }
    }
}
