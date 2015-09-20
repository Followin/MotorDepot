using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AutoMapper;
using MotorDepot.BLL.Abstract;
using MotorDepot.BLL.DTO;
using MotorDepot.BLL.Models;
using MotorDepot.WEB.Abstract;
using MotorDepot.WEB.Models;
using NLog;

namespace MotorDepot.WEB.Logics
{
    public class AccountLogics : IAccountLogics
    {
        private IAuthService _authService;
        private IVehicleService _vehicleService;
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public AccountLogics(IAuthService authService, IVehicleService vehicleService)
        {
            _authService = authService;
            _vehicleService = vehicleService;
        }

        public void Register(RegisterUserViewModel model)
        {
            var userDto = Mapper.Map<UserDTO>(model);
            var result = _authService.RegisterUser(userDto);

            var eventInfo = new LogEventInfo(LogLevel.Info, _logger.Name, "New user registration");
            eventInfo.Properties["Nickname"] = model.Nickname;
            if (result.Status == ServiceResultStatus.Success)
                _logger.Info(eventInfo);
            else _logger.Error(eventInfo);
        }

        public LoginResponse Login(LoginViewModel model)
        {
            var userId = _authService.ValidateUser(model.Login, model.Password);

            var eventInfo = new LogEventInfo(LogLevel.Info, _logger.Name, "Login");
            eventInfo.Properties["Login"] = model.Login;

            if (userId.HasValue)
            {
                var userInfo = _authService.GetUserInfo(userId.Value);
                var ticket = new FormsAuthenticationTicket(1, userInfo.Nickname, DateTime.Now, DateTime.Now.AddDays(1),
                    model.Persistent, userId.Value.ToString());
                var ticketStr = FormsAuthentication.Encrypt(ticket);
                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, ticketStr);

                _logger.Info(eventInfo);
                return new LoginResponse { Name = userInfo.Nickname, Cookie = cookie, Role = userInfo.Role.Name };

                

            }
            _logger.Error(eventInfo);
            return null;
        }

        public bool IsNicknameFree(string nickname)
        {
            return _authService.IsUsernameFree(nickname);
        }

        public IEnumerable<UserViewModel> GetUsers()
        {
            return Mapper.Map<List<UserViewModel>>(_authService.GetUsers().Where(x => x.Role.Name != "Admin"));
        }

        public UserViewModel GetUser(int id)
        {
            return Mapper.Map<UserViewModel>(_authService.GetUserInfo(id));
        }

        public Boolean Confirm(int userId)
        {
            var eventInfo = new LogEventInfo(LogLevel.Info, _logger.Name, "User confirmed");

            var result = _authService.ConfirmUser(userId);

            if (result.Status == ServiceResultStatus.Success)
            {
                _logger.Info(eventInfo);
                return true;
            }
            _logger.Error(eventInfo);
            return false;

        }

        public Boolean Refuse(int userId)
        {
            var eventInfo = new LogEventInfo(LogLevel.Info, _logger.Name, "User deleted");

            var result = _authService.DeleteUser(userId);

            if (result.Status == ServiceResultStatus.Success)
            {
                _logger.Info(eventInfo);
                return true;
            }
            _logger.Error(eventInfo);
            return false;
        }

        public IEnumerable<RoleViewModel> GetRoles()
        {
            return Mapper.Map<List<RoleViewModel>>(_authService.GetRoles());
        }

        public bool ChangeUserRole(int userId, int roleId)
        {
            var eventInfo = new LogEventInfo(LogLevel.Info, _logger.Name, "Changing role");
            eventInfo.Properties["userId"] = userId;
            eventInfo.Properties["roleId"] = roleId;

            var result = _authService.ChangeRoleForUser(userId, roleId);

            if (result.Status == ServiceResultStatus.Success)
            {
                _logger.Info(eventInfo);
                return true;
            }
            _logger.Error(eventInfo);
            return false;
        }
    }
}