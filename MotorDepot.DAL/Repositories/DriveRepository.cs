using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MotorDepot.DAL.Abstract;
using MotorDepot.DAL.EF;
using MotorDepot.DAL.Entities;

namespace MotorDepot.DAL.Repositories
{
    public class DriveRepository : IRepository<Drive>
    {
        private MotorDepotContext _db;

        public DriveRepository(MotorDepotContext db)
        {
            _db = db;
        }

        private IEnumerable<Drive> Drives
        {
            get { return _db.Drives.Include(x => x.DriveType).Include(x => x.Fuel).ToList(); }
        } 
        public IEnumerable<Drive> GetAll()
        {
            return Drives;
        }

        public Drive Get(int id)
        {
            return Drives.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Drive> Find(Func<Drive, bool> predicate)
        {
            return Drives.Where(predicate);
        }

        public void Create(Drive item)
        {
            _db.Drives.Add(item);
        }

        public void Remove(int id)
        {
            var item = _db.Drives.Find(id);
            if (item != null)
                _db.Drives.Remove(item);
        }

        public void Update(Drive item)
        {
            _db.Entry(item).State = EntityState.Modified;
        }
    }
}
