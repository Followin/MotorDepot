using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AutoMapper;
using MotorDepot.WEB.Abstract;
using MotorDepot.WEB.Extensions;
using MotorDepot.WEB.Models;

namespace MotorDepot.WEB.Controllers
{
    [Authorize(Roles = "Admin")]
    public class VehicleController : Controller
    {
        private IControllersLogics _sLogics;
        private IVehiclesLogics _logics;

        public VehicleController(IControllersLogics sLogics, IVehiclesLogics logics)
        {
            _sLogics = sLogics;
            _logics = logics;
        }

        public ActionResult Index()
        {
            return View(_logics.GetVehicles());
        }

        public ActionResult Create()
        {
            ViewBag.FuelTypes = _logics.GetFuelTypesSelectList();
            ViewBag.VehicleClasses = _sLogics.GetVehicleClassesSelectList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(CreateVehicleViewModel model)
        {


            if (ModelState.IsValid)
            {
                _logics.Create(model);
                SuccessMessage("Автомобиль успешно добавлен");
                return RedirectToAction("Index");
            }

            ViewBag.FuelTypes = _logics.GetFuelTypesSelectList();
            ViewBag.VehicleClasses = _sLogics.GetVehicleClassesSelectList();
            ErrorMessage("При добавлении автомобиля произошла ошибка");
            return View(model);

        }

        public ActionResult Edit(Int32 id)
        {
            var model = _logics.GetEditVehicleInfo(id);

            var vehicleClasses = _sLogics.GetVehicleClassesSelectList();
            vehicleClasses.FirstOrDefault(x => Int32.Parse(x.Value) == model.VehicleClassId).Selected = true;

            var fuelTypes = _logics.GetFuelTypesSelectList();
            fuelTypes.FirstOrDefault(x => Int32.Parse(x.Value) == model.Drive.FuelTypeId).Selected = true;

            ViewBag.VehicleClasses = vehicleClasses;
            ViewBag.FuelTypes = fuelTypes;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditVehicleViewModel model)
        {
            if (ModelState.IsValid)
            {
                _logics.SubmitEdit(model);
                SuccessMessage("Автомобиль успешно изменен");
                return RedirectToAction("Index");
            }
            ErrorMessage("При изменении автомобиля произошла ошибка");
            return View(model);
        }



        [Authorize]
        public ActionResult FullInfo(Int32 id)
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView("_fullVehicleInfo", _logics.GetVehicleInfo(id));
            }
            return PartialView("_fullVehicleInfo", _logics.GetVehicleInfo(id));
        }

        public ActionResult FreeVehicles(DateTime startTime, DateTime endTime)
        {
            return PartialView("_chooseAuto", _logics.GetFreeVehicles(startTime, endTime));
        }


        public ActionResult Delete(Int32 id)
        {
            if (_logics.DeleteVehicle(id))
            {

                SuccessMessage("Автомобиль успешно удален", "Отменить",
                    "/Vehicle/UndoDeletion/" + id);
                return RedirectToAction("Index");
            }

            ErrorMessage(
                "Невозможно удалить данный автомобиль. Возможно он участвует в выполняемых рейсах");
            return RedirectToAction("Index");

        }

        
        public ActionResult UndoDeletion(Int32 id)
        {
            _logics.UndoDeletion(id);

            return RedirectToAction("Index");
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
