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
    public class FuelTypeRepository : IRepository<FuelType>
    {
        private MotorDepotContext _db;

        public FuelTypeRepository(MotorDepotContext db)
        {
            _db = db;
        }

        private IEnumerable<FuelType> FuelTypes
        {
            get { return _db.FuelTypes.AsNoTracking(); }
        } 

        public IEnumerable<FuelType> GetAll()
        {
            return FuelTypes;
        }

        public FuelType Get(int id)
        {
            return FuelTypes.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<FuelType> Find(Func<FuelType, bool> predicate)
        {
            return FuelTypes.Where(predicate);
        }

        public void Create(FuelType item)
        {
            _db.FuelTypes.Add(item);
        }

        public void Remove(int id)
        {
            var item = _db.FuelTypes.Find(id);
            if (item != null)
                _db.FuelTypes.Remove(item);
        }

        public void Update(FuelType item)
        {
            _db.Entry(item).State = EntityState.Modified;
        }
    }
}
