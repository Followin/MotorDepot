using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using MotorDepot.DAL.Entities;

namespace MotorDepot.DAL.EF
{
    public class MotorDepotContext : DbContext
    {
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<DriverLicense> DriverLicenses { get; set; }
        public DbSet<FuelType> FuelTypes { get; set; }
        public DbSet<Drive> Drives { get; set; }
        public DbSet<VehicleClass> VehicleClasses { get; set; }
        public virtual DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Voyage> Voyages { get; set; }
        public DbSet<DriverVoyageRequest> DriverVoyageRequests { get; set; }
        //public DbSet<VehiclePreferences> VehiclePreferences { get; set; }


        public MotorDepotContext(String connectionString) : base(connectionString)
        {
            //System.Data.Entity.Database.SetInitializer(new Initializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.ComplexType<Dimensions>();
            modelBuilder.ComplexType<VoyageLifeCycle>();
            modelBuilder.ComplexType<VoyagePoint>();
            modelBuilder.ComplexType<Image>();

            modelBuilder.Entity<User>()
                .HasOptional(x => x.Driver)
                .WithOptionalPrincipal(x => x.User);

            modelBuilder.Entity<Driver>()
                .HasRequired(x => x.DriverLicense)
                .WithRequiredPrincipal(x => x.Driver);
        }
    }

    public class MotorDepotContextFactory : IDbContextFactory<MotorDepotContext>
    {
        public MotorDepotContext Create()
        {
            return new MotorDepotContext("SqlServerMotorDepot");
        }
    }


    public class Initializer : DropCreateDatabaseAlways<MotorDepotContext>
    {
        protected override void Seed(MotorDepotContext context)
        {
            var admin = context.Roles.Add(new Role {Name = "Admin"});
            context.Roles.Add(new Role {Name = "Controller"});
            context.Roles.Add(new Role {Name = "Driver"});

            context.Users.Add(new User
            {
                Nickname = "Admin",
                Password = "ALxwUQXpYTEGg5sqkFb9pLfSp9fuPOTUXzS78ebvGaFOO1dgG6ZMJU5ydWZlVnMVLQ==",
                Email = "Me@motordepot.com",
                IsConfirmed = true,
                Role = admin
            });

            context.VehicleClasses.Add(new VehicleClass { Name = "A", Description = "Мотоциклы" });
            context.VehicleClasses.Add(new VehicleClass { Name = "A1", Description = "Легкие мотоциклы" });
            context.VehicleClasses.Add(new VehicleClass { Name = "B", Description = "Легковые автомобили" });
            context.VehicleClasses.Add(new VehicleClass { Name = "BE", Description = "Легковые автомобили с прицепом" });
            context.VehicleClasses.Add(new VehicleClass { Name = "B1", Description = "Трициклы" });
            context.VehicleClasses.Add(new VehicleClass { Name = "C", Description = "Грузовые автомобили" });
            context.VehicleClasses.Add(new VehicleClass { Name = "CE", Description = "Грузовые автомобили с прицепом" });
            context.VehicleClasses.Add(new VehicleClass { Name = "C1", Description = "Легкие грузовики" });
            context.VehicleClasses.Add(new VehicleClass { Name = "C1E", Description = "Легкие грузовики с прицепом" });
            context.VehicleClasses.Add(new VehicleClass { Name = "D", Description = "Автобусы" });
            context.VehicleClasses.Add(new VehicleClass { Name = "DE", Description = "Автобусы с прицепом" });
            context.VehicleClasses.Add(new VehicleClass { Name = "D1", Description = "Небольшие автобусы" });
            context.VehicleClasses.Add(new VehicleClass { Name = "D1E", Description = "Небольшие автобусы с прицепом" });
            context.VehicleClasses.Add(new VehicleClass { Name = "M", Description = "Мопеды" });

            context.FuelTypes.Add(new FuelType { Name = "А-72" });
            context.FuelTypes.Add(new FuelType { Name = "А-76 неэтил" });
            context.FuelTypes.Add(new FuelType { Name = "А-76 этил" });
            context.FuelTypes.Add(new FuelType { Name = "АИ-91" });
            context.FuelTypes.Add(new FuelType { Name = "АИ-93" });
            context.FuelTypes.Add(new FuelType { Name = "АИ-95" });



            context.SaveChanges();
        }
    }
}
