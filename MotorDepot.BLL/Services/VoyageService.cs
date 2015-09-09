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
using VoyageStatus = MotorDepot.DAL.Entities.VoyageStatus;
using System.Data.Entity.Validation;

namespace MotorDepot.BLL.Services
{
    public class VoyageService : IVoyageService
    {

        private IMotorDepotUnitOfWork _db;

        public VoyageService(IMotorDepotUnitOfWork db)
        {
            _db = db;
        }
        public ServiceResult AddVoyage(VoyageDTO voyageDto)
        {
            var result = new ServiceResult();

            if(voyageDto.LifeCycle == null)
                result.Errors.Add(new PropertyMessagePair {PropertyName = "LifeCycle", Message = "Voyage gotta have some life cycle"});
            if(voyageDto.EndPoint  == null)
                result.Errors.Add(new PropertyMessagePair {PropertyName = "EndPoint", Message = "Voyage gotta have some end point"});
            if(voyageDto.StartPoint == null)
                result.Errors.Add(new PropertyMessagePair {PropertyName = "StartPoint", Message = "Voyage gotta have some start point"});

            if (voyageDto.Driver == null || voyageDto.LifeCycle == null || voyageDto.StartPoint == null ||
                voyageDto.EndPoint == null) return result;

            try
            {
                _db.Voyages.Create(Mapper.Map<Voyage>(voyageDto));
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

        public ServiceResult CancelVoyage(int voyageId)
        {
            var result = new ServiceResult();

            var voyage = _db.Voyages.Get(voyageId);

            if (voyage == null)
            {
                result.Errors.Add(new PropertyMessagePair {PropertyName = "voyageId", Message = "There is no voyage with such id"});
                return result;
            }
            if (voyage.Status == VoyageStatus.Processing)
            {
                result.Errors.Add(new PropertyMessagePair {PropertyName = "voyageId", Message = "You can't cancel voyage while it's processing"});
                return result;
            }
            if (voyage.Status == VoyageStatus.Succeded)
            {
                result.Errors.Add(new PropertyMessagePair {PropertyName = "voyageId", Message = "You can't cancel voyage if it's already succeded"});
                return result;
            }
            if (voyage.Status == VoyageStatus.Canceled)
            {
                result.Errors.Add(new PropertyMessagePair {PropertyName = "voyageId", Message = "You can't cancel already canceled voyage"});
                return result;
            }

            voyage.Status = VoyageStatus.Canceled;
            voyage.LifeCycle.Canceled = DateTime.Now;

            try
            {
                _db.Voyages.Update(voyage);
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

        public ServiceResult DeleteVoyage(int voyageId)
        {
            var result = new ServiceResult();

            var voyage = _db.Voyages.Get(voyageId);

            if (voyage == null)
            {
                result.Errors.Add(new PropertyMessagePair { PropertyName = "voyageId", Message = "There is no voyage with such id" });
                return result;
            }
            if (voyage.Status == VoyageStatus.Processing)
            {
                result.Errors.Add(new PropertyMessagePair { PropertyName = "voyageId", Message = "You can't delete voyage while it's processing" });
                return result;
            }

            if (voyage.Status == VoyageStatus.Open || voyage.Status == VoyageStatus.Accepted)
            {
                voyage.Status = VoyageStatus.Canceled;
                voyage.LifeCycle.Canceled = DateTime.Now;
            }

            voyage.EntryState = EntryState.Removed;

            try
            {
                _db.Voyages.Update(voyage);
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

        public ServiceResult ModifyVoyage(VoyageDTO voyageDto)
        {
            var result = new ServiceResult();

            var voyage = _db.Voyages.Get(voyageDto.Id);

            if (voyage == null)
            {
                result.Errors.Add(new PropertyMessagePair { PropertyName = "Id", Message = "There is no such voyage in db you wanna modify"});
                return result;
            }
            if (voyage.Status == VoyageStatus.Processing)
            {
                result.Errors.Add(new PropertyMessagePair { PropertyName = "voyageDto", Message = "You can't modify voyage while it's processing" });
                return result;
            }
            if (voyage.Status == VoyageStatus.Accepted)
            {
                result.Errors.Add(new PropertyMessagePair { PropertyName = "voyageDto", Message = "You can't modify voyage after accepting" });
                return result;
            }
            if (voyage.Status == VoyageStatus.Succeded)
            {
                result.Errors.Add(new PropertyMessagePair { PropertyName = "voyageDto", Message = "You can't modify voyage after it's succeded" });
                return result;
            }

            try
            {
                _db.Voyages.Update(Mapper.Map<Voyage>(voyageDto));
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

        public ServiceResult MakeDriverVoyageRequest(int voyageId, int driverId)
        {
            var result = new ServiceResult();

            var voyage = _db.Voyages.Get(voyageId);
            var driver = _db.Drivers.Get(driverId);

            if (voyage == null || driver == null)
            {
                if(voyage == null)
                    result.Errors.Add(new PropertyMessagePair {PropertyName = "voyageId", Message = "There is no voyage in db with such id"});
                if(driver == null)
                    result.Errors.Add(new PropertyMessagePair {PropertyName = "driverId", Message = "There is no driver in db with such id"});
                return result;
            }

            var isFree =
                (!_db.DriverVoyageRequests.Find(
                    x =>
                        x.Driver == driver && voyage.RequestedEndTime.AddHours(1) > x.Voyage.RequestedStartTime &&
                        voyage.RequestedStartTime < x.Voyage.RequestedEndTime.AddHours(1)).Any());

            if (isFree)
            {
                result.Errors.Add(new PropertyMessagePair {PropertyName = "driverId", Message = "Driver is busy for this period of time"});
                return result;
            }

            try
            {
                _db.DriverVoyageRequests.Create(new DriverVoyageRequest {Voyage = voyage, Driver = driver});
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

        public ServiceResult CancelDriverVoyageRequest(int voyageId, int driverId)
        {
            var result = new ServiceResult();

            var voyage = _db.Voyages.Get(voyageId);
            var driver = _db.Drivers.Get(driverId);

            if (voyage == null || driver == null)
            {
                if (voyage == null)
                    result.Errors.Add(new PropertyMessagePair { PropertyName = "voyageId", Message = "There is no voyage in db with such id" });
                if (driver == null)
                    result.Errors.Add(new PropertyMessagePair { PropertyName = "driverId", Message = "There is no driver in db with such id" });
                return result;
            }

            var voyageRequest = _db.DriverVoyageRequests.Find(x => x.Voyage == voyage && x.Driver == driver).FirstOrDefault();
            if (voyageRequest == null)
            {
                result.Errors.Add(new PropertyMessagePair {PropertyName = "voyageId, driverId", Message = "There is no such request"});
                return result;
            }

            try
            {
                _db.DriverVoyageRequests.Remove(voyageId);
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

        public IEnumerable<VoyageDTO> GetVoyages()
        {
            return
                Mapper.Map<IEnumerable<Voyage>, List<VoyageDTO>>(_db.Voyages.Find(x => x.EntryState == EntryState.Active));
        }

        public IEnumerable<VoyageDTO> GetVoyages(Func<VoyageDTO, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public VoyageDTO GetVoyageInfo(int voyageId)
        {
            return Mapper.Map<VoyageDTO>(_db.Voyages.Get(voyageId));
        }
    }
}
