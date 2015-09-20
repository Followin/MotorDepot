using System;

namespace MotorDepot.BLL.DTO
{
    public class UserDTO : EntityBaseDTO
    {
        public String Nickname { get; set; }
        public String Password { get; set; }
        public String Email { get; set; }
        public String ConfirmationToken { get; set; }
        public Boolean IsConfirmed { get; set; }

        public Int32 RoleId { get; set; }
        public RoleDTO Role { get; set; }

        public DriverDTO Driver { get; set; }
    }
}
