using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MotorDepot.DAL.Abstract;
using MotorDepot.DAL.EF;
using MotorDepot.DAL.Entities;

namespace MotorDepot.DAL.Repositories
{
    public class VehicleRepository : IRepository<Vehicle>
    {
        private MotorDepotContext _db;

        public VehicleRepository(MotorDepotContext db)
        {
            _db = db;
        }

        private IEnumerable<Vehicle> Vehicles
        {
            get { return _db.Vehicles.Include(x => x.Class).Include(x => x.Drive).ToList(); }
        } 
        public IEnumerable<Vehicle> GetAll()
        {
            return Vehicles;
        }

        public Vehicle Get(int id)
        {
            return Vehicles.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Vehicle> Find(Func<Vehicle, bool> predicate)
        {
            return Vehicles.Where(predicate);
        }

        public void Create(Vehicle item)
        {
            _db.Vehicles.Add(item);
        }

        public void Remove(int id)
        {
            var item = _db.Vehicles.Find(id);
            if (item != null)
                _db.Vehicles.Remove(item);
        }

        public void Update(Vehicle item)
        {
            _db.Entry(item).State = EntityState.Modified;
        }
    }
}
