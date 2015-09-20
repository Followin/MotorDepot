using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MotorDepot.DAL.Abstract;
using MotorDepot.DAL.EF;
using MotorDepot.DAL.Entities;

namespace MotorDepot.DAL.Repositories
{
    public class VehicleClassRepository : IRepository<VehicleClass>
    {
        private MotorDepotContext _db;

        public VehicleClassRepository(MotorDepotContext db)
        {
            _db = db;
        }

        private IEnumerable<VehicleClass> VehicleClasses
        {
            get { return _db.VehicleClasses.Include(x => x.DriverLicenses); }
        } 
        public IEnumerable<VehicleClass> GetAll()
        {
            return VehicleClasses;
        }

        public VehicleClass Get(int id)
        {
            return VehicleClasses.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<VehicleClass> Find(Func<VehicleClass, bool> predicate)
        {
            return VehicleClasses.Where(predicate);
        }

        public void Create(VehicleClass item)
        {
            _db.VehicleClasses.Add(item);
        }

        public void Remove(int id)
        {
            var item = _db.VehicleClasses.Find(id);
            if (item != null)
                _db.VehicleClasses.Remove(item);
        }

        public void Update(VehicleClass item)
        {
            _db.Entry(item).State = EntityState.Modified;
        }
    }
}
