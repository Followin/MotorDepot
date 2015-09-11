using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MotorDepot.BLL.Abstract;
using MotorDepot.BLL.DTO;
using MotorDepot.BLL.Models;
using MotorDepot.DAL.Abstract;
using MotorDepot.DAL.Entities;
using System.Data.Entity.Validation;
using VoyageStatus = MotorDepot.DAL.Entities.VoyageStatus;

namespace MotorDepot.BLL.Services
{
    public class VehicleService : IVehicleService
    {
        private IMotorDepotUnitOfWork _db;

        public VehicleService(IMotorDepotUnitOfWork db)
        {
            _db = db;
        }
        public ServiceResult AddVehicle(VehicleDTO vehicleDto)
        {
            var result = new ServiceResult();


            if (vehicleDto.Drive == null)
                result.Errors.Add(new PropertyMessagePair { PropertyName = "Drive", Message = "You gotta determine drive" });
            if (vehicleDto.Class == null)
                result.Errors.Add(new PropertyMessagePair { PropertyName = "Class", Message = "You gotta determine vehicle class" });
            if (vehicleDto.Dimensions == null)
                result.Errors.Add(new PropertyMessagePair { PropertyName = "Dimensions", Message = "You gotta determine vehicle dimensions" });

            if (vehicleDto.Class != null && vehicleDto.Dimensions != null)
            {
                _db.Vehicles.Create(Mapper.Map<Vehicle>(vehicleDto));
                _db.Save();
            }

            return result;
        }

        public ServiceResult ModifyVehicle(VehicleDTO vehicleDto)
        {
            var result = new ServiceResult();

            if (vehicleDto.Drive == null)
                result.Errors.Add(new PropertyMessagePair { PropertyName = "Drive", Message = "You gotta determine drive" });
            if (vehicleDto.Class == null)
                result.Errors.Add(new PropertyMessagePair { PropertyName = "Class", Message = "You gotta determine vehicle class" });
            if (vehicleDto.Dimensions == null)
                result.Errors.Add(new PropertyMessagePair { PropertyName = "Dimensions", Message = "You gotta determine vehicle dimensions" });

            if (vehicleDto.Class != null && vehicleDto.Dimensions != null)
            {

                try
                {
                    _db.Vehicles.Update(Mapper.Map<Vehicle>(vehicleDto));
                    _db.Save();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var entityValidationError in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityValidationError.ValidationErrors)
                        {
                            result.Errors.Add(new PropertyMessagePair { PropertyName = validationError.PropertyName, Message = validationError.ErrorMessage });
                        }
                    }
                }
            }

            return result;
        }

        public ServiceResult DeleteVehicle(int vehicleId)
        {
            var result = new ServiceResult();

            var vehicle = _db.Vehicles.Get(vehicleId);
            if (vehicle == null)
            {
                result.Errors.Add(new PropertyMessagePair { PropertyName = "vehicleId", Message = "There is no vehicle wtih such id in db" });
                return result;
            }

            if (!IsVehicleFree(vehicleId, DateTime.Now))
            {
                result.Errors.Add(new PropertyMessagePair { PropertyName = "vehicleId", Message = "This vehicle is in usage" });
                return result;
            }

            vehicle.EntryState = EntryState.Removed;

            try
            {
                _db.Vehicles.Update(vehicle);
                _db.Save();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationError in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationError.ValidationErrors)
                    {
                        result.Errors.Add(new PropertyMessagePair { PropertyName = validationError.PropertyName, Message = validationError.ErrorMessage });
                    }
                }
            }

            return result;

        }

        public bool IsVehicleFree(Int32 vehicleId, DateTime startTime, DateTime? endTime = null)
        {
            var vehicle = _db.Vehicles.Get(vehicleId);

            if (vehicle == null)
                throw new ArgumentException("There is no vehicle with such id in db", "vehicleId");

            if (!endTime.HasValue)
            {
                //searching for last car usage
                var voyages =
                    _db.Voyages.Find(
                        x =>
                            x.EntryState == EntryState.Active && x.Status != VoyageStatus.Canceled &&
                            x.Vehicle == vehicle);
                if (voyages.Any())
                {
                    var lastVoyageTime = voyages
                        .Max(x => x.RequestedEndTime)
                        .AddHours(1);
                    if (lastVoyageTime < startTime)
                        return true;
                    return false;
                }
                return true;
            }

            //searching for intersection
            var isFree =
                !(_db.Voyages.Find(
                    x =>
                        x.EntryState == EntryState.Active && x.Status != VoyageStatus.Canceled && x.Vehicle == vehicle &&
                        x.RequestedEndTime.AddHours(1) > startTime && x.RequestedStartTime < endTime.Value).Any());
            return isFree;


        }

        public IEnumerable<VehicleDTO> GetVehicles()
        {
            return Mapper.Map<IEnumerable<Vehicle>, List<VehicleDTO>>(_db.Vehicles.Find(x => x.EntryState == EntryState.Active));
        }

        public IEnumerable<VehicleDTO> GetVehicles(Func<VehicleDTO, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public VehicleDTO GetVehicleInfo(int vehicleId)
        {
            return Mapper.Map<Vehicle, VehicleDTO>(_db.Vehicles.Get(vehicleId));

        }
    }
}
