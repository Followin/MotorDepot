using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using MotorDepot.WEB.Models;

namespace MotorDepot.WEB.Abstract
{
    public interface IAccountLogics
    {
        void Register(RegisterUserViewModel model);
        LoginResponse Login(LoginViewModel model);
        Boolean IsNicknameFree(String nickname);
        IEnumerable<UserViewModel> GetUsers();
        UserViewModel GetUser(int id);
        Boolean Confirm(Int32 userId);
        Boolean Refuse(Int32 userId);
        IEnumerable<RoleViewModel> GetRoles();
        bool ChangeUserRole(int userId, int roleId);
    }
}
