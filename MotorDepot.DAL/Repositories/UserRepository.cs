using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MotorDepot.DAL.Abstract;
using MotorDepot.DAL.EF;
using MotorDepot.DAL.Entities;

namespace MotorDepot.DAL.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private MotorDepotContext _db;

        public UserRepository(MotorDepotContext db)
        {
            _db = db;
        }

        private IEnumerable<User> Users
        {
            get { return _db.Users.Include(x => x.Driver).Include(x => x.Role); }
        } 
        public IEnumerable<User> GetAll()
        {
            return Users;
        }

        public User Get(int id)
        {
            return Users.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<User> Find(Func<User, bool> predicate)
        {
            return Users.Where(predicate);
        }

        public void Create(User item)
        {
            _db.Users.Add(item);
        }

        public void Remove(int id)
        {
            var item = _db.Users.Find(id);
            if (item != null)
                _db.Users.Remove(item);
        }

        public void Update(User item)
        {
            _db.Entry(item).State = EntityState.Modified;
        }
    }
}
