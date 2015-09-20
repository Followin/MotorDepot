using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MotorDepot.DAL.Abstract;
using MotorDepot.DAL.EF;
using MotorDepot.DAL.Entities;

namespace MotorDepot.DAL.Repositories
{
    public class DriverRepository : IRepository<Driver>
    {
        private MotorDepotContext _db;

        public DriverRepository(MotorDepotContext db)
        {
            _db = db;
        }

        private IEnumerable<Driver> Drivers
        {
            get { return _db.Drivers.AsNoTracking()
                .Include(x => x.DriverLicense)
                .Include(x => x.DriverLicense.VehicleClasses)
                .Include(x => x.User)
                .Include(x => x.Requests)
                .ToList(); }
        } 
        public IEnumerable<Driver> GetAll()
        {
            return Drivers;
        }

        public Driver Get(int id)
        {
            return Drivers.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Driver> Find(Func<Driver, bool> predicate)
        {
            return Drivers.Where(predicate);
        }

        public void Create(Driver item)
        {
            _db.Drivers.Add(item);
        }

        public void Remove(int id)
        {
            var item = _db.Drivers.Find(id);
            if (item != null)
                _db.Drivers.Remove(item);
        }

        public void Update(Driver item)
        {
            _db.Entry(item).State = EntityState.Modified;
        }
    }
}
