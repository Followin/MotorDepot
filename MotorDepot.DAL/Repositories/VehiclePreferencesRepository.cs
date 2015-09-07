using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MotorDepot.DAL.Abstract;
using MotorDepot.DAL.EF;
using MotorDepot.DAL.Entities;

namespace MotorDepot.DAL.Repositories
{
    public class VehiclePreferencesRepository : IRepository<VehiclePreferences>
    {
        private MotorDepotContext _db;

        public VehiclePreferencesRepository(MotorDepotContext db)
        {
            _db = db;
        }

        private IEnumerable<VehiclePreferences> VehiclePreferences
        {
            get
            {
                return
                    _db.VehiclePreferences.Include(x => x.Class)
                        .Include(x => x.DriveType)
                        .Include(x => x.FuelType)
                        .ToList();
            }
        }
        public IEnumerable<VehiclePreferences> GetAll()
        {
            return VehiclePreferences;
        }

        public VehiclePreferences Get(int id)
        {
            return VehiclePreferences.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<VehiclePreferences> Find(Func<VehiclePreferences, bool> predicate)
        {
            return VehiclePreferences.Where(predicate);
        }

        public void Create(VehiclePreferences item)
        {
            _db.VehiclePreferences.Add(item);
        }

        public void Remove(int id)
        {
            var item = _db.VehiclePreferences.Find(id);
            if (item != null)
                _db.VehiclePreferences.Remove(item);
        }

        public void Update(VehiclePreferences item)
        {
            _db.Entry(item).State = EntityState.Modified;
        }
    }
}
