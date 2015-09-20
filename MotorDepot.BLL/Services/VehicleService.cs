using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MotorDepot.BLL.Abstract;
using MotorDepot.BLL.DTO;
using MotorDepot.BLL.Models;
using MotorDepot.DAL.Abstract;
using System.Data.Entity.Validation;
using MotorDepot.DAL.Entities;
using EntryState = MotorDepot.DAL.Entities.EntryState;

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


            if (vehicleDto.Drive == null && vehicleDto.DriveId == 0)
                result.Errors.Add(new PropertyMessagePair { PropertyName = "Drive", Message = "You gotta determine drive" });
            if (vehicleDto.Class == null && vehicleDto.VehicleClassId == 0)
                result.Errors.Add(new PropertyMessagePair { PropertyName = "Class", Message = "You gotta determine vehicle class" });
            if (vehicleDto.Dimensions == null)
                result.Errors.Add(new PropertyMessagePair { PropertyName = "Dimensions", Message = "You gotta determine vehicle dimensions" });

            if ((vehicleDto.Class != null || vehicleDto.VehicleClassId != 0) 
                && (vehicleDto.Drive != null || vehicleDto.DriveId != 0) &&vehicleDto.Dimensions != null)
            {
                _db.Vehicles.Create(Mapper.Map<Vehicle>(vehicleDto));
                _db.Save();
            }

            return result;
        }

        public ServiceResult ModifyVehicle(VehicleDTO vehicleDto)
        {
            var result = new ServiceResult();

            if (vehicleDto.Drive == null && vehicleDto.DriveId == 0)
                result.Errors.Add(new PropertyMessagePair { PropertyName = "Drive", Message = "You gotta determine drive" });
            if (vehicleDto.Class == null && vehicleDto.VehicleClassId == 0)
                result.Errors.Add(new PropertyMessagePair { PropertyName = "Class", Message = "You gotta determine vehicle class" });
            if (vehicleDto.Dimensions == null)
                result.Errors.Add(new PropertyMessagePair { PropertyName = "Dimensions", Message = "You gotta determine vehicle dimensions" });

            if ((vehicleDto.Class != null || vehicleDto.VehicleClassId!=0) &&
                (vehicleDto.Drive != null || vehicleDto.DriveId != 0) &&
                vehicleDto.Dimensions != null)
            {

                try
                {
                    _db.Vehicles.Update(Mapper.Map<Vehicle>(vehicleDto));
                    _db.Save();
                }
                catch (DbEntityValidationException ex)
                {
                    result.Append(ex);
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
                result.Append(ex);
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
                            x.EntryState == EntryState.Active && x.Status != DAL.Entities.VoyageStatus.Canceled &&
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
                        x.EntryState == EntryState.Active && x.Status != DAL.Entities.VoyageStatus.Canceled && x.Vehicle == vehicle &&
                        x.RequestedEndTime.AddHours(1) > startTime && x.RequestedStartTime < endTime.Value).Any());
            return isFree;


        }

        public IEnumerable<VehicleDTO> GetVehicles()
        {
            return Mapper.Map<IEnumerable<Vehicle>, List<VehicleDTO>>(_db.Vehicles.Find(x => x.EntryState == EntryState.Active));
        }

        public IEnumerable<VehicleDTO> GetFreeVehicles(DateTime startTime, DateTime? endTime = null)
        {
            var vehicles = _db.Vehicles.Find(x => x.EntryState == EntryState.Active).Where(x => IsVehicleFree(x.Id, startTime, endTime));
            return Mapper.Map<IEnumerable<Vehicle>, List<VehicleDTO>>(vehicles);
        }

        public IEnumerable<VehicleDTO> GetVehicles(Func<VehicleDTO, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public VehicleDTO GetVehicleInfo(int vehicleId)
        {
            return Mapper.Map<Vehicle, VehicleDTO>(_db.Vehicles.Get(vehicleId));

        }

        public IEnumerable<VehicleClassDTO> GetVehicleClasses()
        {
            return Mapper.Map<IEnumerable<VehicleClass>, List<VehicleClassDTO>>(_db.VehicleClasses.Find(x => x.EntryState == EntryState.Active));
        }

        public VehicleClassDTO GetVehicleClassInfo(int id)
        {
            return Mapper.Map<VehicleClassDTO>(_db.VehicleClasses.Get(id));
        }

        public IEnumerable<FuelTypeDTO> GetFuelTypes()
        {
            return Mapper.Map<IEnumerable<FuelType>, List<FuelTypeDTO>>(_db.FuelTypes.Find(x => x.EntryState == EntryState.Active));
        }

        public FuelTypeDTO GetFuelTypeInfo(int id)
        {
            return Mapper.Map<FuelTypeDTO>(_db.FuelTypes.Get(id));
        }

        public void RestoreVehicle(int id)
        {
            var vehicle = _db.Vehicles.Get(id);

            if (vehicle != null && vehicle.EntryState != EntryState.Active)
            {
                vehicle.EntryState = EntryState.Active;
                _db.Vehicles.Update(vehicle);
                _db.Save();
            }
        }
    }
}
