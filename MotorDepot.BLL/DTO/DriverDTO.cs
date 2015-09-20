using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorDepot.BLL.DTO
{
    public class DriverDTO : EntityBaseDTO
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }

        public Int32 DriverLicenseId { get; set; }
        public DriverLicenseDTO DriverLicense { get; set; }
        public Gender Gender { get; set; }
        public DateTime BithDate { get; set; }

        public UserDTO User { get; set; }
    }

    public enum Gender
    {
        Male, Female
    }
}
