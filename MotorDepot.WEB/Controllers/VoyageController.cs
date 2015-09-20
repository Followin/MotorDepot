using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using Microsoft.Ajax.Utilities;
using MotorDepot.BLL.Abstract;
using MotorDepot.BLL.Services;
using MotorDepot.WEB.Abstract;
using MotorDepot.WEB.Models;

namespace MotorDepot.WEB.Controllers
{
    public class VoyageController : Controller
    {
        private IVoyageLogics _logics;

        public VoyageController(IVoyageLogics logics)
        {
            _logics = logics;
        }


        [Authorize]
        public ActionResult Index(VoyageSortOrders? sortOrder = null, VoyageStatus? status = null)
        {
            if (Request.IsAjaxRequest())
            {
                if (User.IsInRole("Admin") || User.IsInRole("Controller"))
                {
                    return PartialView("_voyagesList", _logics.GetVoyages(sortOrder, status));
                }
                if (User.IsInRole("Driver"))
                {
                    return PartialView("_voyagesList",_logics.GetOpenVoyagesForUser(CurrentUserId, sortOrder, status));
                }
            }

            ViewBag.StatusesSelectList = _logics.GetStatusesSelectList();
            ViewBag.OrdersSelectList = _logics.GetOrdersSelectList();
            if (User.IsInRole("Admin") || User.IsInRole("Controller"))
            {
                return View(_logics.GetVoyages(sortOrder, status));
            }
            if (User.IsInRole("Driver"))
            {
                return View(_logics.GetOpenVoyagesForUser(CurrentUserId, sortOrder, status));
            }

            return RedirectToAction("Login", "Account");
        }

        [Authorize(Roles = "Admin, Controller")]
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Controller")]
        public ActionResult Create(CreateVoyageViewModel model)
        {
            if (ModelState.IsValid)
            {
                _logics.Create(model);
                SuccessMessage("Рейс успешно создан");
                return RedirectToAction("Index", "Voyage");
            }

            ErrorMessage("Создание рейса завершилось ошибкой");
            return View(model);


        }


        [Authorize(Roles = "Driver")]
        public JsonResult RequestVoyage(Int32 voyageId)
        {

            var success = _logics.RequestVoyage(CurrentUserId, voyageId);
            return Json(success, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Admin, Controller")]
        public ActionResult RequestsForVoyage(Int32 voyageId)
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView("_requestsList", _logics.GetRequestsForVoyage(voyageId));
            }
            return null;
        }

        [Authorize(Roles = "Admin, Controller")]
        public JsonResult AcceptRequest(Int32 voyageId, Int32 driverId)
        {
            return Json(_logics.AcceptRequest(voyageId, driverId), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Admin, Controller")]
        public ActionResult Details(Int32 voyageId)
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView("_voyageInfo", _logics.GetVoyage(voyageId));
            }
            return View("_voyageInfo", _logics.GetVoyage(voyageId));
        }

        [Authorize(Roles = "Admin, Controller")]
        public ActionResult Cancel(Int32 id)
        {
            if (_logics.Cancel(id))
            {
                SuccessMessage("Рейс отменен", "Вернуть", Url.Action("UndoCancelation", new {id = id}));
                return RedirectToAction("Index");
            }
            ErrorMessage("Рейс более нельзя отменить");
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Driver")]
        public ActionResult My(VoyageSortOrders? sortOrder = null, VoyageStatus? status = null)
        {
            var model = _logics.GetUserVoyages(CurrentUserId, sortOrder, status);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_myVoyageList", model);
            }
            ViewBag.StatusesSelectList = _logics.GetStatusesSelectList();
            ViewBag.OrdersSelectList = _logics.GetOrdersSelectList();
            return View(model);
        }

        public ActionResult Complete(int id)
        {
            if (_logics.Complete(id, CurrentUserId))
            {
                SuccessMessage("Рейс выполнен успешно");
                return RedirectToAction("My");
            }

            ErrorMessage("На данный момент завершение рейса невозможно");
            return RedirectToAction("My");

        }


        #region privateMethods
        private Int32 CurrentUserId
        {
            get
            {
                var authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                if (authCookie != null)
                {
                    var ticket = FormsAuthentication.Decrypt(authCookie.Value);
                    var id = Convert.ToInt32(ticket.UserData);
                    return id;
                }
                return 0;
            }
        }

        private void TempMessage(TempMessageType type, String message, String linkText = null, String linkHref = null)
        {
            TempMessage tempMessage;
            if (linkText != null && linkHref != null)
                tempMessage = new LinkTempMessage(type, message, linkText, linkHref);
            else tempMessage = new TempMessage(type, message);

            if (TempData.ContainsKey("TempMessages"))
                ((Collection<TempMessage>)(TempData["TempMessages"])).Add(tempMessage);
            else TempData.Add("TempMessages", new Collection<TempMessage> { tempMessage });
        }

        private void SuccessMessage(String message, String linkText = null, String linkHref = null)
        {
            TempMessage(TempMessageType.Success, message, linkText, linkHref);
        }

        private void ErrorMessage(String message, String linkText = null, String linkHref = null)
        {
            TempMessage(TempMessageType.Error, message, linkText, linkHref);
        }

        #endregion
    }
}
