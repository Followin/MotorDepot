using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MotorDepot.DAL.Abstract;
using MotorDepot.DAL.EF;
using MotorDepot.DAL.Entities;

namespace MotorDepot.DAL.Repositories
{
    public class DriverLicenseTypeRepository : IRepository<DriverLicenseType>
    {
        private MotorDepotContext _db;

        public DriverLicenseTypeRepository(MotorDepotContext db)
        {
            _db = db;
        }

        private IEnumerable<DriverLicenseType> DriverLicenseTypes
        {
            get { return _db.DriverLicenseTypes; }
        } 
        public IEnumerable<DriverLicenseType> GetAll()
        {
            return DriverLicenseTypes;
        }

        public DriverLicenseType Get(int id)
        {
            return DriverLicenseTypes.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<DriverLicenseType> Find(Func<DriverLicenseType, bool> predicate)
        {
            return DriverLicenseTypes.Where(predicate);
        }

        public void Create(DriverLicenseType item)
        {
            _db.DriverLicenseTypes.Add(item);
        }

        public void Remove(int id)
        {
            var item = _db.DriverLicenseTypes.Find(id);
            if (item != null)
                _db.DriverLicenseTypes.Remove(item);
        }

        public void Update(DriverLicenseType item)
        {
            _db.Entry(item).State = EntityState.Modified;
        }
    }
}
