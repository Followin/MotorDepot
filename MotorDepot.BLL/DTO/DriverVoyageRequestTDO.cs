using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorDepot.BLL.DTO
{
    public class DriverVoyageRequestDTO
    {
        public Int32 Id { get; set; }
        public DriverDTO Driver { get; set; }
        public VoyageDTO Voyage { get; set; }
    }
}
