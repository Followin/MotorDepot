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

            if (voyageDto.EndPoint == null)
                result.Errors.Add(new PropertyMessagePair { PropertyName = "EndPoint", Message = "Voyage gotta have some end point" });
            if (voyageDto.StartPoint == null)
                result.Errors.Add(new PropertyMessagePair { PropertyName = "StartPoint", Message = "Voyage gotta have some start point" });

            if (voyageDto.StartPoint == null || voyageDto.EndPoint == null)
                return result;

            voyageDto.Status = DTO.VoyageStatus.Open;
            voyageDto.LifeCycle = new VoyageLifeCycleDTO { Opened = DateTime.Now };

            try
            {
                _db.Voyages.Create(Mapper.Map<Voyage>(voyageDto));
                _db.Save();
            }
            catch (DbEntityValidationException ex)
            {
                result.Append(ex);
            }

            return result;
        }

        public ServiceResult CancelVoyage(int voyageId)
        {
            CheckStatus();

            var result = new ServiceResult();

            var voyage = _db.Voyages.Get(voyageId);

            if (voyage == null)
            {
                result.Errors.Add(new PropertyMessagePair { PropertyName = "voyageId", Message = "There is no voyage with such id" });
                return result;
            }
            if (voyage.Status == VoyageStatus.Processing)
            {
                result.Errors.Add(new PropertyMessagePair { PropertyName = "voyageId", Message = "You can't cancel voyage while it's processing" });
                return result;
            }
            if (voyage.Status == VoyageStatus.Succeded)
            {
                result.Errors.Add(new PropertyMessagePair { PropertyName = "voyageId", Message = "You can't cancel voyage if it's already succeded" });
                return result;
            }
            if (voyage.Status == VoyageStatus.Canceled)
            {
                result.Errors.Add(new PropertyMessagePair { PropertyName = "voyageId", Message = "You can't cancel already canceled voyage" });
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
                result.Append(ex);
            }

            return result;


        }

        public ServiceResult DeleteVoyage(int voyageId)
        {
            CheckStatus();

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

            voyage.EntryState = DAL.Entities.EntryState.Removed;

            try
            {
                _db.Voyages.Update(voyage);
                _db.Save();
            }
            catch (DbEntityValidationException ex)
            {
                result.Append(ex);
            }

            return result;


        }

        public ServiceResult ModifyVoyage(VoyageDTO voyageDto)
        {
            var result = new ServiceResult();

            var voyage = _db.Voyages.Get(voyageDto.Id);

            if (voyage == null)
            {
                result.Errors.Add(new PropertyMessagePair { PropertyName = "Id", Message = "There is no such voyage in db you wanna modify" });
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
                result.Append(ex);
            }
            return result;
        }

        public ServiceResult MakeDriverVoyageRequest(int voyageId, int userId)
        {
            CheckStatus();

            var result = new ServiceResult();

            var voyage = _db.Voyages.Get(voyageId);
            var user = _db.Users.Get(userId);
            var driver = user.Driver;

            if (voyage == null || driver == null)
            {
                if (voyage == null)
                    result.Errors.Add(new PropertyMessagePair { PropertyName = "voyageId", Message = "There is no voyage in db with such id" });
                if (driver == null)
                    result.Errors.Add(new PropertyMessagePair { PropertyName = "driverId", Message = "There is no driver in db with such id" });
                return result;
            }

            var isFree =
                (!_db.DriverVoyageRequests.Find(
                    x =>
                        x.Driver == driver && voyage.RequestedEndTime.AddHours(1) > x.Voyage.RequestedStartTime &&
                        voyage.RequestedStartTime < x.Voyage.RequestedEndTime.AddHours(1)).Any());

            if (!isFree)
            {
                result.Errors.Add(new PropertyMessagePair { PropertyName = "driverId", Message = "Driver is busy for this period of time" });
                return result;
            }

            try
            {
                _db.DriverVoyageRequests.Create(new DriverVoyageRequest { VoyageId = voyage.Id, DriverId = driver.Id });
                _db.Save();
            }
            catch (DbEntityValidationException ex)
            {
                result.Append(ex);
            }
            return result;
        }

        public ServiceResult CancelDriverVoyageRequest(int voyageId, int userId)
        {
            var result = new ServiceResult();

            var voyage = _db.Voyages.Get(voyageId);
            var user = _db.Users.Get(userId);
            var driver = user.Driver;

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
                result.Errors.Add(new PropertyMessagePair { PropertyName = "voyageId, driverId", Message = "There is no such request" });
                return result;
            }

            try
            {
                _db.DriverVoyageRequests.Remove(voyageId);
                _db.Save();
            }
            catch (DbEntityValidationException ex)
            {
                result.Append(ex);
            }
            return result;
        }

        public IEnumerable<VoyageDTO> GetVoyages()
        {
            CheckStatus();
            return
                Mapper.Map<IEnumerable<Voyage>, List<VoyageDTO>>(_db.Voyages.Find(x => x.EntryState == DAL.Entities.EntryState.Active));
        }

        public IEnumerable<VoyageDTO> GetVoyages(Func<VoyageDTO, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public VoyageDTO GetVoyageInfo(int voyageId)
        {
            return Mapper.Map<VoyageDTO>(_db.Voyages.Get(voyageId));
        }

        public IEnumerable<DriverDTO> GetRequestsForVoyage(int voyageId)
        {
            var driverIds = _db.DriverVoyageRequests.Find(x => x.VoyageId == voyageId).Select(x => x.DriverId).ToList();
            return Mapper.Map<List<DriverDTO>>(_db.Drivers.Find(x => driverIds.Contains(x.Id)));
        }

        public ServiceResult AcceptRequest(int voyageId, int driverId)
        {
            CheckStatus();

            var result = new ServiceResult();

            var request = _db.DriverVoyageRequests.Find(x => x.DriverId == driverId && x.VoyageId == voyageId).FirstOrDefault();
            var allRequestsForVoyage = _db.DriverVoyageRequests.Find(x => x.VoyageId == voyageId).ToList();

            if (request == null)
            {
                result.Errors.Add(new PropertyMessagePair { PropertyName = "voyageId/driverId", Message = "There is no request in db with such id" });
                return result;
            }

            var voyage = _db.Voyages.Get(voyageId);
            voyage.Requests = null;

            try
            {
                allRequestsForVoyage.ForEach(x => _db.DriverVoyageRequests.Remove(x.Id));

                voyage.DriverId = driverId;
                voyage.Status = VoyageStatus.Accepted;
                voyage.LifeCycle.Acceped = DateTime.Now;

                _db.Voyages.Update(voyage);

                _db.Save();
            }
            catch (DbEntityValidationException ex)
            {
                result.Append(ex);
            }

            return result;

        }

        public IEnumerable<VoyageDTO> GetOpenVoyagesForUser(int id)
        {
            CheckStatus();


            var user = _db.Users.Get(id);

            if (user == null) return new List<VoyageDTO>();
            var driver = _db.Drivers.Find(x => x.Id == user.Driver.Id).FirstOrDefault();

            if (driver == null) return new List<VoyageDTO>();


            //Searching insersection of voyages
            var driverRequestsVoyageIds = _db.DriverVoyageRequests.Find(x => x.DriverId == driver.Id).Select(x => x.VoyageId).ToList();
            var driverVoyages =
                _db.Voyages.Find(
                    x =>
                        x.Status != VoyageStatus.Canceled && x.Status != VoyageStatus.Succeded &&
                        (x.DriverId == driver.Id || driverRequestsVoyageIds.Contains(x.Id))).ToList();
            var openedVoyages = _db.Voyages.Find(x => x.Status == VoyageStatus.Open).ToList();
            var voyages =
                openedVoyages.Where(
                    x =>
                        driver.DriverLicense.VehicleClasses.Select(_ => _.Id).Contains(x.Vehicle.Class.Id) &&
                        !driverVoyages.Any(
                            inner =>
                                inner.RequestedEndTime > x.RequestedStartTime &&
                                inner.RequestedStartTime < x.RequestedEndTime)).ToList();
            return Mapper.Map<List<VoyageDTO>>(voyages);
        }

        public IEnumerable<VoyageDTO> GetUserVoyages(int id)
        {
            CheckStatus();

            var user = _db.Users.Get(id);
            if (user == null) return new List<VoyageDTO>();

            var driver = _db.Drivers.Find(x => x.Id == user.Driver.Id).FirstOrDefault();
            if (driver == null) return new List<VoyageDTO>();

            var driverRequestsVoyageIds = _db.DriverVoyageRequests.Find(x => x.DriverId == driver.Id).Select(x => x.VoyageId).ToList();
            var driverVoyages = _db.Voyages.Find(x => x.DriverId == driver.Id || driverRequestsVoyageIds.Contains(x.Id));
            return Mapper.Map<List<VoyageDTO>>(driverVoyages);

        }

        public ServiceResult Complete(int id, int userId)
        {
            CheckStatus();

            var result = new ServiceResult();

            var user = _db.Users.Get(userId);
            if (user == null)
            {
                result.Errors.Add(new PropertyMessagePair {PropertyName = "userId", Message = "There is no such user in db"});
                return result;
            }

            var driver = _db.Drivers.Get(user.Driver.Id);
            if (driver == null)
            {
                result.Errors.Add(new PropertyMessagePair {PropertyName = "userId", Message = "User has no driver attached"});
                return result;
            }

            var voyage = _db.Voyages.Get(id);
            if (voyage == null)
            {
                result.Errors.Add(new PropertyMessagePair {Message = "There is no voyage in db with such id", PropertyName = "id"});
                return result;
            }

            if (voyage.DriverId != driver.Id)
            {
                result.Errors.Add(new PropertyMessagePair {PropertyName = "driverId", Message = "Only voyage driver can mark voyage as succeded"});
                return result;
            }

            voyage.Status = VoyageStatus.Succeded;
            voyage.LifeCycle.Succeded = DateTime.Now;

            try
            {
                _db.Voyages.Update(voyage);
                _db.Save();
            }
            catch (DbEntityValidationException ex)
            {
                result.Append(ex);
            }

            return result;
        }

        public void CheckStatus()
        {
            var acceptedVoyages =
                _db.Voyages.Find(x => x.Status == VoyageStatus.Accepted && x.RequestedStartTime < DateTime.Now);
            foreach (var voyage in acceptedVoyages)
            {
                voyage.Status = VoyageStatus.Processing;
                voyage.LifeCycle.ProcessingStart = voyage.RequestedStartTime;
                _db.Voyages.Update(voyage);
                _db.Save();
            }

            var processingVoyages =
                _db.Voyages.Find(
                    x => x.Status == VoyageStatus.Processing && x.RequestedEndTime.AddHours(1) < DateTime.Now);
            foreach (var voyage in processingVoyages)
            {
                voyage.Status = VoyageStatus.Canceled;
                voyage.LifeCycle.Canceled = DateTime.Now;
                _db.Voyages.Update(voyage);
                _db.Save();
            }

            var openedVoyages =
                _db.Voyages.Find(
                    x => x.Status == VoyageStatus.Open && x.RequestedStartTime < DateTime.Now);
            foreach (var voyage in openedVoyages)
            {
                voyage.Status = VoyageStatus.Canceled;
                voyage.LifeCycle.Canceled = DateTime.Now;
                _db.Voyages.Update(voyage);
                _db.Save();
            }
        }
    }
}
