using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using MotorDepot.BLL.Abstract;
using MotorDepot.BLL.DTO;
using MotorDepot.BLL.Models;
using MotorDepot.WEB.Abstract;
using MotorDepot.WEB.Extensions;
using MotorDepot.WEB.Models;
using NLog;

namespace MotorDepot.WEB.Logics
{
    public class VehicleLogics : IVehiclesLogics
    {
        private IVehicleService _vehicleService;
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public VehicleLogics(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }
        public IEnumerable<SelectListItem> GetFuelTypesSelectList()
        {
            var fuelTypes = _vehicleService.GetFuelTypes().Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToList();
            return fuelTypes;
        }

        public void Create(CreateVehicleViewModel model)
        {
            var eventInfo = new LogEventInfo(LogLevel.Info, _logger.Name, "Vehicle create");
            eventInfo.Properties["ModelName"] = model.Name;

            var vehicleDto = Mapper.Map<VehicleDTO>(model);
            var result =  _vehicleService.AddVehicle(vehicleDto);

            if (result.Status == ServiceResultStatus.Success)
                _logger.Info(eventInfo);
            else _logger.Error(eventInfo);
        }

        public IEnumerable<VehicleViewModel> GetVehicles()
        {
            return Mapper.Map<List<VehicleViewModel>>(_vehicleService.GetVehicles());
        }

        public VehicleViewModel GetVehicleInfo(Int32 id)
        {
            return Mapper.Map<VehicleViewModel>(_vehicleService.GetVehicleInfo(id));
        }

        public EditVehicleViewModel GetEditVehicleInfo(Int32 id)
        {
            return Mapper.Map<EditVehicleViewModel>(_vehicleService.GetVehicleInfo(id));
        }

        public void SubmitEdit(EditVehicleViewModel model)
        {
            var eventInfo = new LogEventInfo(LogLevel.Info, _logger.Name, "Vehicle edit");
            eventInfo.Properties["ModelName"] = model.Name;

            var vehicleDto = Mapper.Map<VehicleDTO>(model);
            if (model.Photo == null)
                vehicleDto.Photo = _vehicleService.GetVehicleInfo(model.Id).Photo;
            else
                vehicleDto.Photo = new ImageDTO
                {
                    Type = model.Photo.ContentType,
                    Bytes = model.Photo.InputStream.ReadBytes(model.Photo.ContentLength)
                };
            var result = _vehicleService.ModifyVehicle(vehicleDto);

            if (result.Status == ServiceResultStatus.Success)
                _logger.Info(eventInfo);
            else _logger.Error(eventInfo);
        }

        public IEnumerable<VehicleViewModel> GetFreeVehicles(DateTime startTime, DateTime endTime)
        {
            return Mapper.Map<List<VehicleViewModel>>(_vehicleService.GetFreeVehicles(startTime, endTime));
        }

        public Boolean DeleteVehicle(Int32 id)
        {
            var eventInfo = new LogEventInfo(LogLevel.Info, _logger.Name, "Vehicle deletion");
            eventInfo.Properties["id"] = id;

            var result = _vehicleService.DeleteVehicle(id);

            if (result.Status == ServiceResultStatus.Success)
                _logger.Info(eventInfo);
            else _logger.Error(eventInfo);

            return result.Status == ServiceResultStatus.Success;
        }

        public void UndoDeletion(int id)
        {
            var eventInfo = new LogEventInfo(LogLevel.Info, _logger.Name, "Vehicle deletion undo");
            eventInfo.Properties["id"] = id;

            _vehicleService.RestoreVehicle(id);

            _logger.Info(eventInfo);
        }
    }
}