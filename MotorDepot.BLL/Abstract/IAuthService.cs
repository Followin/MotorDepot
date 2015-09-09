using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MotorDepot.BLL.DTO;
using MotorDepot.BLL.Models;

namespace MotorDepot.BLL.Abstract
{
    public interface IAuthService
    {
        /// <summary>
        /// Registers user
        /// </summary>
        /// <param name="user">user param</param>
        /// <returns>Status obj</returns>
        ServiceResult RegisterUser(UserDTO user);

        /// <summary>
        /// Checks if user with such login and pass exists
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns>User id. Null if user doesn't exist</returns>
        Int32? ValidateUser(String login, String password);

        UserDTO GetUserInfo(Int32 id);
        ServiceResult ChangeRoleForUser(UserDTO userDto, RoleDTO roleDto);
    }
}
