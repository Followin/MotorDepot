using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MotorDepot.DAL.Entities;

namespace MotorDepot.BLL.DTO
{
    public class UserDTO : EntityBase
    {
        public String Nickname { get; set; }
        public String Password { get; set; }
        public String Email { get; set; }
        public String ConfirmationToken { get; set; }
        public Boolean IsConfirmed { get; set; }
        public Role Role { get; set; }

        public Driver Driver { get; set; }
    }
}
