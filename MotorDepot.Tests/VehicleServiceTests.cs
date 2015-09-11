using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MotorDepot.BLL.DTO;
using MotorDepot.BLL.Services;
using MotorDepot.DAL.Abstract;
using MotorDepot.DAL.EF;
using MotorDepot.DAL.Entities;
using MotorDepot.DAL.Repositories;

namespace MotorDepot.Tests
{
    [TestClass]
    public class VehicleServiceTests
    {

        [TestMethod]
        public void GetAllVehicles()
        {
            //Arrange
            var data = new List<Vehicle>
            {
                new Vehicle {Name = "first"},
                new Vehicle {Name = "second"}
            };

            

            var mockRep = new Mock<IRepository<Vehicle>>();
            mockRep.Setup(x => x.GetAll()).Returns(data.AsEnumerable());
            mockRep.Setup(x => x.Find(It.IsAny<Func<Vehicle, Boolean>>())).Returns(data.AsEnumerable());

            var mockUnit = new Mock<IMotorDepotUnitOfWork>();

            mockUnit.Setup(x => x.Vehicles).Returns(mockRep.Object);


            var service = new VehicleService(mockUnit.Object);
            Mapper.CreateMap<Vehicle, VehicleDTO>();

            //Act
            var result = service.GetVehicles().Count();

            //Assert
            mockRep.Verify(x => x.Find(It.IsAny<Func<Vehicle, Boolean>>()), Times.Once);
            Assert.AreEqual(2, result);
        }


        [TestMethod]
        public void VehicleIsFree_True()
        {
            //Arrange
            var vehicle = new Vehicle {Id = 1, Name = "NewVehicle"};
            var vehiclesData = new List<Vehicle>
            {
                vehicle
            };

            var mockVehicleRep = new Mock<IRepository<Vehicle>>();
            mockVehicleRep.Setup(x => x.Get(It.Is<Int32>(_ => _ == 1))).Returns(vehicle);

            var mockRequestsRep = new Mock<IRepository<Voyage>>();
            mockRequestsRep.Setup(x => x.Find(It.IsAny<Func<Voyage, bool>>()))
                .Returns(new List<Voyage>());

            var mockUnit = new Mock<IMotorDepotUnitOfWork>();
            mockUnit.Setup(x => x.Vehicles).Returns(mockVehicleRep.Object);
            mockUnit.Setup(x => x.Voyages).Returns(mockRequestsRep.Object);

            var service = new VehicleService(mockUnit.Object);

            //Act
            var result = service.IsVehicleFree(1, DateTime.Now);

            //Assert
            Assert.IsTrue(result);
        }





        
    }
}
