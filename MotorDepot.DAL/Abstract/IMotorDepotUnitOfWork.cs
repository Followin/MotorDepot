﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MotorDepot.DAL.Entities;

namespace MotorDepot.DAL.Abstract
{
    public interface IMotorDepotUnitOfWork
    {
        IRepository<Driver> Drivers { get; }
        IRepository<DriverLicense> DriverLicenses { get; }
        IRepository<DriverLicenseType> DriverLicenseTypes { get; }
        IRepository<Drive> Drives { get; }
        IRepository<DriverVoyageRequest> DriverVoyageRequests { get; }
        IRepository<FuelType> FuelTypes { get; }
        IRepository<Role> Roles { get; }
        IRepository<User> Users { get; }
        IRepository<Vehicle> Vehicles { get; }
        IRepository<VehicleClass> VehicleClasses { get; }
        IRepository<Voyage> Voyages { get; } 
    }
}
