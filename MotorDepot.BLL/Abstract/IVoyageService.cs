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
    public interface IVoyageService
    {
        ServiceResult AddVoyage(VoyageDTO voyageDto);
        ServiceResult CancelVoyage(Int32 voyageId);
        ServiceResult DeleteVoyage(Int32 voyageId);
        ServiceResult ModifyVoyage(VoyageDTO voyageDto);
        ServiceResult MakeDriverVoyageRequest(Int32 voyageId, Int32 userId);
        ServiceResult CancelDriverVoyageRequest(Int32 voyageId, Int32 userId);
        IEnumerable<VoyageDTO> GetVoyages();
        IEnumerable<VoyageDTO> GetVoyages(Func<VoyageDTO, Boolean> predicate);
        VoyageDTO GetVoyageInfo(Int32 voyageId);
        IEnumerable<DriverDTO> GetRequestsForVoyage(Int32 voyageId);
        ServiceResult AcceptRequest(Int32 voyageId, Int32 driverId);
        IEnumerable<VoyageDTO> GetOpenVoyagesForUser(int id);
        IEnumerable<VoyageDTO> GetUserVoyages(Int32 id);
        ServiceResult Complete(int id, int userId);
    }
}
