using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MotorDepot.DAL.Abstract;
using MotorDepot.DAL.EF;
using MotorDepot.DAL.Entities;

namespace MotorDepot.DAL.Repositories
{
    public class DriverVoyageRequestRepository : IRepository<DriverVoyageRequest>
    {
        private MotorDepotContext _db;

        public DriverVoyageRequestRepository(MotorDepotContext db)
        {
            _db = db;
        }

        private IEnumerable<DriverVoyageRequest> DriverVoyageRequests
        {
            get
            {
                return
                    _db.DriverVoyageRequests.Include(x => x.Driver)
                        .Include(x => x.Voyage)
                        .ToList();
            }
        } 

        public IEnumerable<DriverVoyageRequest> GetAll()
        {
            return DriverVoyageRequests;
        }

        public DriverVoyageRequest Get(int id)
        {
            return DriverVoyageRequests.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<DriverVoyageRequest> Find(Func<DriverVoyageRequest, bool> predicate)
        {
            return DriverVoyageRequests.Where(predicate);
        }

        public void Create(DriverVoyageRequest item)
        {
            _db.DriverVoyageRequests.Add(item);
        }

        public void Remove(int id)
        {
            var item = _db.DriverVoyageRequests.Find(id);
            if (item != null)
                _db.DriverVoyageRequests.Remove(item);
        }

        public void Update(DriverVoyageRequest item)
        {
            _db.Entry(item).State = EntityState.Modified;
        }
    }
}
