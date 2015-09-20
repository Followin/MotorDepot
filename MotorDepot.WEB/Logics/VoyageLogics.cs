using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mime;
using System.Web.Mvc;
using AutoMapper;
using MotorDepot.BLL.Abstract;
using MotorDepot.BLL.DTO;
using MotorDepot.BLL.Models;
using MotorDepot.WEB.Abstract;
using MotorDepot.WEB.Models;
using NLog;
using VoyageStatus = MotorDepot.WEB.Models.VoyageStatus;

namespace MotorDepot.WEB.Logics
{
    public class VoyageLogics : IVoyageLogics
    {
        private IVoyageService _voyageService;

        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public VoyageLogics(IVoyageService voyageService)
        {
            _voyageService = voyageService;
        }
        public void Create(CreateVoyageViewModel model)
        {
            var eventInfo = new LogEventInfo(LogLevel.Info, _logger.Name, "Voyage create");
            eventInfo.Properties["Name"] = model.Name;

            var voyageDto = Mapper.Map<VoyageDTO>(model);
            var result = _voyageService.AddVoyage(voyageDto);

            if (result.Status == ServiceResultStatus.Success)
                _logger.Info(eventInfo);
            else _logger.Error(eventInfo);
        }

        public IEnumerable<VoyageViewModel> GetVoyages(VoyageSortOrders? sortOrder = null, VoyageStatus? status = null)
        {
            var voyages = _voyageService.GetVoyages();
            voyages = Filter(voyages, sortOrder, status);
            return Mapper.Map<List<VoyageViewModel>>(voyages);
        }

        public bool RequestVoyage(int userId, int voyageId)
        {
            var eventInfo = new LogEventInfo(LogLevel.Info, _logger.Name, "Voyage request");
            eventInfo.Properties["voyageId"] = voyageId;

            var result = _voyageService.MakeDriverVoyageRequest(voyageId, userId);
            if (result.Status == ServiceResultStatus.Success)
            {
                _logger.Info(eventInfo);
                return true;
            }
            _logger.Error(eventInfo);
            return false;
        }

        public IEnumerable<DriverViewModel> GetRequestsForVoyage(int voyageId)
        {
            return Mapper.Map<List<DriverViewModel>>(_voyageService.GetRequestsForVoyage(voyageId));
        }

        public bool AcceptRequest(int voyageId, int driverId)
        {
            var eventInfo = new LogEventInfo(LogLevel.Info, _logger.Name, "Request accept");
            eventInfo.Properties["voyageId"] = voyageId;
            eventInfo.Properties["driverId"] = driverId;

            var result = _voyageService.AcceptRequest(voyageId, driverId);
            if (result.Status == ServiceResultStatus.Success)
            {
                _logger.Info(eventInfo);
                return true;
            }
            _logger.Error(eventInfo);
            return false;
        }

        public IEnumerable<VoyageViewModel> GetOpenVoyagesForUser(int id, VoyageSortOrders? sortOrder = null, VoyageStatus? status = null)
        {
            var voyages = _voyageService.GetOpenVoyagesForUser(id);
            voyages = Filter(voyages, sortOrder, status);
            return Mapper.Map<List<VoyageViewModel>>(voyages);
        }

        public VoyageViewModel GetVoyage(int id)
        {
            return Mapper.Map<VoyageViewModel>(_voyageService.GetVoyageInfo(id));
        }

        public bool Cancel(int id)
        {
            var eventInfo = new LogEventInfo(LogLevel.Info, _logger.Name, "Voyage calcelation");
            eventInfo.Properties["voyageId"] = id;

            var result = _voyageService.CancelVoyage(id);


            if (result.Status == ServiceResultStatus.Success)
            {
                _logger.Info(eventInfo);
                return true;
            }
            _logger.Error(eventInfo);
            return false;
        }

        public IEnumerable<VoyageViewModel> GetUserVoyages(int id, VoyageSortOrders? sortOrder = null, VoyageStatus? status = null)
        {
            var voyages = _voyageService.GetUserVoyages(id);
            voyages = Filter(voyages, sortOrder, status);
            return Mapper.Map<List<VoyageViewModel>>(voyages);
        }

        public bool Complete(int id, int currentUserId)
        {
            var eventInfo = new LogEventInfo(LogLevel.Info, _logger.Name, "Voyage complete");
            eventInfo.Properties["voyageId"] = id;
            eventInfo.Properties["userId"] = currentUserId;

            var result = _voyageService.Complete(id, currentUserId);

            if (result.Status == ServiceResultStatus.Success)
            {
                _logger.Info(eventInfo);
                return true;
            }
            _logger.Error(eventInfo);
            return false;
        }

        public IEnumerable<SelectListItem> GetOrdersSelectList()
        {
            return Enum.GetValues(typeof(VoyageSortOrders)).Cast<Object>()
                .Select(v => new SelectListItem
                {
                    Text = GetEnumValueDisplayName(v),
                    Value = v.ToString()
                });
        }

        public IEnumerable<SelectListItem> GetStatusesSelectList()
        {
            var result =  Enum.GetValues(typeof (VoyageStatus)).Cast<Object>()
                .Select(v => new SelectListItem
                {
                    Text = GetEnumValueDisplayName(v),
                    Value = v.ToString()
                }).ToList();
            result.Insert(0,new SelectListItem {Text = "Все", Value = null, Selected = true});
            return result;

        }

        #region privateMethods

        private IEnumerable<VoyageDTO> Filter(IEnumerable<VoyageDTO> voyages,
            VoyageSortOrders? sortOrder = null, VoyageStatus? status = null)
        {
            if (status != null)
                voyages = voyages.Where(x => x.Status == (BLL.DTO.VoyageStatus)status);
            if (sortOrder != null)
            {
                switch ((VoyageSortOrders)sortOrder)
                {
                    case VoyageSortOrders.Name:
                        voyages = voyages.OrderBy(x => x.Name);
                        break;
                    case VoyageSortOrders.Status:
                        voyages = voyages.OrderBy(x => x.Status);
                        break;
                    case VoyageSortOrders.StartTime:
                        voyages = voyages.OrderBy(x => x.RequestedStartTime);
                        break;
                    case VoyageSortOrders.EndTime:
                        voyages = voyages.OrderBy(x => x.RequestedEndTime);
                        break;
                    case VoyageSortOrders.LeadTime:
                        voyages = voyages.OrderBy(x => x.RequestedEndTime - x.RequestedStartTime);
                        break;
                    default:
                        voyages = voyages.OrderBy(x => x.Name);
                        break;
                }
            }
            return voyages;
        } 
        private String GetEnumValueDisplayName(object o)
        {
            var result = null as string;
            var display = o.GetType()
                .GetMember(o.ToString()).First()
                .GetCustomAttributes(false)
                .OfType<DisplayAttribute>()
                .LastOrDefault();
            if (display != null)
            {
                result = display.GetName();
            }

            return result ?? o.ToString();
        }
        #endregion
    }
}