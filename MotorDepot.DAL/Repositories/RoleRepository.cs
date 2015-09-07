using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MotorDepot.DAL.Abstract;
using MotorDepot.DAL.EF;
using MotorDepot.DAL.Entities;

namespace MotorDepot.DAL.Repositories
{
    public class RoleRepository : IRepository<Role>
    {
        private MotorDepotContext _db;

        public RoleRepository(MotorDepotContext db)
        {
            _db = db;
        }

        private IEnumerable<Role> Roles
        {
            get { return _db.Roles.Include(x => x.Users).ToList(); }
        } 

        public IEnumerable<Role> GetAll()
        {
            return Roles;
        }

        public Role Get(int id)
        {
            return Roles.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Role> Find(Func<Role, bool> predicate)
        {
            return Roles.Where(predicate);
        }

        public void Create(Role item)
        {
            _db.Roles.Add(item);
        }

        public void Remove(Int32 id)
        {
            var item = _db.Roles.Find(id);
            if (item != null)
                _db.Roles.Remove(item);
        }

        public void Update(Role item)
        {
            _db.Entry(item).State = EntityState.Modified;
        }
    }
}
