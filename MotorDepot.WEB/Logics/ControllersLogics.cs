using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MotorDepot.BLL.Abstract;
using MotorDepot.WEB.Abstract;

namespace MotorDepot.WEB.Logics
{
    public class ControllersLogics : IControllersLogics
    {

        private IVehicleService _vehicleService;

        public ControllersLogics(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }
        public IEnumerable<SelectListItem> GetVehicleClassesSelectList()
        {
            var vehicleClasses = _vehicleService.GetVehicleClasses();
            var result = vehicleClasses.Select(licenseType => new SelectListItem
            {
                Text = licenseType.Name + " (" + licenseType.Description + ")",
                Value = licenseType.Id.ToString()
            }).ToList();

            return result;
        }
    }
}