using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using MotorDepot.BLL.DTO;
using MotorDepot.BLL.Models;

namespace MotorDepot.BLL.Abstract
{
    public interface IVehicleService
    {
        ServiceResult AddVehicle(VehicleDTO vehicleDto);
        ServiceResult ModifyVehicle(VehicleDTO vehicleDto);
        ServiceResult DeleteVehicle(Int32 vehicleId);
        Boolean IsVehicleFree(Int32 vehicleId, DateTime startTime, DateTime? endTime = null);
        IEnumerable<VehicleDTO> GetVehicles();
        IEnumerable<VehicleDTO> GetFreeVehicles(DateTime startTime, DateTime? endTime = null);
        IEnumerable<VehicleDTO> GetVehicles(Func<VehicleDTO, Boolean> predicate);
        VehicleDTO GetVehicleInfo(Int32 vehicleId);
        IEnumerable<VehicleClassDTO> GetVehicleClasses();
        VehicleClassDTO GetVehicleClassInfo(Int32 id);
        IEnumerable<FuelTypeDTO> GetFuelTypes();
        FuelTypeDTO GetFuelTypeInfo(Int32 id);
        void RestoreVehicle(int id);
    }
}
