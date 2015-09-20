using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MotorDepot.BLL.Abstract;
using MotorDepot.WEB.Abstract;
using MotorDepot.WEB.Models;

namespace MotorDepot.WEB.Controllers
{
    public class AccountController : Controller
    {
        private IControllersLogics _sLogics;
        private IAccountLogics _logics;

        public AccountController(IControllersLogics sLogics, IAccountLogics logics)
        {
            _sLogics = sLogics;
            _logics = logics;
        }

        [Authorize(Roles="Admin")]
        public ActionResult Index()
        {
            ViewBag.Roles = _logics.GetRoles();
            return View(_logics.GetUsers());
        }


        public JsonResult ChangeUserRole(Int32 userId, Int32 roleId)
        {
            if (!Request.IsAjaxRequest()) return Json(false);

            if (_logics.ChangeUserRole(userId, roleId))
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UserInfo(Int32 id)
        {
            return View(_logics.GetUser(id));
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Confirm(Int32 id)
        {
            if (_logics.Confirm(id))
                InfoMessage("Пользователь подтвержден");
            else ErrorMessage("Невозможно подтвердить данного пользователя");

            return RedirectToAction("Index");
        }

        [Authorize(Roles="Admin")]
        public ActionResult Refuse(Int32 id)
        {
            if (_logics.Refuse(id))
                InfoMessage("Пользователь был удален");
            else ErrorMessage("Данный пользователь уже был удален");

            return RedirectToAction("Index");
        }
        



        public ActionResult Register()
        {

            ViewBag.VehicleClasses = _sLogics.GetVehicleClassesSelectList();
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Register(RegisterUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                _logics.Register(model);
                SuccessMessage("Пользователь успешно зарегестрирован");
                InfoMessage("Ожидайте подтверждения аккаунта администратором");
            }
            else
            {
                ViewBag.VehicleClasses = _sLogics.GetVehicleClassesSelectList();
                return View(model);
            }

            return RedirectToAction("Index", "Voyage");
        }

        public ActionResult Login()
        {
            if(!User.Identity.IsAuthenticated)
                return View();
            return RedirectToAction("Index", "Voyage");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (!User.Identity.IsAuthenticated && ModelState.IsValid)
            {
                var responseObj = _logics.Login(model);
                if (responseObj != null)
                {
                    Response.Cookies.Add(responseObj.Cookie);
                    SuccessMessage("Вы успешно вошли в систему");
                }
                else
                {
                    ModelState.AddModelError("","Логин или пароль неверен");
                    return View();
                }
            }
            return RedirectToAction("Index", "Voyage");
        }

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            HttpContext.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);
            InfoMessage("Вы вышли из системы");
            return RedirectToAction("Index", "Voyage");
        }

        

        public JsonResult IsNicknameFree(String nickname)
        {
            return Json(_logics.IsNicknameFree(nickname), JsonRequestBehavior.AllowGet);
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

        private void InfoMessage(String message)
        {
            TempMessage(TempMessageType.Info, message);
        }

        #endregion

    }
}
