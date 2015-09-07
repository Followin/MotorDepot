using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MotorDepot.DAL.Abstract;
using MotorDepot.DAL.EF;
using MotorDepot.DAL.Entities;

namespace MotorDepot.DAL.Repositories
{
    public class VoyageRepository : IRepository<Voyage>
    {
        private MotorDepotContext _db;

        public VoyageRepository(MotorDepotContext db)
        {
            _db = db;
        }

        private IEnumerable<Voyage> Voyages
        {
            get { return _db.Voyages.Include(x => x.Driver).Include(x => x.Vehicle).ToList(); }
        }  
        public IEnumerable<Voyage> GetAll()
        {
            return Voyages;
        }

        public Voyage Get(int id)
        {
            return Voyages.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Voyage> Find(Func<Voyage, bool> predicate)
        {
            return Voyages.Where(predicate);
        }

        public void Create(Voyage item)
        {
            _db.Voyages.Add(item);
        }

        public void Remove(int id)
        {
            var item = _db.Voyages.Find(id);
            if (item != null)
                _db.Voyages.Remove(item);
        }

        public void Update(Voyage item)
        {
            _db.Entry(item).State = EntityState.Modified;
        }
    }
}
