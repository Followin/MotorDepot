using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using MotorDepot.BLL.DTO;
using MotorDepot.WEB.Models;

namespace MotorDepot.WEB.Abstract
{
    public interface IVehiclesLogics
    {
        IEnumerable<SelectListItem> GetFuelTypesSelectList();
        void Create(CreateVehicleViewModel model);
        IEnumerable<VehicleViewModel> GetVehicles();
        VehicleViewModel GetVehicleInfo(Int32 id);
        IEnumerable<VehicleViewModel> GetFreeVehicles(DateTime startTime, DateTime endTime);
        EditVehicleViewModel GetEditVehicleInfo(Int32 id);
        void SubmitEdit(EditVehicleViewModel model);
        Boolean DeleteVehicle(Int32 id);
        void UndoDeletion(int id);
    }
}
