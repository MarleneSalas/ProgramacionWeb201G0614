using Microsoft.EntityFrameworkCore.Migrations.Operations;
using U3A1.Models.Entities;

namespace U3A1.Repositories
{
    public class Repository<H> where H : class
    {
        public NeatContext Context { get; }
        public Repository(NeatContext context) 
        {
            Context = context;
        }

        public virtual IEnumerable<H> GetAll()
        {
           return Context.Set<H>();
        }

        public virtual H? Get(object id)
        {
            return Context.Find<H>(id);
        }

    
        public virtual void Insert (H entity)
        {
            Context.Add(entity);
            Context.SaveChanges();
        }

        public virtual void Update (H entity)
        {
            Context.Update(entity);
            Context.SaveChanges();
        }

        public virtual void Delete (H entity) 
        {
            Context.Remove(entity);
            Context.SaveChanges();
        }

        public virtual void Delete(object id)
        {
            var entity = Get(id);
            if (entity != null)
            {
                Delete(id);
            }
        }
    }
}
