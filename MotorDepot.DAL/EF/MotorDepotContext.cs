using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MotorDepot.DAL.Entities;

namespace MotorDepot.DAL.EF
{
    public class MotorDepotContext : DbContext
    {
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<DriverLicenseType> DriverLicenseTypes { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<DriverLicense> DriverLicenses { get; set; }
        public DbSet<FuelType> FuelTypes { get; set; }
        public DbSet<Drive> Drives { get; set; }
        public DbSet<VehicleClass> VehicleClasses { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Voyage> Voyages { get; set; }
        public DbSet<DriverVoyageRequest> DriverVoyageRequests { get; set; }
        //public DbSet<VehiclePreferences> VehiclePreferences { get; set; }


        public MotorDepotContext(String connectionString) : base(connectionString)
        {
            
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.ComplexType<Dimensions>();
            modelBuilder.ComplexType<VoyageLifeCycle>();
            modelBuilder.ComplexType<VoyagePoint>();

            modelBuilder.Entity<User>()
                .HasOptional(x => x.Driver)
                .WithOptionalPrincipal(x => x.User);

            modelBuilder.Entity<Driver>()
                .HasRequired(x => x.DriverLicense)
                .WithRequiredPrincipal(x => x.Driver);
        }
    }
}
