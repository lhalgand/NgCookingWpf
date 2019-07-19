using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using apis.Data;
using apis.Models;

using Microsoft.EntityFrameworkCore;

namespace apis.Data
{
    public class CommunityRepository : IRepository<User>
    {
        DbSet<User> _set = null;
        NgContext _ngContext;

        public CommunityRepository(NgContext ngContext)
        {
            _ngContext = ngContext;
            _set = ngContext.Set<User>();
        }

        public void Add(User entity)
        {
            _set.Add(entity);
            _ngContext.SaveChanges();
        }

        public void Delete(User entity)
        {
            _set.Remove(entity);
            _ngContext.SaveChanges();
        }

        public IQueryable<User> Get()
        {
            return _set.AsQueryable<User>();
        }

        public void Update(User entity)
        {
            _set.Update(entity);
            _ngContext.SaveChanges();
        }
    }
}
