using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorDepot.DAL.Entities
{
    public class VehicleClass : EntityBase
    {
        public String Name { get; set; }
        public String Description { get; set; }

        public ICollection<DriverLicense> DriverLicenses { get; set; } 
    }
}
