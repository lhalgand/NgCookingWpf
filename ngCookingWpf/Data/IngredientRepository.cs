using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using apis.Models;

using Microsoft.EntityFrameworkCore;

namespace apis.Data
{
    public class IngredientRepository : IRepository<Ingredient>
    {
        DbSet<Ingredient> _set = null;
        NgContext _ngContext;

        public IngredientRepository(NgContext ngContext)
        {
            _ngContext = ngContext;
            _set = ngContext.Set<Ingredient>();
        }

        public void Add(Ingredient entity)
        {
            _set.Add(entity);
            _ngContext.SaveChanges();
        }

        public void Delete(Ingredient entity)
        {
            _set.Remove(entity);
            _ngContext.SaveChanges();
        }

        public IQueryable<Ingredient> Get()
        {
            return _set.AsQueryable<Ingredient>()
                .Include(r => r.IngredientsRecettes);
        }

        public void Update(Ingredient entity)
        {
            _set.Update(entity);
            _ngContext.SaveChanges();
        }
    }
}
