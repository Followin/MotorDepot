using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MotorDepot.BLL.Abstract;

namespace MotorDepot.WEB.Controllers
{
    public class VoyageController : Controller
    {
        private IAuthService _authService;

        public VoyageController(IAuthService authService)
        {
            _authService = authService;
        }

        public ActionResult Index()
        {
            var i = _authService.ValidateUser("Admin", "Admin");
            return View();
        }

    }
}
