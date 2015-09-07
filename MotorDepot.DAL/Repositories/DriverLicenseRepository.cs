using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MotorDepot.DAL.Abstract;
using MotorDepot.DAL.EF;
using MotorDepot.DAL.Entities;

namespace MotorDepot.DAL.Repositories
{
    public class DriverLicenseRepository : IRepository<DriverLicense>
    {
        private MotorDepotContext _db;

        public DriverLicenseRepository(MotorDepotContext db)
        {
            _db = db;
        }

        private IEnumerable<DriverLicense> DriverLicenses
        {
            get { return _db.DriverLicenses.Include(x => x.DriverLicenseType).Include(x => x.Driver); }
        } 
        public IEnumerable<DriverLicense> GetAll()
        {
            return DriverLicenses;
        }

        public DriverLicense Get(int id)
        {
            return DriverLicenses.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<DriverLicense> Find(Func<DriverLicense, bool> predicate)
        {
            return DriverLicenses.Where(predicate);
        }

        public void Create(DriverLicense item)
        {
            _db.DriverLicenses.Add(item);
        }

        public void Remove(int id)
        {
            var item = _db.DriverLicenses.Find(id);
            if (item != null)
                _db.DriverLicenses.Remove(item);
        }

        public void Update(DriverLicense item)
        {
            _db.Entry(item).State = EntityState.Modified;
        }
    }
}
