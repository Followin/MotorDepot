using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorDepot.BLL.DTO
{
    public class DriverLicenseDTO
    {
        public Int32 Id { get; set; }
        public DriverLicenseTypeDTO DriverLicenseType { get; set; }
        public DateTime IssueDate { get; set; }
        public String IssuedBy { get; set; }
        public DriverDTO Driver { get; set; }
    }
}
