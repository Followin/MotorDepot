using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using MotorDepot.WEB.Models;

namespace MotorDepot.WEB.Abstract
{
    public interface IVoyageLogics
    {
        void Create(CreateVoyageViewModel model);
        IEnumerable<VoyageViewModel> GetVoyages(VoyageSortOrders? sortOrder = null, VoyageStatus? status = null);
        Boolean RequestVoyage(int userId, int voyageId);
        IEnumerable<DriverViewModel> GetRequestsForVoyage(int voyageId);
        Boolean AcceptRequest(Int32 voyageId, Int32 driverId);
        IEnumerable<VoyageViewModel> GetOpenVoyagesForUser(int id, VoyageSortOrders? sortOrder = null, VoyageStatus? status = null);
        VoyageViewModel GetVoyage(Int32 id);
        Boolean Cancel(int id);
        IEnumerable<VoyageViewModel> GetUserVoyages(Int32 id, VoyageSortOrders? sortOrder = null, VoyageStatus? status = null);
        Boolean Complete(int id, int currentUserId);
        IEnumerable<SelectListItem> GetOrdersSelectList();
        IEnumerable<SelectListItem> GetStatusesSelectList();
    }
}