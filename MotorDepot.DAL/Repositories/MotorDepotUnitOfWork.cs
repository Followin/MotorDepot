using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MotorDepot.DAL.Abstract;
using MotorDepot.DAL.EF;
using MotorDepot.DAL.Entities;

namespace MotorDepot.DAL.Repositories
{
    public class MotorDepotUnitOfWork : IMotorDepotUnitOfWork
    {
        private MotorDepotContext _db;
        private DriveRepository _driveRepository;
        private DriverLicenseRepository _driverLicenseRepository;
        private DriverRepository _driverRepository;
        private DriverVoyageRequestRepository _driverVoyageRequestRepository;
        private FuelTypeRepository _fuelTypeRepository;
        private RoleRepository _roleRepository;
        private UserRepository _userRepository;
        private VehicleClassRepository _vehicleClassRepository;
        private VehicleRepository _vehicleRepository;
        private VoyageRepository _voyageRepository;

        public MotorDepotUnitOfWork(MotorDepotContext db)
        {
            _db = db;
        }



        public IRepository<Driver> Drivers
        {
            get { return _driverRepository ?? (_driverRepository = new DriverRepository(_db)); }
        }

        public IRepository<DriverLicense> DriverLicenses
        {
            get { return _driverLicenseRepository ?? (_driverLicenseRepository = new DriverLicenseRepository(_db)); }
        }

        

        public IRepository<Drive> Drives
        {
            get { return _driveRepository ?? (_driveRepository = new DriveRepository(_db)); }
        }

        public IRepository<DriverVoyageRequest> DriverVoyageRequests
        {
            get
            {
                return _driverVoyageRequestRepository ??
                       (_driverVoyageRequestRepository = new DriverVoyageRequestRepository(_db));
            }
        }

        public IRepository<FuelType> FuelTypes
        {
            get { return _fuelTypeRepository ?? (_fuelTypeRepository = new FuelTypeRepository(_db)); }
        }

        public IRepository<Role> Roles
        {
            get { return _roleRepository ?? (_roleRepository = new RoleRepository(_db)); }
        }

        public IRepository<User> Users
        {
            get { return _userRepository ?? (_userRepository = new UserRepository(_db)); }
        }

        public virtual IRepository<Vehicle> Vehicles
        {
            get { return _vehicleRepository ?? (_vehicleRepository = new VehicleRepository(_db)); }
        }

        public IRepository<VehicleClass> VehicleClasses
        {
            get { return _vehicleClassRepository ?? (_vehicleClassRepository = new VehicleClassRepository(_db)); }
        }

        public IRepository<Voyage> Voyages
        {
            get { return _voyageRepository ?? (_voyageRepository = new VoyageRepository(_db)); }
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        private Boolean _disposed = false;
        public void Dispose(Boolean disposing)
        {
            if (!_disposed && disposing)
                _db.Dispose();
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
